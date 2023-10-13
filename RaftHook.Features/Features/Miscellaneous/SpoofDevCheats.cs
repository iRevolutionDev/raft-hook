using HarmonyLib;
using MelonLoader;
using RaftHook.Utilities;

namespace RaftHook.Features.Features.Miscellaneous
{
    [HarmonyPatch(typeof(Cheat), "AllowCheatsForLocalPlayer")]
    internal static class AllowCheatsForLocalPlayerPatch
    {
        private static bool Postfix(bool __result)
        {
            Cheat.UseCheats = RaftSettings.SpoofDevCheats;
            if (RaftSettings.SpoofDevCheats) MelonLogger.Msg("[AllowCheatsForLocalPlayerPatch] Spoofed dev cheats");
            return RaftSettings.SpoofDevCheats;
        }
    }

    [HarmonyPatch(typeof(Cheat), "AllowCheatsForUser")]
    internal static class AllowCheatsForUserPatch
    {
        private static bool Postfix(bool __result)
        {
            Cheat.UseCheats = RaftSettings.SpoofDevCheats;
            if (RaftSettings.SpoofDevCheats) MelonLogger.Msg("[AllowCheatsForUserPatch] Spoofed dev cheats");
            return RaftSettings.SpoofDevCheats;
        }
    }

    [HarmonyPatch(typeof(Cheat), "IsUserDev")]
    internal static class IsUserDevPatch
    {
        private static bool Postfix(bool __result)
        {
            if (RaftSettings.SpoofDevCheats) MelonLogger.Msg("[IsUserDevPatch] Spoofed dev cheats");
            return RaftSettings.SpoofDevCheats;
        }
    }
}