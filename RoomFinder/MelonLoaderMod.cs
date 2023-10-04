#if MELONLOADER
using MelonLoader;
using RoomFinder;

[assembly: MelonInfo(
    typeof(MelonLoaderMod),
    RoomFinderBase.ModName,
    RoomFinderBase.ModVersion,
    RoomFinderBase.ModAuthor,
    "https://github.com/orendain/DemeoMods")]
[assembly: MelonGame("Resolution Games", "Demeo")]
[assembly: MelonGame("Resolution Games", "Demeo PC Edition")]
[assembly: MelonID("566788")]
[assembly: VerifyLoaderVersion("0.5.7", true)]

namespace RoomFinder
{
    internal class MelonLoaderMod : MelonMod
    {
        public override void OnInitializeMelon()
        {
            RoomFinderBase.Init(this);
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            RoomFinderBase.OnSceneLoaded(buildIndex);
        }
    }
}
#endif
