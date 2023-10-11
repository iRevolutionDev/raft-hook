using UnityEngine;

namespace RaftHook.Utilities
{
    public abstract class MouseUtil
    {
        private static void ToggleCursor(bool state, bool lockMouseLook)
        {
            Helper.SetCursorVisibleAndLockState(state, lockMouseLook ? CursorLockMode.None : CursorLockMode.Locked);
            ComponentManager<Network_Player>.Value.PlayerScript.SetLockMouseLook(lockMouseLook);
            CanvasHelper.ActiveMenu = state ? MenuType.PauseMenu : MenuType.None;
        }

        public static void ToggleCursor(bool state)
        {
            ToggleCursor(state, state);
        }

        public static void ToggleCursor()
        {
            Helper.SetCursorLockState(Cursor.lockState == CursorLockMode.Locked
                ? CursorLockMode.Confined
                : CursorLockMode.Locked);
            Helper.SetCursorVisible(Cursor.lockState == CursorLockMode.Locked);
            ComponentManager<Network_Player>.Value.PlayerScript.SetLockMouseLook(Cursor.lockState !=
                CursorLockMode.Locked);
        }
    }
}