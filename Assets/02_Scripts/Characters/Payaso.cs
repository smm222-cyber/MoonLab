using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Payaso : MonoBehaviour
{
    public Animator animator;
    public float idleTime = 3f;
    public float inflateTime = 2f;

    private float timer;
    private bool isInflating = false;

    void Start()
    {
        timer = idleTime;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            isInflating = !isInflating;
            animator.SetBool("Inflando", isInflating);

            timer = isInflating ? inflateTime : idleTime;
        }
    }
}
