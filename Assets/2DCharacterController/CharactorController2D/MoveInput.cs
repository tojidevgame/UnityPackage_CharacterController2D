using UnityEngine;

[CreateAssetMenu(fileName = "MoveInput", menuName = "CharacterController/MoveInput", order = 1)]
public class MoveInput : ScriptableObject
{
    private float horizontalInput;
    private bool dashInput;
    private bool jumpInput;

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

        jumpInput = Input.GetKeyDown("Jump");

        dashInput = Input.GetKeyDown("Dash");
    }
}
