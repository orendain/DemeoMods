namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.BoardEntities.Abilities;
    using Boardgame.GameplayEffects;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Core.Types;

    public sealed class PartyElectricityDamageOverriddenRule : Rule, IConfigWritable<bool>, IPatchable,
        IMultiplayerSafe
    {
        public override string Description => "Heroes can't hurt or stun each other with electricity damage";

        private static bool _isActivated;

        private readonly bool _adjustments;

        public PartyElectricityDamageOverriddenRule(bool adjustments)
        {
            _adjustments = adjustments;
        }

        public bool GetConfigObject() => _adjustments;

        protected override void OnActivate(GameContext gameContext)
        {
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

            var targetPiece = Traverse.Create(__instance).Field("targetPiece").GetValue<Piece>();
            var abilityContext = Traverse.Create(__instance).Field("abilityContext").GetValue<AbilityContext>();
            var damage = Traverse.Create(abilityContext).Field("damage").GetValue<Damage>();
            var attackerTarget = Traverse.Create(abilityContext).Field("attacker").GetValue<Target>();
            var attacker = Traverse.Create(attackerTarget).Field("piece").GetValue<Piece>();

            if (attacker.IsPlayer() && targetPiece.IsPlayer() && damage.HasTag(DamageTag.Electricity))
            {
                targetPiece.effectSink.SubtractHealth(0);
                var localizedText = Traverse.Create(damage).Method("GetLocalizedText", paramTypes: new[] { typeof(string), typeof(bool) }, arguments: new object[] { "Ui/pieceUi/notification/damage/noDamage", false }).GetValue<string>();
                Notification.ShowGoldenText(targetPiece, new Target(targetPiece).gameObject, localizedText);

                return false; // Don't run the original OnStarted method.
            }

            return true;
        }
    }
}
