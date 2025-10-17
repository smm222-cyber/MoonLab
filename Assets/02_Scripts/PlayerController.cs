using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public float speed = 5f;
    //Para salto
    public float jumpForce = 3f;
    public float raycastLenght = 0.1f;
    public LayerMask layerFloor;
    private bool onGround;
    //
    private Rigidbody2D rb;

    //animator
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Jump();
        
    }
    void Movement()
    {
        //float speedX = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float speedX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        //Changes the player orientation
        Vector3 scale = transform.localScale;
        if (speedX < 0)
            scale.x = -Mathf.Abs(scale.x);
        else if (speedX > 0)
            scale.x = Mathf.Abs(scale.x);

        transform.localScale = scale;

        //Animation
        animator.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal")));

        //Move
        Vector3 position = transform.position;
        transform.position = new Vector3(position.x + speedX, position.y, position.z);
    }

    void Jump()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastLenght, layerFloor);
        onGround = hit.collider != null;

        if (onGround && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }

        animator.SetBool("IsGrounded", onGround);

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * raycastLenght);
    }

}
