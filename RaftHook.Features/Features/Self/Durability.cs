using RaftHook.Utilities;

namespace RaftHook.Features.Features.Self
{
    public static class Durability
    {
        public static void SetDurability(int durability)
        {
            var player = RaftClient.LocalPlayer;
            if (player == null) return;

            var selectedHotBarItem = player.Inventory.GetSelectedHotbarSlot();
            if (selectedHotBarItem == null && selectedHotBarItem.HasValidItemInstance()) return;

            selectedHotBarItem.SetUses(durability);
        }

        public static void SetDurabilityToMax()
        {
            var player = RaftClient.LocalPlayer;
            if (player == null) return;

            var selectedHotBarItem = player.Inventory.GetSelectedHotbarSlot();
            if (selectedHotBarItem == null && selectedHotBarItem.HasValidItemInstance()) return;

            selectedHotBarItem.SetUses(selectedHotBarItem.GetMaxStackUses());
        }
    }
}