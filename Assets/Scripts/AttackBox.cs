using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBox : MonoBehaviour
{
    private enum TargetType { player, enemy}
    [SerializeField] private TargetType targetID;

    [SerializeField] private LayerMask whatIsTarget;

    [SerializeField] private GameObject entityHit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject entity = collision.gameObject;
        if(targetID == TargetType.enemy)
        {
            Enemy enemy = entity.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.Hit(10);
                Instantiate(entityHit, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            }
        }
        else if(targetID == TargetType.player)
        {
            Player player = entity.GetComponent<Player>();
            if (player != null)
            {
                player.Hit(7);
                Instantiate(entityHit, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
            }
        }

        CrateObject crate = entity.GetComponent<CrateObject>();
        if (crate != null)
        {
            Debug.Log("Break Box");
            crate.OpenCrate();
        }
    }
}
