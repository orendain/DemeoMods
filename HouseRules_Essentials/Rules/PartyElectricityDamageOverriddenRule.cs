namespace HouseRules.Essentials.Rules
{
    using System.IO;
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.BoardEntities.Abilities;
    using Boardgame.GameplayEffects;
    using Boardgame.SerializableEvents;
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
        private static GameContext _gameContext;

        public PartyElectricityDamageOverriddenRule(bool adjustments)
        {
            _adjustments = adjustments;
        }

        public bool GetConfigObject() => _adjustments;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalAdjustments = _adjustments;
            _isActivated = true;
            _gameContext = gameContext;
        }

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(ProjectileHitSequence), "OnStarted"),
                prefix: new HarmonyMethod(
                    typeof(PartyElectricityDamageOverriddenRule),
                    nameof(ProjectileHitSequence_OnStarted_Prefix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(SerializableEventQueue), "ValidateSerializableEvent"),
                prefix: new HarmonyMethod(
                    typeof(PartyElectricityDamageOverriddenRule),
                    nameof(SerializableEventQueue_ValidateSerializableEvent_Prefix)));
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
                var localizedText = Traverse.Create(damage).Method("GetLocalizedText", paramTypes: new[] { typeof(string), typeof(bool) }, arguments: new object[] { "Ui/pieceUi/notification/damage/noDamage", false }).GetValue<string>();
                Notification.ShowGoldenText(new Target(targetPiece).gameObject, localizedText);
                _gameContext.serializableEventQueue.SendEventRequest(new SerializeableEventSetStatusEffects(targetPiece.networkID, new EffectStateType[] { }, new EffectStateType[] { EffectStateType.Stunned }));

                return false; // Don't run the original OnStarted method.
            }

            return true;
        }

        private static bool SerializableEventQueue_ValidateSerializableEvent_Prefix(byte[] serializableEventData, ref SerializableEvent __result)
        {
            if (!_isActivated)
            {
                return true;
            }

            if (__result == null)
            {
                SerializableEvent action = SerializableEvent.Deserialize(new BinaryReader(new MemoryStream(serializableEventData)));
                if (action.type == SerializableEvent.Type.SetStatusEffects)
                {
                    // EssentialsMod.Logger.Msg($"Bypassing Validate for {action.type}");
                    __result = action;
                    return false; // Don't run the original OnStarted method.
                }
            }

            return true;
        }
    }
}
