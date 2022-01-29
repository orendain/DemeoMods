namespace Rules.Rule
{
    using System.Linq;
    using Boardgame.BoardEntities.Abilities;
    using UnityEngine;

    public sealed class BallistaAttackDamageAdjustedRule : RulesAPI.Rule
    {
        public override string Description => "Ballista attack damage is adjusted";

        private readonly int _attackDamage;
        private int _originalAttackDamage;

        public BallistaAttackDamageAdjustedRule(int attackDamage)
        {
            _attackDamage = attackDamage;
        }

        protected override void OnPostGameCreated()
        {
            var abilities = Resources.FindObjectsOfTypeAll<Ability>();
            var ability = abilities.First(c => c.name.Equals("TurretDamageProjectile(Clone)"));
            _originalAttackDamage = ability.abilityDamage.targetDamage;
            ability.abilityDamage.targetDamage = _attackDamage;
        }

        protected override void OnDeactivate()
        {
            var abilities = Resources.FindObjectsOfTypeAll<Ability>();
            var ability = abilities.First(c => c.name.Equals("TurretDamageProjectile(Clone)"));
            ability.abilityDamage.targetDamage = _originalAttackDamage;
        }
    }
}
