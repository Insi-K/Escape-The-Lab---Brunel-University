using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerController : PhysicsObject
{
    [SerializeField] float maxSpeed = 1;
    private float currentSpeed = 0;
    [SerializeField] float jumpForce = 1;

    private bool isCrouching = false;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        targetVelocity = new Vector2(Input.GetAxis("Horizontal") * currentSpeed, 0);

        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpForce;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && grounded)
        {
            currentSpeed = 0;
            isCrouching = true;
            Debug.Log("I am stopping!!");
        }
        else 
        { 
            currentSpeed = maxSpeed;
            isCrouching = false;
        } 
        
    }
}
