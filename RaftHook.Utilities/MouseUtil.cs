using UnityEngine;

namespace RaftHook.Utilities
{
    public abstract class MouseUtil
    {
        public static void ToggleCursor(bool state, bool lockMouseLook)
        {
            Helper.SetCursorLockState(state ? CursorLockMode.Confined : CursorLockMode.Locked);
            Helper.SetCursorVisible(state);
            ComponentManager<Network_Player>.Value.PlayerScript.SetLockMouseLook(lockMouseLook);
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