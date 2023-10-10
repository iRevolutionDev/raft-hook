using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MelonLoader;
using RaftHook.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Resources = RaftHook.UI.ItemSpawner.Properties.Resources;

namespace RaftHook.UI.ItemSpawner
{
    public class ItemSpawner : MonoBehaviour
    {
        private const KeyCode MenuKey = KeyCode.F8;

        public List<string> ignoredItems = new List<string>
        {
            ""
        };

        public bool itemsInitialized;

        private readonly Dictionary<string, GameObject> _gameItems = new Dictionary<string, GameObject>();

        private readonly List<AI_NetworkBehaviourType> _navmeshAnimals = new List<AI_NetworkBehaviourType>
        {
            AI_NetworkBehaviourType.Boar,
            AI_NetworkBehaviourType.Rat,
            AI_NetworkBehaviourType.Bear,
            AI_NetworkBehaviourType.MamaBear,
            AI_NetworkBehaviourType.Pig,
            AI_NetworkBehaviourType.ButlerBot,
            AI_NetworkBehaviourType.Rat_Tangaroa
        };

        private AssetBundle _asset;
        private int _currentAmount;
        private Item_Base _currentItem;
        private string _currentTab = "GameItems";

        private TMP_InputField _gameAmountInputField;
        private string _gameItemsSearchTerm;
        private bool _isShown;

        private GameObject _itemPrefabEntry;
        private Canvas _menu;
        private TMP_InputField _moddedAmountInputField;
        private string _moddedItemsSearchTerm;

        public void Start()
        {
            MelonLogger.Msg("Starting ItemSpawner...");
            StartCoroutine(StartMod());
        }

        public void Update()
        {
            if (!RaftClient.IsInGame || !_asset) return;
            if (Input.GetKeyDown(MenuKey) || (Input.GetKeyDown(KeyCode.Escape) && _isShown))
            {
                _isShown = !_isShown;
                SelectTab(_currentTab);
                _menu.GetComponent<CanvasGroup>().alpha = _isShown ? 1 : 0;
                //menu.GetComponent<CanvasGroup>().interactable = true;
                _menu.GetComponent<CanvasGroup>().blocksRaycasts = _isShown;
                MouseUtil.ToggleCursor(_isShown);
            }

            if (!_isShown) return;
            if (!_moddedAmountInputField || !_gameAmountInputField) return;

            if (_currentAmount <= 0)
            {
                _currentAmount = 1;
                _moddedAmountInputField.text = _currentAmount.ToString();
                _gameAmountInputField.text = _currentAmount.ToString();
            }

            if (_moddedAmountInputField.text == "") _moddedAmountInputField.text = _currentAmount.ToString();

            if (_gameAmountInputField.text == "") _gameAmountInputField.text = _currentAmount.ToString();
        }

        private IEnumerator StartMod()
        {
            MelonLogger.Msg("Checking assets...");

            AssetBundle.GetAllLoadedAssetBundles().ToList().ForEach(assetBundle =>
            {
                if (assetBundle.name == "itemspawner.assets") assetBundle.Unload(true);
            });

            MelonLogger.Msg("Loading assets...");

            var request =
                AssetBundle.LoadFromMemoryAsync(Resources.itemspawner);
            yield return request;

            MelonLogger.Msg("Assets loaded!");

            _asset = request.assetBundle;

            var menuGameObject = Instantiate(_asset.LoadAsset<GameObject>("ItemSpawnerCanvas"));
            DontDestroyOnLoad(menuGameObject);

            _menu = menuGameObject.GetComponent<Canvas>();

            _menu.transform.Find("Background").Find("Tabs").Find("GameItems").GetComponent<Button>().onClick
                .AddListener(() => SelectTab("GameItems"));
            _menu.transform.Find("Background").Find("Menus").Find("GameItems").Find("Searchbar")
                .GetComponent<TMP_InputField>().onValueChanged.AddListener(t =>
                {
                    _gameItemsSearchTerm = t;
                    UpdateItemsSearch();
                });
            _menu.transform.Find("Background").Find("Tabs").Find("ModdedItems").GetComponent<Button>().onClick
                .AddListener(() => SelectTab("ModdedItems"));
            _menu.transform.Find("Background").Find("Menus").Find("ModdedItems").Find("Searchbar")
                .GetComponent<TMP_InputField>().onValueChanged.AddListener(t =>
                {
                    _moddedItemsSearchTerm = t;
                    InitModdedItemsTab();
                });

            _menu.transform.Find("Background").Find("Topbar").Find("CloseButton").GetComponent<Button>().onClick
                .AddListener(() =>
                {
                    _isShown = false;
                    _menu.GetComponent<CanvasGroup>().alpha = _isShown ? 1 : 0;
                    //menu.GetComponent<CanvasGroup>().interactable = isShown;
                    _menu.GetComponent<CanvasGroup>().blocksRaycasts = _isShown;
                    MouseUtil.ToggleCursor(_isShown);
                });

            _gameAmountInputField = _menu.transform.Find("Background").Find("Menus").Find("GameItems")
                .Find("GiveCategory").Find("AmountInputField").GetComponent<TMP_InputField>();
            _moddedAmountInputField = _menu.transform.Find("Background").Find("Menus").Find("ModdedItems")
                .Find("GiveCategory").Find("AmountInputField").GetComponent<TMP_InputField>();

            _gameAmountInputField.onValueChanged.AddListener(t => { _currentAmount = int.Parse(t); });
            _moddedAmountInputField.onValueChanged.AddListener(t => { _currentAmount = int.Parse(t); });

            _menu.transform.Find("Background").Find("Menus").Find("ModdedItems").Find("GiveCategory").Find("GiveButton")
                .GetComponent<Button>().onClick.AddListener(() => GiveItem(_currentItem, _currentAmount));

            _menu.transform.Find("Background").Find("Menus").Find("ModdedItems").Find("GiveCategory")
                .Find("SpawnButton").GetComponent<Button>().onClick.AddListener(() =>
                {
                    SpawnItem(_currentItem, _currentAmount);
                });

            _menu.transform.Find("Background").Find("Menus").Find("GameItems").Find("GiveCategory").Find("GiveButton")
                .GetComponent<Button>().onClick.AddListener(() => { GiveItem(_currentItem, _currentAmount); });

            _menu.transform.Find("Background").Find("Menus").Find("GameItems").Find("GiveCategory").Find("SpawnButton")
                .GetComponent<Button>().onClick.AddListener(() => { SpawnItem(_currentItem, _currentAmount); });

            _menu.transform.Find("Background").Find("Menus").Find("ModdedItems").Find("GiveCategory")
                .Find("StackButton").GetComponent<Button>().onClick.AddListener(() =>
                {
                    _currentAmount = _currentItem.settings_Inventory.StackSize;
                    _moddedAmountInputField.text = _currentAmount.ToString();
                });

            _menu.transform.Find("Background").Find("Menus").Find("GameItems").Find("GiveCategory").Find("StackButton")
                .GetComponent<Button>().onClick.AddListener(() =>
                {
                    _currentAmount = _currentItem.settings_Inventory.StackSize;
                    _gameAmountInputField.text = _currentAmount.ToString();
                });

            _menu.transform.Find("Background").Find("Menus").Find("ModdedItems").Find("GiveCategory").Find("-Button")
                .GetComponent<Button>().onClick.AddListener(() =>
                {
                    _currentAmount = int.Parse(_menu.transform.Find("Background").Find("Menus").Find("ModdedItems")
                        .Find("GiveCategory").Find("AmountInputField").GetComponent<TMP_InputField>().text) - 1;
                    _moddedAmountInputField.text = _currentAmount.ToString();
                });

            _menu.transform.Find("Background").Find("Menus").Find("ModdedItems").Find("GiveCategory").Find("+Button")
                .GetComponent<Button>().onClick.AddListener(() =>
                {
                    _currentAmount = int.Parse(_menu.transform.Find("Background").Find("Menus").Find("ModdedItems")
                        .Find("GiveCategory").Find("AmountInputField").GetComponent<TMP_InputField>().text) + 1;
                    _moddedAmountInputField.text = _currentAmount.ToString();
                });

            _menu.transform.Find("Background").Find("Menus").Find("GameItems").Find("GiveCategory").Find("-Button")
                .GetComponent<Button>().onClick.AddListener(() =>
                {
                    _currentAmount = int.Parse(_menu.transform.Find("Background").Find("Menus").Find("GameItems")
                        .Find("GiveCategory").Find("AmountInputField").GetComponent<TMP_InputField>().text) - 1;
                    _gameAmountInputField.text = _currentAmount.ToString();
                });

            _menu.transform.Find("Background").Find("Menus").Find("GameItems").Find("GiveCategory").Find("+Button")
                .GetComponent<Button>().onClick.AddListener(() =>
                {
                    _currentAmount = int.Parse(_menu.transform.Find("Background").Find("Menus").Find("GameItems")
                        .Find("GiveCategory").Find("AmountInputField").GetComponent<TMP_InputField>().text) + 1;
                    _gameAmountInputField.text = _currentAmount.ToString();
                });

            _menu.transform.Find("Background").Find("Tabs").Find("Animals").GetComponent<Button>().onClick
                .AddListener(() => SelectTab("Animals"));
            _menu.transform.Find("Background").Find("Tabs").Find("Islands").GetComponent<Button>().onClick
                .AddListener(() => SelectTab("Islands"));
            _itemPrefabEntry = _asset.LoadAsset<GameObject>("ItemSlot");
            var cb = _itemPrefabEntry.GetComponent<Button>().colors;
            cb.disabledColor = new Color(53 / 255f, 168 / 255f, 223 / 255f);
            _itemPrefabEntry.GetComponent<Button>().colors = cb;
            SelectTab("GameItems");

            _menu.transform.Find("Background").Find("Menus").Find("Animals").Find("KillButton").GetComponent<Button>()
                .onClick.AddListener(() =>
                {
                    var array = FindObjectsOfType<Network_Entity>();
                    Network_Entity currentEntity = null;
                    var currentDistance = 999999f;
                    foreach (var networkEntity in array)
                    {
                        if (networkEntity.entityType == EntityType.Player || networkEntity.IsDead) continue;

                        var distance = Vector3.Distance(RaftClient.LocalPlayer.transform.position,
                            networkEntity.transform.position);

                        if (!(distance < currentDistance)) continue;

                        currentDistance = distance;
                        currentEntity = networkEntity;
                    }

                    if (currentEntity != null)
                        try
                        {
                            var currentEntityTransform = currentEntity.transform;
                            ComponentManager<Network_Host>.Value.DamageEntity(currentEntity, currentEntityTransform,
                                9999f, currentEntityTransform.position, Vector3.up, EntityType.Player);

                            var npc = currentEntity.GetComponent<AI_NetworkBehaviour_NPC>();
                            if (currentEntity.GetComponent<AI_NetworkBehaviour_NPC>()) Destroy(npc.gameObject);

                            if (!currentEntity.GetComponent<AI_NetworkBehaviour_Boss_Varuna>()) return;
                            {
                                var boss =
                                    currentEntity.GetComponent<AI_NetworkBehaviour_Boss_Varuna>();
                                Destroy(boss.gameObject);
                            }

                            if (!currentEntity.GetComponent<AI_NetworkBehaviour_HyenaBoss>()) return;
                            {
                                var boss =
                                    currentEntity.GetComponent<AI_NetworkBehaviour_HyenaBoss>();
                                Destroy(boss.gameObject);
                            }
                        }
                        catch
                        {
                            MelonLogger.Msg("Failed to kill nearest entity!");
                        }
                });

            _menu.transform.Find("Background").Find("Menus").Find("Islands").Find("RemoveButton").GetComponent<Button>()
                .onClick.AddListener(() =>
                {
                    ComponentManager<ChunkManager>.Value.ClearAllChunkPoints();
                    //FindObjectOfType<HNotify>().AddNotification(HNotify.NotificationType.normal, "Successfully cleared all landmarks!", 3, HNotify.CheckSprite);
                });

            InitAnimalsTab();
            InitIslandsTab();

            _menu.GetComponent<CanvasGroup>().alpha = 0;
            //menu.GetComponent<CanvasGroup>().interactable = false;
            _menu.GetComponent<CanvasGroup>().blocksRaycasts = false;

            Debug.Log("ItemSpawner has been loaded!");

            InitGameItemsTab();
        }

        public void InitAnimalsTab()
        {
            var content = _menu.transform.Find("Background").Find("Menus").Find("Animals").Find("ScrollView")
                .Find("Viewport").Find("Content");

            var currentlySupported = new List<AI_NetworkBehaviourType>();

            content.Find("Shark").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnAnimal(AI_NetworkBehaviourType.Shark));
            currentlySupported.Add(AI_NetworkBehaviourType.Shark);
            content.Find("Seagull").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnSeagull());
            content.Find("StoneBird").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnAnimal(AI_NetworkBehaviourType.StoneBird));
            currentlySupported.Add(AI_NetworkBehaviourType.StoneBird);
            content.Find("Pufferfish").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnAnimal(AI_NetworkBehaviourType.PufferFish));
            currentlySupported.Add(AI_NetworkBehaviourType.PufferFish);
            content.Find("Llama").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnAnimal(AI_NetworkBehaviourType.Llama));
            currentlySupported.Add(AI_NetworkBehaviourType.Llama);
            content.Find("Goat").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnAnimal(AI_NetworkBehaviourType.Goat));
            currentlySupported.Add(AI_NetworkBehaviourType.Goat);
            content.Find("Chicken").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnAnimal(AI_NetworkBehaviourType.Chicken));
            currentlySupported.Add(AI_NetworkBehaviourType.Chicken);
            content.Find("Boar").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnAnimal(AI_NetworkBehaviourType.Boar));
            currentlySupported.Add(AI_NetworkBehaviourType.Boar);
            content.Find("Rat").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnAnimal(AI_NetworkBehaviourType.Rat));
            currentlySupported.Add(AI_NetworkBehaviourType.Rat);
            content.Find("Bear").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnAnimal(AI_NetworkBehaviourType.Bear));
            currentlySupported.Add(AI_NetworkBehaviourType.Bear);
            content.Find("MamaBear").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnAnimal(AI_NetworkBehaviourType.MamaBear));
            currentlySupported.Add(AI_NetworkBehaviourType.MamaBear);
            content.Find("BugSwarm").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnAnimal(AI_NetworkBehaviourType.BugSwarm_Bee));
            currentlySupported.Add(AI_NetworkBehaviourType.BugSwarm_Bee);

            content.Find("Boss_Varuna").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnAnimal(AI_NetworkBehaviourType.Boss_Varuna));
            currentlySupported.Add(AI_NetworkBehaviourType.Boss_Varuna);
            content.Find("Anglerfish").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnAnimal(AI_NetworkBehaviourType.AnglerFish));
            currentlySupported.Add(AI_NetworkBehaviourType.AnglerFish);
            content.Find("PolarBear").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnAnimal(AI_NetworkBehaviourType.PolarBear));
            currentlySupported.Add(AI_NetworkBehaviourType.PolarBear);
            content.Find("Roach").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnAnimal(AI_NetworkBehaviourType.Roach));
            currentlySupported.Add(AI_NetworkBehaviourType.Roach);
            content.Find("Puffin").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnAnimal(AI_NetworkBehaviourType.Puffin));
            currentlySupported.Add(AI_NetworkBehaviourType.Puffin);
            content.Find("Hyena").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnAnimal(AI_NetworkBehaviourType.Hyena));
            currentlySupported.Add(AI_NetworkBehaviourType.Hyena);
            content.Find("HyenaBoss").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnAnimal(AI_NetworkBehaviourType.HyenaBoss));
            currentlySupported.Add(AI_NetworkBehaviourType.HyenaBoss);

            // SHITTY NPC'S
            content.Find("NPC_ANNISA").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnAnimal(AI_NetworkBehaviourType.NPC_Annisa));
            currentlySupported.Add(AI_NetworkBehaviourType.NPC_Annisa);

            content.Find("NPC_CITRA").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnAnimal(AI_NetworkBehaviourType.NPC_Citra));
            currentlySupported.Add(AI_NetworkBehaviourType.NPC_Citra);

            content.Find("NPC_IKA").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnAnimal(AI_NetworkBehaviourType.NPC_Ika));
            currentlySupported.Add(AI_NetworkBehaviourType.NPC_Ika);

            content.Find("NPC_ISAC").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnAnimal(AI_NetworkBehaviourType.NPC_Isac));
            currentlySupported.Add(AI_NetworkBehaviourType.NPC_Isac);

            content.Find("NPC_JOHAN").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnAnimal(AI_NetworkBehaviourType.NPC_Johan));
            currentlySupported.Add(AI_NetworkBehaviourType.NPC_Johan);

            content.Find("NPC_KARTIKA").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnAnimal(AI_NetworkBehaviourType.NPC_Kartika));
            currentlySupported.Add(AI_NetworkBehaviourType.NPC_Kartika);

            content.Find("NPC_LARRY").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnAnimal(AI_NetworkBehaviourType.NPC_Larry));
            currentlySupported.Add(AI_NetworkBehaviourType.NPC_Larry);

            content.Find("NPC_MAX").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnAnimal(AI_NetworkBehaviourType.NPC_Max));
            currentlySupported.Add(AI_NetworkBehaviourType.NPC_Max);

            content.Find("NPC_NOAH").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnAnimal(AI_NetworkBehaviourType.NPC_Noah));
            currentlySupported.Add(AI_NetworkBehaviourType.NPC_Noah);

            content.Find("NPC_OLIVER").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnAnimal(AI_NetworkBehaviourType.NPC_Oliver));
            currentlySupported.Add(AI_NetworkBehaviourType.NPC_Oliver);

            content.Find("NPC_TIMUR").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnAnimal(AI_NetworkBehaviourType.NPC_Timur));
            currentlySupported.Add(AI_NetworkBehaviourType.NPC_Timur);

            content.Find("NPC_TOSHIRO").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnAnimal(AI_NetworkBehaviourType.NPC_Toshiro));
            currentlySupported.Add(AI_NetworkBehaviourType.NPC_Toshiro);

            content.Find("NPC_ULLA").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnAnimal(AI_NetworkBehaviourType.NPC_Ulla));
            currentlySupported.Add(AI_NetworkBehaviourType.NPC_Ulla);

            content.Find("NPC_ZAYANA").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnAnimal(AI_NetworkBehaviourType.NPC_Zayana));
            currentlySupported.Add(AI_NetworkBehaviourType.NPC_Zayana);

            content.Find("NPC_VANESSA").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnAnimal(AI_NetworkBehaviourType.NPC_Vanessa));
            currentlySupported.Add(AI_NetworkBehaviourType.NPC_Vanessa);

            var animalPrefab = _asset.LoadAsset<GameObject>("AnimalSlot");
            foreach (AI_NetworkBehaviourType animal in typeof(AI_NetworkBehaviourType).GetEnumValues())
            {
                if (animal == AI_NetworkBehaviourType.None || animal == AI_NetworkBehaviourType.TEST) continue;

                if (!currentlySupported.Contains(animal))
                {
                    var t = Instantiate(animalPrefab, content);
                    t.name = animal.ToString();

                    var sprite = _asset.LoadAsset<Sprite>(animal.ToString());
                    if (sprite != null) t.transform.Find("Icon").GetComponent<Image>().sprite = sprite;

                    if (animal == AI_NetworkBehaviourType.StoneBird_Caravan)
                        t.transform.Find("TopBar").Find("Label").GetComponent<TextMeshProUGUI>().fontSize = 15f;

                    t.transform.Find("TopBar").Find("Label").GetComponent<TextMeshProUGUI>().text = animal.ToString();
                    t.transform.Find("SpawnButton").GetComponent<Button>().onClick
                        .AddListener(() => SpawnAnimal(animal));
                }
            }
        }

        public void InitIslandsTab()
        {
            var content = _menu.transform.Find("Background").Find("Menus").Find("Islands").Find("ScrollView")
                .Find("Viewport").Find("Content");

            var currentlySupported = new List<ChunkPointType>();

            content.Find("SmallIsland").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnLandmark(ChunkPointType.Landmark_Small));
            currentlySupported.Add(ChunkPointType.Landmark_Small);
            content.Find("LargeIsland").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnLandmark(ChunkPointType.Landmark_Big));
            currentlySupported.Add(ChunkPointType.Landmark_Big);
            content.Find("FloatingRaft").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnLandmark(ChunkPointType.Landmark_FloatingRaft));
            currentlySupported.Add(ChunkPointType.Landmark_FloatingRaft);
            content.Find("BalboaIsland").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnLandmark(ChunkPointType.Landmark_Balboa));
            currentlySupported.Add(ChunkPointType.Landmark_Balboa);
            content.Find("BoatIsland").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnLandmark(ChunkPointType.Landmark_Boat));
            currentlySupported.Add(ChunkPointType.Landmark_Boat);
            content.Find("PilotIsland").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnLandmark(ChunkPointType.Landmark_Pilot));
            currentlySupported.Add(ChunkPointType.Landmark_Pilot);
            content.Find("RadioTower").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnLandmark(ChunkPointType.Landmark_RadioTower));
            currentlySupported.Add(ChunkPointType.Landmark_RadioTower);
            content.Find("Vasagatan").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnLandmark(ChunkPointType.Landmark_Vasagatan));
            currentlySupported.Add(ChunkPointType.Landmark_Vasagatan);
            content.Find("Varunapoint").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnLandmark(ChunkPointType.Landmark_VarunaPoint));
            currentlySupported.Add(ChunkPointType.Landmark_VarunaPoint);
            content.Find("Temperance").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnLandmark(ChunkPointType.Landmark_Temperance));
            currentlySupported.Add(ChunkPointType.Landmark_Temperance);
            content.Find("Utopia").Find("SpawnButton").GetComponent<Button>().onClick
                .AddListener(() => SpawnLandmark(ChunkPointType.Landmark_Utopia));
            currentlySupported.Add(ChunkPointType.Landmark_Utopia);

            var islandPrefab = _asset.LoadAsset<GameObject>("IslandSlot");
            foreach (ChunkPointType cpt in typeof(ChunkPointType).GetEnumValues())
            {
                if (cpt == ChunkPointType.None || cpt == ChunkPointType.Landmark_Test) continue;

                if (currentlySupported.Contains(cpt)) continue;

                var t = Instantiate(islandPrefab, content);
                t.name = cpt.ToString();
                var sprite = _asset.LoadAsset<Sprite>(cpt.ToString().ToLower().Replace("landmark_", ""));
                if (sprite != null) t.transform.Find("Icon").GetComponent<Image>().sprite = sprite;

                t.transform.Find("TopBar").Find("Label").GetComponent<TextMeshProUGUI>().text =
                    cpt.ToString().ToLower().Replace("landmark_", "");
                t.transform.Find("SpawnButton").GetComponent<Button>().onClick
                    .AddListener(() => SpawnLandmark(cpt));
            }
        }

        public void SpawnLandmark(ChunkPointType cpt)
        {
            if (!Raft_Network.IsHost) return;

            var ruleFromPointType = ComponentManager<ChunkManager>.Value.GetRuleFromPointType(cpt);
            if (ruleFromPointType)
            {
                int value;

                switch (cpt)
                {
                    case ChunkPointType.Landmark_Balboa:
                        value = 400;
                        break;
                    case ChunkPointType.None:
                    case ChunkPointType.Landmark_Small:
                    case ChunkPointType.Landmark_Big:
                    case ChunkPointType.Landmark_Pilot:
                    case ChunkPointType.Landmark_RadioTower:
                    case ChunkPointType.Landmark_FloatingRaft:
                    case ChunkPointType.Landmark_Boat:
                    case ChunkPointType.Landmark_Test:
                    case ChunkPointType.Landmark_Vasagatan:
                    case ChunkPointType.Landmark_CaravanIsland:
                    case ChunkPointType.Landmark_Tangaroa:
                    case ChunkPointType.Landmark_VarunaPoint:
                    case ChunkPointType.Landmark_Temperance:
                    case ChunkPointType.Landmark_Utopia:
                    default:
                        throw new ArgumentOutOfRangeException(nameof(cpt), cpt, null);
                }

                ComponentManager<ChunkManager>.Value.AddChunkPointCheat(cpt, Raft.direction * value);
                MelonLogger.Msg("Landmark successfully spawned!");
            }
            else
            {
                MelonLogger.Msg("This island is in the game but isn't fully implemented currently!");
            }
        }

        public void SpawnSeagull()
        {
            GameModeValueManager.GetCurrentGameModeValue().seagullVariables.shouldSpawn = true;
            var playerTransform = RaftClient.LocalPlayer.transform;
            var spawnPos = playerTransform.position + playerTransform.forward * 2;
            ComponentManager<Network_Host_Entities>.Value.CreateSeagull(spawnPos, SaveAndLoad.GetUniqueObjectIndex(),
                SaveAndLoad.GetUniqueObjectIndex(), NetworkUpdateManager.GetUniqueBehaviourIndex());
            ComponentManager<SeagullParent>.Value.currentSeagullCount++;
        }

        public void SpawnAnimal(AI_NetworkBehaviourType animal)
        {
            if (!Raft_Network.IsHost) return;

            var localPlayerTransform = RaftClient.LocalPlayer.transform;
            var spawnPos = localPlayerTransform.position +
                           localPlayerTransform.forward * 2;

            NavMeshHit hit;
            var foundNavmesh = NavMesh.SamplePosition(spawnPos, out hit, 5, NavMesh.AllAreas);
            if (_navmeshAnimals.Contains(animal))
            {
                if (foundNavmesh)
                {
                    var b =
                        ComponentManager<Network_Host_Entities>.Value.CreateAINetworkBehaviour(animal, hit.position);
                    (b as AI_NetworkBehaviour_Domestic)?.QuickTameLate();
                }
                else
                {
                    MelonLogger.Msg("This animal can only be spawned on an island!");
                }
            }
            else
            {
                var b =
                    ComponentManager<Network_Host_Entities>.Value.CreateAINetworkBehaviour(animal, spawnPos);
                (b as AI_NetworkBehaviour_Domestic)?.QuickTameLate();
            }
        }


        public void GiveItem(Item_Base item, int amount)
        {
            RaftClient.LocalPlayer.Inventory.AddItem(item.UniqueName, amount);
        }

        public void SpawnItem(Item_Base item, int amount)
        {
            var player = RaftClient.LocalPlayer;
            Helper.DropItem(new ItemInstance(item, amount, item.MaxUses), player.transform.position,
                player.CameraTransform.forward, player.transform.ParentedToRaft());
        }

        public void SelectTab(string tabName)
        {
            _currentTab = tabName;
            switch (tabName)
            {
                case "GameItems":
                    ResetGameItemsCurrentItem();
                    _gameItemsSearchTerm = "";
                    _menu.transform.Find("Background").Find("Menus").Find("GameItems").Find("Searchbar")
                        .GetComponent<TMP_InputField>().text = "";
                    _menu.transform.Find("Background").Find("Menus").Find("GameItems").Find("Searchbar")
                        .GetComponent<TMP_InputField>().OnDeselect(null);
                    break;
                case "ModdedItems":
                    ResetModdedItemsCurrentItem();
                    _moddedItemsSearchTerm = "";
                    _menu.transform.Find("Background").Find("Menus").Find("ModdedItems").Find("Searchbar")
                        .GetComponent<TMP_InputField>().text = "";
                    _menu.transform.Find("Background").Find("Menus").Find("ModdedItems").Find("Searchbar")
                        .GetComponent<TMP_InputField>().OnDeselect(null);
                    InitModdedItemsTab();
                    break;
            }

            foreach (Transform t in _menu.transform.Find("Background").Find("Menus"))
            {
                var c = t.GetComponent<CanvasGroup>();
                if (!c)
                    c = t.gameObject.AddComponent<CanvasGroup>();

                c.alpha = t.transform.name == tabName ? 1 : 0;
                c.blocksRaycasts = t.transform.name == tabName;
                //c.interactable = t.transform.name == name ? true : false;
                t.gameObject.SetActive(true);
            }
        }

        public void UpdateItemsSearch()
        {
            var search = _gameItemsSearchTerm.ToLower();
            _gameItems.ToList().ForEach(x => { x.Value.SetActive(x.Key.Contains(_gameItemsSearchTerm.ToLower())); });
        }

        private List<Item_Base> GetItems()
        {
            var invalid = ItemManager.GetItemByIndex(375).settings_Inventory.Sprite;

            var items = ItemManager.GetAllItems().Where(x => x.UniqueIndex <= 1000
                                                             && !ignoredItems.Contains(
                                                                 x.UniqueName.ToLower())
                                                             && !x.UniqueName.StartsWith("Block_")
                                                             && x.settings_Inventory.Sprite != invalid
                                                             && x.settings_Inventory.Sprite != null)
                .ToList();

            // Sort items by HungerYield and ThirstYield
            var sortedItems = items.OrderByDescending(x => x.settings_consumeable.HungerYield)
                .ThenByDescending(x => x.settings_consumeable.ThirstYield)
                .ThenBy(x => -x.settings_buildable.GetBlockPrefabs().Length)
                .ToList();

            // Separate blueprints from the sorted items
            var blueprints = sortedItems.Where(x => x.settings_recipe.IsBlueprint).ToList();
            var nonBlueprintItems = sortedItems.Where(x => !x.settings_recipe.IsBlueprint).ToList();

            // Concatenate non-blueprint items and blueprints
            var result = nonBlueprintItems.Concat(blueprints).ToList();

            return result;
        }

        public void InitGameItemsTab()
        {
            if (itemsInitialized) return;

            itemsInitialized = true;
            _gameItems.Clear();
            ResetGameItemsCurrentItem();
            var content = _menu.transform.Find("Background").Find("Menus").Find("GameItems")
                .Find("ScrollView").Find("Viewport").Find("Content").gameObject;

            foreach (Transform t in content.transform) Destroy(t.gameObject);

            var items = GetItems();

            items.ForEach(item =>
            {
                var displayName = (item.UniqueIndex + "#" + item.settings_Inventory.DisplayName).ToLower();
                if (_gameItems.ContainsKey(displayName)) return;

                var itemInstance = Instantiate(_itemPrefabEntry, content.transform);
                _gameItems.Add(displayName, itemInstance);
                itemInstance.transform.Find("Icon").GetComponent<Image>().sprite =
                    item.settings_Inventory.Sprite;
                var btn = itemInstance.GetComponent<Button>();
                btn.onClick.AddListener(() =>
                {
                    foreach (Transform t in content.transform) t.GetComponent<Button>().interactable = true;

                    itemInstance.GetComponent<Button>().interactable = false;
                    SelectGameItemsCurrentItem(item);
                });
            });
            UpdateItemsSearch();
        }

        public void SelectGameItemsCurrentItem(Item_Base item)
        {
            _currentItem = item;
            _currentAmount = 1;
            _gameAmountInputField.text = _currentAmount.ToString();
            _menu.transform.Find("Background").Find("Menus").Find("GameItems").Find("GiveCategory").Find("Name")
                    .GetComponent<TextMeshProUGUI>().text =
                "Name : <color=#bdc3c7>" + item.settings_Inventory.DisplayName + "</color>";
            _menu.transform.Find("Background").Find("Menus").Find("GameItems").Find("GiveCategory").Find("Description")
                    .GetComponent<TextMeshProUGUI>().text =
                "Description : <color=#bdc3c7>" + item.settings_Inventory.Description + "</color>";
            _menu.transform.Find("Background").Find("Menus").Find("GameItems").Find("GiveCategory").Find("StackSize")
                    .GetComponent<TextMeshProUGUI>().text = "Stack Size : <color=#bdc3c7>" +
                                                            item.settings_Inventory.StackSize + "</color>";
            _menu.transform.Find("Background").Find("Menus").Find("GameItems").Find("GiveCategory").Find("Sprite")
                .GetComponent<Image>().sprite = item.settings_Inventory.Sprite;
            _menu.transform.Find("Background").Find("Menus").Find("GameItems").Find("GiveCategory")
                .GetComponent<CanvasGroup>().interactable = true;
        }

        public void ResetGameItemsCurrentItem()
        {
            var content = _menu.transform.Find("Background").Find("Menus").Find("GameItems").Find("ScrollView")
                .Find("Viewport").Find("Content").gameObject;
            foreach (Transform t in content.transform) t.GetComponent<Button>().interactable = true;

            _currentItem = null;
            _menu.transform.Find("Background").Find("Menus").Find("GameItems").Find("GiveCategory").Find("Name")
                .GetComponent<TextMeshProUGUI>().text = "Name : <color=#bdc3c7>Select an item...</color>";
            _menu.transform.Find("Background").Find("Menus").Find("GameItems").Find("GiveCategory").Find("Description")
                .GetComponent<TextMeshProUGUI>().text = "Description : <color=#bdc3c7>Select an item...</color>";
            _menu.transform.Find("Background").Find("Menus").Find("GameItems").Find("GiveCategory").Find("StackSize")
                .GetComponent<TextMeshProUGUI>().text = "Stack Size : <color=#bdc3c7>Select an item...</color>";
            _menu.transform.Find("Background").Find("Menus").Find("GameItems").Find("GiveCategory").Find("Sprite")
                .GetComponent<Image>().sprite = ItemManager.GetItemByIndex(236).settings_Inventory.Sprite;
            _menu.transform.Find("Background").Find("Menus").Find("GameItems").Find("GiveCategory")
                .GetComponent<CanvasGroup>().interactable = false;
        }

        public void InitModdedItemsTab()
        {
            ResetModdedItemsCurrentItem();
            var content = _menu.transform.Find("Background").Find("Menus").Find("ModdedItems").Find("ScrollView")
                .Find("Viewport").Find("Content").gameObject;
            foreach (Transform t in content.transform) Destroy(t.gameObject);

            foreach (var item in ItemManager.GetAllItems())
            {
                if (item.UniqueIndex <= 1000) continue;
                if (!item.settings_Inventory.DisplayName.ToLower().Contains(_moddedItemsSearchTerm.ToLower())) continue;

                var itemInstance = Instantiate(_itemPrefabEntry, content.transform);
                itemInstance.transform.Find("Icon").GetComponent<Image>().sprite =
                    item.settings_Inventory.Sprite;
                itemInstance.GetComponent<Button>().onClick.AddListener(() =>
                {
                    foreach (Transform t in content.transform) t.GetComponent<Button>().interactable = true;

                    itemInstance.GetComponent<Button>().interactable = false;
                    SelectModdedItemsCurrentItem(item);
                });
            }
        }

        public void SelectModdedItemsCurrentItem(Item_Base item)
        {
            _currentItem = item;
            _currentAmount = 1;
            _moddedAmountInputField.text = _currentAmount.ToString();
            _menu.transform.Find("Background").Find("Menus").Find("ModdedItems").Find("GiveCategory").Find("Name")
                    .GetComponent<TextMeshProUGUI>().text =
                "Name : <color=#bdc3c7>" + item.settings_Inventory.DisplayName + "</color>";
            _menu.transform.Find("Background").Find("Menus").Find("ModdedItems").Find("GiveCategory")
                    .Find("Description").GetComponent<TextMeshProUGUI>().text = "Description : <color=#bdc3c7>" +
                item.settings_Inventory.Description +
                "</color>";
            _menu.transform.Find("Background").Find("Menus").Find("ModdedItems").Find("GiveCategory").Find("StackSize")
                    .GetComponent<TextMeshProUGUI>().text = "Stack Size : <color=#bdc3c7>" +
                                                            item.settings_Inventory.StackSize + "</color>";
            _menu.transform.Find("Background").Find("Menus").Find("ModdedItems").Find("GiveCategory").Find("Sprite")
                .GetComponent<Image>().sprite = item.settings_Inventory.Sprite;
            _menu.transform.Find("Background").Find("Menus").Find("ModdedItems").Find("GiveCategory")
                .GetComponent<CanvasGroup>().interactable = true;
        }

        public void ResetModdedItemsCurrentItem()
        {
            var content = _menu.transform.Find("Background").Find("Menus").Find("ModdedItems").Find("ScrollView")
                .Find("Viewport").Find("Content").gameObject;

            foreach (Transform t in content.transform) t.GetComponent<Button>().interactable = true;

            _currentItem = null;
            _menu.transform.Find("Background").Find("Menus").Find("ModdedItems").Find("GiveCategory").Find("Name")
                .GetComponent<TextMeshProUGUI>().text = "Name : <color=#bdc3c7>Select an item...</color>";
            _menu.transform.Find("Background").Find("Menus").Find("ModdedItems").Find("GiveCategory")
                    .Find("Description").GetComponent<TextMeshProUGUI>().text =
                "Description : <color=#bdc3c7>Select an item...</color>";
            _menu.transform.Find("Background").Find("Menus").Find("ModdedItems").Find("GiveCategory").Find("StackSize")
                .GetComponent<TextMeshProUGUI>().text = "Stack Size : <color=#bdc3c7>Select an item...</color>";
            _menu.transform.Find("Background").Find("Menus").Find("ModdedItems").Find("GiveCategory").Find("Sprite")
                .GetComponent<Image>().sprite = ItemManager.GetItemByIndex(236).settings_Inventory.Sprite;
            _menu.transform.Find("Background").Find("Menus").Find("ModdedItems").Find("GiveCategory")
                .GetComponent<CanvasGroup>().interactable = false;
        }

        public void OnModUnload()
        {
            if (_asset)
                _asset.Unload(true);
            Debug.Log("ItemSpawner has been unloaded!");
        }
    }
}