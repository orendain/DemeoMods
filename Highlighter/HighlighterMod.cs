namespace Highlighter
{
    using BepInEx;
    using HarmonyLib;

    [BepInPlugin("com.orendain.demeomods.highlighter", "Highlighter", "2.0.0")]
    public class HighlighterMod : BaseUnityPlugin
    {
        private void Awake()
        {
            var harmony = new Harmony("com.orendain.demeomods.highlighter");
            MoveHighlighter.Patch(harmony);
        }
    }
}
