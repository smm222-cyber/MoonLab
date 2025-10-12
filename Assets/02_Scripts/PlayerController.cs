using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    public float speed = 5f;
    //For Jumping
    public float jumpForce = 3f;
    public float raycastLenght = 0.1f;
    public LayerMask layerFloor;
    private bool onGround;
    //
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Jump();
       
        
    }
    void Movement()
    {
        float speedX = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        //Changes the player orientation
        if (speedX < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);

        }
        if (speedX > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        Vector3 position = transform.position;
        transform.position = new Vector3(speedX + position.x, position.y, position.z);
    }
    void Jump()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastLenght, layerFloor);
        onGround = hit.collider != null;
        if (onGround && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * raycastLenght);
    }

}
