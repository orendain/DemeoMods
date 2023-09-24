#if BEPINEX
namespace Highlighter
{
    using BepInEx;
    using HarmonyLib;

    [BepInPlugin(HighlighterCore.ModId, HighlighterCore.ModName, HighlighterCore.ModVersion)]
    public class BepInExPlugin : BaseUnityPlugin
    {
        internal Harmony Harmony { get; private set; }

        private void Awake()
        {
            Harmony = new Harmony(HighlighterCore.ModId);
            HighlighterCore.Init(this);
        }
    }
}
#endif
