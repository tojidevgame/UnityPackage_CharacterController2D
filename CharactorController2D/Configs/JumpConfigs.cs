using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JumpConfig", menuName = "CharacterController/Jump", order = 1)]
public class JumpConfigs : ScriptableObject
{
    [SerializeField] private float jumpForce;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private int extraJumpCount = 1;
    [SerializeField] private GameObject jumpEffect;

    public float JumpForce { get { return jumpForce; } }
    public float FallMultiplier { get {  return fallMultiplier; } }
    public float GroundCheckRadius { get { return groundCheckRadius; } }
    public LayerMask GroundLayer { get { return groundLayer; } }
    public int ExtraJumpCount { get {  return extraJumpCount; } }
    public GameObject JumpEffect { get {  return jumpEffect; } }
}
