#if BEPINEX
namespace RoomCode
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
<<<<<<< HEAD

=======
>>>>>>> main
    using BepInEx;
    using BepInEx.Logging;
    using HarmonyLib;

<<<<<<< HEAD
    [BepInPlugin(RoomCode.ModId, RoomCode.ModName, RoomCode.ModVersion)]
    public class BepInExPlugin : BaseUnityPlugin
    {
        internal ManualLogSource Log { get; private set; }

        internal Harmony Harmony { get; private set; }

        internal bool Enabled { get; private set; }

        internal List<string> RoomCodes { get; private set; }
=======
    [BepInPlugin(RoomCodeBase.ModId, RoomCodeBase.ModName, RoomCodeBase.ModVersion)]
    internal class BepInExPlugin : BaseUnityPlugin
    {
        internal ManualLogSource? Log { get; private set; }

        internal Harmony? Harmony { get; private set; }

        internal bool Enabled { get; private set; }

        internal List<string> RoomCodes { get; private set; } = new();
>>>>>>> main

        private void Awake()
        {
            Log = Logger;
<<<<<<< HEAD
            Harmony = new Harmony(RoomCode.ModId);
            LoadConfiguration();
            RoomCode.Init(this);
=======
            Harmony = new Harmony(RoomCodeBase.ModId);
            LoadConfiguration();
            RoomCodeBase.Init(this);
>>>>>>> main
        }

        private void LoadConfiguration()
        {
            var enabledEntry = Config.Bind(
                "General",
                "Enabled",
                true,
                "Whether or not RoomCode is enabled.");

            var roomCodesEntry = Config.Bind(
                "General",
                "Codes",
                string.Empty,
                "Room codes to use, comma-seperated and ordered by preference.");

            Enabled = enabledEntry.Value;
            RoomCodes = roomCodesEntry.Value
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .ToList();
        }
    }
}
#endif
