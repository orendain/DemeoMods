namespace HouseRules.Essentials.Rules
{
    using System;
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.BoardEntities.Abilities;
    using Boardgame.Data;
    using Boardgame.GameplayEffects;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;
    using UnityEngine;

    public sealed class DontShockFriendsRule : Rule, IConfigWritable<bool>, IPatchable,
        IMultiplayerSafe
    {
        public override string Description => "Player on player electricity damage is zero.";

        private static bool _globalAdjustments;
        private static bool _isActivated;

        private readonly bool _adjustments;

        public DontShockFriendsRule(bool adjustments)
        {
            _adjustments = adjustments;
        }

        public bool GetConfigObject() => _adjustments;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalAdjustments = _adjustments;
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Damage), "DealDamage", parameters: new[] { typeof(Target), typeof(Damage), typeof(IntPoint2D), typeof(Target), typeof(PieceAndTurnController), typeof(BoardModel), typeof(OverkillController), typeof(bool) }),
                prefix: new HarmonyMethod(
                    typeof(DontShockFriendsRule),
                    nameof(Ability_DealDamage_Prefix)));
        }

        private static bool Ability_DealDamage_Prefix(Target target, Damage damage, Target attacker, ref int __result)
        {
            if (!_isActivated)
            {
                return true;
            }

            if (attacker.piece.IsPlayer() && target.piece.IsPlayer() && damage.HasTag(DamageTag.Electricity))
            {
                var localizedText = Traverse.Create(damage).Method("GetLocalizedText", paramTypes: new[] { typeof(string), typeof(bool) }, arguments: new object[] { "Ui/pieceUi/notification/damage/noDamage", false }).GetValue<string>();
                Notification.ShowGoldenText(target.gameObject, localizedText);
                __result = 0;
                return false;
            }

            return true;
        }
    }
}
