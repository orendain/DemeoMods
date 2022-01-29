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
        protected internal virtual void OnActivate()
        {
        }

        /// <summary>
        /// Called when the rule is deactivated.
        /// </summary>
        /// <remarks>
        /// This method should undo any persistant changes made during <see cref="OnActivate"/>.
        /// </remarks>
        protected internal virtual void OnDeactivate()
        {
        }

        /// <summary>
        /// Called before a game is created.
        /// </summary>
        protected internal virtual void PreGameCreated()
        {
        }

        /// <summary>
        /// Called after a game is created.
        /// </summary>
        /// <remarks>
        /// Note that even though the game is created, the level/POIs/enemies may not yet fully be loaded/spawned.
        /// </remarks>
        protected internal virtual void PostGameCreated()
        {
        }
    }
}
