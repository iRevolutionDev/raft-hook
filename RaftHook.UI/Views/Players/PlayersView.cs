using System.Linq;
using RaftHook.UI.Models;
using RaftHook.Utilities;
using UnityEngine;

namespace RaftHook.UI.Views.Players
{
    public class PlayersView : View
    {
        private readonly SelectedPlayerView _selectedPlayerView = new SelectedPlayerView();

        public PlayersView() : base("Players")
        {
            AddChild(_selectedPlayerView);
        }

        protected override void Render(int id)
        {
            if (RaftClient.Players.Count == 0) return;

            var players = RaftClient.Players.Where(player => player.Value != null)
                .Where(player => player.Value != RaftClient.LocalPlayer).ToList();

            if (players.Count == 0)
            {
                GUILayout.Label("No players found");
                return;
            }

            foreach (var player in players.Where(player => GUILayout.Button(player.Value.name)))
            {
                if (_selectedPlayerView.IsVisible() && _selectedPlayerView.CurrentSelectedPlayer == player.Value)
                {
                    _selectedPlayerView.Hide();
                    continue;
                }

                _selectedPlayerView.Show();
                _selectedPlayerView.SetSelectedPlayer(player.Value);
            }

            base.Render(id);
        }
    }
}