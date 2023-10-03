#if MELONLOADER
using System.Collections.Generic;
using MelonLoader;
using RoomCode;

[assembly: MelonInfo(
    typeof(MelonLoaderMod),
    RoomCodeBase.ModName,
    RoomCodeBase.ModVersion,
    RoomCodeBase.ModAuthor,
    "https://github.com/orendain/DemeoMods")]
[assembly: MelonGame("Resolution Games", "Demeo")]
[assembly: MelonGame("Resolution Games", "Demeo PC Edition")]
[assembly: MelonID("581308")]
[assembly: VerifyLoaderVersion("0.5.3", true)]

namespace RoomCode
{
    using MelonLoader;

    internal class MelonLoaderMod : MelonMod
    {
        internal MelonPreferences_Entry<bool> Enabled { get; private set; } = new();

        internal MelonPreferences_Entry<List<string>> RoomCodes { get; private set; } = new();

        public override void OnInitializeMelon()
        {
            LoadConfiguration();
            RoomCodeBase.Init(this);
        }

        private void LoadConfiguration()
        {
            var configCategory = MelonPreferences.CreateCategory("RoomCode");
            Enabled = configCategory.CreateEntry("enabled", true);
            RoomCodes = configCategory.CreateEntry("codes", new List<string>());
        }
    }
}
#endif
