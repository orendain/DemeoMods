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

        internal void PreGameCreated()
        {
            RulesAPI.Logger.Msg($"Calling OnPreGameCreated for rule type: {GetType()}");
            OnPreGameCreated();
        }

        internal void PostGameCreated()
        {
            RulesAPI.Logger.Msg($"Calling OnPostGameCreated for rule type: {GetType()}");
            OnPostGameCreated();
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

        /// <summary>
        /// Called before a game is created.
        /// </summary>
        protected abstract void OnPreGameCreated();

        /// <summary>
        /// Called after a game is created.
        /// </summary>
        /// <remarks>
        /// Note that even though the game is created, the level/POIs/enemies may not yet fully be loaded/spawned.
        /// </remarks>
        protected abstract void OnPostGameCreated();
    }
}
