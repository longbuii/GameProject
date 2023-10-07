using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class PlayerMove : MonoBehaviour
{

    Rigidbody2D rb;
    Animator animator;
    [Header("GroundCheck")]
    [SerializeField] Collider2D standingCollider;
    [SerializeField] Transform groundCheckCollider;

    [Header("Crouch")]
    [SerializeField] Transform overheadCheckCollider;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] TrailRenderer tr;
    private Transform platformParent;


    const float groundCheckRadius = 0.2f;
    const float overheadCheckRadius = 0.2f;
    public float speed = 2;
    [SerializeField] float jumpPower = 100;
    [SerializeField] int totalJumps;
    int availableJumps;
    float horizontalValue;
    float runSpeedModifier = 2f;
    float crouchSpeedModifier = 0.5f;
    private float horizontalInput;


    bool isGrounded;
    bool isRunning;
    bool facingRight = true;
    bool crouchPressed;
    bool mutipleJump;
    bool coyotejump;
    bool isDead = false;

    [Header("Sounds")]
    [SerializeField] private AudioClip jumpSound;
    public InteractionSystem interactionSystem;






    void Start()
    {
        availableJumps = totalJumps;

        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();

        interactionSystem = FindObjectOfType<InteractionSystem>();

        platformParent = null;

        if (PlayerPrefs.HasKey("PlayerSpeed"))
        {
            speed = PlayerPrefs.GetFloat("PlayerSpeed");
        }
    }
    void OnApplicationQuit()
    {
        // Khi ứng dụng được đóng, đặt giá trị coin về 0 và lưu vào PlayerPrefs        
        Reset();
    }

    public void Reset()
    {
        PlayerPrefs.SetFloat("PlayerLuck", speed);

    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove() == false)
            return;
        if (!movementEnabled)
            return;
        //Store the horizontal value        
        horizontalValue = Input.GetAxisRaw("Horizontal");

        // Setting to Key "Run" for player
        if (Input.GetKeyDown(KeyCode.LeftShift))
            isRunning = true;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            isRunning = false;

        // Setting to Key "Jump" for player
        if (Input.GetButtonDown("Jump"))
            Jump();

        //// Setting to Key "Dash" for player
        //if (Input.GetKeyDown(KeyCode.RightShift))
        //    StartCoroutine(Dash());


        // Setting to Key "Crouch" for player
        if (Input.GetButtonDown("Crouch"))
            crouchPressed = true;
        else if (Input.GetButtonUp("Crouch"))
            crouchPressed = false;


        // Set to the yVelocity in the animator
        animator.SetFloat("yVelocity", rb.velocity.y);


    }

    void FixedUpdate()
    {

        //if (isDashing)
        //{
        //    return;
        //}

        GroundCheck();
        Move(horizontalValue, crouchPressed);
    }



    #region CheckGround

    void GroundCheck()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;
        // That Check is Ground for player moving, if yes (CheckGround = true), no (CheckGround = false)
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        if (colliders.Length > 0)
        {
            isGrounded = true;
            if (!wasGrounded)
            {
                availableJumps = totalJumps;
                mutipleJump = false;
                Debug.Log("Jumping");
            }
            //Check if any of the colliders is moving platform
            //Parent it to this transform
            foreach (var c in colliders)
            {
                if (c.tag == "MovingPlatform")
                    transform.parent = c.transform;
            }
        }
        else
        {
            transform.parent = null;

            if (wasGrounded)
                StartCoroutine(CoyoteJumpDelay());
        }

        //Add to setting the animator for action "Jump" of player with value bool
        animator.SetBool("Jump", !isGrounded);

    }

    #endregion


    #region Jump
    IEnumerator CoyoteJumpDelay()
    {
        coyotejump = true;
        yield return new WaitForSeconds(0.2f);
        coyotejump = false;
    }

    void Jump()
    {
        if (!crouchPressed) // Only jump if the player is not crouching
        {
            if (!isDead)
            {
                if (isGrounded)
                {
                    mutipleJump = true;
                    availableJumps--;

                    rb.velocity = Vector2.up * jumpPower;
                    animator.SetBool("Jump", true);

                }
                else
                {
                    if (coyotejump)
                    {
                        Debug.Log("Coyote Jump");
                    }

                    if (mutipleJump && availableJumps > 0)
                    {
                        availableJumps--;

                        rb.velocity = Vector2.up * jumpPower;
                        animator.SetBool("Jump", false);
                    }
                }
            }
        }
    }
    #endregion 



    void Move(float dir, bool crouchFlag)
    {


        #region Crouch
        // Player will check to groundlayer, if the ground overhead to player,  we can press "S" and player will crouch it.

        if (isGrounded)
        {

            if (!crouchFlag)
            {
                if (Physics2D.OverlapCircle(overheadCheckCollider.position, overheadCheckRadius, groundLayer))
                    crouchFlag = true;
            }

            animator.SetBool("Crouch", crouchFlag);
            standingCollider.enabled = !crouchFlag;

        }


        #endregion

        #region Move & Run
        // Set Value of x for player by the speed and dir
        float xVal = dir * speed * 100 * Time.fixedDeltaTime;

        //When we are running, it's setting to run by the running modifier
        if (isRunning)
            xVal *= runSpeedModifier;

        //When we are crouch, it's setting to run by the crouch modifier
        if (crouchFlag)
            xVal *= crouchSpeedModifier;

        // Create to Vec2 for the velocity and Set the player's velocity
        Vector2 targetVelocity = new Vector2(xVal, rb.velocity.y);
        rb.velocity = targetVelocity;


        // Set to look right/left and if click left/right for player 

        if (facingRight && dir < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            facingRight = false;
        }

        else if (!facingRight && dir > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            facingRight = true;
        }




        //Set the float according to the x of the RigiBody2D
        //(Idle = 0, Walk = 4, Run = 8)
        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        #endregion
    }


    private void OnDrawGizmosSelected()
    {
        // Set the Gizmos color to yellow and draw a sphere at the position of groundCheckCollider
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(groundCheckCollider.position, groundCheckRadius);

        // Set the Gizmos color to red and draw a sphere at the position of overheadCheckCollider
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(overheadCheckCollider.position, overheadCheckRadius);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Die(); // Gọi hàm Die() khi va chạm với trap
        }
    }


    public bool CanMove()
    {
        bool can = true;

        if (FindObjectOfType<InteractionSystem>().isExamining)
            can = false;
        if (FindObjectOfType<InventorySystem>().isOpen)
            can = false;
        if (isDead)
            can = false;

        return can;
    }
    #region Don't move when player Attacking something
    public float GetHorizontalValue()
    {
        return horizontalValue;
    }
    public bool GetRunning()
    {
        return isRunning;
    }


    public bool CanAttack()
    {
        return horizontalInput == 0;
    }
    private bool movementEnabled = true;

    public void DisableMovement()
    {
        movementEnabled = false;
    }

    public void EnableMovement()
    {
        movementEnabled = true;
    }
    #endregion

    public void Die()
    {
        isDead = true;
    }

    public void ResetPlayer()
    {
        isDead = false;
    }


    #region Moving Platform
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MovingPlatform"))
        {
            // Gắn player vào platform
            platformParent = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("MovingPlatform"))
        {
            // Bỏ gắn player ra khỏi platform
            platformParent = null;
        }
    }
    #endregion

}
