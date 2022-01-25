namespace RulesAPI
{
    internal class ModState
    {
        internal Ruleset SelectedRuleset { get; set; }

        internal static ModState NewInstance()
        {
            return new ModState();
        }

        private ModState()
        {
        }
    }
}
