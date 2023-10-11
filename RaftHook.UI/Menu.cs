using RaftHook.UI.Views;
using RaftHook.Utilities;
using UnityEngine;

namespace RaftHook.UI
{
    public class Menu : MonoBehaviour
    {
        private static bool _isInLobby;
        private static bool _isInGame;

        private MainWindowView _mainWindowView;

        private void Start()
        {
            _mainWindowView = new MainWindowView();
            _mainWindowView.Start();
        }

        private void Update()
        {
            ToggleMenu();

            _mainWindowView.Update();

            _isInLobby = LoadSceneManager.IsLoadingLobbyScene;
            _isInGame = LoadSceneManager.IsGameSceneLoaded;
        }

        private void OnGUI()
        {
            if (!RaftSettings.ShowMenu) return;

            _mainWindowView.OnGUI();
        }

        private static void ToggleMenu()
        {
            //if (!_isInLobby && !_isInGame) return;

            if (!Input.GetKeyDown(KeyCode.F4)) return;

            RaftSettings.ShowMenu = !RaftSettings.ShowMenu;
            MouseUtil.ToggleCursor(RaftSettings.ShowMenu);
        }

        private void UI(int pID)
        {
            GUI.backgroundColor = Color.black;
            GUI.contentColor = Color.white;
            switch (pID)
            {
                case 0:

                    break;
                case 1:

                    break;
                case 2:

                    break;
                case 3:

                    break;
                case 4:
                    foreach (var player in RaftClient.Players)
                    {
                        GUILayout.Label(player.Value.name);
                        GUILayout.Label("SteamID: " + player.Key);
                        GUILayout.Label("Health: " + player.Value.Stats.stat_health.Value);
                        GUILayout.Label("Hunger: " + player.Value.Stats.stat_hunger.normalConsumable.Value);
                        GUILayout.Label("Thirst: " + player.Value.Stats.stat_thirst.normalConsumable.Value);
                        GUILayout.Label("Oxygen: " + player.Value.Stats.stat_oxygen.Value);
                        GUILayout.Label("Position: " + player.Value.transform.position);
                        GUILayout.Label("Rotation: " + player.Value.transform.rotation);
                        GUILayout.Label("IsLocalPlayer: " + player.Value.IsLocalPlayer);
                    }

                    break;
            }

            GUI.DragWindow();
        }
    }
}