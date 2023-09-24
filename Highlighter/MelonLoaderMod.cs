#if MELONLOADER
using Highlighter;
using MelonLoader;

[assembly: MelonInfo(typeof(MelonLoaderMod), HighlighterCore.ModName, HighlighterCore.ModVersion, HighlighterCore.ModAuthor, "https://github.com/orendain/DemeoMods")]
[assembly: MelonGame("Resolution Games", "Demeo")]
[assembly: MelonGame("Resolution Games", "Demeo PC Edition")]
[assembly: VerifyLoaderVersion("0.5.3", true)]

namespace Highlighter
{
    internal class MelonLoaderMod : MelonMod
    {
        public override void OnInitializeMelon()
        {
            HighlighterCore.Init(this);
        }
    }
}
#endif
