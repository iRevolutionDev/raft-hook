namespace RaftHook.Utilities
{
    public static class RaftClient
    {
        public static Network_Player LocalPlayer => SingletonGeneric<Network_Entity>.Singleton.Network.GetLocalPlayer();
        public static bool IsInLobby => LoadSceneManager.IsLoadingLobbyScene;
        public static bool IsInGame => LoadSceneManager.IsGameSceneLoaded;

        public static void GiveItem(string itemName, int amount = 1)
        {
            var item = ItemManager.GetItemByName(itemName);
            if (item == null) return;

            LocalPlayer.Inventory.AddItem(item.UniqueName, amount);
        }
    }
}