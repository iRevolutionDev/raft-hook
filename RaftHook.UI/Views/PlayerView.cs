﻿using RaftHook.Features.Features.Self;
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
            RaftSettings.UnlimitedOxygen = GUILayout.Toggle(RaftSettings.UnlimitedOxygen, "Unlimited Oxygen");
            RaftSettings.NoBuildRestrictions =
                GUILayout.Toggle(RaftSettings.NoBuildRestrictions, "No Build Restrictions");
            RaftSettings.NoBuildResourceRestrictions =
                GUILayout.Toggle(RaftSettings.NoBuildResourceRestrictions, "No Build Resource Restrictions");
            RaftSettings.NoCraftingRestrictions =
                GUILayout.Toggle(RaftSettings.NoCraftingRestrictions, "No Crafting Restrictions");
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

            if (GUILayout.Button("Fill hand item durability")) Durability.SetDurabilityToMax();

            base.Render(id);
        }
    }
}