using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DosPintacarita : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        animator.Play("Idle");
    }
}
