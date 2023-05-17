namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.BoardEntities.Abilities;
    using Boardgame.GameplayEffects;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class PartyElectricityDamageOverriddenRule : Rule, IConfigWritable<bool>, IPatchable,
        IMultiplayerSafe
    {
        public override string Description => "Player on player electricity damage is zero.";

        private static bool _globalAdjustments;
        private static bool _isActivated;

        private readonly bool _adjustments;

        public PartyElectricityDamageOverriddenRule(bool adjustments)
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
                original: AccessTools.Method(typeof(ProjectileHitSequence), "OnStarted"),
                prefix: new HarmonyMethod(
                    typeof(PartyElectricityDamageOverriddenRule),
                    nameof(ProjectileHitSequence_OnStarted_Prefix)));
        }

        private static bool ProjectileHitSequence_OnStarted_Prefix(ref ProjectileHitSequence __instance)
        {
            if (!_isActivated)
            {
                return true;
            }

            var abilityContext = Traverse.Create(__instance).Field("abilityContext").GetValue<AbilityContext>();
            var damage = Traverse.Create(abilityContext).Field("damage").GetValue<Damage>();
            if (!damage.HasTag(DamageTag.Electricity))
            {
                return true;
            }

            var attackerTarget = Traverse.Create(abilityContext).Field("attacker").GetValue<Target>();
            var attacker = Traverse.Create(attackerTarget).Field("piece").GetValue<Piece>();
            if (attacker.characterClass != CharacterClass.Sorcerer)
            {
                return true;
            }

            var targetPiece = Traverse.Create(__instance).Field("targetPiece").GetValue<Piece>();
            BoardPieceId targetId = targetPiece.boardPieceId;
            bool canBeHit = true;
            if (targetId == BoardPieceId.RootVine || targetId == BoardPieceId.ProximityMine || targetId == BoardPieceId.EnemyTurret || targetId == BoardPieceId.SporeFungus || targetId.ToString().Contains("SandPile") || targetPiece.HasPieceType(PieceType.ExplodingLamp))
            {
                canBeHit = false;
            }

            if (targetPiece.IsPlayer() || targetPiece.IsBot() || (targetPiece.IsProp() && canBeHit))
            {
                var localizedText = Traverse.Create(damage).Method("GetLocalizedText", paramTypes: new[] { typeof(string), typeof(bool) }, arguments: new object[] { "Ui/pieceUi/notification/damage/noDamage", false }).GetValue<string>();
                Notification.ShowGoldenText(targetPiece, new Target(targetPiece).gameObject, localizedText);
                targetPiece.effectSink.SubtractHealth(0);
                return false; // Don't run the original OnStarted method.
            }

            return true;
        }
    }
}
