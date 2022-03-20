# HouseRules Essentials

A collection of predefined rules and rulesets for HouseRules_Core

![HouseRules Logo](../docs/images/house-rules-logo1.png)

See the [HouseRules_Core readme](../HouseRules_Core/README.md) for more information about
HouseRules API.

## Rulesets

### Built-in Rulesets

- __The Swirl__ : Only poison, fireballs and vortexes. Health and POIs aplenty, but must defeat all enemies to escape.
- __Beat The Clock__ : Ultra health. Ultra card recycling. Only 15 rounds to escape...
- __Hunter's Paradise__ : Pets, pets, pets! And hunter's mark.
- __Difficulty Easy__ : Decreased game difficulty for a more casual playstyle.
- __Difficulty Hard__ : Increased game difficulty for a greater challenge.
- __Difficulty Legendary__ : Increased game difficulty for those who want to be a legend.
- __No Surprises__ :  No surprises in the dark or coming through doors.
- __Quick and the Dead__ : A mode with a small hand but fast turnaround time on cards means you need to not hesitate.

### JSON Rulesets

Rulesets may also be configured as JSON files and stored within the game directory `<GAME_DIR>/UserData/HouseRules/<rulesetname>.json`

A selection are available within this repository. These are intended to be fun to play alternative games, and as a good examples for others wanting to create their own rulesets.

- __[🎲LuckyDip🎲 Ruleset](../docs/rulesets/LuckyDip.json)__ : Players each start with two 'Drop Chest' cards instead of their normal
starting cards, meaning that no two games start the same. Many potions have AOE effect, because it's rude not to share. 
Many other changes included for faster gameplay with an aim of around 90 minutes per game.
- __[🕷️Arachnophobia🕷️ Ruleset](../docs/rulesets/Arachnophobia.json)__ offers a fresh adventure to be played on the RootsOfEvil Map.
Chased by violent thugs from their ancestral homes in Sunderhaven, the King and Queen flee into the woods. 
Befriended by money spiders, they hatch a plan to rebuild their fallen empires, but first they're going to need some cash.

The [Settings Reference](../docs/SettingsReference.md) contains lists of all different BehaviourIDs, AbilityKeys and other data types used by the Rules.

## Rules and Configurations

- __AbilityActionCostAdjusted__: Adjusts the casting costs for player abilities.
  - Overrides the Ability.CostAP setting for player abilities.
  - To configure:
    - Specify the [AbilityKey](../docs/SettingsReference.md#abilitykeys) of the ability to modify.
    - Specify `true` if the ability should cost AP to cast, or `false` if it should not.
  
  ###### _Example JSON config for AbilityActionCostAdjusted_

  ```json
  {
    "Rule": "AbilityActionCostAdjusted",
    "Config": {
      "Zap": false,
      "CourageShanty": false,
      "HealingPotion": true,
    }
  },
  ```

- __AbilityAoeAdjusted__: Adjusts the Area of Effect range(s) for abilities.
  - Does not work with all abilities.
  - To configure:
    - Specify the [AbilityKey](../docs/SettingsReference.md#abilitykeys) of the ability to modify.
    - Specify a positive integer to increase range, or a negative number to decrease it. E.g.: `"Fireball": 1` will increases the Fireball AOE from a 3x3 to 5x5
  
  ###### _Example JSON config for AbilityAoeAdjusted_

  ```json
  {
    "Rule": "AbilityAoeAdjusted",
    "Config": {
      "CourageShanty": 1,
      "StrengthPotion": 1,
      "SwiftnessPotion": 1,
      "HealingPotion": 1,
    }
  },
  ```

- __AbilityBackstabAdjusted__: Adjusts the enableBackstabBonus setting for abilities.
  - Does not work with all abilities.
  - To configure:
    - Specify the [AbilityKey](../docs/SettingsReference.md#abilitykeys) of the ability to modify.
    - Specify `true` if the ability should give a backstab bonus, or `false` if it should not.
  
  ###### _Example JSON config for AbilityBackstabAdjusted_

  ```json
  {
    "Rule": "AbilityBackstabAdjusted",
    "Config": {
      "Zap": true,
      "HunterArrow": true,
      "PiercingArrow": true,
      "PoisonTip": true,
      "Fireball": true,
      "Freeze": true,
    }
  },
  ```

- __AbilityDamageAdjusted__: Ability damage is adjusted
  - Only functions for abilities which do damage. (You can't make a HealingPotion hurt).
  - CriticalHitDamage is adjusted to double normal damage.
  - To configure:
    - Specify the [AbilityKey](../docs/SettingsReference.md#abilitykeys) of the ability to modify.
    - Specify a positive integer to increase damage, or a negative number to decrease it. E.g.: `"Zap": 1` will increase Zap damage from 1 to 2.

  ###### _Example JSON config for AbilityDamageAdjusted_

  ```json
  {
    "Rule": "AbilityDamageAdjusted",
    "Config": {
      "Zap": 1,
      "WhirlwindAttack": 1,
    }
  },
  ```

- __AbilityRandomPieceList__: The randomPieceList for Abilities is adjusted
  - 🚧 _Skirmish-only - Does not work properly in multiplayer games._ 🚧
  - Some abilities (BeastWhisperer, RatBomb) have lists which are used to spawn random pieces.
  - This rule allows the list to be replaced with a different one.
  - To configure:
    - Specify the [AbilityKey](../docs/SettingsReference.md#abilitykeys) of the ability to modify.
    - Specify a list of [BoardPieceIds](../docs/SettingsReference.md#boardpieceids) for the pieces that the ability could spawn.

  ###### _Example JSON config for AbilityRandomPieceList_

  ```json
  {
    "Rule": "AbilityRandomPieceList",
    "Config": {
      "BeastWhisperer": [
        "GoblinRanger",
        "Slime",
      ]
    }
  },
  ```

- __BackstabConfigOverridden__: A list of Pieces may use 🔪Backstab🔪 instead of just the Assassin
  - Replaces the hardcoded default of HeroRogue with a configurable list.
  - Now everyone can benefit from Backstab bonus.
  - To configure:
    - Specify the list of [BoardPieceIds](../docs/SettingsReference.md#boardpieceids) that should have the ability to backstab.

  ###### _Example JSON config for BackstabConfigOverridden_

  ```json
    {
      "Rule": "BackstabConfigOverridden",
      "Config": [ "HeroGuardian", "HeroHunter", "HeroSorcerer", "HeroRogue", "HeroBard" ]
    },
  ```

- __CardAdditionOverridden__: Overrides the lists of cards which players receive from chests & card energy.
  - The default card allocation mechanism is intercepted and changed to use a user-defined list of cards.
  - To configure:
    - Specify the [BoardPieceId](../docs/SettingsReference.md#boardpieceids) that should have its card pool overridden.
    - Specify a list of [AbilityKeys](../docs/SettingsReference.md#abilitykeys) for the cards that should make up the card pool.

  ###### _Example JSON config for CardAdditionOverridden_

  ```json
  {
    "Rule": "CardAdditionOverridden",
    "Config": {
      "HeroSorcerer": ["StrengthPotion", "SwiftnessPotion", "Bone", "Fireball", "Freeze", "BottleOfLye", "Teleportation", "HeavensFury", "RevealPath"],
      "HeroGuardian": ["WhirlwindAttack", "Charge", "CallCompanion", "HealingPotion"],
    }
  },
  ```

- __CardEnergyFromAttackMultiplied__: Card energy from attack is multiplied
  - Multiply how quickly the mana bar fills up when you attack enemies.
  - To configure:
    - Specify a decimal number representing how the energy is multiplied.

  ###### _Example JSON config for CardEnergyFromAttackMultiplied_

  ```json
  {
    "Rule": "CardEnergyFromAttackMultiplied",
    "Config": 2.0
  },
  ```

- __CardEnergyFromRecyclingMultiplied__: Card energy from recycling is multiplied
  - To configure:
    - Specify a decimal number representing how the energy is multiplied.

  ###### _Example JSON config for CardEnergyFromRecyclingMultiplied_

  ```json
  {
    "Rule": "CardEnergyFromRecyclingMultiplied",
    "Config": 2.0
  },
  ```

- __CardLimitModified__: Card limit is modified
  - 🚧 _Skirmish-only - Does not work properly in multiplayer games._ 🚧
  - Change the size of the player's card hand from the default 10/11.
  - To configure:
    - Specify an integer representing the total size of the player's hand.

  ###### _Example JSON config for CardLimitModified_

  ```json
  {
    "Rule": "CardLimitModified",
    "Config": 20
  },
  ```

- __CardSellValueMultiplied__: Card sell values are multiplied
  - 🚧 _Skirmish-only - Does not work properly in multiplayer games._ 🚧
  - Increase card sale values in the shop.
  - To configure:
    - Specify a decimal number representing how sell values are multiplied.

  ###### _Example JSON config for CardSellValueMultiplied_

  ```json
  {
    "Rule": "CardSellValueMultiplied",
    "Config": 2.0
  },
  ```

- __CardClassRestrictionOverridden__: Overrides Character Class assignments for cards.
  - Cards with a character class of `None` are usable by all players.
  - Cards may be disabled from play by assigning to a non-player Character
  - Cards may be reassigned to other player characters
  - To configure:
    - Specify the [AbilityKey](../docs/SettingsReference.md#abilitykeys) of the card to modify.
    - Specify the [BoardPieceId](../docs/SettingsReference.md#boardpieceids) of the piece to restrict the card to.

  ###### _Example JSON config for CardClassRestrictionOverridden_

  ```json
    {
      "Rule": "CardClassRestrictionOverridden",
      "Config": {
        "BeastWhisperer": "SporeFungus",
        "Sneak": "Guardian",
        "Zap": "Hunter",
      }
    },
  ```

- __EnemyAttackScaled__: Enemy ⚔️attack⚔️ damage is scaled
  - To configure:
    - Specify a decimal number representing how enemy attack damage is multiplied.

  ###### _Example JSON config for EnemyAttackScaled_

  ```json
  {
    "Rule": "EnemyAttackScaled",
    "Config": 0.85
  },
  ```

- __EnemyDoorOpeningDisabled__: Enemy 🚪door🚪 opening ability disabled
  - To configure:
    - Specify `true` to disable enemies opening doors.

  ###### _Example JSON config for EnemyDoorOpeningDisabled_

  ```json
  {
    "Rule": "EnemyDoorOpeningDisabled",
    "Config": true
  },
  ```

- __EnemyHealthScaled__: Enemy health is scaled
  - To configure:
    - Specify a decimal number representing how enemy health is multiplied.

  ###### _Example JSON config for EnemyHealthScaled_

  ```json
  {
    "Rule": "EnemyHealthScaled",
    "Config": 0.85
  },
  ```

- __EnemyRespawnDisabled__: Enemy respawns are disabled
  - To configure:
    - Specify `true` to disable enemy respawns.

  ###### _Example JSON config for EnemyRespawnDisabled_

  ```json
  {
    "Rule": "EnemyRespawnDisabled",
    "Config": true
  },
  ```

- __GoldPickedUpMultiplied__: 💰Gold💰 picked up is multiplied
  - To configure:
    - Specify a decimal number representing how gold is multiplied.

  ###### _Example JSON config for GoldPickedUpMultiplied_

  ```json
  {
    "Rule": "GoldPickedUpMultiplied",
    "Config": 1.25
  },
  ```

- __LevelExitLockedUntilAllEnemiesDefeated__: The 🔒exit🔑 from each level will not open if any enemies remain.
  - This rule needs to be used in combination with other rules or it will not be possible to complete a level. (e.g. EnemyRespawnDisabled)
  - To configure:
    - Specify `true` to keep the level exit locked until all enemies are defeated.

  ###### _Example JSON config for LevelExitLockedUntilAllEnemiesDefeated_

  ```json
  {
    "Rule": "LevelExitLockedUntilAllEnemiesDefeated",
    "Config": true
  },
  ```

- __LevelPropertiesModified__: Level properties are modified
  - Allows customisation of Loot, Chests and HealingFountains on a per-floor basis
  - To configure:
    - Specify the [LevelProperty](../docs/LevelProperties.md) to modify.
    - Specify an integer or decimal number for new value for the property.

  ###### _Example JSON config for LevelPropertiesModified_

  ```json
  {
    "Rule": "LevelPropertiesModified",
    "Config": {
      "BigGoldPileChance": 100,
      "FloorOneHealingFountains": 9,
      "FloorOneLootChests": 9,
      "FloorTwoHealingFountains": 9,
      "FloorTwoLootChests": 9,
      "FloorThreeHealingFountains": 9,
      "FloorThreeLootChests": 9,
    }
  },
  ```

- __PetsFocusHunterMark__: Pets focus on hunter marked enemies
  - To configure:
    - Specify `true` to have pets focus on enemies that are marked.

  ###### _Example JSON config for PetsFocusHunterMark_

  ```json
  {
    "Rule": "PetsFocusHunterMark",
    "Config": true
  },
  ```

- __PieceAbilityListOverridden__: The list of abilities for a ♟️BoardPiece is overridden.
  - Board pieces have abilities such as LaySpiderEgg or SpawnCultists. This rule allows those lists to be overridden.
  - With the right combination of rules, you can turn 🕷️spiderlings into thieves who steal your gold, cards, etc.
  - Assigning an Ability to a BoardPiece does not necessarily mean that the piece will have a Behaviour to use it.
  - This rule works in conjunction with `PieceBehavioursListOverridden` and `PiecePieceTypesListOverridden`
  - To configure:
    - Specify the [BoardPieceId](../docs/SettingsReference.md#boardpieceids) of the piece to modify.
    - Specify a list of [AbilityKeys](../docs/SettingsReference.md#abilitykeys) representing the abilities the piece should have. 

  ###### _Example JSON config for PieceAbilityListOverridden_

  ```json
  {
    "Rule": "PieceAbilityListOverridden",
    "Config": {
      "Spiderling": [ "SpiderWebshot", "LaySpiderEgg", "EarthShatter", "AcidSpit", "DropChest", "EnemyStealCard", "EnemyStealGold" ],
      "Rat": [ "DiseasedBite", "SpawnRat", "EnemyStealGold", "SpawnMushrooms", "DropChest", "EnemyStealCard", "EnemyStealGold" ],
      "GoblinFighter": [ "SpawnCultists", "EnemyStealGold", "DropChest", "EnemyStealCard", "EnemyStealGold" ],
    }
  },
  ```

- __PieceBehavioursListOverridden__: The list of behaviours that a ♟️BoardPiece behaves is overridden.
  - Board pieces have behaviours such as Patrol, SpawnPiece, AttackandRetreat. This rule allows those lists to be overridden.
  - With the right combination of rules, you can turn 🕷️spiderlings into thieves who steal your gold, cards, etc.
  - Assigning a behaviour to a particular BoardPiece does not mean that the piece is of the correct PieceType to perform it.
  - This rule works in conjunction with `PieceAbilityListOverridden` and `PiecePieceTypesListOverridden`
  - To configure:
    - Specify the [BoardPieceId](../docs/SettingsReference.md#boardpieceids) of the piece to modify.
    - Specify the list of [Behaviours](../docs/SettingsReference.md#behaviours) that the piece should have.

  ###### _Example JSON config forPieceBehavioursListOverridden_

  ```json
  {
    "Rule": "PieceBehavioursListOverridden",
    "Config": {
      "Spiderling": [ "AttackAndRetreat", "Patrol", "FleeToFOW", "HealFromFOW", "ChargeMove" ],
      "Rat": [ "Patrol", "SpawnPiece" ],
      "GoblinFighter": [ "FollowPlayerRangedAttacker", "RangedSpellCaster" ],
    }
  },
  ```

- __PieceConfigAdjusted__: Allows customization of any numeric field for any ♟️BoardPiece
  - Allows customization of many of the properties for each game Piece. 🩺Health, 🎲ActionPoints, 🏃Movement, ⚔️MeleeDamage, etc.
  - Only works for numeric fields. The configured value replaces the default.
  - To configure:
    - Specify the [BoardPieceId](../docs/SettingsReference.md#boardpieceids) of the piece to modify.
    - Specify the [PieceProperty](../docs/PieceConfig.md) to modify.
    - Specify an integer or decimal number representing the new value for the property.

  ###### _Example JSON config for PieceConfigAdjusted_

  ```json
  {
    "Rule": "PieceConfigAdjusted",
    "Config": [
      { "Piece": "HeroSorcerer", "Property": "StartHealth", "Value": 20 },
      { "Piece": "HeroSorcerer", "Property": "MoveRange", "Value": 5 },
      { "Piece": "HeroSorcerer", "Property": "ActionPoint", "Value": 3 },
      { "Piece": "Lure", "Property": "StartHealth", "Value": 30 },
      { "Piece": "TheBehemoth", "Property": "ActionPoint", "Value": 2 },
      { "Piece": "HeroSorcerer", "Property": "BerserkBelowHealth", "Value": 0.8 }
    ]
  },
  ```
  
- __PieceImmunityListAdjusted__: Allows the list of immunities for any ♟️BoardPiece to be overridden
  - Allows customization of many the list of immunities for each game Piece. 🤢Diseased, 😵Stunned, 🤕Weakened, 🥶Frozen, 🧶Tangled, 💤Petrified , etc
  - To configure:
    - Specify the [BoardPieceId](../docs/SettingsReference.md#boardpieceids) of the piece to modify.
    - Specify the list of [EffectStates](../docs/EffectStates.md) that the piece should be immune to.

  ###### _Example JSON config for PieceImmunityListAdjusted_

  ```json
  {
    "Rule": "PieceImmunityListAdjusted",
    "Config": {
      "HeroSorcerer": [ "Diseased", "HuntersMark", "Weaken", "Frozen", "Tangled", "Petrified" ],
      "HeroGuardian": [ "Frozen" ],
    }
  },
  ```

- __PiecePieceTypeListOverridden__: Allows the list of PieceTypes for a ♟️BoardPiece to be overridden.
  - Board pieces have PieceTypes such as IgnoreWhenCharmed, Brittle, Enemy, Prop, Interactable which dictate certain behaviours.
  - With the right combination of rules, you can turn 🕷️spiderlings into thieves who steal your gold, cards, etc.
  - Assigning an PieceType to a BoardPiece does not necessarily mean that the piece will change its behaviour.
  - This rule works in conjunction with `PieceAbilityListOverridden` and `PieceBehavioursListOverridden`.  
  - To configure:
    - Specify the [BoardPieceId](../docs/SettingsReference.md#boardpieceids) of the piece to modify.
    - Specify the list of [PieceTypes](../docs/SettingsReference.md#piecetypes) that the piece should be.

  ###### _Example JSON config for PieceImmunityListAdjusted_

  ```json
  {
    "Rule": "PiecePieceTypeListOverridden",
    "Config": {
      "Spiderling": [ "Enemy", "Goblin", "Thief", "Canine" ],
      "Rat": [ "Enemy", "Goblin", "Thief", "Canine" ],
      "GoblinFighter": [ "Enemy", "Goblin", "Thief", "Canine" ],
    }
  },
  ```
  
- __PieceUseWhenKilledOverridden__: Allows the list of UseWhenKilled abilities for any ♟️BoardPiece to be overridden
  - Abilities are triggered when a piece dies.
  - To configure:
    - Specify the [BoardPieceId](../docs/SettingsReference.md#boardpieceids) of the piece to modify.
    - Specify a list of [AbilityKeys](../docs/SettingsReference.md#abilitykeys) for the abilities to be triggered when that piece is killed.

  ###### _Example JSON config for PieceUseWhenKilledOverridden_

  ```json
  {
    "Rule": "PieceUseWhenKilledOverridden",
    "Config": {
      "Spiderling": [ "HealingPotion" ],
      "CaveTroll": [ "Rejuvenation" ],
    }
  },
  ```

- __RatNestsSpawnGold__: Rat nests spawn 💰gold💰
  - 🚧 _Skirmish-only - Does not work properly in multiplayer games._ 🚧
  - To configure:
    - Specify an integer representing the maximum piles of gold that should be spawned each time.

  ###### _Example JSON config for RatNestsSpawnGold_

  ```json
  {
    "Rule": "RatNestsSpawnGold",
    "Config": 4
  },
  ```

- __RegainAbilityIfMaxxedOutOverridden__: Controls whether you get a potion back when you cast it on someone who is already at max.
  - This rule is to overcome a 'feature' that occurs if you apply an AOE range onto Strength/Speed potions. By default when you cast a strength or speed potion on someone who is already maxed out, you get it returned to your hand. If you cast a potion on a group who are maxed, you get one-potion-per-player returned back to your hand.
  - This rule exists to control that behaviour.
  - To configure:
    - Specify the [AbilityKeys](../docs/SettingsReference.md#abilitykeys) of the ability to modify.
    - Specify `true` is using the ability while maxed out should return the ability card to the player, or `false` to prevent the card being returned to the player's hand.

  ###### _Example JSON config for RegainAbilityIfMaxxedOutOverridden_

  ```json
    {
      "Rule": "RegainAbilityIfMaxxedOutOverridden",
      "Config": {
        "SwiftnessPotion": false,
        "StrengthPotion": false
      }
    },
  ```

- __RoundCountLimited__:  Sets a limit for the maximum number of rounds a game may take.
  - For ⏳ beat-the-clock ⏳ type gameplay.
  - If the game is not completed (i.e., the boss defeated) before the specified rounds are up, the game ends as a loss.
  - To configure:
    - Specify the maximum number of rounds allowed.

  ###### _Example JSON config for RoundCountLimited_

  ```json
  {
    "Rule": "RoundCountLimited",
    "Config": 40
  },
  ```

- __SampleRule__: A [sample rule](Rules/SampleRule.cs) documenting the anatomy of a HouseRule rule.

- __SpawnCategoryOverridden__:  Overrides the Spawn Categories which control distribution of pieces in each map.
  - Each dungeon has a list of pieces which may appear, and controlling properties.
  - This rule replaces the list (for all dungeons) with a new one.
  - Pieces which are not listed in the config will have `IsSpawningEnabled` set to `false` to disable pieces from auto-populating a map.
  - Does not have absolute control over what monsters will appear. Bosses bring support chars etc.
  - To configure:
    - Specify the [BoardPieceId](../docs/SettingsReference.md#boardpieceids) of the piece to appear in each map.
    - Specify a list with values for `MaxPerDeck`, `PreFill` and `FirstAllowedLevelIndex`, respectively, for each piece.

  ###### _Example JSON config for SpawnCategoryOverridden_

  ```json
    {
      "Rule": "SpawnCategoryOverridden",
      "Config": {
        "Spiderling": [ 200, 50, 1 ],
        "SpiderEgg": [ 20, 10, 1 ] ,
        "GiantSpider": [ 30, 10, 1 ],
        "RatKing": [ 1, 1, 1 ],
        "ElvenQueen": [ 1, 1, 2 ],
      }
    },
  ```

- __StartCardsModified__: Player 🎴 starting cards 🎴 are modified
  - Removes all default cards from Player's hand and replaces them with custom ones.
  - Replenishable cards do not leave a players hand once cast (e.g. RepairArmor, HunterArrow or Zap).
  - Max of two replenishable cards per player.
  - To configure:
    - Specify the [BoardPieceId](../docs/SettingsReference.md#boardpieceids) of the piece to replace the starting card hand of.
    - Specify the [AbilityKey](../docs/SettingsReference.md#boardpieceids) of the cards to add to the piece's hand.
    - Specify `true` if the card should replenish, or `false` if it should not.

  ###### _Example JSON config for StartCardsModified_

  ```json
  {
    "Rule": "StartCardsModified",
    "Config": {
      "HeroGuardian": [
        { "Card": "HealingPotion", "IsReplenishable": false },
        { "Card": "ReplenishArmor", "IsReplenishable": true },
        { "Card": "WhirlwindAttack", "IsReplenishable": true },
        { "Card": "PiercingThrow", "IsReplenishable": false },
        { "Card": "CoinFlip", "IsReplenishable": false },
        { "Card": "TheBehemoth", "IsReplenishable": false },
        { "Card": "SwordOfAvalon", "IsReplenishable": false },
      ],
      "HeroHunter": [
        { "Card": "HealingPotion", "IsReplenishable": false },
        { "Card": "HunterArrow", "IsReplenishable": true },
        { "Card": "HunterArrow", "IsReplenishable": true },
        { "Card": "CoinFlip", "IsReplenishable": false },
        { "Card": "DropChest", "IsReplenishable": false },
      ],
      "HeroSorcerer": [
        { "Card": "HealingPotion", "IsReplenishable": false },
        { "Card": "Zap", "IsReplenishable": true },
        { "Card": "WhirlwindAttack", "IsReplenishable": true },
        { "Card": "Freeze", "IsReplenishable": false },
        { "Card": "Fireball", "IsReplenishable": false },
        { "Card": "CallCompanion", "IsReplenishable": false },
      ],
    }
  },
  ```

- __StatModifiersOverriden__: The additiveBonus parameters of StatModifiers are overridden.
  - There are only six different StatModifiers in the game. They are used by 💪StrengthPotion, 🦶SwiftnessPotion, 🛡️ReplenishArmor, HuntersMark, etc.
  - These modifiers control the power of each corresponding ability.  E.g., by default the stat modifier for SongOfResilience is 5, as it grants 5 units of armor.  
  - To configure:
    - Specify the [AbilityKey](../docs/SettingsReference.md#boardpieceids) of the ability whose stat modifer should be replaced.
    - Specify an integer representing the new value of the stat modifier.

  ###### _Example JSON config for StatModifiersOverriden_

  ```json
  {
    "Rule": "StatModifiersOverriden",
    "Config": {
      "StrengthPotion": 2,
      "SwiftnessPotion": 2,
      "HuntersMark": -4,
      "ReplenishBarkArmor": 4,
      "SongOfResilience": 6,
      "ReplenishArmor": 4,
    }
  },
  ```

- __StatusEffectConfig__: The parameters of different StatusEffects (🔥Torch, 🤢Poison, 🥶Frozen) can be overridden
  - Default values can be found in `StatusEffectsConfig.effectsConfig`.
  - To configure:
    - Specify a list of status effects that should replace existing ones of the same type.

  ###### _Example JSON config for StatusEffectConfig_

  ```json
  {
    "Rule": "StatusEffectConfig",
    "Config": [
      {
        "effectStateType": "TorchPlayer",
        "durationTurns": 15,
        "tickWhen": "StartTurn",
        "stacks": true,
        "damagePerTurn": 0,
        "clearOnNewLevel": false,
        "damageTags": null,
        "healPerTurn": 0
      },
      {
        "effectStateType": "HealingSong",
        "durationTurns": 4,
        "tickWhen": "StartTurn",
        "stacks": false,
        "damagePerTurn": 0,
        "clearOnNewLevel": false,
        "damageTags": null,
        "healPerTurn": 3
      },
    ]
  },
  ```
