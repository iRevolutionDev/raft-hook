using RaftHook.UI.Models;
using RaftHook.UI.Views.Players;
using UnityEngine;

namespace RaftHook.UI.Views
{
    public class MainWindowView : View
    {
        private readonly GUIStyle _labelStyle = new GUIStyle();

        private readonly MiscellaneousView _miscellaneousView = new MiscellaneousView();
        private readonly PlayersView _playersView = new PlayersView();
        private readonly PlayerView _playerView = new PlayerView();
        private readonly VisualsView _visualsView = new VisualsView();

        public MainWindowView() : base("Raft Hook", true)
        {
            _labelStyle.fontSize = 12;
            _labelStyle.normal.textColor = Color.black;
            _labelStyle.alignment = TextAnchor.MiddleCenter;

            AddChild(_playerView);
            AddChild(_visualsView);
            AddChild(_miscellaneousView);
            AddChild(_playersView);
        }

        protected override void Render(int id)
        {
            GUI.backgroundColor = Color.black;
            GUI.contentColor = Color.white;

            GUILayout.Label("Press F4 for Menu", _labelStyle);
            GUILayout.Label("Delete to Unhook the Cheat", _labelStyle);
            if (GUILayout.Button("Player")) _playerView.ToggleVisible();
            if (GUILayout.Button("Visual")) _visualsView.ToggleVisible();
            if (GUILayout.Button("Miscellaneous")) _miscellaneousView.ToggleVisible();
            if (GUILayout.Button("Players")) _playersView.ToggleVisible();

            base.Render(id);
        }
    }
}