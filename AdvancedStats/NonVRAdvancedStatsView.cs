namespace AdvancedStats
{
    using System.Collections.Generic;
    using System.Text;
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.NonVR;
    using Boardgame.NonVR.Ui;
    using Boardgame.Ui;
    using DataKeys;
    using HarmonyLib;
    using UnityEngine;

    internal static class NonVRAdvancedStatsView
    {
        internal static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(NonVrInfoPanelController), "OnSelectPiece"),
                postfix: new HarmonyMethod(
                    typeof(NonVRAdvancedStatsView),
                    nameof(NonVrInfoPanelController_OnSelectPiece_Postfix)));
        }

        private static void NonVrInfoPanelController_OnSelectPiece_Postfix(PiceDetectionData selectData)
        {
            if (selectData.Grabbable)
            {
                if (selectData.HasHudInstantiator)
                {
                    GrabbedPieceHudInstantiator data = selectData.HudInstantiator;
                    Piece myPiece = data.MyPiece;

                    if (!myPiece.IsPlayer())
                    {
                        return;
                    }

                    var pieceNameController = Traverse.Create(data).Field<IPieceNameController>("pieceNameController").Value;
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
                    var pieceConfig = Traverse.Create(data).Field<PieceConfigData>("pieceConfig").Value;
                    EffectStateType[] immuneToStatusEffects = pieceConfig.ImmuneToStatusEffects;

                    if (immuneToStatusEffects == null || immuneToStatusEffects.Length == 0)
                    {
                        hasimmunities = false;
                    }

                    if (strength == 0 && speed == 0 && magic == 0 && resist == 0)
                    {
                        hasbonuses = false;
                    }

                    Color lightblue = new Color(0f, 0.75f, 1f);
                    Color lightgreen = new Color(0f, 1f, 0.5f);
                    Color orange = new Color(1f, 0.499f, 0f);
                    Color pink = new Color(0.984f, 0.3765f, 0.498f);
                    Color gold = new Color(1f, 1f, 0.6f);
                    Color mustard = new Color(0.73f, 0.55f, 0.07f);
                    string name = pieceNameController.GetPieceName();
                    var sb = new StringBuilder();
                    sb.AppendLine(ColorizeString($"<u>{name}</u>", Color.yellow));
                    sb.Append(ColorizeString("Downed times remaining: ", pink));
                    switch (numdowns)
                    {
                        case 0:
                            sb.AppendLine(ColorizeString("3", Color.green));
                            break;
                        case 1:
                            sb.AppendLine(ColorizeString("2", gold));
                            break;
                        case 2:
                            sb.AppendLine(ColorizeString("1", orange));
                            break;
                        case 3:
                            sb.AppendLine(ColorizeString("0", Color.red));
                            break;
                    }

                    if (myPiece.HasEffectState(EffectStateType.Weaken1Turn) || myPiece.HasEffectState(EffectStateType.Weaken2Turns))
                    {
                        sb.Append(ColorizeString("Weakened (Half damage)", mustard));
                    }

                    sb.AppendLine();
                    sb.Append(ColorizeString("--", Color.gray));
                    sb.Append(ColorizeString(" Bonus Stats ", Color.white));
                    sb.AppendLine(ColorizeString("--", Color.gray));

                    if (hasbonuses)
                    {
                        if (strength > 0)
                        {
                            sb.Append(ColorizeString("Strength: ", Color.cyan));
                            sb.AppendLine(ColorizeString($"{strength}/{maxstrength}", lightgreen));
                        }

                        if (speed > 0)
                        {
                            sb.Append(ColorizeString("Swiftness: ", Color.cyan));
                            sb.AppendLine(ColorizeString($"{speed}/{maxspeed}", lightgreen));
                        }

                        if (magic > 0)
                        {
                            sb.Append(ColorizeString("Magic: ", Color.cyan));
                            sb.AppendLine(ColorizeString($"{magic}/{maxmagic}", lightgreen));
                        }

                        if (resist > 0)
                        {
                            sb.Append(ColorizeString("Damage Resist: ", Color.cyan));
                            sb.AppendLine(ColorizeString($"{resist}/{maxresist}", lightgreen));
                        }
                    }
                    else
                    {
                        sb.AppendLine(ColorizeString("None", Color.cyan));
                    }

                    sb.AppendLine();
                    sb.Append(ColorizeString("--", Color.gray));
                    sb.Append(ColorizeString(" Immunities ", Color.white));
                    sb.AppendLine(ColorizeString("--", Color.gray));

                    if (maxmagic != 5 && !hasimmunities && !myPiece.HasEffectState(EffectStateType.FireImmunity) && !myPiece.HasEffectState(EffectStateType.IceImmunity))
                    {
                        sb.AppendLine(ColorizeString("None", lightblue));

                        // Pad lines to raise text higher on PC-Edition screen
                        sb.AppendLine();
                        sb.AppendLine();
                        sb.AppendLine();
                        sb.AppendLine();
                        sb.AppendLine();
                        sb.AppendLine();
                        sb.AppendLine();
                        sb.AppendLine();
                        sb.AppendLine(ColorizeString(" ", Color.clear));
                        GameUI.ShowCameraMessage(sb.ToString(), 5);
                        return;
                    }
                    else if (maxmagic != 5)
                    {
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
                                    localizedTitle = ColorizeString("Netted", lightblue);
                                }
                                else if (localizedTitle.Contains("Undefined"))
                                {
                                    continue;
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
                                    sb.Append(ColorizeString(", ", lightblue));
                                }

                                sb.Append(ColorizeString($"{localizedTitle}", lightblue));
                            }
                        }
                    }
                    else
                    {
                        switch (myPiece.boardPieceId)
                        {
                            case BoardPieceId.HeroGuardian:
                                sb.Append(ColorizeString("Weaken, Fire", lightblue));
                                break;
                            case BoardPieceId.HeroSorcerer:
                                sb.Append(ColorizeString("Stunned, Electricity", lightblue));
                                break;
                            case BoardPieceId.HeroWarlock:
                                sb.Append(ColorizeString("Corrupted Rage, Corruption", lightblue));
                                break;
                            case BoardPieceId.HeroHunter:
                                sb.Append(ColorizeString("Frozen, Ice", lightblue));
                                break;
                            case BoardPieceId.HeroBarbarian:
                                sb.Append(ColorizeString("Petrify, Slime", lightblue));
                                break;
                            case BoardPieceId.HeroRogue:
                                sb.Append(ColorizeString("Tangled, Netted", lightblue));
                                break;
                            case BoardPieceId.HeroBard:
                                sb.Append(ColorizeString("Poisoned, Blinded", lightblue));
                                break;
                        }
                    }

                    sb.AppendLine();
                    if (myPiece.HasEffectState(EffectStateType.FireImmunity))
                    {
                        int rounds = myPiece.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.FireImmunity);
                        sb.AppendLine(ColorizeString($"Fire ({rounds} turns left)", lightblue));
                    }

                    if (myPiece.HasEffectState(EffectStateType.IceImmunity))
                    {
                        int rounds = myPiece.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.IceImmunity);
                        sb.AppendLine(ColorizeString($"Frozen, Ice ({rounds} turns left)", lightblue));
                    }

                    // Pad lines to raise text higher on PC-Edition screen
                    sb.AppendLine();
                    sb.AppendLine();
                    sb.AppendLine();
                    sb.AppendLine();
                    sb.AppendLine();
                    sb.AppendLine();
                    sb.AppendLine();
                    sb.AppendLine();
                    sb.AppendLine(ColorizeString(" ", Color.clear));
                    GameUI.ShowCameraMessage(sb.ToString(), 5);
                }
            }
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
