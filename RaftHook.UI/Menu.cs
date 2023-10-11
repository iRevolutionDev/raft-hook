using RaftHook.Features.Features.Boat;
using RaftHook.Features.Features.Miscellaneous;
using RaftHook.Features.Features.Self;
using RaftHook.Utilities;
using UnityEngine;

namespace RaftHook.UI
{
    public class Menu : MonoBehaviour
    {
        private static bool _isInLobby;
        private static bool _isInGame;

        private readonly GUIStyle _labelStyle = new GUIStyle();
        private bool _bMiscellaneousWindow;

        private bool _bPlayerWindow;
        private bool _bVisualWindow;
        private Rect _mainWindow;
        private Rect _miscellaneousWindow;
        private Rect _playerWindow;
        private Rect _visualWindow;

        private void Update()
        {
            ToggleMenu();

            _isInLobby = LoadSceneManager.IsLoadingLobbyScene;
            _isInGame = LoadSceneManager.IsGameSceneLoaded;
        }

        private void OnGUI()
        {
            if (!RaftSettings.ShowMenu) return;

            GUI.backgroundColor = Color.black;
            _mainWindow = GUILayout.Window(0, _mainWindow, UI, "Menu");
            if (_bPlayerWindow) _playerWindow = GUILayout.Window(1, _playerWindow, UI, "Player");
            if (_bVisualWindow) _visualWindow = GUILayout.Window(2, _visualWindow, UI, "Visual");
            if (_bMiscellaneousWindow)
                _miscellaneousWindow = GUILayout.Window(3, _miscellaneousWindow, UI, "Miscellaneous");
        }

        private static void ToggleMenu()
        {
            if (!_isInLobby && !_isInGame) return;

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
                    GUILayout.Label("Press F4 for Menu", _labelStyle);
                    GUILayout.Label("Delete to Unhook the Cheat", _labelStyle);
                    if (GUILayout.Button("Player")) _bPlayerWindow = !_bPlayerWindow;
                    if (GUILayout.Button("Visual")) _bVisualWindow = !_bVisualWindow;
                    if (GUILayout.Button("Miscellaneous")) _bMiscellaneousWindow = !_bMiscellaneousWindow;
                    break;
                case 1:
                    RaftSettings.UnlimitedHealth = GUILayout.Toggle(RaftSettings.UnlimitedHealth, "Unlimited Health");
                    RaftSettings.NoThirst = GUILayout.Toggle(RaftSettings.NoThirst, "No Thirst");
                    RaftSettings.NoHunger = GUILayout.Toggle(RaftSettings.NoHunger, "No Hunger");
                    RaftSettings.NoFallDamage = GUILayout.Toggle(RaftSettings.NoFallDamage, "No Fall Damage");
                    if (GUILayout.Button("Give Armor Set")) Armor.GiveArmor();
                    break;
                case 2:
                    RaftSettings.EnableEsp = GUILayout.Toggle(RaftSettings.EnableEsp, "Enable ESP");
                    RaftSettings.Landmark = GUILayout.Toggle(RaftSettings.Landmark,
                        "Landmark [" + Mathf.Round(RaftSettings.FLandmark) + "m]");
                    RaftSettings.FLandmark =
                        Mathf.Round(GUILayout.HorizontalSlider(RaftSettings.FLandmark, 1f, 1000f) * 1000f) / 1000f;
                    RaftSettings.TradingPost = GUILayout.Toggle(RaftSettings.TradingPost,
                        "Trading Post [" + Mathf.Round(RaftSettings.FTradingPost) + "m]");
                    RaftSettings.FTradingPost =
                        Mathf.Round(GUILayout.HorizontalSlider(RaftSettings.FTradingPost, 1f, 500f) * 500f) / 500f;
                    RaftSettings.Treasures = GUILayout.Toggle(RaftSettings.Treasures,
                        "Treasures [" + Mathf.Round(RaftSettings.FTreasures) + "m]");
                    RaftSettings.FTreasures =
                        Mathf.Round(GUILayout.HorizontalSlider(RaftSettings.FTreasures, 1f, 500f) * 500f) / 500f;
                    RaftSettings.Item = GUILayout.Toggle(RaftSettings.Item, "Item ESP");
                    RaftSettings.ItemDefault = GUILayout.Toggle(RaftSettings.ItemDefault,
                        "Default Items [" + Mathf.Round(RaftSettings.FItemDefault) + "m]");
                    RaftSettings.FItemDefault =
                        Mathf.Round(GUILayout.HorizontalSlider(RaftSettings.FItemDefault, 1f, 150f) * 150f) / 150f;
                    RaftSettings.ItemDomesticAnimal = GUILayout.Toggle(RaftSettings.ItemDomesticAnimal,
                        "Domestic Animal Items [" + Mathf.Round(RaftSettings.FItemDomesticAnimal) + "m]");
                    RaftSettings.FItemDomesticAnimal =
                        Mathf.Round(GUILayout.HorizontalSlider(RaftSettings.FItemDomesticAnimal, 1f, 150f) * 150f) /
                        150f;
                    RaftSettings.ItemNoteBookNote = GUILayout.Toggle(RaftSettings.ItemNoteBookNote,
                        "Notebook Notes [" + Mathf.Round(RaftSettings.FItemNoteBookNote) + "m]");
                    RaftSettings.FItemNoteBookNote =
                        Mathf.Round(GUILayout.HorizontalSlider(RaftSettings.FItemNoteBookNote, 1f, 250f) * 250f) / 250f;
                    RaftSettings.ItemQuestItem = GUILayout.Toggle(RaftSettings.ItemQuestItem,
                        "Quest Items [" + Mathf.Round(RaftSettings.FItemQuestItem) + "m]");
                    RaftSettings.FItemQuestItem =
                        Mathf.Round(GUILayout.HorizontalSlider(RaftSettings.FItemQuestItem, 1f, 250f) * 250f) / 250f;
                    RaftSettings.HostileAnimal = GUILayout.Toggle(RaftSettings.HostileAnimal,
                        "Hostile Animals [" + Mathf.Round(RaftSettings.FHostileAnimal) + "m]");
                    RaftSettings.FHostileAnimal =
                        Mathf.Round(GUILayout.HorizontalSlider(RaftSettings.FHostileAnimal, 1f, 250f) * 250f) / 250f;
                    RaftSettings.FriendlyAnimal = GUILayout.Toggle(RaftSettings.FriendlyAnimal,
                        "Animals [" + Mathf.Round(RaftSettings.FFriendlyAnimal) + "m]");
                    RaftSettings.FFriendlyAnimal =
                        Mathf.Round(GUILayout.HorizontalSlider(RaftSettings.FFriendlyAnimal, 1f, 250f) * 250f) / 250f;
                    RaftSettings.Npc =
                        GUILayout.Toggle(RaftSettings.Npc, "NPC [" + Mathf.Round(RaftSettings.FNpc) + "m]");
                    RaftSettings.FNpc = Mathf.Round(GUILayout.HorizontalSlider(RaftSettings.FNpc, 1f, 200f) * 200f) /
                                        200f;
                    break;
                case 3:
                    RaftSettings.Fly = GUILayout.Toggle(RaftSettings.Fly, "Fly [F1]");
                    if (GUILayout.Button("Force Anchor")) ForceAnchor.AddAnchor();
                    if (GUILayout.Button("Unlock All")) Unlockables.UnlockAll();
                    if (GUILayout.Button("Unlock Achievements")) Unlockables.UnlockAchievements();
                    if (GUILayout.Button("Kill All Entities")) KillEntities.KillAllEntities();
                    if (GUILayout.Button("Kill All Enemies")) KillEntities.KillAllEnemies();
                    break;
            }

            GUI.DragWindow();
        }
    }
}