using System.Reflection;
using MelonLoader;

namespace RaftHook.Features
{
    public abstract class FeaturesPatcher
    {
        public static void Patch()
        {
            MelonLogger.Msg("Patching features...");

            HarmonyLib.Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());

            MelonLogger.Msg("Features patched!");
        }

        public static void Unpatch()
        {
            MelonLogger.Msg("UnPatching features...");

            HarmonyLib.Harmony.UnpatchAll();

            MelonLogger.Msg("Features unpatched!");
        }
    }
}