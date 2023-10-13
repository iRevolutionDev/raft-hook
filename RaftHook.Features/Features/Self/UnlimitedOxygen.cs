using RaftHook.Utilities;
using UnityEngine;

namespace RaftHook.Features.Features.Self
{
    public class UnlimitedOxygen : MonoBehaviour
    {
        private void Update()
        {
            var localPlayer = RaftClient.LocalPlayer;
            var oxygen = localPlayer.Stats.stat_oxygen.Value;
            var maxOxygen = localPlayer.Stats.stat_oxygen.Max;

            if (RaftSettings.UnlimitedOxygen && oxygen < maxOxygen)
                localPlayer.Stats.stat_oxygen.SetToMaxValue();
        }
    }
}