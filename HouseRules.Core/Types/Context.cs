using Boardgame;
using Boardgame.BoardEntities.Abilities;

namespace HouseRules.Core.Types;

public class Context
{
    /// <summary>
    /// Gets the current game context.
    /// </summary>
    public GameContext GameContext { get; }

    /// <summary>
    /// Gets the ability factory used to manage abilities.
    /// </summary>
    public AbilityFactory AbilityFactory { get; }

    public Context(GameContext gameContext, AbilityFactory abilityFactory)
    {
        GameContext = gameContext;
        AbilityFactory = abilityFactory;
    }
}
