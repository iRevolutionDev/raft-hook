using HarmonyLib;
using RaftHook.Utilities;

namespace RaftHook.Features.Features.Self
{
    [HarmonyPatch(typeof(BlockCreator), "HasEnoughResourcesToBuild", typeof(CostMultiple[]))]
    internal static class NoBuildResourcesRestrictions
    {
        private static bool Postfix(bool __result)
        {
            return RaftSettings.NoBuildResourceRestrictions || __result;
        }
    }
}