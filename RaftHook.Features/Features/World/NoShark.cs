using HarmonyLib;
using RaftHook.Utilities;

namespace RaftHook.Features.Features.World
{
    [HarmonyPatch(typeof(Network_Host_Entities), "CreateShark")]
    internal static class NoShark
    {
        private static bool Prefix()
        {
            return !RaftSettings.NoShark;
        }
    }
}