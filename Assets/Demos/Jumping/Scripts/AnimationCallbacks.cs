using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCallbacks : MonoBehaviour
{
   [SerializeField] private JumpingStuff jumpingStuff;

    public void EndLanding()
    {
        jumpingStuff.landAnimFinished = true;
    }
}
