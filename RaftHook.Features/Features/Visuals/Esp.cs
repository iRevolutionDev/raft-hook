using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RaftHook.Utilities;
using Steamworks;
using UnityEngine;

namespace RaftHook.Features.Features.Visuals
{
    public class Esp : MonoBehaviour
    {
        private static float _reiTimer;
        private static Camera _reiCamera;

        private static bool _isInGame = LoadSceneManager.IsGameSceneLoaded;

        private static Dictionary<CSteamID, Network_Player> _players = new Dictionary<CSteamID, Network_Player>();

        private static List<Landmark> _landmarks = new List<Landmark>();

        private static List<TradingPost> _tradingPosts = new List<TradingPost>();

        private static List<TreasurePoint> _treasurePoints = new List<TreasurePoint>();

        private static List<PickupItem> _item = new List<PickupItem>();

        private static List<AI_NetworkBehaviour_Animal> _ai = new List<AI_NetworkBehaviour_Animal>();

        private static List<Seagull> _seagulls = new List<Seagull>();

        private readonly List<AI_NetworkBehaviourType> _friendlyCheck = new List<AI_NetworkBehaviourType>
        {
            AI_NetworkBehaviourType.Llama,
            AI_NetworkBehaviourType.Goat,
            AI_NetworkBehaviourType.Chicken,
            AI_NetworkBehaviourType.Puffin,
            AI_NetworkBehaviourType.Dolphin,
            AI_NetworkBehaviourType.Whale,
            AI_NetworkBehaviourType.Turtle,
            AI_NetworkBehaviourType.Stingray
        };

        private readonly List<AI_NetworkBehaviourType> _hostileCheck = new List<AI_NetworkBehaviourType>
        {
            AI_NetworkBehaviourType.StoneBird,
            AI_NetworkBehaviourType.PufferFish,
            AI_NetworkBehaviourType.Boar,
            AI_NetworkBehaviourType.Rat,
            AI_NetworkBehaviourType.Shark,
            AI_NetworkBehaviourType.Bear,
            AI_NetworkBehaviourType.MamaBear,
            AI_NetworkBehaviourType.BugSwarm_Bee,
            AI_NetworkBehaviourType.Pig,
            AI_NetworkBehaviourType.StoneBird_Caravan,
            AI_NetworkBehaviourType.ButlerBot,
            AI_NetworkBehaviourType.Rat_Tangaroa,
            AI_NetworkBehaviourType.Boss_Varuna,
            AI_NetworkBehaviourType.AnglerFish,
            AI_NetworkBehaviourType.PolarBear,
            AI_NetworkBehaviourType.Roach,
            AI_NetworkBehaviourType.BirdPack,
            AI_NetworkBehaviourType.Hyena,
            AI_NetworkBehaviourType.HyenaBoss
        };

        private readonly List<AI_NetworkBehaviourType> _npcCheck = new List<AI_NetworkBehaviourType>
        {
            AI_NetworkBehaviourType.NPC_Annisa,
            AI_NetworkBehaviourType.NPC_Citra,
            AI_NetworkBehaviourType.NPC_Ika,
            AI_NetworkBehaviourType.NPC_Isac,
            AI_NetworkBehaviourType.NPC_Johan,
            AI_NetworkBehaviourType.NPC_Kartika,
            AI_NetworkBehaviourType.NPC_Larry,
            AI_NetworkBehaviourType.NPC_Max,
            AI_NetworkBehaviourType.NPC_Noah,
            AI_NetworkBehaviourType.NPC_Oliver,
            AI_NetworkBehaviourType.NPC_Timur,
            AI_NetworkBehaviourType.NPC_Toshiro,
            AI_NetworkBehaviourType.NPC_Ulla,
            AI_NetworkBehaviourType.NPC_Zayana,
            AI_NetworkBehaviourType.NPC_Vanessa
        };

        private void Update()
        {
            _reiCamera = Camera.main;
            _reiTimer += Time.deltaTime;
            _isInGame = LoadSceneManager.IsGameSceneLoaded;

            if (!(_reiTimer >= 5f)) return;

            _reiTimer = 0f;

            if (RaftSettings.Players) _players = RaftClient.Players;
            if (RaftSettings.Landmark) _landmarks = FindObjectsOfType<Landmark>().ToList();
            if (RaftSettings.TradingPost) _tradingPosts = FindObjectsOfType<TradingPost>().ToList();
            if (RaftSettings.Treasures) _treasurePoints = FindObjectsOfType<TreasurePoint>().ToList();
            if (RaftSettings.Item) _item = FindObjectsOfType<PickupItem>().ToList();
            if (!RaftSettings.HostileAnimal && !RaftSettings.FriendlyAnimal) return;

            _ai = FindObjectsOfType<AI_NetworkBehaviour_Animal>().ToList();
            _seagulls = FindObjectsOfType<Seagull>().ToList();
        }

        private void OnGUI()
        {
            if (!_isInGame) return;
            if (Event.current.type != EventType.Repaint) return;
            if (!RaftSettings.EnableEsp) return;

            if (RaftSettings.Players && _players.Count > 0)
            {
                var players = _players.Where(x => x.Value != null && x.Value.gameObject != null)
                    .Where(x => !x.Value.IsLocalPlayer)
                    .Where(x => x.Value.gameObject.activeSelf).ToList();

                foreach (var player in players)
                {
                    var position = player.Value.transform.position;
                    var vector = _reiCamera.WorldToScreenPoint(position);
                    var num = Vector3.Distance(
                        SingletonGeneric<Network_Entity>.Singleton.Network.GetLocalPlayer().transform.position,
                        position);

                    if (!Render.IsOnScreen(vector) || !(num < RaftSettings.FPlayers)) continue;

                    Render.DrawString(new Vector3(vector.x, Screen.height - vector.y),
                        player.Value.name, Color.white, true, 12, FontStyle.Normal);
                    Render.DrawString(new Vector3(vector.x, Screen.height - vector.y + 12f),
                        Mathf.Round(num) + "m", Color.yellow, true, 12, FontStyle.Normal);

                    if (!RaftSettings.PlayersBox) continue;

                    var head = player.Value.PlayerHeadBone.position;
                    var headVector = _reiCamera.WorldToScreenPoint(head);

                    var foot = player.Value.FeetPosition;
                    var footVector = _reiCamera.WorldToScreenPoint(foot);

                    Render.DrawBox(new Vector2(headVector.x, Screen.height - headVector.y),
                        footVector, 1f, Color.white);
                }
            }

            if (RaftSettings.Landmark && _landmarks.Count > 0)
                foreach (var landmark in _landmarks)
                    if (landmark)
                    {
                        var position = landmark.transform.position;
                        var vector = _reiCamera.WorldToScreenPoint(position);
                        var num = Vector3.Distance(
                            SingletonGeneric<Network_Entity>.Singleton.Network.GetLocalPlayer().transform.position,
                            position);

                        if (!Render.IsOnScreen(vector) || !(num < RaftSettings.FLandmark)) continue;

                        Render.DrawString(new Vector3(vector.x, Screen.height - vector.y),
                            GetIslandNames(landmark.name), Color.magenta, true, 12, FontStyle.Normal);
                        Render.DrawString(new Vector3(vector.x, Screen.height - vector.y + 12f),
                            Mathf.Round(num) + "m", Color.yellow, true, 12, FontStyle.Normal);
                    }

            if (RaftSettings.TradingPost && _tradingPosts.Count > 0)
                foreach (var tradingPost in _tradingPosts)
                    if (tradingPost)
                    {
                        var position = tradingPost.transform.position;
                        var vector2 = _reiCamera.WorldToScreenPoint(position);
                        var num2 = Vector3.Distance(
                            SingletonGeneric<Network_Entity>.Singleton.Network.GetLocalPlayer().transform.position,
                            position);

                        if (!Render.IsOnScreen(vector2) || !(num2 < RaftSettings.FTradingPost)) continue;

                        Render.DrawString(new Vector3(vector2.x, Screen.height - vector2.y), "Trading Post",
                            Color.blue, true, 12, FontStyle.Normal);
                        Render.DrawString(new Vector3(vector2.x, Screen.height - vector2.y + 12f),
                            Mathf.Round(num2) + "m", Color.yellow, true, 12, FontStyle.Normal);
                    }

            if (RaftSettings.Treasures && _treasurePoints.Count > 0)
                foreach (var treasurePoint in _treasurePoints)
                    if (treasurePoint)
                    {
                        var position = treasurePoint.transform.position;
                        var vector3 = _reiCamera.WorldToScreenPoint(position);
                        var num3 = Vector3.Distance(
                            SingletonGeneric<Network_Entity>.Singleton.Network.GetLocalPlayer().transform.position,
                            position);

                        if (!Render.IsOnScreen(vector3) || !(num3 < RaftSettings.FTreasures) ||
                            !treasurePoint.IsBuried) continue;

                        Render.DrawString(new Vector3(vector3.x, Screen.height - vector3.y),
                            GetTreasureNames(treasurePoint.name), Color.cyan, true, 12, FontStyle.Normal);
                        Render.DrawString(new Vector3(vector3.x, Screen.height - vector3.y + 12f),
                            Mathf.Round(num3) + "m", Color.yellow, true, 12, FontStyle.Normal);
                    }

            if (RaftSettings.Item && _item.Count > 0)
                foreach (var pickupItem in _item)
                    if (pickupItem)
                    {
                        var position = pickupItem.transform.position;
                        var vector4 = _reiCamera.WorldToScreenPoint(position);
                        var num4 = Vector3.Distance(
                            SingletonGeneric<Network_Entity>.Singleton.Network.GetLocalPlayer().transform.position,
                            position);

                        if (!Render.IsOnScreen(vector4) || !pickupItem.canBePickedUp) continue;

                        if (RaftSettings.ItemDefault && num4 < RaftSettings.FItemDefault &&
                            pickupItem.pickupItemType == PickupItemType.Default)
                        {
                            Render.DrawString(new Vector3(vector4.x, Screen.height - vector4.y),
                                pickupItem.PickupName, Color.white, true, 12, FontStyle.Normal);
                            Render.DrawString(new Vector3(vector4.x, Screen.height - vector4.y + 12f),
                                Mathf.Round(num4) + "m", Color.yellow, true, 12, FontStyle.Normal);
                        }

                        if (RaftSettings.ItemDomesticAnimal && num4 < RaftSettings.FItemDomesticAnimal &&
                            pickupItem.pickupItemType == PickupItemType.DomesticAnimal)
                        {
                            Render.DrawString(new Vector3(vector4.x, Screen.height - vector4.y),
                                pickupItem.PickupName, Color.green, true, 12, FontStyle.Normal);
                            Render.DrawString(new Vector3(vector4.x, Screen.height - vector4.y + 12f),
                                Mathf.Round(num4) + "m", Color.yellow, true, 12, FontStyle.Normal);
                        }

                        if (RaftSettings.ItemNoteBookNote && num4 < RaftSettings.FItemNoteBookNote &&
                            pickupItem.pickupItemType == PickupItemType.NoteBookNote)
                        {
                            Render.DrawString(new Vector3(vector4.x, Screen.height - vector4.y),
                                pickupItem.PickupName, Color.cyan, true, 12, FontStyle.Normal);
                            Render.DrawString(new Vector3(vector4.x, Screen.height - vector4.y + 12f),
                                Mathf.Round(num4) + "m", Color.yellow, true, 12, FontStyle.Normal);
                        }

                        if (!RaftSettings.ItemQuestItem || !(num4 < RaftSettings.FItemQuestItem) ||
                            pickupItem.pickupItemType != PickupItemType.QuestItem) continue;

                        Render.DrawString(new Vector3(vector4.x, Screen.height - vector4.y),
                            pickupItem.PickupName, Color.cyan, true, 12, FontStyle.Normal);
                        Render.DrawString(new Vector3(vector4.x, Screen.height - vector4.y + 12f),
                            Mathf.Round(num4) + "m", Color.yellow, true, 12, FontStyle.Normal);
                    }

            if (RaftSettings.HostileAnimal && _ai.Count > 0)
                foreach (var aiNetworkBehaviourAnimal in _ai)
                    if (aiNetworkBehaviourAnimal)
                    {
                        var position = aiNetworkBehaviourAnimal.transform.position;
                        var vector5 = _reiCamera.WorldToScreenPoint(position);
                        var num5 = Vector3.Distance(
                            SingletonGeneric<Network_Entity>.Singleton.Network.GetLocalPlayer().transform.position,
                            position);

                        if (!Render.IsOnScreen(vector5) || !(num5 < RaftSettings.FHostileAnimal) ||
                            !_hostileCheck.Contains(aiNetworkBehaviourAnimal.behaviourType)) continue;

                        Render.DrawString(new Vector3(vector5.x, Screen.height - vector5.y),
                            GetAnimalNames_Hostile(aiNetworkBehaviourAnimal.name), Color.red, true, 12,
                            FontStyle.Normal);
                        Render.DrawString(new Vector3(vector5.x, Screen.height - vector5.y + 12f),
                            Mathf.Round(num5) + "m", Color.yellow, true, 12, FontStyle.Normal);
                    }

            if (RaftSettings.HostileAnimal && _seagulls.Count > 0)
                foreach (var seagull in _seagulls)
                    if (seagull)
                    {
                        var position = seagull.transform.position;
                        var vector6 = _reiCamera.WorldToScreenPoint(position) + Vector3.up * 1f;
                        var num6 = Vector3.Distance(
                            SingletonGeneric<Network_Entity>.Singleton.Network.GetLocalPlayer().transform.position,
                            position);

                        if (!Render.IsOnScreen(vector6) || !(num6 < RaftSettings.FHostileAnimal)) continue;

                        Render.DrawString(new Vector3(vector6.x, Screen.height - vector6.y), "Seagull",
                            Color.red, true, 12, FontStyle.Normal);
                        Render.DrawString(new Vector3(vector6.x, Screen.height - vector6.y + 12f),
                            Mathf.Round(num6) + "m", Color.yellow, true, 12, FontStyle.Normal);
                    }

            if (RaftSettings.FriendlyAnimal && _ai.Count > 0)
                foreach (var aiNetworkBehaviourAnimal2 in _ai)
                    if (aiNetworkBehaviourAnimal2)
                    {
                        var position = aiNetworkBehaviourAnimal2.transform.position;
                        var vector7 = _reiCamera.WorldToScreenPoint(position);
                        var num7 = Vector3.Distance(
                            SingletonGeneric<Network_Entity>.Singleton.Network.GetLocalPlayer().transform.position,
                            position);

                        if (!Render.IsOnScreen(vector7) || !(num7 < RaftSettings.FFriendlyAnimal) ||
                            !_friendlyCheck.Contains(aiNetworkBehaviourAnimal2.behaviourType)) continue;

                        Render.DrawString(new Vector3(vector7.x, Screen.height - vector7.y),
                            GetAnimalNames_Friendly(aiNetworkBehaviourAnimal2.name), Color.green, true, 12,
                            FontStyle.Normal);
                        Render.DrawString(new Vector3(vector7.x, Screen.height - vector7.y + 12f),
                            Mathf.Round(num7) + "m", Color.yellow, true, 12, FontStyle.Normal);
                    }

            if (!RaftSettings.Npc || _ai.Count <= 0) return;
            {
                foreach (var aiNetworkBehaviourAnimal3 in _ai)
                    if (aiNetworkBehaviourAnimal3)
                    {
                        var position = aiNetworkBehaviourAnimal3.transform.position;
                        var vector8 = _reiCamera.WorldToScreenPoint(position);
                        var num8 = Vector3.Distance(
                            SingletonGeneric<Network_Entity>.Singleton.Network.GetLocalPlayer().transform.position,
                            position);
                        if (Render.IsOnScreen(vector8) && num8 < RaftSettings.FFriendlyAnimal &&
                            _npcCheck.Contains(aiNetworkBehaviourAnimal3.behaviourType))
                        {
                            Render.DrawString(new Vector3(vector8.x, Screen.height - vector8.y),
                                GetNPCNames(aiNetworkBehaviourAnimal3.name), Color.cyan, true, 12,
                                FontStyle.Normal);
                            Render.DrawString(new Vector3(vector8.x, Screen.height - vector8.y + 12f),
                                Mathf.Round(num8) + "m", Color.yellow, true, 12, FontStyle.Normal);
                        }
                    }
            }
        }

        public static string GetTreasureNames(string pText)
        {
            pText = pText.Replace("Pickup_Base_Treasure_Index0_Tier1(Clone)", "Buried Pile of Junk")
                .Replace("Pickup_Base_Treasure_Index1_Tier2(Clone)", "Buried Briefcase")
                .Replace("Pickup_Base_Treasure_Index2_Tier3(Clone)", "Buried Safe")
                .Replace("Pickup_Base_Treasure_Index3_TikiPiece1(Clone)", "Buried Tiki Piece 1")
                .Replace("Pickup_Base_Treasure_Index4_TikiPiece2(Clone)", "Buried Tiki Piece 2")
                .Replace("Pickup_Base_Treasure_Index5_TikiPiece3(Clone)", "Buried Tiki Piece 3")
                .Replace("Pickup_Base_Treasure_Index6_TikiPiece4(Clone)", "Buried Tiki Piece 4");
            return new Regex("\\([\\d-]\\)",
                    RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant)
                .Replace(pText, string.Empty);
        }

        public static string GetAnimalNames_Hostile(string pText)
        {
            pText = pText.Replace("AI_StoneBird(Clone)", "Screecher").Replace("AI_PufferFish(Clone)", "Poison Puffer")
                .Replace("AI_Boar(Clone)", "Warthog")
                .Replace("AI_Rat(Clone)", "Lurker")
                .Replace("AI_Shark(Clone)", "Shark")
                .Replace("AI_Bear(Clone)", "Bear")
                .Replace("AI_MamaBear(Clone)", "Mama Bear")
                .Replace("AI_BeeSwarm(Clone)", "Bee Swarm")
                .Replace("AI_Pig(Clone)", "Mudhog")
                .Replace("AI_StoneBird_Caravan(Clone)", "White Screecher")
                .Replace("AI_ButlerBot(Clone)", "Butler Bot")
                .Replace("AI_RatTangaroa(Clone)", "Tangaroa Lurker")
                .Replace("AI_Boss_Varuna(Clone)", "Rhino Shark")
                .Replace("AI_AnglerFish(Clone)", "Angler Fish")
                .Replace("AI_PolarBear(Clone)", "Polar Bear")
                .Replace("AI_Roach(Clone)", "Scuttler")
                .Replace("AI_BirdPack(Clone)", "Seagull")
                .Replace("AI_Hyena(Clone)", "Hyena")
                .Replace("AI_HyenaBoss(Clone)", "Hyena Boss");
            return new Regex("\\([\\d-]\\)",
                    RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant)
                .Replace(pText, string.Empty);
        }

        public static string GetAnimalNames_Friendly(string pText)
        {
            pText = pText.Replace("AI_Llama(Clone)", "Llama").Replace("AI_Goat(Clone)", "Goat")
                .Replace("AI_Chicken(Clone)", "Clucker")
                .Replace("AI_Puffin(Clone)", "Clucker")
                .Replace("AI_Dolphin(Clone)", "Dolphin")
                .Replace("AI_Whale(Clone)", "Whale")
                .Replace("AI_Turtle(Clone)", "Turtle")
                .Replace("AI_Stingray(Clone)", "Stingray");
            return new Regex("\\([\\d-]\\)",
                    RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant)
                .Replace(pText, string.Empty);
        }

        public static string GetNPCNames(string pText)
        {
            pText = pText.Replace("AI_NPC_Female_Annisa(Clone)", "Annisa")
                .Replace("AI_NPC_Female_Citra(Clone)", "Citra").Replace("AI_NPC_Female_Ika(Clone)", "Ika")
                .Replace("AI_NPC_Male_Isac(Clone)", "Isac")
                .Replace("AI_NPC_Male_Johan(Clone)", "Johan")
                .Replace("AI_NPC_Female_Kartika(Clone)", "Kartika")
                .Replace("AI_NPC_Male_Larry(Clone)", "Larry")
                .Replace("AI_NPC_Male_Max(Clone)", "Max")
                .Replace("AI_NPC_Male_Noah(Clone)", "Noah")
                .Replace("AI_NPC_Male_Oliver(Clone)", "Oliver")
                .Replace("AI_NPC_Male_Timur(Clone)", "Timur")
                .Replace("AI_NPC_Male_Toshiro(Clone)", "Toshiro")
                .Replace("AI_NPC_Female_Ulla(Clone)", "Ulla")
                .Replace("AI_NPC_Female_Zayana(Clone)", "Zayana")
                .Replace("AI_NPC_Female_Vanessa(Clone)", "Vanessa");
            return new Regex("\\([\\d-]\\)",
                    RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant)
                .Replace(pText, string.Empty);
        }

        // Token: 0x06000026 RID: 38 RVA: 0x00005C3C File Offset: 0x00003E3C
        public static string GetIslandNames(string pText)
        {
            pText = pText.Replace("15#Landmark_Raft#Floating raft", "Floating Raft")
                .Replace("16#Landmark_Raft#Floating raft", "Floating Raft")
                .Replace("17#Landmark_Raft#Floating raft", "Floating Raft")
                .Replace("18#Landmark_Raft#Floating raft", "Floating Raft")
                .Replace("19#Landmark_Radar#Big radio tower", "Big Radio Tower")
                .Replace("21#Landmark_Raft#Floating raft", "Floating Raft")
                .Replace("28#Landmark_Big#OG", "Big Island Original")
                .Replace("29#Landmark_Big#Cresent", "Big Island Cresent")
                .Replace("30#Landmark_Big#Twin peak", "Big Island Twin Peak")
                .Replace("31#Landmark_Pilot#", "Pilot Island")
                .Replace("32#Landmark_Big#", "Big Island")
                .Replace("33#Landmark_Boat#", "Boat Island")
                .Replace("34#Landmark_Small#1", "Small Island")
                .Replace("35#Landmark_Small#2", "Small Island")
                .Replace("36#Landmark_Small#3", "Small Island")
                .Replace("37#Landmark_Small#4", "Small Island")
                .Replace("38#Landmark_Small#5", "Small Island")
                .Replace("39#Landmark_Small#6", "Small Island")
                .Replace("40#Landmark_Small#7", "Small Island")
                .Replace("41#Landmark_Small#8", "Small Island")
                .Replace("42#Landmark_Small#9", "Small Island")
                .Replace("43#Landmark_Small#10", "Small Island")
                .Replace("44#Landmark_Vasagatan", "Vasagatan")
                .Replace("45#Landmark_BalboaIsland", "Balboa Island")
                .Replace("49#Landmark_CaravanIsland#RealDeal", "Caravan Island")
                .Replace("50#Landmark_Tangaroa#", "Tangaroa")
                .Replace("54#Landmark_VarunaPoint#", "Varuna Point")
                .Replace("55#Landmark_Temperance#", "Temperance")
                .Replace("56#Landmark_Utopia#", "Utopia");
            return new Regex("\\([\\d-]\\)",
                    RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant)
                .Replace(pText, string.Empty);
        }
    }
}