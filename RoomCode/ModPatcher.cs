namespace RoomCode
{
    using System.Linq;
    using HarmonyLib;

    internal static class ModPatcher
    {
        private static int _roomCodeAttempts;

        internal static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(NetworkRoomUtils), "GetRandomRoomCode"),
                prefix: new HarmonyMethod(typeof(ModPatcher), nameof(NetworkRoomUtils_GetRandomRoomCode_Prefix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(CreatingGameState), "OnJoinedRoom"),
                prefix: new HarmonyMethod(typeof(ModPatcher), nameof(CreatingGameState_OnJoinedRoom_Prefix)));
        }

        private static bool NetworkRoomUtils_GetRandomRoomCode_Prefix(ref string __result)
        {
            if (!RoomCodeMod.Enabled)
            {
                return true;
            }

            if (!RoomCodeMod.RoomCodes.Any())
            {
                return true;
            }

            if (_roomCodeAttempts >= RoomCodeMod.RoomCodes.Count)
            {
                RoomCodeMod.Logger.Msg("All proposed room codes unavailable.");
                return true;
            }

            __result = RoomCodeMod.RoomCodes.ElementAt(_roomCodeAttempts);
            ++_roomCodeAttempts;

            RoomCodeMod.Logger.Msg($"Proposing room code: {__result}");
            return false;
        }

        private static void CreatingGameState_OnJoinedRoom_Prefix()
        {
            _roomCodeAttempts = 0;
        }
    }
}
