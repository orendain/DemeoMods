namespace RulesAPI
{
    public abstract class Rule
    {
        /// <summary>
        /// Gets the description of the rule.
        /// </summary>
        public abstract string Description { get; }

        internal void Activate()
        {
            RulesAPI.Logger.Msg($"Activating rule type: {GetType()}");
            OnActivate();
        }

        internal void Deactivate()
        {
            RulesAPI.Logger.Msg($"Deactivating rule type: {GetType()}");
            OnDeactivate();
        }

        /// <summary>
        /// Called when the rule is activated.
        /// </summary>
        protected abstract void OnActivate();

        /// <summary>
        /// Called when the rule is deactivated.
        /// </summary>
        /// <remarks>
        /// This method should undo any persistant changes made during <see cref="OnActivate"/>.
        /// </remarks>
        protected abstract void OnDeactivate();
    }
}
