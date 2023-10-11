﻿using RaftHook.Utilities.Models;
using UnityEngine;

namespace RaftHook.Utilities
{
    public static class RaftClient
    {
        public static Network_Player LocalPlayer => SingletonGeneric<Network_Entity>.Singleton.Network.GetLocalPlayer();
        public static bool IsInLobby => LoadSceneManager.IsLoadingLobbyScene;
        public static bool IsInGame => LoadSceneManager.IsGameSceneLoaded;
        public static Sprite CheatLogo => GetItemSprite("Placeable_Anchor_Stationary_Advanced");

        public static void GiveItem(string itemName, int amount = 1)
        {
            var item = ItemManager.GetItemByName(itemName);
            if (item == null) return;

            LocalPlayer.Inventory.AddItem(item.UniqueName, amount);
        }

        public static void SendNotification(string message)
        {
            var notificationManager = ComponentManager<NotificationManager>.Value;
            if (notificationManager == null) return;

            (notificationManager.ShowNotification("QuestItem") as Notification_QuestItem)
                ?.infoQue.Enqueue(new RNotificationInfo(message));
        }

        private static Sprite GetItemSprite(string itemName)
        {
            var item = ItemManager.GetItemByName(itemName);
            return item.settings_Inventory?.Sprite;
        }
    }
}