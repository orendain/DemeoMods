namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using DataKeys;
    using HouseRules.Core.Types;
    using HouseRules.Essentials.Rules;

    internal static class HardcoreRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "Hardcore";
            const string description = "You only get one life. No knockdowns. Can your team survive?";
            const string longdesc = "";

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
                longdesc,
                pieceDownedCountRule);
        }
    }
}
