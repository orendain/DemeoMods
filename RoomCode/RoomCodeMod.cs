namespace RoomCode
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BepInEx;
    using BepInEx.Logging;
    using HarmonyLib;

    [BepInPlugin("com.orendain.demeomods.roomcode", "RoomCode", "2.0.0")]
    public class RoomCodeMod : BaseUnityPlugin
    {
        internal static bool Enabled { get; private set; }

        internal static List<string> RoomCodes { get; private set; }

        internal static ManualLogSource Log { get; private set; }

        private void Awake()
        {
            Log = Logger;
            var harmony = new Harmony("com.orendain.demeomods.roomcode");
            ModPatcher.Patch(harmony);
            InitializeConfiguration();
        }

        private void InitializeConfiguration()
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
