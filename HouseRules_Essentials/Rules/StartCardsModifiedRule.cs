namespace HouseRules.Essentials.Rules
{
    using System;
    using System.Collections.Generic;
    using Boardgame;
    using Boardgame.BoardEntities;
    using Boardgame.BoardEntities.Abilities;
    using DataKeys;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class StartCardsModifiedRule : Rule, IConfigWritable<Dictionary<BoardPieceId, List<StartCardsModifiedRule.CardConfig>>>,
        IPatchable, IMultiplayerSafe, IDisableOnReconnect
    {
        public override string Description => "Hero start cards are modified";

        private static Dictionary<BoardPieceId, List<CardConfig>> _globalHeroStartCards;
        private static bool _isActivated;

        private readonly Dictionary<BoardPieceId, List<CardConfig>> _heroStartCards;

        public struct CardConfig
        {
            public AbilityKey Card;
            public int ReplenishFrequency;
        }

        public StartCardsModifiedRule(Dictionary<BoardPieceId, List<CardConfig>> heroStartCards)
        {
            _heroStartCards = heroStartCards;
        }

        public Dictionary<BoardPieceId, List<CardConfig>> GetConfigObject() => _heroStartCards;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalHeroStartCards = _heroStartCards;
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(PieceSpawner), "CreatePieceInternal"),
                prefix: new HarmonyMethod(
                    typeof(StartCardsModifiedRule),
                    nameof(Piece_CreatePieceInternal_Prefix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(Inventory), "RestoreReplenishables"),
                prefix: new HarmonyMethod(
                    typeof(StartCardsModifiedRule),
                    nameof(Inventory_RestoreReplenishables_Prefix)));
        }

        private static bool Inventory_RestoreReplenishables_Prefix(ref bool __result, Piece piece)
        {
            if (!_isActivated)
            {
                return true;
            }

            __result = false;
            for (int i = 0; i < piece.inventory.Items.Count; i++)
            {
                Inventory.Item value = piece.inventory.Items[i];
                int targetRefresh = 1;
                if (value.abilityKey == AbilityKey.DiseasedBite || value.abilityKey == AbilityKey.EnemyFlashbang)
                {
                    targetRefresh = 2;
                }

                var countdown = (value.flags >> 4) & 7;

                if (piece.inventory.Items[i].IsReplenishing)
                {
                    bool skipReplenishing = false;
                    if (!AbilityFactory.TryGetAbility(value.abilityKey, out Ability ability))
                    {
                        throw new Exception("Failed to get ability prefab from ability key while attempting to replenish hand!");
                    }

                    int j = 0;
                    int count = ability.effectsPreventingReplenished.Count;
                    while (j < count)
                    {
                        if (piece.HasEffectState(ability.effectsPreventingReplenished[j]))
                        {
                            skipReplenishing = true;
                        }

                        j++;
                    }

                    if (!skipReplenishing)
                    {
                        // If countdown was zero when we got called, then we need to set it.
                        if (countdown == 0)
                        {
                            if (piece.inventory.Items[i].abilityKey != AbilityKey.Sneak)
                            {
                                countdown = targetRefresh;
                            }
                            else
                            {
                                // set delay for sneak cooldown
                                countdown = 5;
                            }
                        }
                        else if (countdown == 4)
                        {
                            // cooldown sneak on 2nd turn
                            countdown = 1;
                        }

                        // If we reached our desired turn count we can unset isReplenishing and return true
                        if (countdown == 1)
                        {
                            value.flags &= -3; // unsets isReplenishing (bit1 ) allowing card to be used again.
                            __result = true;
                        }

                        countdown -= 1;
                        value.flags &= 911; // Zero only the countdown bits using a bitmask
                        value.flags |= countdown << 4; // OR with countdown to set them again.
                        piece.inventory.Items[i] = value;
                        Traverse.Create(piece.inventory.Items).Property<bool>("needSync").Value = true;
                    }
                }
            }

            return false;
        }

        private static void Piece_CreatePieceInternal_Prefix(PieceSpawnSettings spawnSettings)
        {
            if (!_isActivated)
            {
                return;
            }

            if (!_globalHeroStartCards.ContainsKey(spawnSettings.boardPieceId))
            {
                return;
            }

            var inventory = CreateInventory(spawnSettings.boardPieceId);
            Traverse.Create(spawnSettings).Property<Inventory>("Inventory").Value = inventory;
            Traverse.Create(spawnSettings).Property<bool>("SetInventory").Value = true;
        }

        private static Inventory CreateInventory(BoardPieceId boardPieceId)
        {
            var inventory = new Inventory();

            foreach (var card in _globalHeroStartCards[boardPieceId])
            {
                // flag bits
                // 0 : isReplenishable
                // 1 : isReplenishing
                // 2 : abilityDisabledOnStatusEffect
                // 3 : disableCooldown
                // 4-6 : ReplenishCounter - 3-bit range used by RestoreReplenishables for counting rounds.
                // 7-9 : ReplenishFrequency - 3-bit number, user-configured target.
                int flags = 0;
                if (card.ReplenishFrequency > 0)
                {
                    Traverse.Create(inventory).Field<int>("numberOfReplenishableCards").Value += 1;
                    flags = 1;
                    int refreshFrequency = (card.ReplenishFrequency > 7) ? 7 : card.ReplenishFrequency; // Limit to max of 7 turns.
                    flags |= refreshFrequency << 7; // logical or with refreshFrequency shifted 7 bits to the left to become ReplenishFrequency bits 7-9
                }

                inventory.Items.Add(new Inventory.Item
                {
                    abilityKey = card.Card,
                    flags = flags,
                    originalOwner = -1,
                    replenishCooldown = card.ReplenishFrequency,
                });
            }

            return inventory;
        }
    }
}
