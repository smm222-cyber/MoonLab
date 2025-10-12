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
   
}
