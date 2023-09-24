#if MELONLOADER
using MelonLoader;
using RoomFinder;

[assembly: MelonInfo(typeof(MelonLoaderMod), RoomFinderCore.ModName, RoomFinderCore.ModVersion, RoomFinderCore.ModAuthor, "https://github.com/orendain/DemeoMods")]
[assembly: MelonGame("Resolution Games", "Demeo")]
[assembly: MelonGame("Resolution Games", "Demeo PC Edition")]
[assembly: MelonID("566788")]
[assembly: VerifyLoaderVersion("0.5.3", true)]

namespace RoomFinder
{
    internal class MelonLoaderMod : MelonMod
    {
        public override void OnInitializeMelon()
        {
            RoomFinderCore.Init(this);
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            RoomFinderCore.OnSceneLoaded(buildIndex);
        }
    }
}
#endif
