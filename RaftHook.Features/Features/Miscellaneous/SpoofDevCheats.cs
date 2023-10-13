using HarmonyLib;
using MelonLoader;
using RaftHook.Utilities;

namespace RaftHook.Features.Features.Miscellaneous
{
    [HarmonyPatch(typeof(Cheat), "AllowCheatsForLocalPlayer")]
    internal static class SpoofDevCheats
    {
        private static bool Prefix()
        {
            if (RaftSettings.SpoofDevCheats) MelonLogger.Msg("Spoofed dev cheats");
            return !RaftSettings.SpoofDevCheats;
        }
    }
}