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
    }
}