namespace RulesAPI
{
    using System;
    using System.Collections.Generic;

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

        public bool IsRegistered(Type ruleType)
        {
            return RuleTypes.Contains(ruleType);
        }

        public bool IsRegistered(Ruleset ruleset)
        {
            return Rulesets.Contains(ruleset);
        }

        // TODO(orendain): Disallow registration after a certain point in init process.
        public void Register(Type ruleType)
        {
            RulesAPI.Logger.Msg($"Registering rule type: {ruleType}");

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
            RulesAPI.Logger.Msg($"Registering ruleset: {ruleset.GetType()} (with {ruleset.Rules.Count} rules)");

            if (Rulesets.Contains(ruleset))
            {
                throw new ArgumentException("Ruleset already registered.");
            }

            foreach (var rule in ruleset.Rules)
            {
                if (!IsRegistered(rule.GetType()))
                {
                    throw new ArgumentException($"Ruleset includes unregistered rule type: {rule.GetType()}");
                }
            }

            Rulesets.Add(ruleset);
        }
    }
}
