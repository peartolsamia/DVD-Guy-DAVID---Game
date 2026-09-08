using UnityEngine;

// All bounces are reported. Every bounce (edge or corner) grants the same reward.
public class PlayerBounceRewardHandler : MonoBehaviour
{
    [SerializeField] private int bounceReward = 1;

    public void RegisterBounce()
    {
        Player.Instance?.SkillPoints?.EarnSkillPoints(bounceReward);
    }
}