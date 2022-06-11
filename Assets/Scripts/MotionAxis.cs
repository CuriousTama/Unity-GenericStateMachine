using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class MotionAxis : MonoBehaviour
{
    private const float MinInterpolateValue = 0.001f;


    public InputActionReference horizontalInput;
    public InputActionReference verticalInput;

    [Header("Configuration")]
    public float speed = 3.5f;
    public bool local = true;
    [Space(8f)]
    public bool interpolateInput = false;
    [Min(MinInterpolateValue)] public float timeForInterpolation = 0.25f;

    private float m_prevHorizontal;
    private float m_currentHorizontal;
    private float m_timeSinceLastInput = 0f;

    private float m_prevVertical;
    private float m_currentVertical;


    private void Start()
    {
        if (horizontalInput != null)
        {
            horizontalInput.action.performed += MoveHorizontal;
            horizontalInput.action.canceled += MoveHorizontal;
        }

        if (verticalInput != null)
        {
            verticalInput.action.performed += MoveVertical;
            verticalInput.action.canceled += MoveVertical;
        }
    }

    private void OnEnable()
    {
        if (horizontalInput != null)
            horizontalInput.action.Enable();

        if (verticalInput != null)
            verticalInput.action.Enable();
    }

    private void OnDisable()
    {
        if (horizontalInput != null)
            horizontalInput.action.Disable();

        if (verticalInput != null)
            verticalInput.action.Disable();
    }

    private void Update()
    {
        timeForInterpolation = Mathf.Max(MinInterpolateValue, timeForInterpolation);
        m_timeSinceLastInput += Time.deltaTime * (1f / timeForInterpolation);

        Vector3 trueMove = new Vector3(m_currentHorizontal, 0f, m_currentVertical);
        if (trueMove.magnitude > 1f) trueMove.Normalize();

        Vector3 prevMove = new Vector3(m_prevHorizontal, 0f, m_prevVertical);
        if (prevMove.magnitude > 1f) prevMove.Normalize();

        trueMove = (interpolateInput) ? Vector3.Lerp(prevMove, trueMove, m_timeSinceLastInput) : trueMove;

        if (local) trueMove = transform.rotation * trueMove;

        transform.position += trueMove * speed * Time.deltaTime;
    }

    private void MoveHorizontal(InputAction.CallbackContext ctx)
    {
        if (ctx.valueType != typeof(float))
        {
            Debug.LogWarning($"\"{ctx.action.name}\" action cannot be converted to float.");
            return;
        }

        m_prevHorizontal = m_currentHorizontal;
        m_prevVertical = m_currentVertical;
        m_currentHorizontal = ctx.ReadValue<float>();
        m_timeSinceLastInput = 0f;
    }

    private void MoveVertical(InputAction.CallbackContext ctx)
    {
        if (ctx.valueType != typeof(float))
        {
            Debug.LogWarning($"\"{ctx.action.name}\" action cannot be converted to float.");
            return;
        }

        m_prevHorizontal = m_currentHorizontal;
        m_prevVertical = m_currentVertical;
        m_currentVertical = ctx.ReadValue<float>();
        m_timeSinceLastInput = 0f;
    }
}
