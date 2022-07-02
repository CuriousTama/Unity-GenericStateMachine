using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [HideInInspector] public bool isGrounded = true;
    [SerializeField] private LayerMask toTouch;
    [SerializeField, Range(0f, 2f)] private float checkDistance = 0.05f;

    private void LateUpdate()
    {
        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.01f, Vector3.down, 0.5f + 0.01f, toTouch);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.down * (checkDistance + 0.01f)));
    }
}
