using HarmonyLib;
using RaftHook.Utilities;

namespace RaftHook.Features.Features.World
{
    [HarmonyPatch(typeof(Shark), "ChangeState")]
    internal class NoSharkAttackPlayer
    {
        private static bool Prefix(SharkState newState)
        {
            if (RaftSettings.NoSharkAttackPlayer && newState == SharkState.AttackPlayer) return false;

            return !RaftSettings.NoSharkAttack;
        }
    }
}