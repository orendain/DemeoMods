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

            // display in the log every buff as it ticks.

            if (piece == null)
            {
                return;
            }

            if (!HR.SelectedRuleset.Name.Contains("Heroes "))
            {
                return;
            }

            // HouseRulesEssentialsBase.LogDebug($"PieceID = {piece.boardPieceId} Has Effect: {__instance.effectStateType} Ticks left: {__instance.DurationTurnsLeft}");
            //  if (__instance.effectStateType == EffectStateType.SummoningSickness)
            //  {
            //    piece.ForceSyncState(Traverse.Create(typeof(GameHub)).Field<GameContext>("gameContext").Value);
            //    piece.AddGold(0);
            //  }

            // for Insanity random monster buffs.
            if (HR.SelectedRuleset.Name.Contains("INSANE") && (__instance.effectStateType == EffectStateType.Deflect || __instance.effectStateType == EffectStateType.Courageous || __instance.effectStateType == EffectStateType.Frenzy || __instance.effectStateType == EffectStateType.Recovery || __instance.effectStateType == EffectStateType.MagicShield))
            {
                if (piece.HasPieceType(PieceType.Creature) && !piece.HasEffectState(EffectStateType.Confused) && !piece.HasPieceType(PieceType.ExplodingLamp) && piece.boardPieceId != BoardPieceId.Verochka && piece.boardPieceId != BoardPieceId.WarlockMinion)
                {
                    if (piece.effectSink.GetEffectStateDurationTurnsLeft(__instance.effectStateType) > 53)
                    {
                        // remove the current effect/buff.
                        piece.DisableEffectState(__instance.effectStateType);

                        // set a new random effect/buff.
                        int nextPhase = UnityEngine.Random.Range(1, 6);
                        if (__instance.effectStateType == EffectStateType.Deflect || __instance.effectStateType == EffectStateType.MagicShield)
                        {
                            nextPhase = UnityEngine.Random.Range(2, 5);
                        }

                        switch (nextPhase)
                        {
                            case 1:
                                piece.EnableEffectState(EffectStateType.Deflect, 55);
                                break;
                            case 2:
                                piece.EnableEffectState(EffectStateType.Courageous, 55);
                                break;
                            case 3:
                                piece.EnableEffectState(EffectStateType.Frenzy, 55);
                                break;
                            case 4:
                                piece.EnableEffectState(EffectStateType.Recovery, 55);
                                break;
                            case 5:
                                piece.EnableEffectState(EffectStateType.MagicShield, 55);
                                break;
                        }
                    }
                }
            }

            if (!piece.IsPlayer())
            {
                return;
            }

            piece.effectSink.TryGetStat(Stats.Type.InnateCounterDirections, out int currentCount);
            // get a list of all heroes/players.
            var gameContext = Traverse.Create(typeof(GameHub)).Field<GameContext>("gameContext").Value;
            Piece[] listHeroes = gameContext.pieceAndTurnController.GetPieceByType(PieceType.Player);

            // Overcharge check
            if (__instance.effectStateType == EffectStateType.Overcharge && piece.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.Overcharge) == 3)
            {
                // if the hero has the maximum Overcharge effect, set the counter to be active, buff everyone.
                if (currentCount < 1)
                {
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDirections, 6);
                    currentCount = 6;
                }

                if (currentCount > 0)
                {
                    HouseRulesEssentialsBase.LogDebug("Buffing all Heroes.");

                    int heroesListLength = listHeroes.GetLength(0);
                    for (int temp = 0; temp < heroesListLength; temp++)
                    {
                        if (listHeroes[temp].HasEffectState(EffectStateType.ElvenHand3RespawnCooldown))
                        {
                            // fourth stage.
                            HouseRulesEssentialsBase.LogDebug($"Hero # {temp}: {listHeroes[temp].boardPieceId} is gaining 4th stage power up!");
                            listHeroes[temp].DisableEffectState(EffectStateType.ElvenHand3RespawnCooldown);
                            listHeroes[temp].TryAddAbilityToInventory(AbilityKey.ScrollElectricity); // grant powerful ability.
                            listHeroes[temp].effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDirections, 0);
                            currentCount = 0;
                            HouseRulesEssentialsBase.LogDebug($"Decreasing buff counter to 0.");
                            listHeroes[temp].AddGold(0);
                        }
                        else if (listHeroes[temp].HasEffectState(EffectStateType.ElvenHand2RespawnCooldown))
                        {
                            // third stage.
                            HouseRulesEssentialsBase.LogDebug($"Hero # {temp}: {listHeroes[temp].boardPieceId} is gaining 3rd stage power up!");
                            listHeroes[temp].DisableEffectState(EffectStateType.ElvenHand2RespawnCooldown);
                            listHeroes[temp].SetStatBaseValue(Stats.Type.MagicArmor, listHeroes[temp].GetStat(Stats.Type.MagicArmor) + 3.0f);
                            listHeroes[temp].EnableEffectState(EffectStateType.ElvenHand3RespawnCooldown, 3);
                            listHeroes[temp].effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDirections, currentCount - 1);
                            HouseRulesEssentialsBase.LogDebug($"Decreasing buff counter to {currentCount - 1}.");
                            if (currentCount - 1 < 1)
                            {
                                listHeroes[temp].DisableEffectState(EffectStateType.ElvenHand3RespawnCooldown);
                            }

                            listHeroes[temp].AddGold(0);
                        }
                        else if (listHeroes[temp].HasEffectState(EffectStateType.ElvenHand1RespawnCooldown))
                        {
                            // second stage.
                            HouseRulesEssentialsBase.LogDebug($"Hero # {temp}: {listHeroes[temp].boardPieceId} is gaining 2nd stage power up!");
                            listHeroes[temp].DisableEffectState(EffectStateType.ElvenHand1RespawnCooldown);
                            listHeroes[temp].EnableEffectState(EffectStateType.Recovery, 3);
                            listHeroes[temp].EnableEffectState(EffectStateType.ElvenHand2RespawnCooldown, 3);
                            listHeroes[temp].effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDirections, currentCount - 1);
                            HouseRulesEssentialsBase.LogDebug($"Decreasing buff counter to {currentCount - 1}.");
                            if (currentCount - 1 < 1)
                            {
                                listHeroes[temp].DisableEffectState(EffectStateType.ElvenHand2RespawnCooldown);
                            }

                            listHeroes[temp].AddGold(0);
                        }
                        else
                        {
                            // first stage.
                            if (listHeroes[temp].GetStat(Stats.Type.InnateCounterDirections) < 1)
                            {
                                listHeroes[temp].SetStatBaseValue(Stats.Type.InnateCounterDirections, piece.GetStat(Stats.Type.InnateCounterDirections));
                            }

                            HouseRulesEssentialsBase.LogDebug($"Hero # {temp}: {listHeroes[temp].boardPieceId} is gaining 1st stage power up!");
                            listHeroes[temp].effectSink.RemoveStatusEffect(EffectStateType.MagicShield);
                            listHeroes[temp].DisableEffectState(EffectStateType.MagicShield);
                            listHeroes[temp].EnableEffectState(EffectStateType.MagicShield);
                            listHeroes[temp].effectSink.SetStatusEffectDuration(EffectStateType.MagicShield, 3);
                            listHeroes[temp].EnableEffectState(EffectStateType.ElvenHand1RespawnCooldown, 3);
                            listHeroes[temp].effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDirections, currentCount - 1);
                            HouseRulesEssentialsBase.LogDebug($"Decreasing buff counter to {currentCount - 1}.");
                            if (currentCount - 1 < 1)
                            {
                                listHeroes[temp].DisableEffectState(EffectStateType.ElvenHand1RespawnCooldown);
                            }

                            listHeroes[temp].AddGold(0);
                        }
                    }
                }
            }
            else if (__instance.effectStateType == EffectStateType.Overcharge && currentCount > 0)
            {
                // decrease the counter by 1.
                currentCount--;
                HouseRulesEssentialsBase.LogDebug($"Decreasing buff counter to {currentCount}.");
                if (currentCount < 1)
                {
                    int heroesListLength = listHeroes.GetLength(0);
                    for (int temp = 0; temp < heroesListLength; temp++)
                    {
                        listHeroes[temp].effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDirections, 0);
                        listHeroes[temp].DisableEffectState(EffectStateType.ElvenHand1RespawnCooldown);
                        listHeroes[temp].DisableEffectState(EffectStateType.ElvenHand2RespawnCooldown);
                        listHeroes[temp].DisableEffectState(EffectStateType.ElvenHand3RespawnCooldown);
                    }
                }
                else
                {
                    int heroesListLength = listHeroes.GetLength(0);
                    for (int temp = 0; temp < heroesListLength; temp++)
                    {
                        listHeroes[temp].effectSink.TrySetStatBaseValue(Stats.Type.InnateCounterDirections, currentCount);
                    }
                }
            }
        }
    }
}
