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
    using UnityEngine;

    public sealed class FixStatsViewOnPickupRule : Rule, IConfigWritable<bool>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Show more stats when piece picked up";

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

            // var pieceAndTurnController = Traverse.Create(__instance).Field<PieceAndTurnController>("pieceAndTurnController").Value;
            Piece myPiece = __instance.MyPiece;

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
            bool hasbonuses = true;
            bool hasimmunities = true;
            var pieceConfig = Traverse.Create(__instance).Field<PieceConfigData>("pieceConfig").Value;
            EffectStateType[] immuneToStatusEffects = pieceConfig.ImmuneToStatusEffects;

            if (immuneToStatusEffects == null || immuneToStatusEffects.Length == 0)
            {
                hasimmunities = false;
            }

            if (strength == 0 && speed == 0 && magic == 0 && resist == 0)
            {
                hasbonuses = false;
            }

            string name = pieceNameController.GetPieceName();
            var sb = new StringBuilder();
            sb.AppendLine(ColorizeString($"<< {name} >>", Color.yellow));
            sb.AppendLine(ColorizeString($"Downed times remaining: {3 - numdowns}", Color.green));
            sb.AppendLine();
            sb.AppendLine(ColorizeString("-- Bonus Stats --", Color.white));

            if (hasbonuses)
            {
                if (strength > 0)
                {
                    sb.AppendLine(ColorizeString($"Strength: {strength}/{maxstrength}", Color.cyan));
                }

                if (speed > 0)
                {
                    sb.AppendLine(ColorizeString($"Swiftness: {speed}/{maxspeed}", Color.cyan));
                }

                if (magic > 0)
                {
                    sb.AppendLine(ColorizeString($"Magic: {magic}/{maxmagic}", Color.cyan));
                }

                if (resist > 0)
                {
                    sb.AppendLine(ColorizeString($"Damage Resist: {resist}/{maxresist}", Color.cyan));
                }
            }
            else
            {
                sb.AppendLine(ColorizeString("None", Color.cyan));
            }

            sb.AppendLine();
            sb.AppendLine(ColorizeString("-- Immunities --", Color.white));

            if (!hasimmunities)
            {
                sb.AppendLine(ColorizeString("None", Color.magenta));
                GameUI.ShowCameraMessage(sb.ToString(), 5);
                return;
            }

            int num = immuneToStatusEffects.Length;
            bool weak = false;
            List<string> list = new List<string>();

            for (int i = 0; i < num; i++)
            {
                string localizedTitle = StatusEffectsConfig.GetLocalizedTitle(immuneToStatusEffects[i]);
                if (!list.Contains(localizedTitle))
                {
                    if (localizedTitle.Contains("Netted"))
                    {
                        localizedTitle = ColorizeString("Netted", Color.magenta);
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
                        sb.Append(ColorizeString(", ", Color.magenta));
                    }

                    sb.Append(ColorizeString($"{localizedTitle}", Color.magenta));
                }
            }

            if (HR.SelectedRuleset.Name.Contains("Demeo Revolutions"))
            {
                switch (myPiece.boardPieceId)
                {
                    case BoardPieceId.HeroGuardian:
                        sb.Append(ColorizeString(", Fire", Color.magenta));
                        break;
                    case BoardPieceId.HeroSorcerer:
                        sb.Append(ColorizeString(", Electricity", Color.magenta));
                        break;
                    case BoardPieceId.HeroWarlock:
                        sb.Append(ColorizeString(", Corruption", Color.magenta));
                        break;
                    case BoardPieceId.HeroHunter:
                        sb.Append(ColorizeString(", Ice", Color.magenta));
                        break;
                    case BoardPieceId.HeroBarbarian:
                        sb.Append(ColorizeString(", Slime", Color.magenta));
                        break;
                }
            }

            GameUI.ShowCameraMessage(sb.ToString(), 5);
        }

        private static string ColorizeString(string text, Color color)
        {
            return string.Concat(new string[]
            {
        "<color=#",
        ColorUtility.ToHtmlStringRGB(color),
        ">",
        text,
        "</color>",
            });
        }
    }
}
