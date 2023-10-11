using MelonLoader;
using RaftHook.Features;
using RaftHook.Features.Features.Miscellaneous;
using RaftHook.Features.Features.Self;
using RaftHook.Features.Features.Visuals;
using RaftHook.UI;
using RaftHook.UI.ItemSpawner;
using UnityEngine;

namespace RaftHook
{
    public class RaftHookMod : MelonMod
    {
        private static GameObject _gameObject;

        private static void Initialize()
        {
            MelonLogger.Msg("Initializing Raft Hook...");

            _gameObject = new GameObject
            {
                name = "RaftHook",
                hideFlags = HideFlags.HideAndDontSave
            };

            _gameObject.AddComponent<Menu>();
            _gameObject.AddComponent<ItemSpawner>();

            // Features
            _gameObject.AddComponent<NoHunger>();
            _gameObject.AddComponent<NoThirst>();
            _gameObject.AddComponent<UnlimitedHealth>();
            _gameObject.AddComponent<Esp>();
            _gameObject.AddComponent<FlyMode>();

            Object.DontDestroyOnLoad(_gameObject);

            MelonLogger.Msg("Raft Hook initialized!");
        }

        private static void Terminate()
        {
            MelonLogger.Msg("Terminating Raft Hook...");

            Object.Destroy(_gameObject);

            MelonLogger.Msg("Raft Hook terminated!");
        }

        public override void OnInitializeMelon()
        {
            Initialize();

            MelonLogger.Msg("Subscribing to events...");

            MelonEvents.OnUpdate.Subscribe(() =>
            {
                if (Input.GetKeyDown(KeyCode.Delete)) Terminate();
            });

            FeaturesPatcher.Patch();

            MelonLogger.Msg("Raft Hook loaded!");
        }

        public override void OnDeinitializeMelon()
        {
            Terminate();
            FeaturesPatcher.Unpatch();
            MelonLogger.Msg("Raft Hook unloaded!");
        }
    }
}