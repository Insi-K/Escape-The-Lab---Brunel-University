using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private float startTime;
    private float duration = 0.2f;

    private void Start()
    {
        startTime = Time.time;
    }
    // Update is called once per frame
    void Update()
    {
        if(Time.time >= startTime + duration)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Player player))
        {
            player.Hit(50);
        }
        if(collision.TryGetComponent(out Enemy enemy))
        {
            enemy.Hit(100);
        }
    }
}


