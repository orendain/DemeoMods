#if BEPINEX
namespace RoomCode
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using BepInEx;
    using BepInEx.Logging;
    using HarmonyLib;

    [BepInPlugin(RoomCode.ModId, RoomCode.ModName, RoomCode.ModVersion)]
    public class BepInExPlugin : BaseUnityPlugin
    {
        internal ManualLogSource Log { get; private set; }

        internal Harmony Harmony { get; private set; }

        internal bool Enabled { get; private set; }

        internal List<string> RoomCodes { get; private set; }

        private void Awake()
        {
            Log = Logger;
            Harmony = new Harmony(RoomCode.ModId);
            LoadConfiguration();
            RoomCode.Init(this);
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
