namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Core;
    using HouseRules.Core.Types;

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

            // gets the piece ID associated with this Effect. Then gets the turn controller for the Effect. Then creates a piece for the current piece's turn.
            // How do we know that the piece whose turn it is is the piece with the current Effect?
            var pieceId = Traverse.Create(__instance).Field<int>("sourcePieceId").Value;
            var pieceAndTurnController = Traverse.Create(__instance).Field<PieceAndTurnController>("pieceAndTurnController").Value;
            Piece piece = pieceAndTurnController.GetPiece(pieceId);

            HouseRulesEssentialsBase.LogDebug($"PieceID = {piece.boardPieceId} Has Effect: {__instance.effectStateType} Ticks left: {__instance.DurationTurnsLeft}");

            if (piece == null)
            {
                return;
            }

            //  if (__instance.effectStateType == EffectStateType.SummoningSickness)
            //  {
            //    piece.ForceSyncState(Traverse.Create(typeof(GameHub)).Field<GameContext>("gameContext").Value);
            //    piece.AddGold(0);
            //  }

            // for Insanity random monster buffs.
            if (HR.SelectedRuleset.Name.Contains("Heroes ") && HR.SelectedRuleset.Name.Contains("Insane") && (__instance.effectStateType == EffectStateType.Deflect || __instance.effectStateType == EffectStateType.MagicShield || __instance.effectStateType == EffectStateType.Invisibility || __instance.effectStateType == EffectStateType.Recovery || __instance.effectStateType == EffectStateType.Courageous))
            {
                if (piece.HasEffectState(__instance.effectStateType) && piece.effectSink.GetEffectStateDurationTurnsLeft(__instance.effectStateType) > 53)
                {
                    // remove the current effect/buff.
                    piece.DisableEffectState(__instance.effectStateType);

                    // set a new random effect/buff.
                    int nextPhase = UnityEngine.Random.Range(1, 6);
                    switch (nextPhase)
                    {
                        case 1:
                            piece.EnableEffectState(EffectStateType.Deflect, 55);
                            break;
                        case 2:
                            piece.EnableEffectState(EffectStateType.MagicShield, 55);
                            break;
                        case 3:
                            piece.EnableEffectState(EffectStateType.Invisibility, 55);
                            break;
                        case 4:
                            piece.EnableEffectState(EffectStateType.Recovery, 55);
                            break;
                        case 5:
                            piece.EnableEffectState(EffectStateType.Courageous, 55);
                            break;
                    }
                }
            }

            if (!piece.IsPlayer())
            {
                return;
            }

            // ElvenHand1RespawnCooldown
            if (piece.IsPlayer() && __instance.effectStateType == EffectStateType.ElvenHand1RespawnCooldown && HR.SelectedRuleset.Name.Contains("Heroes "))
            {
                // make sure its a hero/player. // if (piece.IsPlayer())
                if (piece.boardPieceId == BoardPieceId.HeroBarbarian || piece.boardPieceId == BoardPieceId.HeroBard || piece.boardPieceId == BoardPieceId.HeroGuardian || piece.boardPieceId == BoardPieceId.HeroHunter || piece.boardPieceId == BoardPieceId.HeroRogue || piece.boardPieceId == BoardPieceId.HeroSorcerer || piece.boardPieceId == BoardPieceId.HeroWarlock)
                {
                    HouseRulesEssentialsBase.LogDebug($"Player {piece.boardPieceId} has ElvenHand1RespawnCooldown, counter = {piece.GetStat(Stats.Type.InnateCounterDirections)}.");

                    // if the hero used spell power potion, then they have the effect, check if the counter is still active, buff everyone.
                    if (piece.GetStat(Stats.Type.InnateCounterDirections) >= 1)
                    {
                        // set the effect duration to the counter.
                        piece.effectSink.SetStatusEffectDuration(EffectStateType.Overcharge, piece.GetStat(Stats.Type.InnateCounterDirections));
                        HouseRulesEssentialsBase.LogDebug("Removing ElvenHand1RespawnCooldown effect and Buffing all Heroes.");
                        // piece.DisableEffectState(EffectStateType.SpellPower); apply after dissipate, keeps adding it again.
                        piece.effectSink.RemoveStatusEffect(EffectStateType.ElvenHand1RespawnCooldown);

                        // get a list of all heroes/players.
                        var gameContext = Traverse.Create(typeof(GameHub)).Field<GameContext>("gameContext").Value;
                        Piece[] listHeroes = gameContext.pieceAndTurnController.GetPieceByType(PieceType.Player);

                        int heroesListLength = listHeroes.GetLength(0);
                        for (int temp = 0; temp < heroesListLength; temp++)
                        {
                            if (listHeroes[temp].GetStat(Stats.Type.InnateCounterDirections) > 0)
                            {
                                if (listHeroes[temp].HasEffectState(EffectStateType.ElvenHand3RespawnCooldown))
                                {
                                    // fourth stage.
                                    HouseRulesEssentialsBase.LogDebug($"Hero # {temp}: {listHeroes[temp].boardPieceId} is gaining 4th stage power up!");
                                    listHeroes[temp].DisableEffectState(EffectStateType.ElvenHand3RespawnCooldown);
                                    listHeroes[temp].DisableEffectState(EffectStateType.Wet);
                                    listHeroes[temp].EnableEffectState(EffectStateType.Overcharge, listHeroes[temp].GetStat(Stats.Type.InnateCounterDirections));
                                    listHeroes[temp].EnableEffectState(EffectStateType.ElvenHand4RespawnCooldown, 1);
                                    listHeroes[temp].TryAddAbilityToInventory(AbilityKey.LightningBolt); // grant powerful ability.
                                    //if (listHeroes[temp].boardPieceId == BoardPieceId.HeroGuardian)
                                    //{
                                    //    listHeroes[temp].TryAddAbilityToInventory(AbilityKey.LuckPotion); // guardian already has zap which will become LightningBolt.
                                    //}
                                    //else
                                    //{
                                    //    listHeroes[temp].TryAddAbilityToInventory(AbilityKey.LightningBolt); // grant powerful ability.
                                    //}

                                    listHeroes[temp].effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDirections, 0);
                                    HouseRulesEssentialsBase.LogDebug($"Hero # {temp}: {listHeroes[temp].boardPieceId} counter is reset to 0.");
                                    listHeroes[temp].AddGold(0);
                                }
                                else if (listHeroes[temp].HasEffectState(EffectStateType.ElvenHand2RespawnCooldown))
                                {
                                    // third stage.
                                    HouseRulesEssentialsBase.LogDebug($"Hero # {temp}: {listHeroes[temp].boardPieceId} is gaining 3rd stage power up!");
                                    listHeroes[temp].DisableEffectState(EffectStateType.ElvenHand2RespawnCooldown);
                                    listHeroes[temp].SetStatBaseValue(Stats.Type.MagicArmor, listHeroes[temp].GetStat(Stats.Type.MagicArmor) + 3.0f);
                                    listHeroes[temp].EnableEffectState(EffectStateType.ElvenHand3RespawnCooldown, 3);
                                    listHeroes[temp].AddGold(0);
                                }
                                else if (listHeroes[temp].HasEffectState(EffectStateType.ElvenHand1RespawnCooldown))
                                {
                                    // second stage.
                                    HouseRulesEssentialsBase.LogDebug($"Hero # {temp}: {listHeroes[temp].boardPieceId} is gaining 2nd stage power up!");
                                    listHeroes[temp].DisableEffectState(EffectStateType.ElvenHand1RespawnCooldown);
                                    listHeroes[temp].EnableEffectState(EffectStateType.Recovery, 3);
                                    listHeroes[temp].EnableEffectState(EffectStateType.ElvenHand2RespawnCooldown, 3);
                                    listHeroes[temp].AddGold(0);
                                }
                                else
                                {
                                    // first stage.
                                    HouseRulesEssentialsBase.LogDebug($"Hero # {temp}: {listHeroes[temp].boardPieceId} is gaining 1st stage power up!");
                                    listHeroes[temp].DisableEffectState(EffectStateType.Stunned);
                                    listHeroes[temp].DisableEffectState(EffectStateType.StunSelf);
                                    listHeroes[temp].DisableEffectState(EffectStateType.Wet);
                                    listHeroes[temp].RestoreReplenishableAbility(AbilityKey.Zap); // just in case the guardian used their zap.
                                    listHeroes[temp].EnableEffectState(EffectStateType.Overcharge, 3);
                                    listHeroes[temp].EnableEffectState(EffectStateType.ElvenHand1RespawnCooldown, 3);
                                    listHeroes[temp].AddGold(0);
                                }
                            }
                            else
                            {
                                // they should be at 0. reset the counter to zero.
                                piece.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDirections, 0);
                                HouseRulesEssentialsBase.LogDebug($"Ending counter, {piece.GetStat(Stats.Type.InnateCounterDirections)} for {piece.boardPieceId}");
                            }
                        }
                    }

                    // decrease the counter by 1.
                    piece.effectSink.TryGetStat(Stats.Type.InnateCounterDirections, out int countCurrent2);
                    if (countCurrent2 > 0)
                    {
                        piece.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDirections, countCurrent2 - 1);
                        HouseRulesEssentialsBase.LogDebug($"Decreasing {piece.boardPieceId} counter to {countCurrent2 - 1}");
                    }
                }
            }

            // when an effect ticks, we get the piece who's turn it is. then if its a hero with the effect Oversharge.
            if (__instance.effectStateType == EffectStateType.SpellPower && HR.SelectedRuleset.Name.Contains("Heroes "))
            {
                HouseRulesEssentialsBase.LogDebug("Detected SpellPower State.");
                HouseRulesEssentialsBase.LogDebug($"PieceID = {piece.boardPieceId} Has Counter: {piece.GetStat(Stats.Type.InnateCounterDirections)}");

                if (piece.GetStat(Stats.Type.InnateCounterDirections) < 1)
                {
                    HouseRulesEssentialsBase.LogDebug("InnateCounterDirections is < 1.");
                    piece.SetStatBaseValue(Stats.Type.InnateCounterDirections, 0);
                    piece.effectSink.RemoveStatusEffect(EffectStateType.SpellPower);
                    bool success = !piece.effectSink.HasEffectState(EffectStateType.SpellPower);
                    HouseRulesEssentialsBase.LogDebug($"Player {piece.boardPieceId} removed SpellPower ({success}), counter rest to {piece.GetStat(Stats.Type.InnateCounterDirections)}.");

                    return;
                }
            }
        }
    }
}
