namespace RulesAPI
{
    using System;
    using System.Linq;
    using MelonLoader;

    public static class RulesAPI
    {
        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("RulesAPI");

        private static Ruleset SelectedRuleset { get; set; }

        public static void SelectRuleset(string ruleset)
        {
            try
            {
                SelectedRuleset = Registrar.Instance().Rulesets
                    .Single(r => string.Equals(r.Name, ruleset, StringComparison.OrdinalIgnoreCase));
            }
            catch (InvalidOperationException e)
            {
                throw new ArgumentException("Ruleset must first be registered.", e);
            }

            Logger.Msg($"Ruleset selected: {SelectedRuleset.Name}");
        }

        internal static void ActivateSelectedRuleset()
        {
            // TODO(orendain): Do not automatically load the first ruleset when a game is hosted. For dev only.
            if (SelectedRuleset == null)
            {
                SelectRuleset(Registrar.Instance().Rulesets.ElementAt(0).Name);
                return;
            }

            SelectedRuleset.Activate();
        }

        internal static void DeactivateSelectedRuleset()
        {
            SelectedRuleset.Deactivate();
        }
    }
}
