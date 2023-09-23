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
            if (!RoomCode.Enabled)
            {
                return true;
            }

            if (!RoomCode.RoomCodes.Any())
            {
                return true;
            }

            if (_roomCodeAttempts >= RoomCode.RoomCodes.Count)
            {
                RoomCode.LogInfo("All proposed room codes unavailable.");
                return true;
            }

            __result = RoomCode.RoomCodes.ElementAt(_roomCodeAttempts);
            ++_roomCodeAttempts;

            RoomCode.LogDebug($"Proposing room code: {__result}");
            return false;
        }

        private static void CreatingGameState_OnJoinedRoom_Prefix()
        {
            _roomCodeAttempts = 0;
        }
    }
}
