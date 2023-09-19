namespace Highlighter
{
    using MelonLoader;

    internal class HighlighterMod : MelonMod
    {
        public override void OnInitializeMelon()
        {
            MoveHighlighter.Patch(HarmonyInstance);
        }
    }
}
