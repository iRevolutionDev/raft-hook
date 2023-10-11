using HarmonyLib;
using RaftHook.Utilities;

namespace RaftHook.Features.Features.World
{
    [HarmonyPatch(typeof(Shark), "AttackRaftUpdate")]
    internal class NoSharkAttackRaft
    {
        private static void Prefix(Shark __instance)
        {
            if (__instance.state != SharkState.AttackRaft) return;

            var attackingRaft = __instance.targetBlock != null && !(__instance.targetBlock is SharkBait);

            if (!attackingRaft || !RaftSettings.NoSharkAttackRaft) return;

            __instance.ChangeState(SharkState.PassiveWater);
        }
    }
}