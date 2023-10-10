using RaftHook.Utilities;
using UnityEngine;

namespace RaftHook.Features.Features.Self
{
    public class NoThirst : MonoBehaviour
    {
        private void Update()
        {
            var localPlayer = RaftClient.LocalPlayer;
            var thirst = localPlayer.Stats.stat_thirst.Normal.NormalValue;
            var maxThirst = localPlayer.Stats.stat_thirst.Normal.Max;

            if (RaftSettings.NoThirst && thirst < maxThirst)
                localPlayer.Stats.stat_thirst.Normal.SetToMaxValue();
        }
    }
}