namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using Boardgame;
    using Boardgame.BoardEntities.Abilities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;
    using UnityEngine;

    public sealed class AbilityRandomPieceListRule : Rule, IConfigWritable<Dictionary<string, BoardPieceId[]>>, IMultiplayerSafe
    {
        public override string Description => "Ability randomPieceLists are replaced";

        private readonly Dictionary<string, BoardPieceId[]> _adjustments;
        private readonly Dictionary<string, BoardPieceId[]> _originals;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbilityRandomPieceListRule"/> class.
        /// </summary>
        /// <param name="adjustments">Dict of ability name string and list of BoardPieceID[]s
        /// Replaces the list of pieces that certain abilities will spawn (e.g. NaturesCall, SpawnCultists etc).</param>
        public AbilityRandomPieceListRule(Dictionary<string, BoardPieceId[]> adjustments)
        {
            _adjustments = adjustments;
            _originals = new Dictionary<string, BoardPieceId[]>();
        }

        public Dictionary<string, BoardPieceId[]> GetConfigObject() => _adjustments;

        protected override void OnPostGameCreated(GameContext gameContext)
        {
            var abilities = Resources.FindObjectsOfTypeAll<Ability>();
            foreach (var item in _adjustments)
            {
                var ability = abilities.First(c => c.name.Equals($"{item.Key}(Clone)"));
                _originals[item.Key] = ability.randomPieceList;
                var rpl = Traverse.Create(ability).Field<BoardPieceId[]>("randomPieceList");
                rpl.Value = item.Value;
            }
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            var abilities = Resources.FindObjectsOfTypeAll<Ability>();
            foreach (var item in _originals)
            {
                var ability = abilities.First(c => c.name.Equals($"{item.Key}(Clone)"));
                var rpl = Traverse.Create(ability).Field<BoardPieceId[]>("randomPieceList");
                rpl.Value = item.Value;
            }
        }
    }
}
