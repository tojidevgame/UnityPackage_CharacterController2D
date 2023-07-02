using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JumpConfig", menuName = "CharacterController/Jump", order = 1)]
public class JumpConfigs : ScriptableObject
{
    [SerializeField] protected float jumpForce;
    [SerializeField] protected float fallMultiplier;
    [SerializeField] protected float groundCheckRadius;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected int extraJumpCount = 1;
    [SerializeField] protected GameObject jumpEffect;
    [SerializeField] protected bool canMoveWhenJump = true;

    public float JumpForce { get { return jumpForce; } }
    public float FallMultiplier { get {  return fallMultiplier; } }
    public float GroundCheckRadius { get { return groundCheckRadius; } }
    public LayerMask GroundLayer { get { return groundLayer; } }
    public int ExtraJumpCount { get {  return extraJumpCount; } }
    public GameObject JumpEffect { get {  return jumpEffect; } }
}
