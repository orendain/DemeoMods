namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.LevelLoading;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class PieceReplenishAbilityEveryLevelRule : Rule, IConfigWritable<Dictionary<BoardPieceId, AbilityKey>>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "A specific card per class is replenished at the start of new floors";

        private static Dictionary<BoardPieceId, AbilityKey> _globalAdjustments;
        private static bool _isActivated;

        public PieceReplenishAbilityEveryLevelRule(Dictionary<BoardPieceId, AbilityKey> adjustments)
        {
            _adjustments = adjustments;
        }

        private readonly Dictionary<BoardPieceId, AbilityKey> _adjustments;

        public Dictionary<BoardPieceId, AbilityKey> GetConfigObject() => _adjustments;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalAdjustments = _adjustments;
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(LevelManager), "RecreatePieceOnNewLevel"),
                postfix: new HarmonyMethod(
                    typeof(PieceReplenishAbilityEveryLevelRule),
                    nameof(CreatePiece_RecreatePieceOnNewLevel_Postfix)));
        }

        private static void CreatePiece_RecreatePieceOnNewLevel_Postfix(ref Piece __result)
        {
            if (!_isActivated || !__result.IsPlayer())
            {
                return;
            }

            if (!_globalAdjustments.ContainsKey(__result.boardPieceId))
            {
                return;
            }

            Inventory.Item value;
            foreach (var replacement in _globalAdjustments)
            {
                for (int i = 0; i < __result.inventory.Items.Count; i++)
                {
                    value = __result.inventory.Items[i];
                    if (value.abilityKey == replacement.Value)
                    {
                        if (value.IsReplenishing)
                        {
                            value.flags &= (Inventory.ItemFlag)(-3);
                            __result.inventory.Items[i] = value;
                            __result.AddGold(0);
                        }

                        break;
                    }
                }
            }
        }
    }
}
