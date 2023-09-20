namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.BoardEntities.Abilities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Core;
    using HouseRules.Core.Types;

    public sealed class PieceProgressLostRule : Rule, IConfigWritable<bool>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Hero loses a level if revived without using magic or potion";

        private static bool _isActivated;

        public PieceProgressLostRule(bool value)
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
                original: AccessTools.Method(typeof(MotherTracker), "TrackRevive"),
                prefix: new HarmonyMethod(
                    typeof(PieceProgressLostRule),
                    nameof(MotherTracker_TrackRevive_Prefix)));
        }

        private static void MotherTracker_TrackRevive_Prefix(Piece revivedPiece, AbilityKey sourceAbility)
        {
            if (!_isActivated)
            {
                return;
            }

            var ruleSet = HR.SelectedRuleset.Name;
            if (!ruleSet.Contains("PROGRESSIVE") && !ruleSet.Equals("TEST GAME"))
            {
                return;
            }

            // If magic, a potion, or a fountain was used then don't lose a level (except on LEGENDARY)
            if (!ruleSet.Contains("(LEGENDARY"))
            {
                if (sourceAbility != AbilityKey.Revive)
                {
                    return;
                }
            }

            Piece piece = revivedPiece;
            int level = piece.GetStatMax(Stats.Type.CritChance);

            if (level < 2)
            {
                return;
            }

            level -= 1;
            piece.effectSink.TrySetStatMaxValue(Stats.Type.CritChance, level);
            piece.effectSink.SetStatusEffectDuration(EffectStateType.Flying, level);
            piece.DisableEffectState(EffectStateType.Corruption);
            piece.EnableEffectState(EffectStateType.Corruption, 1);
            piece.effectSink.AddStatusEffect(EffectStateType.ConfusedPermanentVisualOnly, -1);
            var charType = piece.boardPieceId;
            string textName = "Player";
            switch (charType)
            {
                case BoardPieceId.HeroGuardian:
                    textName = "Guardian";
                    break;
                case BoardPieceId.HeroHunter:
                    textName = "Hunter";
                    break;
                case BoardPieceId.HeroRogue:
                    textName = "Assassin";
                    break;
                case BoardPieceId.HeroSorcerer:
                    textName = "Sorcerer";
                    break;
                case BoardPieceId.HeroBard:
                    textName = "Bard";
                    break;
                case BoardPieceId.HeroWarlock:
                    textName = "Warlock";
                    break;
                case BoardPieceId.HeroBarbarian:
                    textName = "Barbarian";
                    break;
            }

            GameUI.ShowCameraMessage($"<color=#F0F312>This</color> {textName} <color=#F0F312>has</color> <color=#FF1C06>LOST</color> <color=#F0F312>a level!</color>", 8);
            if (level == 1)
            {
                if (piece.boardPieceId == BoardPieceId.HeroHunter)
                {
                    Inventory.Item value;
                    for (var i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.EnemyFireball)
                        {
                            piece.inventory.Items.Remove(value);
                            break;
                        }
                    }

                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.Arrow,
                        flags = (Inventory.ItemFlag)1,
                        originalOwner = -1,
                        replenishCooldown = 1,
                    });

                    piece.AddGold(0);
                }
                else if (piece.boardPieceId == BoardPieceId.HeroSorcerer)
                {
                    int overcharge = 0;
                    Inventory.Item value;
                    if (piece.HasEffectState(EffectStateType.Overcharge))
                    {
                        overcharge = piece.effectSink.GetEffectStateDurationTurnsLeft(EffectStateType.Overcharge);
                        piece.effectSink.RemoveStatusEffect(EffectStateType.Overcharge);
                    }

                    for (var i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.Electricity)
                        {
                            piece.inventory.Items.Remove(value);
                            break;
                        }
                    }

                    piece.effectSink.RemoveStatusEffect(EffectStateType.Overcharge);
                    piece.inventory.Items.Add(new Inventory.Item
                    {
                        abilityKey = AbilityKey.Overcharge,
                        flags = (Inventory.ItemFlag)1,
                        originalOwner = -1,
                        replenishCooldown = 1,
                    });

                    if (overcharge > 0)
                    {
                        piece.effectSink.AddStatusEffect(EffectStateType.Overcharge);
                        piece.effectSink.SetStatusEffectDuration(EffectStateType.Overcharge, overcharge);
                    }

                    piece.AddGold(0);
                }
                else if (piece.boardPieceId == BoardPieceId.HeroBarbarian)
                {
                    Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                    Inventory.Item value;
                    for (var i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.Net)
                        {
                            piece.inventory.Items.Remove(value);
                            break;
                        }
                    }

                    piece.AddGold(0);
                }
                else if (piece.boardPieceId == BoardPieceId.HeroBard)
                {
                    Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                    Inventory.Item value;
                    for (var i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.EnemyFlashbang)
                        {
                            piece.inventory.Items.Remove(value);
                            break;
                        }
                    }

                    piece.AddGold(0);

                    AbilityFactory.TryGetAbility(AbilityKey.CourageShanty, out var ability);
                }
                else if (piece.boardPieceId == BoardPieceId.HeroGuardian)
                {
                    Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                    Inventory.Item value;
                    for (var i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.Grab)
                        {
                            piece.inventory.Items.Remove(value);
                            break;
                        }
                    }

                    piece.AddGold(0);
                }
                else if (piece.boardPieceId == BoardPieceId.HeroRogue)
                {
                    Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                    Inventory.Item value;
                    for (var i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.DiseasedBite)
                        {
                            piece.inventory.Items.Remove(value);
                            break;
                        }
                    }

                    piece.AddGold(0);
                }
                else if (piece.boardPieceId == BoardPieceId.HeroWarlock)
                {
                    Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                    Inventory.Item value;
                    for (var i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.MinionCharge)
                        {
                            piece.inventory.Items.Remove(value);
                            break;
                        }
                    }

                    piece.AddGold(0);
                }
            }
            else if (level == 2)
            {
                piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, piece.GetMaxHealth() - 1);
            }
            else if (level == 3)
            {
                if (piece.GetStat(Stats.Type.DownedCounter) < 3)
                {
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.DownedCounter, piece.GetStat(Stats.Type.DownedCounter) + 1);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.DownedTimer, piece.GetStat(Stats.Type.DownedTimer) - 1);
                }
            }
            else if (level == 4)
            {
                if (piece.boardPieceId == BoardPieceId.HeroSorcerer || piece.boardPieceId == BoardPieceId.HeroWarlock)
                {
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.MagicBonus, piece.GetStat(Stats.Type.MagicBonus) - 1);
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.MagicBonus, piece.GetStatMax(Stats.Type.MagicBonus) - 1);
                }
                else
                {
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.Strength, piece.GetStat(Stats.Type.Strength) - 1);
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Strength, piece.GetStatMax(Stats.Type.Strength) - 1);
                }
            }
            else if (level == 5)
            {
                piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, piece.GetMaxHealth() - 2);
            }
            else if (level == 6)
            {
                piece.effectSink.TrySetStatBaseValue(Stats.Type.Speed, piece.GetStat(Stats.Type.Speed) - 2);
                piece.effectSink.TrySetStatMaxValue(Stats.Type.Speed, piece.GetStatMax(Stats.Type.Speed) - 2);
            }
            else if (level == 7)
            {
                if (piece.GetStat(Stats.Type.DownedCounter) < 3)
                {
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.DownedCounter, piece.GetStat(Stats.Type.DownedCounter) + 1);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.DownedTimer, piece.GetStat(Stats.Type.DownedTimer) - 1);
                }

                Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                Inventory.Item value;
                for (var i = 0; i < piece.inventory.Items.Count; i++)
                {
                    value = piece.inventory.Items[i];
                    if (value.abilityKey == AbilityKey.Petrify || value.abilityKey == AbilityKey.DropChest || value.abilityKey == AbilityKey.DeathFlurry || value.abilityKey == AbilityKey.Shockwave || value.abilityKey == AbilityKey.AcidSpit)
                    {
                        piece.inventory.Items.Remove(value);
                        break;
                    }
                }

                piece.AddGold(0);
            }
            else if (level == 9)
            {
                if (piece.boardPieceId == BoardPieceId.HeroSorcerer || piece.boardPieceId == BoardPieceId.HeroWarlock)
                {
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.MagicBonus, piece.GetStat(Stats.Type.MagicBonus) - 2);
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.MagicBonus, piece.GetStatMax(Stats.Type.MagicBonus) - 2);
                }
                else
                {
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.Strength, piece.GetStat(Stats.Type.Strength) - 2);
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Strength, piece.GetStatMax(Stats.Type.Strength) - 2);
                }

                piece.effectSink.TryGetStat(Stats.Type.ActionPoints, out int currentAP);
                if (currentAP > 0)
                {
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.ActionPoints, currentAP - 1);
                }
            }
        }
    }
}
