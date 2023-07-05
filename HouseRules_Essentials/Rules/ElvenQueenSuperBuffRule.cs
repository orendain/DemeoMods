namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.BoardEntities.Abilities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class ElvenQueenSuperBuffRule : Rule, IConfigWritable<bool>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "The Elven Queen gains a super buff... eventually";

        private static bool _globalAdjustments;
        private static bool _isActivated;

        private readonly bool _adjustments;

        internal static bool SuperBuff { get; private set; }

        public ElvenQueenSuperBuffRule(bool adjustments)
        {
            _adjustments = adjustments;
        }

        public bool GetConfigObject() => _adjustments;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalAdjustments = _adjustments;
            _isActivated = true;
            SuperBuff = true;
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            _isActivated = false;
            SuperBuff = false;
        }

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(Ability), "GenerateAttackDamage"),
                postfix: new HarmonyMethod(
                    typeof(ElvenQueenSuperBuffRule),
                    nameof(Ability_GenerateAttackDamage_Postfix)));
        }

        private static void Ability_GenerateAttackDamage_Postfix(Piece source)
        {
            if (!_isActivated || source.boardPieceId != BoardPieceId.ElvenQueen)
            {
                return;
            }

            // Elven Queen super buff to make her even more challenging
            if (source.GetHealth() < (source.GetMaxHealth() / 3))
            {
                source.EnableEffectState(EffectStateType.MagicShield);
                source.effectSink.SetStatusEffectDuration(EffectStateType.MagicShield, 69);
                source.EnableEffectState(EffectStateType.Courageous);
                source.effectSink.SetStatusEffectDuration(EffectStateType.Courageous, 69);
                return;
            }
        }
    }
}
