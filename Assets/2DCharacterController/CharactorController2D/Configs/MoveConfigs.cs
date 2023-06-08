using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveConfig", menuName = "CharacterController/Move", order = 1)]
public class MoveConfigs : ScriptableObject
{
    [SerializeField] private float speed;
    [SerializeField] private float groundedRememberTime = 0.25f;


    public float Speed { get { return speed; } }
    public float GroundedRememberTime { get { return groundedRememberTime; } }
}
