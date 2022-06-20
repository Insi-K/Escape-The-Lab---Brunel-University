using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    #region Bullet Controller Variables
    //Components
    Rigidbody2D bulletRb;
    BoxCollider2D bulletCollider;

    //Instance Variables
    public float bulletSpeed;

    //Check Variables
    private enum TargetCollisionType { player, enemy}
    [SerializeField] private TargetCollisionType targetID;

    public LayerMask whatIsTarget;

    public float checkCollisionRadius = 0.08f;

    //Particles
    public GameObject smoke;
    public GameObject entityHit;
    //public GameObject enemyHit;

    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        bulletRb = GetComponent<Rigidbody2D>();
        bulletCollider = GetComponent<BoxCollider2D>();

        if (transform.localRotation.z > 0)
        {
            bulletRb.AddForce(new Vector2(-1, 0) * bulletSpeed, ForceMode2D.Impulse);
        }
        else
        {
            bulletRb.AddForce(new Vector2(1, 0) * bulletSpeed, ForceMode2D.Impulse);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, checkCollisionRadius);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool hitSomeone = CheckCollision(collision.gameObject);
        bulletRb.velocity = new Vector2(0, 0);
        if(hitSomeone) Instantiate(entityHit, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        else Instantiate(smoke, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        Destroy(gameObject);
    }

    private bool CheckCollision(GameObject entity)
    {
        if (targetID == TargetCollisionType.enemy)
        {
            Enemy enemy = entity.GetComponent<Enemy>();
            if (enemy != null)
            {
                //Debug.Log("Enemy Should take hit");
                enemy.Hit(15);
                return true;
            }
            //else Debug.Log("Enemy not found");
        }
        else if (targetID == TargetCollisionType.player)
        {
            Player player = entity.GetComponent<Player>();
            if (player != null)
            {
                player.Hit(10);
                return true;
            }
        }
        return false;
    }
}
