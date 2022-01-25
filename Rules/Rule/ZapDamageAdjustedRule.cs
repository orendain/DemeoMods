namespace Rules.Rule
{
    using System.Linq;
    using Boardgame.BoardEntities.Abilities;
    using UnityEngine;

    public sealed class ZapDamageAdjustedRule : RulesAPI.Rule
    {
        public override string Description => "Zap damage is adjusted";

        private readonly int _damage;
        private int _originalDamage;
        private int _originalCritDamage;

        public ZapDamageAdjustedRule(int damage)
        {
            _damage = damage;
        }

        protected override void OnActivate()
        {
            var abilities = Resources.FindObjectsOfTypeAll<Ability>();
            var ability = abilities.First(c => c.name.Equals("Zap(Clone)"));
            _originalDamage = ability.abilityDamage.targetDamage;
            _originalCritDamage = ability.abilityDamage.critDamage;
            ability.abilityDamage.targetDamage = _damage;
            ability.abilityDamage.critDamage = _damage * 2;
        }

        protected override void OnDeactivate()
        {
            var abilities = Resources.FindObjectsOfTypeAll<Ability>();
            var ability = abilities.First(c => c.name.Equals("Zap(Clone)"));
            ability.abilityDamage.targetDamage = _originalDamage;
            ability.abilityDamage.critDamage = _originalCritDamage;
        }
    }
}
