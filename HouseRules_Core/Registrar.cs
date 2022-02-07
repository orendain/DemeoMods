namespace HouseRules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Registrar
    {
        private static Registrar _instance;

        internal HashSet<Type> RuleTypes { get; }

        internal HashSet<Ruleset> Rulesets { get; }

        public static Registrar Instance()
        {
            if (_instance != null)
            {
                return _instance;
            }

            _instance = new Registrar();
            return _instance;
        }

        private Registrar()
        {
            RuleTypes = new HashSet<Type>();
            Rulesets = new HashSet<Ruleset>();
        }

        // TODO(orendain): Disallow registration after a certain point in init process.
        public void Register(Type ruleType)
        {
            HR.Logger.Msg($"Registering rule type: {ruleType}");

            if (RuleTypes.Contains(ruleType))
            {
                throw new ArgumentException("Rule type already registered.");
            }

            if (!typeof(Rule).IsAssignableFrom(ruleType))
            {
                throw new ArgumentException("Rule type incompatible.");
            }

            RuleTypes.Add(ruleType);
        }

        public void Register(Ruleset ruleset)
        {
            HR.Logger.Msg($"Registering ruleset: {ruleset.Name} (with {ruleset.Rules.Count} rules)");

            if (IsRulesetRegistered(ruleset.Name))
            {
                throw new ArgumentException("Ruleset with that name already registered.");
            }

            foreach (var rule in ruleset.Rules)
            {
                if (!IsRuleRegistered(rule.GetType()))
                {
                    throw new ArgumentException($"Ruleset includes unregistered rule type: {rule.GetType()}");
                }
            }

            var hasDuplicatePatchable = ruleset.Rules.Where(r => r is IPatchable)
                .GroupBy(r => r.GetType().Name)
                .Any(g => g.Count() > 1);
            if (hasDuplicatePatchable)
            {
                throw new ArgumentException("Ruleset must not include duplicate IPatchable rules.");
            }

            Rulesets.Add(ruleset);
        }

        private bool IsRuleRegistered(Type ruleType)
        {
            return RuleTypes.Contains(ruleType);
        }

        private bool IsRulesetRegistered(string ruleset)
        {
            return Rulesets.Any(r => string.Equals(r.Name, ruleset, StringComparison.OrdinalIgnoreCase));
        }
    }
}
