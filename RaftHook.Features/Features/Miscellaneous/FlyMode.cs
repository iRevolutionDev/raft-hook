using RaftHook.Utilities;
using UnityEngine;

namespace RaftHook.Features.Features.Miscellaneous
{
    public class FlyMode : MonoBehaviour
    {
        private static bool _toggleFly;

        private void Update()
        {
            if (!RaftSettings.Fly) return;
            if (Input.GetKeyDown(KeyCode.F1)) _toggleFly = !_toggleFly;
            switch (_toggleFly)
            {
                case true:
                {
                    if (!SingletonGeneric<Network_Entity>.Singleton.Network.GetLocalPlayer().flightCamera
                            .enabled) Fly(true);
                    break;
                }
                case false when SingletonGeneric<Network_Entity>.Singleton.Network.GetLocalPlayer().flightCamera
                    .enabled:
                    Fly(false);
                    break;
            }
        }

        public void Fly(bool pToggle)
        {
            var localPlayer = RaftClient.LocalPlayer;

            switch (_toggleFly)
            {
                case true when pToggle &&
                               !localPlayer.flightCamera.enabled:
                    localPlayer.flightCamera.Enable(true);
                    return;
                case false when !pToggle &&
                                localPlayer.flightCamera.enabled:
                    localPlayer.flightCamera.Disable(true);
                    break;
            }
        }
    }
}