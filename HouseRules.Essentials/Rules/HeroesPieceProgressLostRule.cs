namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.BoardEntities.Abilities;
    using Boardgame.BoardEntities.AI;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Core;
    using HouseRules.Core.Types;

    public sealed class HeroesPieceProgressLostRule : Rule, IConfigWritable<bool>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Hero loses a level if revived without using magic or potion";

        private static bool _isActivated;

        public HeroesPieceProgressLostRule(bool value)
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
                    typeof(HeroesPieceProgressLostRule),
                    nameof(MotherTracker_TrackRevive_Prefix)));
        }

        private static void MotherTracker_TrackRevive_Prefix(Piece revivedPiece, AbilityKey sourceAbility)
        {
            if (!_isActivated)
            {
                return;
            }

            var ruleSet = HR.SelectedRuleset.Name;
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
                piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, piece.GetMaxHealth() - 1);

                if (piece.boardPieceId == BoardPieceId.HeroBarbarian)
                {
                    AbilityFactory.TryGetAbility(AbilityKey.TauntingScream, out var ability);
                    ability.costActionPoint = true;
                }
                else if (piece.boardPieceId == BoardPieceId.HeroBard)
                {
                    AbilityFactory.TryGetAbility(AbilityKey.CourageShanty, out var ability);
                    ability.costActionPoint = true;
                }
                else if (piece.boardPieceId == BoardPieceId.HeroGuardian)
                {
                    AbilityFactory.TryGetAbility(AbilityKey.BlindingLight, out var ability);
                    ability.costActionPoint = true;
                }
                else if (piece.boardPieceId == BoardPieceId.HeroRogue)
                {
                    AbilityFactory.TryGetAbility(AbilityKey.Sneak, out var ability);
                    ability.costActionPoint = true;
                }
                else if (piece.boardPieceId == BoardPieceId.HeroHunter)
                {
                    AbilityFactory.TryGetAbility(AbilityKey.Whip, out var ability);
                    ability.costActionPoint = true;
                }
                else if (piece.boardPieceId == BoardPieceId.HeroSorcerer)
                {
                    AbilityFactory.TryGetAbility(AbilityKey.SnakeBossLongRange, out var ability);
                    ability.costActionPoint = true;
                }
                else if (piece.boardPieceId == BoardPieceId.HeroWarlock)
                {
                    AbilityFactory.TryGetAbility(AbilityKey.MagicMissile, out var ability);
                    ability.costActionPoint = true;
                }

                piece.AddGold(0);
            }
            else if (level == 2)
            {
                piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, piece.GetMaxHealth() - 1);

                if (piece.boardPieceId == BoardPieceId.HeroBarbarian)
                {
                    Inventory.Item value;
                    for (var i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.EnemyJavelin)
                        {
                            piece.inventory.Items.Remove(value);
                            Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                            break;
                        }
                    }

                    piece.AddGold(0);
                }
                else if (piece.boardPieceId == BoardPieceId.HeroBard)
                {
                    Inventory.Item value;
                    for (var i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.TeleportLamp)
                        {
                            piece.inventory.Items.Remove(value);
                            Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                            break;
                        }
                    }

                    piece.AddGold(0);
                }
                else if (piece.boardPieceId == BoardPieceId.HeroGuardian)
                {
                    Inventory.Item value;
                    for (var i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.Zap)
                        {
                            piece.inventory.Items.Remove(value);
                            Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                        }
                        else if (value.abilityKey == AbilityKey.TurretHealProjectile)
                        {
                            piece.inventory.Items.Remove(value);
                            Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                        }
                    }

                    piece.AddGold(0);
                }
                else if (piece.boardPieceId == BoardPieceId.HeroRogue)
                {
                    Inventory.Item value;
                    for (var i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.EnemyFireball)
                        {
                            piece.inventory.Items.Remove(value);
                            Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                        }
                        else if (value.abilityKey == AbilityKey.DiseasedBite)
                        {
                            piece.inventory.Items.Remove(value);
                            Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                        }
                    }

                    piece.AddGold(0);
                }
                else if (piece.boardPieceId == BoardPieceId.HeroHunter)
                {
                    Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                    Inventory.Item value;
                    for (var i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.TornadoCharge)
                        {
                            piece.inventory.Items.Remove(value);
                            Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                        }
                        else if (value.abilityKey == AbilityKey.WaterDive)
                        {
                            piece.inventory.Items.Remove(value);
                            Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                        }
                    }

                    piece.AddGold(0);
                }
                else if (piece.boardPieceId == BoardPieceId.HeroSorcerer)
                {
                    Inventory.Item value;
                    for (var i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.TurretHighDamageProjectile)
                        {
                            piece.inventory.Items.Remove(value);
                            Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                            break;
                        }
                    }

                    // Your soul is forfeit.
                    piece.effectSink.RemoveStatusEffect(EffectStateType.Berserk); // added damage.
                    piece.effectSink.RemoveStatusEffect(EffectStateType.Corruption); // just the look.
                    piece.AddGold(0);
                }
                else if (piece.boardPieceId == BoardPieceId.HeroWarlock)
                {
                    Inventory.Item value;
                    for (var i = 0; i < piece.inventory.Items.Count; i++)
                    {
                        value = piece.inventory.Items[i];
                        if (value.abilityKey == AbilityKey.EnemyFireball)
                        {
                            piece.inventory.Items.Remove(value);
                            Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                            break;
                        }
                    }

                    piece.AddGold(0);
                }
            }
            else if (level == 3)
            {
                piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, piece.GetMaxHealth() - 2);
                piece.effectSink.TrySetStatBaseValue(Stats.Type.DownedCounter, piece.GetStat(Stats.Type.DownedCounter) + 1);
                piece.effectSink.TrySetStatBaseValue(Stats.Type.DownedTimer, piece.GetStat(Stats.Type.DownedTimer) - 1);
                piece.effectSink.TrySetStatMaxValue(Stats.Type.Speed, piece.GetStatMax(Stats.Type.Speed) - 1);
            }
            else if (level == 4)
            {
                piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, piece.GetMaxHealth() - 2);

                piece.effectSink.TrySetStatBaseValue(Stats.Type.MagicBonus, piece.GetStat(Stats.Type.MagicBonus) - 1);
                piece.effectSink.TrySetStatMaxValue(Stats.Type.MagicBonus, piece.GetStatMax(Stats.Type.MagicBonus) - 1);

                piece.effectSink.TrySetStatBaseValue(Stats.Type.Strength, piece.GetStat(Stats.Type.Strength) - 1);
                piece.effectSink.TrySetStatMaxValue(Stats.Type.Strength, piece.GetStatMax(Stats.Type.Strength) - 1);
            }
            else if (level == 5)
            {
                if (piece.boardPieceId == BoardPieceId.HeroBarbarian)
                {
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.Strength, piece.GetStat(Stats.Type.Strength) - 1);
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Strength, piece.GetStatMax(Stats.Type.Strength) - 1);
                }
                else if (piece.boardPieceId == BoardPieceId.HeroBard)
                {
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.MagicBonus, piece.GetStat(Stats.Type.MagicBonus) - 1);
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.MagicBonus, piece.GetStatMax(Stats.Type.MagicBonus) - 1);
                }
                else if (piece.boardPieceId == BoardPieceId.HeroGuardian)
                {
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.Armor, piece.GetMagicArmor() + 5);
                }
                else if (piece.boardPieceId == BoardPieceId.HeroRogue)
                {
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.Speed, piece.GetStat(Stats.Type.Speed) - 2);
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Speed, piece.GetStatMax(Stats.Type.Speed) - 2);
                }
                else if (piece.boardPieceId == BoardPieceId.HeroHunter)
                {
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, piece.GetMaxHealth() - 2);
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.MagicBonus, piece.GetStat(Stats.Type.MagicBonus) - 1);
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.MagicBonus, piece.GetStatMax(Stats.Type.MagicBonus) - 1);
                }
                else if (piece.boardPieceId == BoardPieceId.HeroSorcerer)
                {
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.MagicBonus, piece.GetStat(Stats.Type.MagicBonus) - 1);
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.MagicBonus, piece.GetStatMax(Stats.Type.MagicBonus) - 1);
                }
                else if (piece.boardPieceId == BoardPieceId.HeroWarlock)
                {
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.Strength, piece.GetStat(Stats.Type.Strength) - 1);
                    piece.effectSink.TrySetStatMaxValue(Stats.Type.Strength, piece.GetStatMax(Stats.Type.Strength) - 1);

                    // Let's try to decrease their Cana's stats, but only for this warlock that leveled down.
                    var gameContext = Traverse.Create(piece).Property<GameContext>("gameContext").Value;
                    PieceAI pieceAI = piece.pieceAI; // grab the current piece's AI, which can be Arly or Cana.
                    Piece[] listMinions = gameContext.pieceAndTurnController.GetPieceByType(PieceType.HasMinionPowder); // Get a list of all Cana's in the game.
                    int minionListLength = listMinions.GetLength(0);

                    for (int temp = 0; temp < minionListLength; temp++)
                    {
                        HouseRulesEssentialsBase.LogDebug($"Minion PieceIDs = slot {temp}: {listMinions[temp].boardPieceId}");

                        // if the AI is there, and the piece's associated piece is the current iteration of the list of Canas:
                        if (pieceAI != null && pieceAI.memory.TryGetAssociatedPiece(gameContext.pieceAndTurnController, out listMinions[temp]))
                        {
                            // pieceOwner should be the warlock that leveled up.
                            if (listMinions[temp].boardPieceId == piece.boardPieceId)
                            {
                                listMinions[temp].effectSink.TrySetStatMaxValue(Stats.Type.Health, piece.GetMaxHealth() - 2);

                                listMinions[temp].effectSink.TrySetStatBaseValue(Stats.Type.Strength, piece.GetStat(Stats.Type.Strength) - 1);
                                listMinions[temp].effectSink.TrySetStatMaxValue(Stats.Type.Strength, piece.GetStatMax(Stats.Type.Strength) - 1);
                                listMinions[temp].effectSink.TrySetStatBaseValue(Stats.Type.Speed, piece.GetStat(Stats.Type.Speed) - 1);
                                listMinions[temp].effectSink.TrySetStatMaxValue(Stats.Type.Speed, piece.GetStatMax(Stats.Type.Speed) - 1);
                            }
                        }
                    }
                }
            }
            else if (level == 6)
            {
                piece.effectSink.TrySetStatBaseValue(Stats.Type.DownedCounter, piece.GetStat(Stats.Type.DownedCounter) + 1);
                piece.effectSink.TrySetStatBaseValue(Stats.Type.DownedTimer, piece.GetStat(Stats.Type.DownedTimer) - 1);

                piece.effectSink.TrySetStatBaseValue(Stats.Type.Speed, piece.GetStat(Stats.Type.Speed) - 1);
                piece.effectSink.TrySetStatMaxValue(Stats.Type.Speed, piece.GetStatMax(Stats.Type.Speed) - 1);
            }
            else if (level == 7)
            {
                Inventory.Item value;
                for (var i = 0; i < piece.inventory.Items.Count; i++)
                {
                    value = piece.inventory.Items[i];
                    if (value.abilityKey == AbilityKey.Petrify || value.abilityKey == AbilityKey.AcidSpit || value.abilityKey == AbilityKey.DeathFlurry || value.abilityKey == AbilityKey.DeathBeam || value.abilityKey == AbilityKey.HeavensFury || value.abilityKey == AbilityKey.CoinFlip || value.abilityKey == AbilityKey.DropChest)
                    {
                        piece.inventory.Items.Remove(value);
                        Traverse.Create(piece.inventory).Field<int>("numberOfReplenishableCards").Value -= 1;
                        break;
                    }
                }

                piece.AddGold(0);
            }
            else if (level == 8)
            {
                piece.effectSink.TrySetStatBaseValue(Stats.Type.MagicBonus, piece.GetStat(Stats.Type.MagicBonus) - 1);
                piece.effectSink.TrySetStatMaxValue(Stats.Type.MagicBonus, piece.GetStatMax(Stats.Type.MagicBonus) - 1);

                piece.effectSink.TrySetStatBaseValue(Stats.Type.Strength, piece.GetStat(Stats.Type.Strength) - 1);
                piece.effectSink.TrySetStatMaxValue(Stats.Type.Strength, piece.GetStatMax(Stats.Type.Strength) - 1);

                piece.effectSink.TryGetStat(Stats.Type.ActionPoints, out int currentAP);
                if (currentAP > 0)
                {
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.ActionPoints, currentAP - 1);
                }
            }
            else if (level == 9)
            {
                piece.effectSink.TryGetStat(Stats.Type.ActionPoints, out int currentAP);
                if (currentAP > 0)
                {
                    piece.effectSink.TrySetStatBaseValue(Stats.Type.ActionPoints, currentAP - 1);
                }
            }
            else if (level == 11 || level == 14 || level == 16)
            {
                piece.effectSink.TrySetStatMaxValue(Stats.Type.Health, piece.GetMaxHealth() - 2);
            }
            else if (level == 12 || level == 15 || level == 17)
            {
                piece.effectSink.TrySetStatBaseValue(Stats.Type.MagicBonus, piece.GetStat(Stats.Type.MagicBonus) - 1);
                piece.effectSink.TrySetStatMaxValue(Stats.Type.MagicBonus, piece.GetStatMax(Stats.Type.MagicBonus) - 1);

                piece.effectSink.TrySetStatBaseValue(Stats.Type.Strength, piece.GetStat(Stats.Type.Strength) - 1);
                piece.effectSink.TrySetStatMaxValue(Stats.Type.Strength, piece.GetStatMax(Stats.Type.Strength) - 1);
            }
        }
    }
}
