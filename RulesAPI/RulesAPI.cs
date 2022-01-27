namespace RulesAPI
{
    public class RulesAPI
    {
        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("RulesAPI");
        
        private static Ruleset SelectedRuleset { get; set; }
        
        public static void SelectRuleset(Ruleset ruleset)
        {
            if (!Registrar.Instance().IsRegistered(ruleset))
            {
                throw new ArgumentException("Ruleset must first be registered.");
            }

            SelectedRuleset = ruleset;
            Logger.Msg($"Ruleset selected: {ruleset.GetType()}");
        }
        
        internal static void ActivateSelectedRuleset()
        {
            // TODO(orendain): Do not automatically load the first ruleset when a game is hosted. For dev only.
            if (SelectedRuleset == null)
            {
                SelectRuleset(Registrar.Instance().Rulesets.ElementAt(0));
            }

            SelectedRuleset.Activate();
        }
        
        internal static void DeactivateSelectedRuleset()
        {
            SelectedRuleset.Deactivate();
        }
    }
}
