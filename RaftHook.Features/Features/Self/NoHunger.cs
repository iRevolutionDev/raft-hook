using RaftHook.Utilities;
using UnityEngine;

namespace RaftHook.Features.Features.Self
{
    public class NoHunger : MonoBehaviour
    {
        public void Update()
        {
            var localPlayer = RaftClient.LocalPlayer;
            var hunger = localPlayer.Stats.stat_hunger.Normal.NormalValue;
            var maxHunger = localPlayer.Stats.stat_hunger.Normal.Max;

            if (RaftSettings.NoHunger && hunger < maxHunger)
                localPlayer.Stats.stat_hunger.Normal.SetToMaxValue();
        }
    }
}