using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(MoveInput))]
public class CharacterController2D : MonoBehaviour
{
    [Header("Move Data")]
    [SerializeField] protected MoveConfigs moveData;
    [SerializeField] protected JumpConfigs jumpData;
    [SerializeField] protected DashConfigs dashData;
    [SerializeField] protected WallJumpConfigs wallJumpData;

    [Space(10), Header("Input")]
    [SerializeField] protected MoveInput moveInput;


    [Space(10), Header("Check")]
    [SerializeField] protected Transform groundCheck;
    protected bool canMove = true;



    protected bool isGrounded;
    protected bool isDashing = false;
    protected bool actuallyWallGrabbing = false;

    protected Rigidbody2D rigidBody2D;
    protected bool m_facingRight = true;
    protected float m_groundedRemember = 0f;
    protected int m_extraJumps;
    protected float m_extraJumpForce;
    protected float m_dashTime;
    protected bool m_hasDashedInAir = false;
    protected bool m_onWall = false;
    protected bool m_onRightWall = false;
    protected bool m_onLeftWall = false;
    protected bool m_wallGrabbing = false;
    protected readonly float m_wallStickTime = 0.25f;
    protected float m_wallStick = 0f;
    protected bool m_wallJumping = false;
    protected float m_dashCooldown;

    // 0 -> none, 1 -> right, -1 -> left
    protected int m_onWallSide = 0;
    protected int m_playerSide = 1;

    public bool IsGrounded { get { return isGrounded; } }
    public bool ActuallyWallGrabbing { get {  return actuallyWallGrabbing; } }
    public bool IsDashing { get { return isDashing; } }

    protected virtual void Start()
    {
        m_extraJumps = jumpData.ExtraJumpCount;
        m_dashTime = dashData.StartDashTime;
        m_dashCooldown = dashData.DashCooldown;
        m_extraJumpForce = jumpData.JumpForce * 0.7f;

        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        // check if grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, jumpData.GroundCheckRadius, jumpData.GroundLayer);
        var position = transform.position;

        // check if on wall
        m_onWall = Physics2D.OverlapCircle((Vector2)position + wallJumpData.GrabRightOffset, wallJumpData.GrabCheckRadius, jumpData.GroundLayer)
                  || Physics2D.OverlapCircle((Vector2)position + wallJumpData.GrabLeftOffset, wallJumpData.GrabCheckRadius, jumpData.GroundLayer);
        m_onRightWall = Physics2D.OverlapCircle((Vector2)position + wallJumpData.GrabRightOffset, wallJumpData.GrabCheckRadius, jumpData.GroundLayer);
        m_onLeftWall = Physics2D.OverlapCircle((Vector2)position + wallJumpData.GrabLeftOffset, wallJumpData.GrabCheckRadius, jumpData.GroundLayer);

        // calculate player and wall sides as integers
        CalculateSides();

        if ((m_wallGrabbing || isGrounded) && m_wallJumping)
        {
            m_wallJumping = false;
        }

        // horizontal movement
        if (m_wallJumping)
        {
            rigidBody2D.velocity = Vector2.Lerp(rigidBody2D.velocity, (new Vector2(moveInput.HorizontalInput * moveData.Speed, rigidBody2D.velocity.y)), 1.5f * Time.fixedDeltaTime);
        }
        else
        {
            if (canMove && !m_wallGrabbing)
                rigidBody2D.velocity = new Vector2(moveInput.HorizontalInput * moveData.Speed * Time.fixedDeltaTime, rigidBody2D.velocity.y);
            else if (!canMove)
                rigidBody2D.velocity = new Vector2(0f, rigidBody2D.velocity.y);
        }
        // better jump physics
        if (rigidBody2D.velocity.y < 0f)
        {
            rigidBody2D.velocity += Vector2.up * Physics2D.gravity.y * (jumpData.FallMultiplier - 1) * Time.fixedDeltaTime;
        }

        // Flipping
        if (!m_facingRight && moveInput.HorizontalInput > 0f)
            Flip();
        else if (m_facingRight && moveInput.HorizontalInput < 0f)
            Flip();

        // Dashing logic
        if (isDashing)
        {
            if (m_dashTime <= 0f)
            {
                isDashing = false;
                m_dashCooldown = dashData.DashCooldown;
                m_dashTime = dashData.StartDashTime;
                rigidBody2D.velocity = Vector2.zero;
            }
            else
            {
                m_dashTime -= Time.deltaTime;
                if (m_facingRight)
                    rigidBody2D.velocity = Vector2.right * dashData.DashSpeed;
                else
                    rigidBody2D.velocity = Vector2.left * dashData.DashSpeed;
            }
        }

        // wall grab
        if (m_onWall && !isGrounded && rigidBody2D.velocity.y <= 0f && m_playerSide == m_onWallSide)
        {
            actuallyWallGrabbing = true;    // for animation
            m_wallGrabbing = true;
            rigidBody2D.velocity = new Vector2(moveInput.HorizontalInput * moveData.Speed, -wallJumpData.SlideSpeed);
            m_wallStick = m_wallStickTime;
        }
        else
        {
            m_wallStick -= Time.deltaTime;
            actuallyWallGrabbing = false;
            if (m_wallStick <= 0f)
                m_wallGrabbing = false;
        }
        if (m_wallGrabbing && isGrounded)
            m_wallGrabbing = false;

        // enable/disable dust particles
        float playerVelocityMag = rigidBody2D.velocity.sqrMagnitude;
    }

    protected virtual void Update()
    {
        if (isGrounded)
        {
            m_extraJumps = jumpData.ExtraJumpCount;
        }

        // grounded remember offset (for more responsive jump)
        m_groundedRemember -= Time.deltaTime;
        if (isGrounded)
            m_groundedRemember = moveData.GroundedRememberTime;

        if (!isDashing && !m_hasDashedInAir && m_dashCooldown <= 0f)
        {
            // dash input (left shift)
            if (moveInput.DashInput)
            {
                isDashing = true;

                // if player in air while dashing
                if (!isGrounded)
                {
                    m_hasDashedInAir = true;
                }
                // dash logic is in FixedUpdate
            }
        }

        m_dashCooldown -= Time.deltaTime;
        // if has dashed in air once but now grounded
        if (m_hasDashedInAir && isGrounded)
            m_hasDashedInAir = false;

        // Jumping
        if (moveInput.JumpInput && m_extraJumps > 0 && !isGrounded && !m_wallGrabbing)   // extra jumping
        {
            rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, m_extraJumpForce); ;
            m_extraJumps--;
        }
        else if (moveInput.JumpInput && (isGrounded || m_groundedRemember > 0f)) // normal single jumping
        {
            rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, jumpData.JumpForce);
        }
        else if (moveInput.JumpInput && m_wallGrabbing && moveInput.HorizontalInput != m_onWallSide)     // wall jumping off the wall
        {
            m_wallGrabbing = false;
            m_wallJumping = true;
            Debug.Log("Wall jumped");
            if (m_playerSide == m_onWallSide)
                Flip();
            rigidBody2D.AddForce(new Vector2(-m_onWallSide * wallJumpData.WallJumpForce.x, wallJumpData.WallJumpForce.y), ForceMode2D.Impulse);
        }
    }

    protected virtual void Flip()
    {
        m_facingRight = !m_facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    protected virtual void CalculateSides()
    {
        if (m_onRightWall)
            m_onWallSide = 1;
        else if (m_onLeftWall)
            m_onWallSide = -1;
        else
            m_onWallSide = 0;

        if (m_facingRight)
            m_playerSide = 1;
        else
            m_playerSide = -1;
    }
}
