using RaftHook.Features.Features.World;
using RaftHook.UI.Models;
using RaftHook.Utilities;
using UnityEngine;

namespace RaftHook.UI.Views
{
    public class WorldView : View
    {
        public WorldView() : base("World")
        {
        }

        protected override void Render(int id)
        {
            RaftSettings.NoShark = GUILayout.Toggle(RaftSettings.NoShark, "No Shark");
            RaftSettings.NoSharkAttack = GUILayout.Toggle(RaftSettings.NoSharkAttack, "No Shark Attack");
            RaftSettings.NoSharkAttackPlayer =
                GUILayout.Toggle(RaftSettings.NoSharkAttackPlayer, "No Shark Attack Player");
            RaftSettings.NoSharkAttackRaft = GUILayout.Toggle(RaftSettings.NoSharkAttackRaft, "No Shark Attack Raft");

            GUILayout.Space(10f);
            GUILayout.Label("Weather");
            if (GUILayout.Button("Calm Water"))
                ComponentManager<WeatherManager>.Value.SetWeather(UniqueWeatherType.Calm);

            GUILayout.Space(10f);
            GUILayout.Label("Plants");
            if (GUILayout.Button("Grow Plants")) Plants.GrowPlants();
            if (GUILayout.Button("Water Plants")) Plants.WaterPlants();

            base.Render(id);
        }
    }
}