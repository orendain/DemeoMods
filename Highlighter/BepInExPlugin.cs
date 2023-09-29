#if BEPINEX
namespace Highlighter
{
    using BepInEx;
    using HarmonyLib;

    [BepInPlugin(HighlighterBase.ModId, HighlighterBase.ModName, HighlighterBase.ModVersion)]
    internal class BepInExPlugin : BaseUnityPlugin
    {
        internal Harmony? Harmony { get; private set; }

        private void Awake()
        {
            Harmony = new Harmony(HighlighterBase.ModId);
            HighlighterBase.Init(this);
        }
    }
}
#endif
