using UnityEngine;

public class CharacterAnimator2D : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;
    private CharacterController2D characterController;
    private Animator animator;
    private static readonly int Move = Animator.StringToHash("Move");
    private static readonly int JumpState = Animator.StringToHash("JumpState");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    private static readonly int WallGrabbing = Animator.StringToHash("WallGrabbing");
    private static readonly int IsDashing = Animator.StringToHash("IsDashing");

    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController2D>();
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        // Idle & Running animation
        animator.SetFloat(Move, Mathf.Abs(rigidBody2D.velocity.x));

        // Jump state (handles transitions to falling/jumping)
        float verticalVelocity = rigidBody2D.velocity.y;
        animator.SetFloat(JumpState, verticalVelocity);

        // Jump animation
        if (!characterController.IsGrounded && !characterController.ActuallyWallGrabbing)
        {
            animator.SetBool(IsJumping, true);
        }
        else
        {
            animator.SetBool(IsJumping, false);
        }

        if (!characterController.IsGrounded && characterController.ActuallyWallGrabbing)
        {
            animator.SetBool(WallGrabbing, true);
        }
        else
        {
            animator.SetBool(WallGrabbing, false);
        }

        // dash
        animator.SetBool(IsDashing, characterController.IsDashing);
    }
}
