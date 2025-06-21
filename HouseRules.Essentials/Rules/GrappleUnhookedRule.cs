namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.SerializableEvents;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Core.Types;

    public sealed class GrappleUnhookedRule : Rule, IConfigWritable<bool>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Grapple and throwing lamps can be used in the same turn";

        private static GameContext _gameContext;
        private static Context _context;
        private static bool _isActivated;

        public GrappleUnhookedRule(bool value)
        {
        }

        public bool GetConfigObject() => true;

        protected override void OnActivate(Context context)
        {
            _context = context;
            _gameContext = context.GameContext;
            _isActivated = true;
        }

        protected override void OnDeactivate(Context context)
        {
            _isActivated = false;
        }

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(SerializableEventQueue), "PayActionPointCost"),
                postfix: new HarmonyMethod(
                    typeof(GrappleUnhookedRule),
                    nameof(SerializableEventQueue_PayActionPointCost_Postfix)));
        }

        private static void SerializableEventQueue_PayActionPointCost_Postfix(SerializableEvent serializableEvent)
        {
            if (!_isActivated || serializableEvent.type != SerializableEvent.Type.EndAction)
            {
                return;
            }

            var pieceId = Traverse.Create(serializableEvent).Field<int>("pieceId").Value;
            if (pieceId == 0)
            {
                return;
            }

            Piece source = _gameContext.pieceAndTurnController.GetPiece(pieceId);
            if (source != null && source.IsPlayer() && source.boardPieceId == BoardPieceId.HeroBarbarian)
            {
                if (source.HasEffectState(EffectStateType.HasExplodingLamp))
                {
                    Inventory.Item value;
                    for (int i = 0; i < source.inventory.Items.Count; i++)
                    {
                        value = source.inventory.Items[i];
                        if (value.AbilityKey == AbilityKey.ExplodingOilLamp)
                        {
                            if (value.IsDisabled)
                            {
                                value.flags &= Inventory.ItemFlag.IsReplenishable;
                                source.inventory.Items[i] = value;
                                source.AddGold(0);
                            }
                        }
                        else if (value.AbilityKey == AbilityKey.ExplodingVortexLamp)
                        {
                            if (value.IsDisabled)
                            {
                                value.flags &= Inventory.ItemFlag.IsReplenishable;
                                source.inventory.Items[i] = value;
                                source.AddGold(0);
                            }
                        }
                        else if (value.AbilityKey == AbilityKey.ExplodingIceLamp)
                        {
                            if (value.IsDisabled)
                            {
                                value.flags &= Inventory.ItemFlag.IsReplenishable;
                                source.inventory.Items[i] = value;
                                source.AddGold(0);
                            }
                        }
                        else if (value.AbilityKey == AbilityKey.ExplodingGasLamp)
                        {
                            if (value.IsDisabled)
                            {
                                value.flags &= Inventory.ItemFlag.IsReplenishable;
                                source.inventory.Items[i] = value;
                                source.AddGold(0);
                            }
                        }
                        else if (value.AbilityKey == AbilityKey.ExplodingWaterLamp)
                        {
                            if (value.IsDisabled)
                            {
                                value.flags &= Inventory.ItemFlag.IsReplenishable;
                                source.inventory.Items[i] = value;
                                source.AddGold(0);
                            }
                        }
                    }
                }
            }

            return;
        }
    }
}
