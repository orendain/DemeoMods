namespace HouseRules.Essentials.Rules
{
    using System.IO;
    using Boardgame;
    using Boardgame.SerializableEvents;
    using HarmonyLib;
    using HouseRules.Core.Types;

    public sealed class LevelExitLockedUntilOneHeroRemainsRule :
        Rule, IConfigWritable<bool>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Level exit is locked until only one hero remains";

        private static bool _isActivated;
        private static GameContext _gameContext;

        public LevelExitLockedUntilOneHeroRemainsRule(bool enabled)
        {
        }

        public bool GetConfigObject() => true;

        protected override void OnActivate(Context context)
        {
            _gameContext = context.GameContext;
            _isActivated = true;
        }

        protected override void OnDeactivate(Context context) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(SerializableEventQueue), "ValidateSerializableEvent"),
                postfix: new HarmonyMethod(
                    typeof(LevelExitLockedUntilOneHeroRemainsRule),
                    nameof(SerializableEventQueue_ValidateSerializableEvent_Postfix)));
        }

        private static void SerializableEventQueue_ValidateSerializableEvent_Postfix(
            byte[] serializableEventData,
            ref SerializableEvent __result)
        {
            if (!_isActivated)
            {
                return;
            }

            // Demeo has already determined the event was invalid.
            if (__result == null)
            {
                return;
            }

            // Do not apply to the starting floor.
            if (MotherTracker.motherTrackerData.floorIndex == 0)
            {
                return;
            }

            var serializableEvent = SerializableEvent.Deserialize(
                new BinaryReader(new MemoryStream(serializableEventData)));
            var interactEvent = serializableEvent as SerializableEventInteract;
            if (interactEvent == null)
            {
                return;
            }

            var interactable = _gameContext.pieceAndTurnController.GetInteractableAtPosition(interactEvent.targetTile);
            if (interactable.type != Interactable.Type.LevelExit)
            {
                return;
            }

            if (IsPieceLastHeroAlive(interactEvent.pieceId))
            {
                return;
            }

            // Hero is not the last one alive. Disallow them from exiting.
            __result = null;
            GameUI.ShowCameraMessage("Only the last hero alive may exit the level.", 5);
            _gameContext.serializableEventQueue.SendEventRequest(
                new SerializableEventSnapToTile(interactEvent.pieceId));
        }

        private static bool IsPieceLastHeroAlive(int pieceId)
        {
            if (_gameContext.pieceAndTurnController.GetNumberOfPlayerPieces() > 1)
            {
                return false;
            }

            var lastPlayerPiece = _gameContext.pieceAndTurnController.GetPlayerPieces()[0];
            return pieceId == lastPlayerPiece.networkID;
        }
    }
}
