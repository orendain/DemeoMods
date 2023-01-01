namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using System.Text;
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.Ui;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class FixStatsViewOnPickupRule : Rule, IConfigWritable<bool>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Show missing stats when piece picked up";

        private static bool _isActivated;

        public FixStatsViewOnPickupRule(bool value)
        {
        }

        public bool GetConfigObject() => true;

        protected override void OnActivate(GameContext gameContext)
        {
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(GrabbedPieceHudInstantiator), "CloneCurrentHudState"),
                postfix: new HarmonyMethod(
                    typeof(FixStatsViewOnPickupRule),
                    nameof(GrabbedPieceHudInstantiator_CloneCurrentHudState_Postfix)));
        }

        private static void GrabbedPieceHudInstantiator_CloneCurrentHudState_Postfix(ref GrabbedPieceHudInstantiator __instance)
        {
            /*if (!_isActivated)
            {
                return;
            }*/

            var pieceId = Traverse.Create(__instance).Field<int>("pieceId").Value;
            var pieceAndTurnController = Traverse.Create(__instance).Field<PieceAndTurnController>("pieceAndTurnController").Value;
            pieceAndTurnController.TryGetPiece(pieceId, out Piece myPiece);
            if (!myPiece.IsPlayer())
            {
                return;
            }

            var pieceNameController = Traverse.Create(__instance).Field<IPieceNameController>("pieceNameController").Value;
            int strength = myPiece.GetStat(Stats.Type.Strength);
            int maxstrength = myPiece.GetStatMax(Stats.Type.Strength);
            int speed = myPiece.GetStat(Stats.Type.Speed);
            int maxspeed = myPiece.GetStatMax(Stats.Type.Speed);
            int magic = myPiece.GetStat(Stats.Type.MagicBonus);
            int maxmagic = myPiece.GetStatMax(Stats.Type.MagicBonus);
            int resist = myPiece.GetStat(Stats.Type.DamageResist);
            int maxresist = myPiece.GetStatMax(Stats.Type.DamageResist);
            string name = pieceNameController.GetPieceName();
            var sb = new StringBuilder();
            sb.AppendLine($"<< {name} >>");
            sb.AppendLine();
            if (maxstrength > 3 || maxspeed > 3 || maxmagic > 3 || maxresist > 1)
            {
                sb.AppendLine($"Strength: {strength}/{maxstrength}");
                sb.AppendLine($"Movement: {speed}/{maxspeed}");
                sb.AppendLine($"Magic Bonus: {magic}/{maxmagic}");
                sb.AppendLine($"Damage Resist: {resist}/{maxresist}");
            }
            else
            {
                sb.AppendLine($"Magic Bonus: {magic}");
                sb.AppendLine($"Damage Resist: {resist}");
            }

            var pieceConfig = Traverse.Create(__instance).Field<PieceConfigData>("pieceConfig").Value;
            List<string> list = new List<string>();
            EffectStateType[] immuneToStatusEffects = pieceConfig.ImmuneToStatusEffects;
            if (immuneToStatusEffects == null)
            {
                GameUI.ShowCameraMessage(sb.ToString(), 3);
                return;
            }

            int num = immuneToStatusEffects.Length;
            if (num == 0)
            {
                GameUI.ShowCameraMessage(sb.ToString(), 3);
                return;
            }

            sb.AppendLine();
            sb.AppendLine($"Immunities:");
 
            bool weak = false;
            for (int i = 0; i < num; i++)
            {
                string localizedTitle = StatusEffectsConfig.GetLocalizedTitle(immuneToStatusEffects[i]);
                if (!list.Contains(localizedTitle))
                {
                    if (localizedTitle.Contains("Netted"))
                    {
                        localizedTitle = "Netted";
                    }
                    else if (localizedTitle.Contains("Weaken"))
                    {
                        if (weak)
                        {
                            continue;
                        }

                        weak = true;
                    }

                    if (i != 0)
                    {
                        sb.Append(", ");
                    }

                    sb.Append($"{localizedTitle}");
                }
            }

            if (HR.SelectedRuleset.Name.Contains("Demeo Revolutions"))
            {
                switch (myPiece.boardPieceId)
                {
                    case BoardPieceId.HeroGuardian:
                        sb.Append(", Fire");
                        break;
                    case BoardPieceId.HeroSorcerer:
                        sb.Append(", Electricity");
                        break;
                    case BoardPieceId.HeroWarlock:
                        sb.Append(", Corruption");
                        break;
                    case BoardPieceId.HeroHunter:
                        sb.Append(", Ice");
                        break;
                    case BoardPieceId.HeroBarbarian:
                        sb.Append(", Slime");
                        break;
                }
            }

            GameUI.ShowCameraMessage(sb.ToString(), 5);
        }
    }
}
