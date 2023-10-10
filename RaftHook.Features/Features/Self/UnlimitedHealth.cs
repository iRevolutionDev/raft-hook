using RaftHook.Utilities;
using UnityEngine;

namespace RaftHook.Features.Features.Self
{
    public class UnlimitedHealth : MonoBehaviour
    {
        private void Update()
        {
            var localPlayer = RaftClient.LocalPlayer;
            var health = localPlayer.Stats.stat_health.Value;
            var maxHealth = localPlayer.Stats.stat_health.Max;

            if (RaftSettings.UnlimitedHealth && health < maxHealth)
                localPlayer.Stats.stat_health.SetToMaxValue();
        }
    }
}