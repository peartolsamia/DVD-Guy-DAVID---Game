using UnityEngine;

// All bounces will reported
// Reminds last bounce to detect corner hit
// We give rewards for all edge or corner bounces
public class PlayerBounceRewardHandler : MonoBehaviour
{
    [SerializeField] private float cornerBounceThreshold = 0.05f;
    [SerializeField] private int normalBounceReward = 1;
    [SerializeField] private int cornerBounceReward = 100;

    private float lastBounceTime = -Mathf.Infinity;

    public void RegisterBounce()
    {
        float now = Time.time;
        float timeSinceLastBounce = now - lastBounceTime;

        bool isCornerBounce = timeSinceLastBounce <= cornerBounceThreshold;
        int reward = isCornerBounce ? cornerBounceReward : normalBounceReward;

        Player.Instance?.SkillPoints?.EarnSkillPoints(reward);

        lastBounceTime = now;
    }
}