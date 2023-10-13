using HarmonyLib;
using RaftHook.Utilities;

namespace RaftHook.Features.Features.Self
{
    [HarmonyPatch(typeof(BlockCreator), "CanBuildBlock", typeof(Block))]
    internal static class NoBuildRestrictions
    {
        private static BuildError Postfix(BuildError __result)
        {
            return RaftSettings.NoBuildRestrictions ? BuildError.None : __result;
        }
    }
}