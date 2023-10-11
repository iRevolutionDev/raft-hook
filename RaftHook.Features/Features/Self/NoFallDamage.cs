using HarmonyLib;
using RaftHook.Utilities;

namespace RaftHook.Features.Features.Self
{
    [HarmonyPatch(typeof(PersonController), "CalculateFallDamage")]
    internal static class NoFallDamage
    {
        private static bool Prefix()
        {
            return !RaftSettings.NoFallDamage;
        }
    }
}