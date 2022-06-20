using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameObject targetPoint_1;
    public GameObject targetPoint_2;

    public float moveSpeed;

    private Vector3 currentSpeed;

    private Rigidbody2D RB;

    private void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        transform.position = targetPoint_1.transform.position;
    }

    private void FixedUpdate()
    {
        transform.position += currentSpeed;

        if (transform.position == targetPoint_1.transform.position)
        {
            currentSpeed = new Vector3 (0, (-moveSpeed * Time.deltaTime),0);
        }
        else if(transform.position == targetPoint_2.transform.position)
        {
            currentSpeed = new Vector3(0, (moveSpeed * Time.deltaTime),0);
        }
    }

}
