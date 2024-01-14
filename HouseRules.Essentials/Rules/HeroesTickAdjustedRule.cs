namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Core;
    using HouseRules.Core.Types;
    using System.Linq;

    public sealed class HeroesTickAdjustedRule : Rule, IConfigWritable<bool>, IPatchable, IMultiplayerSafe, IDisableOnReconnect
    {
        public override string Description => "Handle how the various buffs count down and are removed";

        private static bool _isActivated;

        

        public HeroesTickAdjustedRule(bool value)
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
                original: AccessTools.Method(typeof(StatusEffect), "Tick"),
                prefix: new HarmonyMethod(
                    typeof(HeroesTickAdjustedRule),
                    nameof(StatsusEffect_Tick_Prefix)));
        }

        private static void StatsusEffect_Tick_Prefix(ref StatusEffect __instance)
        {
            if (!_isActivated)
            {
                return;
            }

            var pieceId = Traverse.Create(__instance).Field<int>("sourcePieceId").Value;
            var pieceAndTurnController = Traverse.Create(__instance).Field<PieceAndTurnController>("pieceAndTurnController").Value;
            Piece piece = pieceAndTurnController.GetPiece(pieceId);

            HouseRulesEssentialsBase.LogDebug($"PieceID = {piece.boardPieceId} Has Effect: {__instance.effectStateType} Ticks left: {__instance.DurationTurnsLeft}");

            if (piece == null)
            {
                return;
            }

            if (!piece.IsPlayer())
            {
                return;
            }

            if (__instance.effectStateType == EffectStateType.PVPLimitArenaBurn || __instance.effectStateType == EffectStateType.PVPLimitArenaBurnLv2 || __instance.effectStateType == EffectStateType.PVPLimitArenaBurnLv3 || __instance.effectStateType == EffectStateType.PVPLimitArenaBurnLv4)
            {
                piece.AddGold(0);
            }

            if (__instance.effectStateType == EffectStateType.SummoningSickness)
            {
                piece.ForceSyncState(Traverse.Create(typeof(GameHub)).Field<GameContext>("gameContext").Value);
                piece.AddGold(0);
            }

            // when an effect ticks, we get the piece who's turn it is. then if its a hero with the effect SpellPower.
            if (__instance.effectStateType == EffectStateType.SpellPower)
            {
                HouseRulesEssentialsBase.LogDebug("Detected SpellPower State.");
                HouseRulesEssentialsBase.LogDebug($"PieceID = {piece.boardPieceId} Has Counter: {piece.GetStat(Stats.Type.InnateCounterDirections)}");

                if (piece.GetStat(Stats.Type.InnateCounterDirections) < 1 && !HR.SelectedRuleset.Name.Contains("Heroes "))
                {
                    HouseRulesEssentialsBase.LogDebug("InnateCounterDirections is < 1.");
                    return;
                }

                // Energy Potion tick/prevention and card removal per class
                if (piece.HasEffectState(EffectStateType.SpellPower))
                {
                    // make sure its a hero/player.
                    if (piece.boardPieceId == BoardPieceId.HeroBarbarian || piece.boardPieceId == BoardPieceId.HeroBard || piece.boardPieceId == BoardPieceId.HeroGuardian || piece.boardPieceId == BoardPieceId.HeroHunter || piece.boardPieceId == BoardPieceId.HeroRogue || piece.boardPieceId == BoardPieceId.HeroSorcerer || piece.boardPieceId == BoardPieceId.HeroWarlock)
                    {
                        HouseRulesEssentialsBase.LogDebug("Piece has SpellPower and is a Hero.");

                        var myBool1 = piece.inventory.HasAbility(AbilityKey.SpellPowerPotion);
                        var myPieceID2 = piece.boardPieceId;
                        HouseRulesEssentialsBase.LogDebug($"PieceID = {myPieceID2} Has SpellPowerPotion: {myBool1}");

                        // if the hero used spell power potion, then buff everyone.
                        // if they don't have the card, and their counter is greater than 1.
                        if (!piece.inventory.HasAbility(AbilityKey.SpellPowerPotion) && piece.GetStat(Stats.Type.InnateCounterDirections) >= 1)
                        {
                            // set the effect duration to the counter.
                            // piece.effectSink.SetStatusEffectDuration(EffectStateType.SpellPower, piece.GetStat(Stats.Type.InnateCounterDirections));
                            var myVariable1 = piece.GetStat(Stats.Type.InnateCounterDirections);
                            var myVariable2 = piece.boardPieceId;
                            HouseRulesEssentialsBase.LogDebug($"InnateCounterDirections = {myVariable1} for {myVariable2}");

                            HouseRulesEssentialsBase.LogDebug("Buff all Heroes.");
                            // piece.DisableEffectState(EffectStateType.SpellPower); // test test
                            // piece.AddGold(0);

                            // track the use of group boost.
                            // piece.GetStat(Stats.Type.InnateCounterDirections);
                            // track the use of group boost. give the card and 1 counter.
                            // piece.effectSink.TrySetStatMaxValue(Stats.Type.InnateCounterDirections, 1);

                            // get a list of hero pieces.
                            // GetPieceConfig().HasPieceType(PieceType.Player);
                            // var PlayerPieceIDs = Traverse.Create(pieceAndTurnController.GetPlayerPieceIds(PlayerId hero));
                            var gameContext = Traverse.Create(typeof(GameHub)).Field<GameContext>("gameContext").Value;
                            Piece[] listHeroes = gameContext.pieceAndTurnController.GetPieceByType(PieceType.Player);
                            // Piece[] listHeroes = gameContext.pieceAndTurnController.GetPlayerPieceIds();

                            gameContext.pieceAndTurnController.NotifyNewAdventureFloorLoaded();

                            int heroesListLength = listHeroes.GetLength(0);

                            for (int temp = 0; temp < heroesListLength; temp++)
                            {
                                HouseRulesEssentialsBase.LogDebug($"Hero PieceIDs = slot {temp}: {listHeroes[temp].boardPieceId}");
                            }

                            for (int temp = 0; temp < heroesListLength; temp++)
                            {
                                if (listHeroes[temp].GetStat(Stats.Type.InnateCounterDirections) <= 5)
                                {
                                    // EffectStateType.PVPLimitArenaBurn
                                    if (listHeroes[temp].HasEffectState(EffectStateType.PVPLimitArenaBurnLv4))
                                    {
                                        piece.DisableEffectState(EffectStateType.SpellPower);
                                        HouseRulesEssentialsBase.LogDebug("Remove SpellPower when this hero get Burn V");
                                        listHeroes[temp].DisableEffectState(EffectStateType.Wet);
                                        listHeroes[temp].EnableEffectState(EffectStateType.PVPLimitArenaBurnLv4);
                                        piece.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDirections, 0);
                                        HouseRulesEssentialsBase.LogDebug($"Hero PieceIDs: {listHeroes[temp].boardPieceId} Has Level 4 Burn.");
                                        piece.AddGold(0);
                                    }
                                    else if (listHeroes[temp].HasEffectState(EffectStateType.PVPLimitArenaBurnLv3))
                                    {
                                        piece.DisableEffectState(EffectStateType.SpellPower);
                                        HouseRulesEssentialsBase.LogDebug("Remove SpellPower when this hero get Burn IV");
                                        listHeroes[temp].DisableEffectState(EffectStateType.PVPLimitArenaBurnLv3);
                                        listHeroes[temp].EnableEffectState(EffectStateType.PVPLimitArenaBurnLv4);
                                        listHeroes[temp].DisableEffectState(EffectStateType.Wet);
                                        if (listHeroes[temp].boardPieceId == BoardPieceId.HeroGuardian)
                                        {
                                            listHeroes[temp].TryAddAbilityToInventory(AbilityKey.LuckPotion); // guardian already has zap which will become LightningBolt.
                                        }
                                        else
                                        {
                                            listHeroes[temp].TryAddAbilityToInventory(AbilityKey.LightningBolt); // grant powerful ability.
                                        }

                                        piece.AddGold(0);
                                    }
                                    else if (listHeroes[temp].HasEffectState(EffectStateType.PVPLimitArenaBurnLv2))
                                    {
                                        piece.DisableEffectState(EffectStateType.SpellPower);
                                        HouseRulesEssentialsBase.LogDebug("Remove SpellPower when this hero get Burn III");
                                        listHeroes[temp].DisableEffectState(EffectStateType.PVPLimitArenaBurnLv2);
                                        listHeroes[temp].EnableEffectState(EffectStateType.PVPLimitArenaBurnLv3);
                                        listHeroes[temp].DisableEffectState(EffectStateType.Wet);
                                        listHeroes[temp].RestoreReplenishableAbility(AbilityKey.Zap); // just in case the guardian used their zap.
                                        listHeroes[temp].EnableEffectState(EffectStateType.Overcharge);
                                        piece.AddGold(0);
                                    }
                                    else if (listHeroes[temp].HasEffectState(EffectStateType.PVPLimitArenaBurn))
                                    {
                                        piece.DisableEffectState(EffectStateType.SpellPower);
                                        HouseRulesEssentialsBase.LogDebug("Remove SpellPower when this hero get Burn II");
                                        listHeroes[temp].DisableEffectState(EffectStateType.PVPLimitArenaBurn);
                                        listHeroes[temp].EnableEffectState(EffectStateType.PVPLimitArenaBurnLv2);
                                        listHeroes[temp].EnableEffectState(EffectStateType.Recovery);
                                        piece.AddGold(0);
                                    }
                                    else
                                    {
                                        piece.DisableEffectState(EffectStateType.SpellPower);
                                        HouseRulesEssentialsBase.LogDebug("Remove SpellPower when this hero get Burn I");
                                        listHeroes[temp].EnableEffectState(EffectStateType.PVPLimitArenaBurn);
                                        listHeroes[temp].EnableEffectState(EffectStateType.MagicShield);
                                        piece.AddGold(0);
                                    }

                                    // increase the counter by 1.
                                    var countCurrent = 0;
                                    listHeroes[temp].effectSink.TryGetStat(Stats.Type.InnateCounterDirections, out countCurrent);
                                    if (countCurrent != 0)
                                    {
                                        listHeroes[temp].effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDirections, countCurrent + 1);
                                        HouseRulesEssentialsBase.LogDebug("Increasing Counter by one.");
                                        piece.AddGold(0);
                                    }
                                }
                                else
                                {
                                    // they should be at 4. reset the counter to zero.
                                    piece.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDirections, 0);
                                    HouseRulesEssentialsBase.LogDebug($"Ending counter, {myVariable1} for {myVariable2}");
                                    piece.AddGold(0);
                                }

                                // var myVariable3 = listHeroes[temp].HasEffectState(EffectStateType.PVPLimitArenaBurn);
                                // var myVariable4 = listHeroes[temp].boardPieceId;
                                // HouseRulesEssentialsBase.LogDebug("InnateCounterDirections is > 0 attempting to apply Burn Effects.");
                                // HouseRulesEssentialsBase.LogDebug($"{myVariable4} has Burn {myVariable3}");
                            }
                        }

                        // decrease the counter by 1.
                        var countCurrent2 = 0;
                        piece.effectSink.TryGetStat(Stats.Type.InnateCounterDirections, out countCurrent2);
                        if (countCurrent2 > 0)
                        {
                            piece.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDirections, countCurrent2 - 1);
                            HouseRulesEssentialsBase.LogDebug($"Decreasing counter to {countCurrent2 - 1}");
                            piece.AddGold(0);
                        }
                    }
                }
            }
        }
    }
}
