using RaftHook.Utilities;

namespace RaftHook.Features.Features.Self
{
    public class Stats
    {
        private static Network_Player LocalPlayer => RaftClient.LocalPlayer;

        public static void Heal()
        {
            LocalPlayer.Stats.stat_health.Value = LocalPlayer.Stats.stat_health.Max;
        }

        public static void Feed()
        {
            LocalPlayer.Stats.stat_hunger.normalConsumable.Value = LocalPlayer.Stats.stat_hunger.normalConsumable.Max;
        }

        public static void Quench()
        {
            LocalPlayer.Stats.stat_thirst.normalConsumable.Value = LocalPlayer.Stats.stat_thirst.normalConsumable.Max;
        }

        public static void FillOxygen()
        {
            LocalPlayer.Stats.stat_oxygen.Value = LocalPlayer.Stats.stat_oxygen.Max;
        }
    }
}