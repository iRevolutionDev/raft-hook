using RaftHook.Features.Features.Self;
using RaftHook.UI.Models;
using RaftHook.Utilities;
using UnityEngine;

namespace RaftHook.UI.Views
{
    public class PlayerView : View
    {
        public PlayerView() : base("Player")
        {
        }


        protected override void Render(int id)
        {
            RaftSettings.UnlimitedHealth = GUILayout.Toggle(RaftSettings.UnlimitedHealth, "Unlimited Health");
            RaftSettings.NoThirst = GUILayout.Toggle(RaftSettings.NoThirst, "No Thirst");
            RaftSettings.NoHunger = GUILayout.Toggle(RaftSettings.NoHunger, "No Hunger");
            RaftSettings.NoFallDamage = GUILayout.Toggle(RaftSettings.NoFallDamage, "No Fall Damage");
            if (GUILayout.Button("Heal")) Stats.Heal();
            if (GUILayout.Button("Feed")) Stats.Feed();
            if (GUILayout.Button("Quench")) Stats.Quench();
            if (GUILayout.Button("Fill Oxygen")) Stats.FillOxygen();
            if (GUILayout.Button("Respawn"))
            {
                var localPlayer = RaftClient.LocalPlayer;
                var bed = BedManager.FindClosestBedToPlayer(localPlayer);
                localPlayer.PlayerScript.StartRespawn(bed, false);
            }

            base.Render(id);
        }
    }
}