#if MELONLOADER
using System.Collections.Generic;
using MelonLoader;
using RoomCode;

<<<<<<< HEAD
[assembly: MelonInfo(typeof(MelonLoaderMod), RoomCode.RoomCode.ModName, RoomCode.RoomCode.ModVersion, RoomCode.RoomCode.ModAuthor, "https://github.com/orendain/DemeoMods")]
=======
[assembly: MelonInfo(typeof(MelonLoaderMod), RoomCodeBase.ModName, RoomCodeBase.ModVersion, RoomCodeBase.ModAuthor, "https://github.com/orendain/DemeoMods")]
>>>>>>> main
[assembly: MelonGame("Resolution Games", "Demeo")]
[assembly: MelonGame("Resolution Games", "Demeo PC Edition")]
[assembly: MelonID("581308")]
[assembly: VerifyLoaderVersion("0.5.3", true)]

namespace RoomCode
{
    using MelonLoader;

    internal class MelonLoaderMod : MelonMod
    {
<<<<<<< HEAD
        internal MelonPreferences_Entry<bool> Enabled { get; private set; }

        internal MelonPreferences_Entry<List<string>> RoomCodes { get; private set; }
=======
        internal MelonPreferences_Entry<bool> Enabled { get; private set; } = new();

        internal MelonPreferences_Entry<List<string>> RoomCodes { get; private set; } = new();
>>>>>>> main

        public override void OnInitializeMelon()
        {
            LoadConfiguration();
<<<<<<< HEAD
            RoomCode.Init(this);
=======
            RoomCodeBase.Init(this);
>>>>>>> main
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
