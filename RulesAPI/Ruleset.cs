namespace RulesAPI
{
    using System;
    using System.Collections.Generic;

    public abstract class Ruleset
    {
        /// <summary>
        /// Gets the description of the ruleset.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Gets the rules of the ruleset.
        /// </summary>
        protected internal abstract HashSet<Rule> Rules { get; }

        internal void Activate()
        {
            RulesAPI.Logger.Msg($"Activating ruleset: {GetType()} (with {Rules.Count} rules)");

            foreach (var rule in Rules)
            {
                try
                {
                    rule.Activate();
                }
                catch (Exception e)
                {
                    // TODO(orendain): Rollback activation.
                    RulesAPI.Logger.Warning($"Failed to activate rule [{rule.GetType()}]: {e}");
                }
            }
        }

        internal void Deactivate()
        {
            RulesAPI.Logger.Msg($"Deactivating ruleset: {GetType()} (with {Rules.Count} rules)");
            foreach (var rule in Rules)
            {
                try
                {
                    rule.Deactivate();
                }
                catch (Exception e)
                {
                    RulesAPI.Logger.Warning($"Failed to deactivate rule [{rule.GetType()}]: {e}");
                }
            }
        }
    }
}
