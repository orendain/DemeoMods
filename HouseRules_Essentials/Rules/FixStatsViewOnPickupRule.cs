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
            int numdowns = myPiece.GetStat(Stats.Type.DownedCounter);
            int maxdowns = myPiece.GetStatMax(Stats.Type.DownedCounter);
            bool hasbonuses = true;
            bool hasimmunities = true;
            var pieceConfig = Traverse.Create(__instance).Field<PieceConfigData>("pieceConfig").Value;
            EffectStateType[] immuneToStatusEffects = pieceConfig.ImmuneToStatusEffects;
            if (immuneToStatusEffects == null || immuneToStatusEffects.Length == 0)
            {
                hasimmunities = false;
            }

            if (numdowns == 0 && strength == 0 && speed == 0 && magic == 0 && resist == 0)
            {
                hasbonuses = false;
                if (!hasimmunities)
                {
                    return;
                }
            }

            string name = pieceNameController.GetPieceName();
            var sb = new StringBuilder();
            sb.AppendLine($"<< {name} >>");
            if (hasbonuses)
            {
                if (numdowns > 0)
                {
                    sb.AppendLine($"Times downed: {numdowns}/{maxdowns}");
                }

                sb.AppendLine();
                sb.AppendLine("-- Bonus Stats --");
                if (strength > 0)
                {
                    if (maxstrength > 3)
                    {
                        sb.AppendLine($"Strength: {strength}/{maxstrength}");
                    }
                    else
                    {
                        sb.AppendLine($"Strength: {strength}");
                    }
                }

                if (speed > 0)
                {
                    if (maxspeed > 3)
                    {
                        sb.AppendLine($"Swiftness: {speed}/{maxspeed}");
                    }
                    else
                    {
                        sb.AppendLine($"Swiftness: {speed}");
                    }
                }

                if (magic > 0)
                {
                    if (maxmagic > 3)
                    {
                        sb.AppendLine($"Magic: {magic}/{maxmagic}");
                    }
                    else
                    {
                        sb.AppendLine($"Magic: {magic}");
                    }
                }

                if (resist > 0)
                {
                    if (maxresist > 1)
                    {
                        sb.AppendLine($"Damage Resist: {resist}/{maxresist}");
                    }
                    else
                    {
                        sb.AppendLine($"Damage Resist: {resist}");
                    }
                }
            }
            else
            {
                sb.AppendLine("No Stat Bonuses... yet!");
            }

            if (!hasimmunities)
            {
                GameUI.ShowCameraMessage(sb.ToString(), 3);
                return;
            }

            int num = immuneToStatusEffects.Length;
            sb.AppendLine();
            sb.AppendLine($"-- Immunities --");

            bool weak = false;
            List<string> list = new List<string>();
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
