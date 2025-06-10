namespace Highlighter
{
    internal static class HighlighterBase
    {
        internal const string ModId = "com.orendain.demeomods.highlighter";
        internal const string ModName = "Highlighter";
        internal const string ModVersion = "2.0.0";
        internal const string ModAuthor = "DemeoMods Team";

        internal static void Init(object loader)
        {
            #if BEPINEX
            if (loader is BepInExPlugin plugin)
            {
                if (plugin.Harmony == null)
                {
                    return;
                }

                MoveHighlighter.Patch(plugin.Harmony);
            }
            #endif

            #if MELONLOADER
            if (loader is MelonLoaderMod mod)
            {
                MoveHighlighter.Patch(mod.HarmonyInstance);
            }
            #endif
        }
    }
}
