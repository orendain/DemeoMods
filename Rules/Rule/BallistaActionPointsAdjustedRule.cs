namespace Rules.Rule
{
    using System.Linq;
    using Boardgame;
    using HarmonyLib;
    using UnityEngine;

    public sealed class BallistaActionPointsAdjustedRule : RulesAPI.Rule
    {
        public override string Description => "Ballista action points are adjusted";

        private readonly int _actionPoints;
        private int _originalActionPoints;

        public BallistaActionPointsAdjustedRule(int actionPoints)
        {
            _actionPoints = actionPoints;
        }

        protected override void OnPostGameCreated()
        {
            var pieceConfigs = Resources.FindObjectsOfTypeAll<PieceConfig>();
            var pieceConfig = pieceConfigs.First(c => c.name.Equals("PieceConfig_SwordOfAvalon"));
            var actionPoint = Traverse.Create(pieceConfig).Property<int>("ActionPoint");
            _originalActionPoints = actionPoint.Value;
            actionPoint.Value = _actionPoints;
        }

        protected override void OnDeactivate()
        {
            var pieceConfigs = Resources.FindObjectsOfTypeAll<PieceConfig>();
            var pieceConfig = pieceConfigs.First(c => c.name.Equals("PieceConfig_SwordOfAvalon"));
            Traverse.Create(pieceConfig).Property<int>("ActionPoint").Value = _originalActionPoints;
        }
    }
}
