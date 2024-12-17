using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    public Rigidbody2D RB;
    public float gravityScale; // Normal Gravity

    [Header("Inscribed")]
    public float maxSpeed = 20; // The fastest I want the jumper to move
    public float acceleration = 2; // How fast the jumper gains speed
    public float deceleration = 1.75f; // How fast the jumper loses speed
    public float jumpHeight = 15f; // The force with which the jumper jumps
    public float gravityMult = 1.5f; // The multiplier for gravity when falling
    public float maxFallSpeed = 25f; // Maximum fall speed
    public float jumpCutMult = .5f; // The multiplier for cutting a jump short

    [Header("Dynamic")]
    public float horizonSpeed = 0; // The jumpers current speed
    public bool isGrounded;
    public bool doubleJump;
    public bool wallSliding;
    public float sinceLastJump; // Time since last jump
    public bool immobile; // If the player should be able to move
    
    [Space]

    //Used for jumping
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(.5f, .01f);

    [Space]

    [SerializeField] private Transform _frontWallCheck;
    [SerializeField] private Transform _backWallCheck;
    [SerializeField] private Vector2 _wallCheckSize = new Vector2(0.5f, 1f);

    [SerializeField] private LayerMask _groundLayer;

    void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        immobile = false;
    }

    void Start()
    {
        gravityScale = RB.gravityScale;
        doubleJump = true;
    }

    void Update()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");

        isGrounded = Physics2D.OverlapBox(_groundCheck.position, _groundCheckSize, 0, _groundLayer); // Check if they are on the ground
        
        if ((Input.GetKeyDown(KeyCode.Space)) & isGrounded == true) // Regular Jump
        {
            Jump();
        }

        if ((Input.GetKeyDown(KeyCode.Space)) & isGrounded == false) // Double Jump and wall jump
        {
            if (doubleJump == true & !wallSliding) // Double jump if possible and not wall sliding
            {
                RB.velocity = new Vector2(RB.velocity.x, 0);
                Jump();
                doubleJump = false;
            }

            if (wallSliding) // Wall jump
            {
                RB.velocity = new Vector2(RB.velocity.x, 0);
                Jump();

                doubleJump = true;

                if (horizonSpeed > 0) // Push away from the wall
                {
                    horizonSpeed = -maxSpeed;
                }

                else
                {
                    horizonSpeed = maxSpeed;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Space)) // Release Space to Jump Cut
        {
            JumpCut();
        }

        if (isGrounded == true)
        {
            doubleJump = true;
        }

        if (wallSliding)
        {
            if (RB.velocity.y < -maxFallSpeed * 0.5) // Max fall speed is halved while wall sliding
            {
                RB.velocity = new Vector2(RB.velocity.x, (float)(-maxFallSpeed * 0.5)); // Fall slower while wall sliding
            }

            if (horizonSpeed > acceleration & xAxis > 0) // Make sure that horizontal movement speed is not at max while on the wall to avoid jump when sliding up a ledge
            {
                horizonSpeed = acceleration;
            }

            if (horizonSpeed < -acceleration & xAxis < 0)
            {
                horizonSpeed = -acceleration;
            }

        }

        if (RB.velocity.y < -maxFallSpeed) // Enforce Max Fall Speed
        {
            RB.velocity = new Vector2(RB.velocity.x, -maxFallSpeed);
        }
    }

    void FixedUpdate()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");
        Vector3 pos = transform.position;


        horizonSpeed = horizonSpeed + (acceleration * xAxis); // Horizontal speed

        if (horizonSpeed > maxSpeed) // Enforcing max speed in both directions
        {
            horizonSpeed = maxSpeed;
        }

        if (horizonSpeed < -maxSpeed)
        {
            horizonSpeed = -maxSpeed;
        }

        if (xAxis == 0)
        {
            horizonSpeed = horizonSpeed / deceleration;
        }
        
        if (!immobile)
        {
            pos.x = pos.x + horizonSpeed * Time.deltaTime; // Position updates
            transform.position = pos;
        }

        if (RB.velocity.y < 0 & !wallSliding) // If falling and not wall sliding, gravity is slightly increased
        {
            RB.gravityScale = RB.gravityScale * gravityMult;
        }
        else // If not falling, gravity is normal
        {
            RB.gravityScale = (float)(gravityScale * 1.4);
        }

        wallSliding = (xAxis != 0 & Physics2D.OverlapBox(_groundCheck.position, _wallCheckSize, 0, _groundLayer)); // Check if they are making contact with the wall

        sinceLastJump = sinceLastJump + Time.deltaTime;

    }

    void Jump()
    {
        RB.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse); // Add upward velocity
        sinceLastJump = 0;
    }

    void JumpCut()
    {
        if (RB.velocity.y > 0 & sinceLastJump < 1.4) // If still rising and before 1.4 seconds since jumping
        {
            RB.velocity = new Vector2(RB.velocity.x, RB.velocity.y * jumpCutMult); // Cut jump by mult
        }
    }

    public void Dying() // Called when the death animation is playing. Makes entirely immobile
    {
        immobile = true;
        RB.simulated = false;
    }

    public void Respawning() // Called when the death animation is playing. Makes mobile again
    {
        immobile = false;
        RB.simulated = true;
    }
}
