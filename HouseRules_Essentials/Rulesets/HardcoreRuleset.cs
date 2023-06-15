namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using DataKeys;
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class HardcoreRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "Hardcore";
            const string description = "You only get one life. No knockdowns. Can your team survive?";

            var pieceDownedCountRule = new PieceDownedCountAdjustedRule(new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.HeroBarbarian, 0 },
                { BoardPieceId.HeroGuardian, 0 },
                { BoardPieceId.HeroHunter, 0 },
                { BoardPieceId.HeroRogue, 0 },
                { BoardPieceId.HeroSorcerer, 0 },
                { BoardPieceId.HeroWarlock, 0 },
                { BoardPieceId.HeroBard, 0 },
            });

            return Ruleset.NewInstance(
                name,
                description,
                pieceDownedCountRule);
        }
    }
}
