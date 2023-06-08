using UnityEngine;

[CreateAssetMenu(fileName = "DashConfig", menuName = "CharacterController/Dash", order = 1)]
public class DashConfigs : ScriptableObject
{
    [SerializeField] private float dashSpeed;
    [Tooltip("Amount of time (in seconds) the player will be in the dashing speed")]
    [SerializeField] private float startDashTime;
    [Tooltip("Time (in seconds) between dashes")]
    [SerializeField] private float dashCooldown;
    [SerializeField] private GameObject dashEffect;

    public float DashSpeed { get { return dashSpeed; }  }
    public float StartDashTime { get {  return startDashTime; } }
    public float DashCooldown { get {  return dashCooldown; } }
    public GameObject DashEffect { get { return dashEffect; } }
}
