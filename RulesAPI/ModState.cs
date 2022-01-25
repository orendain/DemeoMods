namespace RulesAPI
{
    internal class ModState
    {
        internal RuleSet SelectedRuleSet { get; set; }

        internal static ModState NewInstance()
        {
            return new ModState();
        }

        private ModState()
        {
        }
    }
}
