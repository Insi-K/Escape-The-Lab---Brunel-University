using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float ThrowForce = 10f;
    public GameObject explosion;

    private float startTime;
    [SerializeField] private float timer = 5f;

    private Rigidbody2D RB;

    //Throw Speed
    //RB.velocity = transform.right* ThrowForce;

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        if (Time.time >= startTime + timer)
        {
            Instantiate(explosion, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            Destroy(gameObject);
        }
    }
}
