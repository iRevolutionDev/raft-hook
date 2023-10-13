using HarmonyLib;
using RaftHook.Utilities;

namespace RaftHook.Features.Features.Self
{
    [HarmonyPatch(typeof(ItemInstance_Recipe), "HasEnoughResourcesToCraft", typeof(PlayerInventory))]
    internal static class NoCraftingRestrictionsQuickCraftPatch
    {
        private static bool Postfix(bool __result)
        {
            return RaftSettings.NoCraftingRestrictions || __result;
        }
    }

    [HarmonyPatch(typeof(CostCollection), "MeetsRequirements")]
    internal static class NoCraftingRestrictionsPatch
    {
        private static bool Postfix(bool __result)
        {
            return RaftSettings.NoCraftingRestrictions || __result;
        }
    }
}