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
    public float jumpCutMult = .5f; // The multiplier for cutting a jump short

    [Header("Dynamic")]
    public float horizonSpeed = 0; // the jumpers current speed
    public bool isGrounded;
    public bool doubleJump;
    public bool wallSliding;
    public float sinceLastJump; //Time since last jump
    
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
    }

    void Start()
    {
        gravityScale = RB.gravityScale;
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapBox(_groundCheck.position, _groundCheckSize, 0, _groundLayer);
        if ((Input.GetKeyDown(KeyCode.Space)) & isGrounded == true)
        {
            Jump();
        }

        if (Input.GetKeyUp(KeyCode.Space)){
            JumpCut();
        }
    }

    void FixedUpdate()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");
        Vector3 pos = transform.position;


        horizonSpeed = horizonSpeed + (acceleration * xAxis);

        if (horizonSpeed > maxSpeed)
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

        pos.x = pos.x + horizonSpeed * Time.deltaTime;
        transform.position = pos;

        if (RB.velocity.y < 0 & !wallSliding)
        {
            RB.gravityScale = RB.gravityScale * gravityMult;
        }
        else
        {
            RB.gravityScale = gravityScale;
        }

        sinceLastJump = sinceLastJump + Time.deltaTime;
    }

    void Jump()
    {
        RB.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
        sinceLastJump = 0;
    }

    void JumpCut()
    {
        if (RB.velocity.y > 0 & sinceLastJump < 1.4)
        {
            RB.velocity = new Vector2(RB.velocity.x, RB.velocity.y * jumpCutMult);
        }
    }
}
