namespace Highlighter
{
    using BepInEx;
    using Common.Util;
    using HarmonyLib;

    [BepInPlugin(ModId, ModName, ModVersion)]
    public class HighlighterMod : BaseUnityPlugin
    {
        private const string ModId = "com.orendain.demeomods.highlighter";
        private const string ModName = "Highlighter";
        private const string ModVersion = PluginInfo.PLUGIN_VERSION;
        private const string ModAuthor = "DemeoMods Team";
        private const string ModDescription = "In-game highlighting for gameplay tips.";
        private const bool IsModNetworkCompatible = true;

        private void Awake()
        {
            var harmony = new Harmony("com.orendain.demeomods.highlighter");
            MoveHighlighter.Patch(harmony);
            DemeoModRegistry.RegisterMod(ModName, ModVersion, ModAuthor, ModDescription, IsModNetworkCompatible);
        }
    }
}
