namespace HouseRules.Types
{
    using Boardgame;

    public abstract class Rule
    {
        /// <summary>
        /// Gets the description of the rule.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Called when the rule is activated.
        /// </summary>
        /// <param name="gameContext">The game's global GameContext.</param>
        /// <remarks>
        ///     <para>
        ///     Rules should use this as an indication that they may begin to take effect.
        ///     </para>
        ///     <para>
        ///     This will be called at some point after the rule is selected to be used, but before a game begins
        ///     to be created.
        ///     </para>
        /// </remarks>
        protected internal virtual void OnActivate(GameContext gameContext)
        {
        }

        /// <summary>
        /// Called when the rule is deactivated.
        /// </summary>
        /// <param name="gameContext">The game's global GameContext.</param>
        /// <remarks>
        /// This method should undo any persisting changes made by the rule up until this point.
        /// </remarks>
        protected internal virtual void OnDeactivate(GameContext gameContext)
        {
        }

        /// <summary>
        /// Called before a game is created.
        /// </summary>
        /// <param name="gameContext">The game's global GameContext.</param>
        protected internal virtual void OnPreGameCreated(GameContext gameContext)
        {
        }

        /// <summary>
        /// Called after a game is created.
        /// </summary>
        /// <param name="gameContext">The game's global GameContext.</param>
        /// <remarks>
        /// Note that even though the game is created, the level/POIs/enemies may not yet fully be loaded/spawned.
        /// </remarks>
        protected internal virtual void OnPostGameCreated(GameContext gameContext)
        {
        }

        /// <summary>
        /// Gets the type of data that the rule makes modifications to.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///     There is no need to override this field unless the rule makes changes to one of the defined
        ///     <see cref="SpecialSyncData"/> types.
        ///     </para>
        ///     <para>
        ///     Multiple types may be seperated by a vertical bar <c>|</c>.
        ///     </para>
        /// </remarks>
        /// <example><code>SpecialSyncData.PieceData</code></example>
        /// <example><code>SpecialSyncData.PieceData | SpecialSyncData.StatusEffectImmunity</code></example>
        protected internal virtual SpecialSyncData ModifiedData => SpecialSyncData.None;
    }
}
