using RaftHook.UI.Models;
using RaftHook.Utilities;
using UnityEngine;

namespace RaftHook.UI.Views.Players
{
    public class SelectedPlayerView : View
    {
        public SelectedPlayerView() : base("Selected Player")
        {
        }

        public Network_Player CurrentSelectedPlayer { get; private set; }

        protected override void Render(int id)
        {
            if (!IsVisible()) return;
            if (!CurrentSelectedPlayer)
            {
                GUILayout.Label("No player selected");
                return;
            }

            GUILayout.Label($"ID: {CurrentSelectedPlayer.steamID}");
            GUILayout.Label($"Name: {CurrentSelectedPlayer.name}");
            GUILayout.Label($"Position: {CurrentSelectedPlayer.transform.position}");
            GUILayout.Label($"Rotation: {CurrentSelectedPlayer.transform.rotation}");
            GUILayout.Label(
                $"Health: {CurrentSelectedPlayer.Stats.stat_health.Value}/{CurrentSelectedPlayer.Stats.stat_health.Max}");
            GUILayout.Label(
                $"Hunger: {CurrentSelectedPlayer.Stats.stat_hunger.normalConsumable.Value}/{CurrentSelectedPlayer.Stats.stat_hunger.normalConsumable.Max} + {CurrentSelectedPlayer.Stats.stat_hunger.bonusConsumable.Value}/{CurrentSelectedPlayer.Stats.stat_hunger.bonusConsumable.Max}");
            GUILayout.Label(
                $"Thirst: {CurrentSelectedPlayer.Stats.stat_thirst.normalConsumable.Value}/{CurrentSelectedPlayer.Stats.stat_thirst.normalConsumable.Max} + {CurrentSelectedPlayer.Stats.stat_thirst.bonusConsumable.Value}/{CurrentSelectedPlayer.Stats.stat_thirst.bonusConsumable.Max}");
            GUILayout.Label(
                $"Oxygen: {CurrentSelectedPlayer.Stats.stat_oxygen.Value}/{CurrentSelectedPlayer.Stats.stat_oxygen.Max}");
            GUILayout.Label($"Is Dead: {CurrentSelectedPlayer.Stats.IsDead}");

            if (GUILayout.Button("Kill")) CurrentSelectedPlayer.Stats.stat_health.Value = 0;

            if (GUILayout.Button("Heal"))
                CurrentSelectedPlayer.Stats.stat_health.Value = CurrentSelectedPlayer.Stats.stat_health.Max;

            if (GUILayout.Button("Feed"))
            {
                CurrentSelectedPlayer.Stats.stat_hunger.normalConsumable.Value =
                    CurrentSelectedPlayer.Stats.stat_hunger.normalConsumable.Max;
                CurrentSelectedPlayer.Stats.stat_hunger.bonusConsumable.Value =
                    CurrentSelectedPlayer.Stats.stat_hunger.bonusConsumable.Max;
            }

            if (GUILayout.Button("Quench"))
            {
                CurrentSelectedPlayer.Stats.stat_thirst.normalConsumable.Value =
                    CurrentSelectedPlayer.Stats.stat_thirst.normalConsumable.Max;
                CurrentSelectedPlayer.Stats.stat_thirst.bonusConsumable.Value =
                    CurrentSelectedPlayer.Stats.stat_thirst.bonusConsumable.Max;
            }

            if (GUILayout.Button("Fill Oxygen"))
                CurrentSelectedPlayer.Stats.stat_oxygen.Value = CurrentSelectedPlayer.Stats.stat_oxygen.Max;

            if (GUILayout.Button("Revive"))
            {
                var bed = BedManager.FindClosestBedToPlayer(CurrentSelectedPlayer);
                CurrentSelectedPlayer.PlayerScript.StartRespawn(bed, false);
            }

            if (GUILayout.Button("Teleport to Player"))
                RaftClient.LocalPlayer.transform.position = CurrentSelectedPlayer.transform.position;
            if (GUILayout.Button("Teleport Player to You"))
                CurrentSelectedPlayer.transform.position = RaftClient.LocalPlayer.transform.position;
            if (GUILayout.Button("Teleport to Raft"))
                CurrentSelectedPlayer.transform.position = RaftClient.Raft.transform.position;

            base.Render(id);
        }

        public void SetSelectedPlayer(Network_Player player)
        {
            if (CurrentSelectedPlayer != null && CurrentSelectedPlayer == player) return;

            CurrentSelectedPlayer = player;
        }
    }
}