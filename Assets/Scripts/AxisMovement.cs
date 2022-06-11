using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class AxisMovement : MonoBehaviour
{
    private const float MinInterpolateValue = 0.001f;


    public InputActionReference input;

    [Header("Configuration")]
    public float speed = 3.5f;
    public bool local = true;
    [Space(8f)]
    public bool interpolateInput = false;
    [Min(MinInterpolateValue)] public float timeForInterpolation = 0.25f;

    private Vector2 m_prevMove;
    private Vector2 m_move;
    private float m_timeSinceCancelInput = 0f;


    private void Start()
    {
        if (input != null)
        {
            input.action.performed += Move;
            input.action.canceled += Move;
        }
    }

    private void OnEnable()
    {
        if (input != null)
            input.action.Enable();
    }

    private void OnDisable()
    {
        if (input != null)
            input.action.Disable();
    }

    private void Update()
    {
        timeForInterpolation = Mathf.Max(MinInterpolateValue, timeForInterpolation);
        m_timeSinceCancelInput += Time.deltaTime * (1f / timeForInterpolation);

        Vector3 trueMove = new Vector3(m_move.x, 0f, m_move.y);
        Vector3 prevMove = new Vector3(m_prevMove.x, 0f, m_prevMove.y);
        trueMove = (interpolateInput) ? Vector2.Lerp(prevMove, trueMove, m_timeSinceCancelInput) : trueMove;

        if (local) trueMove = transform.rotation * trueMove;

        transform.position += trueMove * speed * Time.deltaTime;
    }

    private void Move(InputAction.CallbackContext ctx)
    {
        if (ctx.valueType != typeof(Vector2))
        {
            Debug.LogWarning($"\"{ctx.action.name}\" action cannot be converted to Vector2.");
            return;
        }

        m_prevMove = m_move;
        m_move = ctx.ReadValue<Vector2>();
        m_timeSinceCancelInput = 0f;
    }
}