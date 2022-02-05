namespace RulesAPI
{
    public abstract class Rule
    {
        /// <summary>
        /// Gets the description of the rule.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Called when the rule is activated.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///     Rules should use this as an indication that they may begin to take effect.
        ///     </para>
        ///     <para>
        ///     This will be called at some point after the rule is selected to be used, but before a game begins
        ///     to be created.
        ///     </para>
        /// </remarks>
        protected internal virtual void OnActivate()
        {
        }

        /// <summary>
        /// Called when the rule is deactivated.
        /// </summary>
        /// <remarks>
        /// This method should undo any persisting changes made by the rule up until this point.
        /// </remarks>
        protected internal virtual void OnDeactivate()
        {
        }

        /// <summary>
        /// Called before a game is created.
        /// </summary>
        protected internal virtual void OnPreGameCreated()
        {
        }

        /// <summary>
        /// Called after a game is created.
        /// </summary>
        /// <remarks>
        /// Note that even though the game is created, the level/POIs/enemies may not yet fully be loaded/spawned.
        /// </remarks>
        protected internal virtual void OnPostGameCreated()
        {
        }
    }
}
