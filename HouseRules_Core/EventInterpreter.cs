namespace HouseRules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.SerializableEvents;
    using DataKeys;
    using HarmonyLib;

    internal class EventInterpreter
    {
        private readonly GameContext _gameContext;

        internal static EventInterpreter NewInstance(GameContext gameContext)
        {
            return new EventInterpreter(gameContext);
        }

        private EventInterpreter(GameContext gameContext)
        {
            _gameContext = gameContext;
        }

        public bool DoesEventRepresentNewSpawn(SerializableEvent serializableEvent)
        {
            switch (serializableEvent.type)
            {
                case SerializableEvent.Type.SpawnPiece:
                case SerializableEvent.Type.UpdateFogAndSpawn:
                case SerializableEvent.Type.SetBoardPieceID:
                case SerializableEvent.Type.SlimeFusion:
                case SerializableEvent.Type.GoToNextLevel:
                    return true;
            }

            if (serializableEvent.type == SerializableEvent.Type.OnAbilityUsed)
            {
                return DoesAbilityEventRepresentNewSpawn((SerializableEventOnAbilityUsed)serializableEvent);
            }

            if (serializableEvent.type == SerializableEvent.Type.PieceDied)
            {
                return DoesPieceDiedEventRepresentNewSpawn((SerializableEventPieceDied)serializableEvent);
            }

            return false;
        }

        private static bool DoesAbilityEventRepresentNewSpawn(SerializableEventOnAbilityUsed onAbilityUsedEvent)
        {
            var abilityKey = Traverse.Create(onAbilityUsedEvent).Field<AbilityKey>("abilityKey").Value;
            switch (abilityKey)
            {
                case AbilityKey.SummonElemental:
                case AbilityKey.SummonBossMinions:
                case AbilityKey.NaturesCall:
                case AbilityKey.Tornado:
                case AbilityKey.MonsterBait:
                case AbilityKey.ProximityMine:
                case AbilityKey.EyeOfAvalon:
                case AbilityKey.SwordOfAvalon:
                case AbilityKey.BeaconOfSmite:
                case AbilityKey.BeaconOfHealing:
                case AbilityKey.RaiseRoots:
                case AbilityKey.CallCompanion:
                    return true;
            }

            var abilityName = abilityKey.ToString();
            var isSpawnAbility = abilityName.Contains("Spawn");
            var isLampAbility = abilityName.Contains("Lamp");

            return isSpawnAbility || isLampAbility;
        }

        private bool DoesPieceDiedEventRepresentNewSpawn(SerializableEventPieceDied pieceDiedEvent)
        {
            foreach (var pieceId in pieceDiedEvent.deadPieces)
            {
                if (!_gameContext.pieceAndTurnController.TryGetPiece(pieceId, out Piece piece))
                {
                    continue;
                }

                if (piece.boardPieceId == BoardPieceId.SpiderEgg)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
