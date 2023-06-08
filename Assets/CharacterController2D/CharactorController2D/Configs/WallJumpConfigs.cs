using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WallJump", menuName = "CharacterController/WallJump", order = 1)]
public class WallJumpConfigs : ScriptableObject
{

    [Space(10), Header("Wall grab & jump")]
    [Tooltip("Right offset of the wall detection sphere")]
    [SerializeField] private Vector2 grabRightOffset = new Vector2(0.16f, 0f);
    [SerializeField] private Vector2 grabLeftOffset = new Vector2(-0.16f, 0f);
    [SerializeField] private float grabCheckRadius = 0.24f;
    [SerializeField] private float slideSpeed = 2.5f;
    [SerializeField] private Vector2 wallJumpForce = new Vector2(10.5f, 18f);
    [SerializeField] private Vector2 wallClimbForce = new Vector2(4f, 14f);

    public Vector2 GrabRightOffset { get { return grabRightOffset; } }
    public Vector2 GrabLeftOffset { get { return grabLeftOffset; } }
    public float GrabCheckRadius { get {  return grabCheckRadius; } }
    public float SlideSpeed { get {  return slideSpeed; } }
    public Vector2 WallJumpForce { get { return wallJumpForce; } }
    public Vector2 WallClimbForce { get { return wallClimbForce; } }
}
