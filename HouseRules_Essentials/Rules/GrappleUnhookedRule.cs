namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities.Abilities;
    using DataKeys;
    using HouseRules.Types;

    public sealed class GrappleUnhookedRule : Rule, IConfigWritable<bool>, IMultiplayerSafe
    {
        public override string Description => "Grapple and throwing lamps can be use in same turn.";

        public GrappleUnhookedRule(bool value)
        {
        }

        public bool GetConfigObject() => true;

        protected override void OnPreGameCreated(GameContext gameContext)
        {
            GrappleUnhooked();
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            GrappleRehooked();
        }

        private static void GrappleUnhooked()
        {
            AbilityFactory.TryGetAbility(AbilityKey.Grapple, out var grapple);
            grapple.effectAppliedToSelf = EffectStateType.It;
            AbilityFactory.TryGetAbility(AbilityKey.ExplodingIceLamp, out var launchIce);
            launchIce.effectAppliedToSelf = EffectStateType.It;
            AbilityFactory.TryGetAbility(AbilityKey.ExplodingOilLamp, out var launchFire);
            launchFire.effectAppliedToSelf = EffectStateType.It;
            AbilityFactory.TryGetAbility(AbilityKey.ExplodingVortexLamp, out var launchVortex);
            launchVortex.effectAppliedToSelf = EffectStateType.It;
            AbilityFactory.TryGetAbility(AbilityKey.ExplodingGasLamp, out var launchGas);
            launchGas.effectAppliedToSelf = EffectStateType.It;
            AbilityFactory.TryGetAbility(AbilityKey.ExplodingWaterLamp, out var launchWater);
            launchWater.effectAppliedToSelf = EffectStateType.It;
        }

        private static void GrappleRehooked()
        {
            AbilityFactory.TryGetAbility(AbilityKey.Grapple, out var grapple);
            grapple.effectAppliedToSelf = EffectStateType.UsedHookThisTurn;
            AbilityFactory.TryGetAbility(AbilityKey.ExplodingIceLamp, out var launchIce);
            launchIce.effectAppliedToSelf = EffectStateType.UsedHookThisTurn;
            AbilityFactory.TryGetAbility(AbilityKey.ExplodingOilLamp, out var launchFire);
            launchFire.effectAppliedToSelf = EffectStateType.UsedHookThisTurn;
            AbilityFactory.TryGetAbility(AbilityKey.ExplodingVortexLamp, out var launchVortex);
            launchVortex.effectAppliedToSelf = EffectStateType.UsedHookThisTurn;
            AbilityFactory.TryGetAbility(AbilityKey.ExplodingGasLamp, out var launchGas);
            launchGas.effectAppliedToSelf = EffectStateType.UsedHookThisTurn;
            AbilityFactory.TryGetAbility(AbilityKey.ExplodingWaterLamp, out var launchWater);
            launchWater.effectAppliedToSelf = EffectStateType.UsedHookThisTurn;

        }
    }
}
