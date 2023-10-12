using HarmonyLib;
using RaftHook.Utilities;

namespace RaftHook.Features.Features.Self
{
    [HarmonyPatch(typeof(BlockCreator), "HasEnoughResourcesToBuild", typeof(CostMultiple[]))]
    internal static class NoBuildRestrictions
    {
        private static bool Postfix(bool __result)
        {
            return RaftSettings.NoBuildRestrictions || __result;
        }
    }
}