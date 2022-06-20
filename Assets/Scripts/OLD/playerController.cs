using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    //Movement Variables
    public float maxSpeed;
    public float JumpForce;
    public Transform groundCheck;
    public LayerMask groundLayer;

    //Shooting Variables
    public Transform gunPoint;
    public GameObject bullet;
    public float fireRate;

    float moveSpeed;
    Rigidbody2D playerRb;
    Animator playerAnim;
    bool facingRight;
    bool isGrounded;
    public float groundCheckRadius = 0.2f;

    float fJumpPressedRemember = 0;
    [SerializeField]
    float fJumpPressedRememberTime = 0.2f;

    float fGroundedRemember = 0;
    [SerializeField]
    float fGroundedRememberTime = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();

        facingRight = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Player has to shoot");
            playerAnim.SetTrigger("Shoot");
            FireBullet();
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        playerAnim.SetBool("grounded", isGrounded);

        fGroundedRemember -= Time.deltaTime;
        if(isGrounded)
        {
            fGroundedRemember = fGroundedRememberTime;
        }

        fJumpPressedRemember -= Time.deltaTime;
        if (Input.GetButtonDown("Jump")) //GetKeyDown --- KeyCode.UpArrow
        {
            Debug.Log("Player has to Jump");
            fJumpPressedRemember = fJumpPressedRememberTime;
        }
        if(fJumpPressedRemember>0 && fGroundedRemember>0)
        {
            fJumpPressedRemember = 0;
            fGroundedRemember = 0;
            playerRb.velocity = new Vector2(playerRb.velocity.x, JumpForce);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("Player is crouching");
            moveSpeed = maxSpeed;
            maxSpeed = 0;
            playerAnim.SetBool("isCrouching", true);
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            Debug.Log("Player is not crouching");
            maxSpeed = moveSpeed;
            playerAnim.SetBool("isCrouching", false);
        }
    }

    // Fixed Update is a frame independent function
    void FixedUpdate()
    {
        float move = Input.GetAxisRaw("Horizontal");

        playerAnim.SetFloat("speed",Mathf.Abs(move));

        playerRb.velocity = new Vector2(move * maxSpeed, playerRb.velocity.y);

        if(move>0 && !facingRight)
        {
            Flip();
        }
        else if(move<0 && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void FireBullet()
    {
        if(facingRight)
        {
            Instantiate(bullet, gunPoint.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        }
        else if(!facingRight)
        {
            Instantiate(bullet, gunPoint.position, Quaternion.Euler(new Vector3(0, 0, 180)));
        }
    }
}
