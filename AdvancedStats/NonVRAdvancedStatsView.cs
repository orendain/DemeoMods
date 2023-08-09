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
                    var revolutions = myPiece.GetStat(Stats.Type.InnateCounterDamageExtraDamage);
                    int level = myPiece.GetStatMax(Stats.Type.CritChance);
                    var freeSlots = 9;
                    Inventory.Item value2;
                    for (var i = 0; i < myPiece.inventory.Items.Count; i++)
                    {
                        value2 = myPiece.inventory.Items[i];
                        if (!value2.IsReplenishable)
                        {
                            freeSlots--;
                        }
                    }

                    if (!myPiece.HasEffectState(EffectStateType.Flying) || myPiece.HasEffectState(EffectStateType.StrengthInNumbers))
                    {
                        level = 0;
                    }

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
                    sb.Append(ColorizeString("Knockdowns Remaining: ", pink));
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
                        default:
                            sb.AppendLine(ColorizeString("0", Color.red));
                            break;
                    }

                    sb.Append(ColorizeString("Free card slots: ", mustard));
                    sb.AppendLine(ColorizeString($"{freeSlots}", gold));
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

                    if (revolutions == 69 || myPiece.HasEffectState(EffectStateType.Flying))
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
                    else if (!hasimmunities && !myPiece.HasEffectState(EffectStateType.FireImmunity) && !myPiece.HasEffectState(EffectStateType.IceImmunity))
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
                    else
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

                    if (level > 0)
                    {
                        int hp = 0;
                        int kd = 0;
                        int bon = 0;
                        bool spd = false;
                        bool app = true;
                        sb.AppendLine();
                        sb.Append(ColorizeString("--", Color.gray));
                        sb.Append(ColorizeString(" Character ", pink));
                        if (level < 10)
                        {
                            sb.Append(ColorizeString($"Level {level}", orange));
                            sb.Append(ColorizeString(" PERKS ", pink));
                            sb.AppendLine(ColorizeString("--", Color.gray));
                        }
                        else
                        {
                            sb.Append(ColorizeString($"Level {level}", orange));
                            sb.Append(ColorizeString($" (MAXED!) ", gold));
                            sb.Append(ColorizeString("PERKS ", pink));
                            sb.AppendLine(ColorizeString("--", Color.gray));
                        }

                        sb.Append(ColorizeString("Critical hits on ", Color.yellow));
                        sb.Append(ColorizeString("LAST ACTION", Color.white));
                        sb.Append(ColorizeString(" activate ", Color.yellow));
                        switch (myPiece.boardPieceId)
                        {
                            case BoardPieceId.HeroGuardian:
                                sb.Append(ColorizeString("Berserk", Color.white));
                                sb.Append(ColorizeString(" and restore ", Color.yellow));
                                if (level > 3)
                                {
                                    sb.AppendLine(ColorizeString("2 Action Points", Color.white));
                                }
                                else
                                {
                                    sb.AppendLine(ColorizeString("1 Action Point", Color.white));
                                }

                                break;
                            case BoardPieceId.HeroSorcerer:
                                sb.Append(ColorizeString("Overcharge", Color.white));
                                sb.Append(ColorizeString(" and gain ", Color.yellow));
                                sb.Append(ColorizeString("Water Bottle", Color.white));
                                sb.AppendLine(ColorizeString(" ability", Color.yellow));
                                break;
                            case BoardPieceId.HeroWarlock:
                                sb.Append(ColorizeString("Self-Deflect", Color.white));
                                sb.Append(ColorizeString(" and gain ", Color.yellow));
                                sb.Append(ColorizeString("Spellpower Potion", Color.white));
                                sb.AppendLine(ColorizeString(" ability", Color.yellow));
                                break;
                            case BoardPieceId.HeroHunter:
                                sb.Append(ColorizeString("Freeze Arrow", Color.white));
                                sb.Append(ColorizeString(" and gain ", Color.yellow));
                                sb.Append(ColorizeString("Bone", Color.white));
                                sb.AppendLine(ColorizeString(" ability", Color.yellow));
                                break;
                            case BoardPieceId.HeroBarbarian:
                                sb.Append(ColorizeString("2 Resilience", Color.white));
                                sb.Append(ColorizeString(" and gain ", Color.yellow));
                                sb.AppendLine(ColorizeString("6 Vargas", Color.white));
                                break;
                            case BoardPieceId.HeroRogue:
                                sb.Append(ColorizeString("Invisibility", Color.white));
                                sb.AppendLine(ColorizeString(" until the NEXT turn ends ", Color.yellow));
                                break;
                            case BoardPieceId.HeroBard:
                                sb.Append(ColorizeString("Deflection", Color.white));
                                sb.Append(ColorizeString(" and gain ", Color.yellow));
                                sb.Append(ColorizeString("Panic Powder", Color.white));
                                sb.AppendLine(ColorizeString(" ability", Color.yellow));
                                break;
                        }

                        if (level > 1)
                        {
                            app = true;
                            switch (myPiece.boardPieceId)
                            {
                                case BoardPieceId.HeroGuardian:
                                    sb.Append(ColorizeString("Gained ", Color.yellow));
                                    sb.Append(ColorizeString("Pull", Color.white));
                                    sb.Append(ColorizeString(" ability", Color.yellow));
                                    break;
                                case BoardPieceId.HeroSorcerer:
                                    sb.Append(ColorizeString("Replaced ", Color.yellow));
                                    sb.Append(ColorizeString("Overcharge", Color.white));
                                    sb.Append(ColorizeString(" with ", Color.yellow));
                                    sb.Append(ColorizeString("Thunderbolt", Color.white));
                                    break;
                                case BoardPieceId.HeroWarlock:
                                    sb.Append(ColorizeString("Gained ", Color.yellow));
                                    sb.Append(ColorizeString("Feral Charge", Color.white));
                                    sb.Append(ColorizeString(" ability", Color.yellow));
                                    break;
                                case BoardPieceId.HeroHunter:
                                    sb.Append(ColorizeString("Replaced ", Color.yellow));
                                    sb.Append(ColorizeString("1 Arrow", Color.white));
                                    sb.Append(ColorizeString(" with a ", Color.yellow));
                                    sb.Append(ColorizeString("Fire Arrow", Color.white));
                                    break;
                                case BoardPieceId.HeroBarbarian:
                                    sb.Append(ColorizeString("Gained ", Color.yellow));
                                    sb.Append(ColorizeString("Net", Color.white));
                                    sb.Append(ColorizeString(" ability ", Color.yellow));
                                    sb.Append(ColorizeString("(3 turn cooldown)", orange));
                                    break;
                                case BoardPieceId.HeroRogue:
                                    sb.Append(ColorizeString("Gained ", Color.yellow));
                                    sb.Append(ColorizeString("Poisonous Bite", Color.white));
                                    sb.Append(ColorizeString(" ability ", Color.yellow));
                                    sb.Append(ColorizeString("(3 turn cooldown)", orange));
                                    break;
                                case BoardPieceId.HeroBard:
                                    sb.Append(ColorizeString("Gained ", Color.yellow));
                                    sb.Append(ColorizeString("Flash Bomb", Color.white));
                                    sb.Append(ColorizeString(" ability ", Color.yellow));
                                    sb.Append(ColorizeString("(3 turn cooldown)", orange));
                                    break;
                            }

                            sb.Append(ColorizeString(" and ", Color.yellow));
                            switch (myPiece.boardPieceId)
                            {
                                case BoardPieceId.HeroSorcerer:
                                    sb.Append(ColorizeString("both ", Color.yellow));
                                    sb.Append(ColorizeString("Zap", Color.white));
                                    sb.Append(ColorizeString(" and ", Color.yellow));
                                    sb.Append(ColorizeString("Lightning Bolt", Color.white));
                                    break;
                                case BoardPieceId.HeroHunter:
                                    sb.Append(ColorizeString("Arrow", Color.white));
                                    break;
                                case BoardPieceId.HeroBarbarian:
                                    sb.Append(ColorizeString("Grapple", Color.white));
                                    break;
                                case BoardPieceId.HeroRogue:
                                    sb.Append(ColorizeString("Sneak", Color.white));
                                    break;
                                case BoardPieceId.HeroBard:
                                    sb.Append(ColorizeString("Courage Shanty", Color.white));
                                    break;
                            }

                            if (myPiece.boardPieceId == BoardPieceId.HeroSorcerer)
                            {
                                sb.Append(ColorizeString(" are now ", Color.yellow));
                                sb.AppendLine(ColorizeString("FREE Actions", lightgreen));
                            }
                            else if (myPiece.boardPieceId == BoardPieceId.HeroGuardian || myPiece.boardPieceId == BoardPieceId.HeroWarlock)
                            {
                                sb.Append(ColorizeString("it is a ", Color.yellow));
                                sb.AppendLine(ColorizeString("FREE Action", lightgreen));
                            }
                            else
                            {
                                sb.Append(ColorizeString(" is now a ", Color.yellow));
                                sb.AppendLine(ColorizeString("FREE Action", lightgreen));
                            }
                        }

                        if (level > 2)
                        {
                            hp++;
                            app = false;
                            sb.Append(ColorizeString("Critical hits gain ", Color.yellow));
                            sb.Append(ColorizeString("20 gold", Color.white));
                        }

                        if (level > 3)
                        {
                            kd++;
                            app = true;
                            sb.Append(ColorizeString(" and restore ", Color.yellow));
                            switch (myPiece.boardPieceId)
                            {
                                case BoardPieceId.HeroGuardian:
                                    sb.Append(ColorizeString("Pull", Color.white));
                                    sb.Append(ColorizeString(" and ", Color.yellow));
                                    sb.AppendLine(ColorizeString("1 Action Point", Color.white));
                                    break;
                                case BoardPieceId.HeroSorcerer:
                                    sb.AppendLine(ColorizeString("Zap", Color.white));
                                    break;
                                case BoardPieceId.HeroWarlock:
                                    sb.AppendLine(ColorizeString("Feral Charge", Color.white));
                                    break;
                                case BoardPieceId.HeroHunter:
                                    sb.AppendLine(ColorizeString("Arrow", Color.white));
                                    break;
                                case BoardPieceId.HeroBarbarian:
                                    sb.AppendLine(ColorizeString("Grapple", Color.white));
                                    break;
                                case BoardPieceId.HeroRogue:
                                    sb.AppendLine(ColorizeString("1 Action Point", Color.white));
                                    break;
                                case BoardPieceId.HeroBard:
                                    sb.AppendLine(ColorizeString("Courage Shanty", Color.white));
                                    break;
                            }
                        }

                        if (level > 4)
                        {
                            bon++;
                        }

                        if (level > 5)
                        {
                            hp += 2;
                        }

                        if (level > 6)
                        {
                            spd = true;
                        }

                        if (level > 7)
                        {
                            kd++;
                            app = true;
                            sb.Append(ColorizeString("Gained ", Color.yellow));
                            Inventory.Item value;
                            for (var i = 0; i < myPiece.inventory.Items.Count; i++)
                            {
                                value = myPiece.inventory.Items[i];
                                if (value.abilityKey == AbilityKey.Petrify || value.abilityKey == AbilityKey.DropChest || value.abilityKey == AbilityKey.Shockwave || value.abilityKey == AbilityKey.AcidSpit || value.abilityKey == AbilityKey.DeathFlurry)
                                {
                                    if (value.abilityKey == AbilityKey.DeathFlurry)
                                    {
                                        sb.Append(ColorizeString("Regal Burst", Color.white));
                                    }
                                    else if (value.abilityKey == AbilityKey.AcidSpit)
                                    {
                                        sb.Append(ColorizeString("Acid Spit", Color.white));
                                    }
                                    else if (value.abilityKey == AbilityKey.DropChest)
                                    {
                                        sb.Append(ColorizeString("Drop Chest", Color.white));
                                    }
                                    else
                                    {
                                        sb.Append(ColorizeString($"{value.abilityKey}", Color.white));
                                    }

                                    break;
                                }
                            }

                            sb.Append(ColorizeString(" ability ", Color.yellow));
                            sb.Append(ColorizeString("(FREE Action) ", lightgreen));
                            sb.AppendLine(ColorizeString("(7 turn cooldown)", orange));
                        }

                        if (level > 9)
                        {
                            bon += 2;
                        }

                        if (level > 8)
                        {
                            app = true;
                            sb.Append(ColorizeString("Gained ", Color.yellow));
                            sb.Append(ColorizeString("1 health regeneration", Color.white));
                            sb.Append(ColorizeString(" per turn ", Color.yellow));
                            sb.AppendLine(ColorizeString("(except if downed, poisoned, or petrified)", orange));
                        }

                        if (!app)
                        {
                            sb.AppendLine();
                            app = true;
                        }

                        if (hp > 0)
                        {
                            app = false;
                            sb.Append(ColorizeString("Gained ", Color.yellow));
                            sb.Append(ColorizeString($"{hp} Max Health", Color.white));
                        }

                        if (kd > 0)
                        {
                            app = false;
                            if (bon > 0)
                            {
                                sb.Append(ColorizeString(", ", Color.yellow));
                            }
                            else
                            {
                                sb.Append(ColorizeString(" and ", Color.yellow));
                            }

                            if (kd > 1)
                            {
                                sb.Append(ColorizeString($"{kd} Knockdowns", Color.white));
                            }
                            else
                            {
                                sb.Append(ColorizeString($"{kd} Knockdown", Color.white));
                            }
                        }

                        if (bon > 0)
                        {
                            app = false;
                            if (spd)
                            {
                                sb.Append(ColorizeString(", ", Color.yellow));
                            }
                            else
                            {
                                sb.Append(ColorizeString(" and ", Color.yellow));
                            }

                            if (myPiece.boardPieceId == BoardPieceId.HeroWarlock || myPiece.boardPieceId == BoardPieceId.HeroSorcerer)
                            {
                                sb.Append(ColorizeString($"{bon} Magic", Color.white));
                            }
                            else
                            {
                                sb.Append(ColorizeString($"{bon} Strength", Color.white));
                            }
                        }

                        if (spd)
                        {
                            app = true;
                            sb.Append(ColorizeString(" and ", Color.yellow));
                            sb.AppendLine(ColorizeString("2 Swiftness", Color.white));
                        }

                        if (level > 9)
                        {
                            app = true;
                            sb.Append(ColorizeString("Gained ", Color.yellow));
                            sb.AppendLine(ColorizeString("1 Extra Action Point", Color.white));
                        }

                        if (level < 10)
                        {
                            if (!app)
                            {
                                sb.AppendLine();
                            }

                            sb.AppendLine();
                            sb.Append(ColorizeString("NEXT PERK: ", Color.cyan));
                            if (level == 1)
                            {
                                switch (myPiece.boardPieceId)
                                {
                                    case BoardPieceId.HeroGuardian:
                                        sb.Append(ColorizeString("Gain ", Color.yellow));
                                        sb.Append(ColorizeString("Pull", Color.white));
                                        sb.Append(ColorizeString(" ability", Color.yellow));
                                        break;
                                    case BoardPieceId.HeroSorcerer:
                                        sb.Append(ColorizeString("Replace ", Color.yellow));
                                        sb.Append(ColorizeString("Overcharge", Color.white));
                                        sb.Append(ColorizeString(" with ", Color.yellow));
                                        sb.Append(ColorizeString("Thunderbolt", Color.white));
                                        break;
                                    case BoardPieceId.HeroWarlock:
                                        sb.Append(ColorizeString("Gain ", Color.yellow));
                                        sb.Append(ColorizeString("Feral Charge", Color.white));
                                        sb.Append(ColorizeString(" ability", Color.yellow));
                                        break;
                                    case BoardPieceId.HeroHunter:
                                        sb.Append(ColorizeString("Replace ", Color.yellow));
                                        sb.Append(ColorizeString("1 Arrow", Color.white));
                                        sb.Append(ColorizeString(" with a ", Color.yellow));
                                        sb.Append(ColorizeString("Fire Arrow", Color.white));
                                        break;
                                    case BoardPieceId.HeroBarbarian:
                                        sb.Append(ColorizeString("Gain ", Color.yellow));
                                        sb.Append(ColorizeString("Net", Color.white));
                                        sb.Append(ColorizeString(" ability ", Color.yellow));
                                        sb.Append(ColorizeString("(3 turn cooldown)", orange));
                                        break;
                                    case BoardPieceId.HeroRogue:
                                        sb.Append(ColorizeString("Gain ", Color.yellow));
                                        sb.Append(ColorizeString("Poisonous Bite", Color.white));
                                        sb.Append(ColorizeString(" ability ", Color.yellow));
                                        sb.Append(ColorizeString("(3 turn cooldown)", orange));
                                        break;
                                    case BoardPieceId.HeroBard:
                                        sb.Append(ColorizeString("Gain ", Color.yellow));
                                        sb.Append(ColorizeString("Flash Bomb", Color.white));
                                        sb.Append(ColorizeString(" ability ", Color.yellow));
                                        sb.Append(ColorizeString("(3 turn cooldown)", orange));
                                        break;
                                }

                                sb.Append(ColorizeString(" and ", Color.yellow));
                                switch (myPiece.boardPieceId)
                                {
                                    case BoardPieceId.HeroSorcerer:
                                        sb.Append(ColorizeString("both ", Color.yellow));
                                        sb.Append(ColorizeString("Zap", Color.white));
                                        sb.Append(ColorizeString(" and ", Color.yellow));
                                        sb.Append(ColorizeString("Lightning Bolt", Color.white));
                                        break;
                                    case BoardPieceId.HeroHunter:
                                        sb.Append(ColorizeString("Arrow", Color.white));
                                        break;
                                    case BoardPieceId.HeroBarbarian:
                                        sb.Append(ColorizeString("Grapple", Color.white));
                                        break;
                                    case BoardPieceId.HeroRogue:
                                        sb.Append(ColorizeString("Sneak", Color.white));
                                        break;
                                    case BoardPieceId.HeroBard:
                                        sb.Append(ColorizeString("Courage Shanty", Color.white));
                                        break;
                                }

                                if (myPiece.boardPieceId == BoardPieceId.HeroSorcerer)
                                {
                                    sb.Append(ColorizeString(" become ", Color.yellow));
                                    sb.AppendLine(ColorizeString("FREE Actions", lightgreen));
                                }
                                else if (myPiece.boardPieceId == BoardPieceId.HeroGuardian || myPiece.boardPieceId == BoardPieceId.HeroWarlock)
                                {
                                    sb.Append(ColorizeString("it is a ", Color.yellow));
                                    sb.AppendLine(ColorizeString("FREE Action", lightgreen));
                                }
                                else
                                {
                                    sb.Append(ColorizeString(" becomes a ", Color.yellow));
                                    sb.AppendLine(ColorizeString("FREE Action", lightgreen));
                                }
                            }

                            if (level == 2)
                            {
                                sb.Append(ColorizeString("Gain ", Color.yellow));
                                sb.Append(ColorizeString("1 Max Health", Color.white));
                                sb.Append(ColorizeString(" and critical hits gain ", Color.yellow));
                                sb.AppendLine(ColorizeString("20 gold", Color.white));
                            }

                            if (level == 3)
                            {
                                sb.Append(ColorizeString("Gain ", Color.yellow));
                                sb.Append(ColorizeString("1 Knockdown", Color.white));
                                sb.Append(ColorizeString(" and critical hits restore ", Color.yellow));
                                switch (myPiece.boardPieceId)
                                {
                                    case BoardPieceId.HeroGuardian:
                                        sb.Append(ColorizeString("Pull", Color.white));
                                        sb.Append(ColorizeString(" and ", Color.yellow));
                                        sb.Append(ColorizeString("1 Action Point", Color.white));
                                        break;
                                    case BoardPieceId.HeroSorcerer:
                                        sb.Append(ColorizeString("Zap", Color.white));
                                        break;
                                    case BoardPieceId.HeroWarlock:
                                        sb.Append(ColorizeString("Feral Charge", Color.white));
                                        break;
                                    case BoardPieceId.HeroHunter:
                                        sb.Append(ColorizeString("Arrow", Color.white));
                                        break;
                                    case BoardPieceId.HeroBarbarian:
                                        sb.Append(ColorizeString("Grapple", Color.white));
                                        break;
                                    case BoardPieceId.HeroRogue:
                                        sb.Append(ColorizeString("1 Action Point", Color.white));
                                        break;
                                    case BoardPieceId.HeroBard:
                                        sb.Append(ColorizeString("Courage Shanty", Color.white));
                                        break;
                                }
                            }

                            if (level == 4)
                            {
                                if (myPiece.boardPieceId == BoardPieceId.HeroWarlock || myPiece.boardPieceId == BoardPieceId.HeroSorcerer)
                                {
                                    sb.Append(ColorizeString("Gain ", Color.yellow));
                                    sb.AppendLine(ColorizeString("1 Magic", Color.white));
                                }
                                else
                                {
                                    sb.Append(ColorizeString("Gain ", Color.yellow));
                                    sb.AppendLine(ColorizeString("1 Strength", Color.white));
                                }
                            }

                            if (level == 5)
                            {
                                sb.Append(ColorizeString("Gain ", Color.yellow));
                                sb.AppendLine(ColorizeString("2 Max Health", Color.white));
                            }

                            if (level == 6)
                            {
                                sb.Append(ColorizeString("Gain ", Color.yellow));
                                sb.AppendLine(ColorizeString("2 Swiftness", Color.white));
                            }

                            if (level == 7)
                            {
                                sb.Append(ColorizeString("Gain ", Color.yellow));
                                sb.Append(ColorizeString("1 Knockdown", Color.white));
                                sb.Append(ColorizeString(" and a ", Color.yellow));
                                sb.Append(ColorizeString("Random", Color.white));
                                sb.Append(ColorizeString(" ability ", Color.yellow));
                                sb.Append(ColorizeString("(FREE Action) ", lightgreen));
                                sb.AppendLine(ColorizeString("(7 turn cooldown)", orange));
                            }

                            if (level == 8)
                            {
                                sb.Append(ColorizeString("Gain ", Color.yellow));
                                sb.Append(ColorizeString("1 health regeneration", Color.white));
                                sb.Append(ColorizeString(" per turn ", Color.yellow));
                                sb.AppendLine(ColorizeString("(except if downed, poisoned, or petrified)", orange));
                            }

                            if (level == 9)
                            {
                                sb.Append(ColorizeString("Gain ", Color.yellow));
                                if (myPiece.boardPieceId == BoardPieceId.HeroWarlock || myPiece.boardPieceId == BoardPieceId.HeroSorcerer)
                                {
                                    sb.Append(ColorizeString("2 Magic", Color.white));
                                }
                                else
                                {
                                    sb.Append(ColorizeString("2 Strength", Color.white));
                                }

                                sb.Append(ColorizeString(" and ", Color.yellow));
                                sb.AppendLine(ColorizeString("1 Action Point", Color.white));
                            }
                        }
                    }

                    if (myPiece.HasEffectState(EffectStateType.StrengthInNumbers))
                    {
                        int points = myPiece.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.StrengthInNumbers);
                        var id = myPiece.networkID;
                        sb.AppendLine();
                        sb.Append(ColorizeString("--", Color.gray));
                        sb.Append(ColorizeString($" [ID: {id}]", Color.cyan));
                        sb.Append(ColorizeString(" Character Points: ", Color.yellow));
                        if (points > 0 && points < 999)
                        {
                            sb.Append(ColorizeString($"{points} ", Color.white));
                        }
                        else
                        {
                            sb.Append(ColorizeString($"0 ", Color.white));
                        }

                        sb.AppendLine(ColorizeString("--", Color.gray));
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
                    sb.AppendLine();
                    sb.AppendLine();
                    sb.AppendLine();
                    sb.AppendLine();
                    sb.AppendLine();
                    sb.AppendLine();
                    sb.AppendLine();
                    sb.AppendLine();
                    sb.AppendLine(ColorizeString(" ", Color.clear));
                    GameUI.ShowCameraMessage(sb.ToString(), 10);
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
