namespace RoomCode
{
    using System.Collections.Generic;
    using System.Linq;
    using MelonLoader;

    internal class RoomCodeMod : MelonMod
    {
        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("RoomCode");

        internal static bool Enabled { get; private set; }

        internal static List<string> RoomCodes { get; private set; }

        [System.Obsolete]
        public override void OnInitializeMelon()
        {
            ModPatcher.Patch(HarmonyInstance);
            InitializeConfiguration();
        }

        private static void InitializeConfiguration()
        {
            var configCategory = MelonPreferences.CreateCategory("RoomCode");
            var enabledEntry = configCategory.CreateEntry("enabled", true);
            var roomCodesEntry = configCategory.CreateEntry("codes", new List<string>());

            Enabled = enabledEntry.Value;
            RoomCodes = roomCodesEntry.Value;
            if (RoomCodes.Any())
            {
                Logger.Warning($"Custom Room Codes were found!");
            }
            else
            {
                Logger.Error("No custom Room Codes were found!");
            }
        }
    }
}
