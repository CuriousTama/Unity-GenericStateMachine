using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingStuff : MonoBehaviour
{
    public Rigidbody rb;
    public TMPro.TMP_Text stateText;
    public GroundCheck groundCheck;
    public Animator animator;
    public float jumpPower = 1f;
    public bool landAnimFinished = false;
    public bool jumped = false;
}
