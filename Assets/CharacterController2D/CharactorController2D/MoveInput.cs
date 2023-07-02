using UnityEngine;

public class MoveInput : MonoBehaviour
{
    protected float horizontalInput;
    protected bool dashInput;
    protected bool jumpInput;

    public float HorizontalInput { get { return horizontalInput; } set {  horizontalInput = value; } }
    public bool DashInput { get { return dashInput; } set { dashInput = value; } }
    public bool JumpInput { get { return jumpInput; } set { jumpInput = value; } }

    protected virtual void Awake() 
    { 
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        jumpInput = Input.GetButtonDown("Jump");

        dashInput = Input.GetButtonDown("Dash");
    }
}
