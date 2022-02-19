# HouseRules Essentials

A collection of predefined rules and rulesets for HouseRules_Core

![HouseRules Logo](../docs/house-rules-logo1.png)

See the [HouseRules_Core readme](../HouseRules_Core/README.md) for more information about
HouseRules API.

## Rulesets

### Built-in Rulesets

- __SampleRuleset__: A [sample ruleset](https://github.com/orendain/DemeoMods/blob/045aec568fdddb95b63a1ed34abcb64065e4ca99/Rules/RulesMod.cs#L27-L28)
  for the purposes of documenting how to create a ruleset (and for testing during development).
- __No Surprises__ :  Prevents any surprises in the dark or coming through doors.
- __Beat The Clock__ : Ultra health. Ultra card recycling. Only 15 rounds to escape...
- __Difficulty Easy__ : This mode decreases the default game difficulty for a more casual playstyle.
- __Difficulty Hard__ : This mode increases the default game difficulty for a greater challenge.
- __Difficulty Legendary__ : This mode increases the default game difficulty for those who want to be a legend.
- __The Swirl__ : Only poison, fireballs and vortexes. Health and POIs aplenty, but must defeat all enemies to escape.
- __Quick and the Dead__ : A mode with a small hand but fast turn around time on cards means you need to not hesitate.


### JSON Rulesets

Rulesets may also be configured as JSON files and stored within the game directory `<GAME_DIR>/UserData/HouseRules/<rulesetname>.json`
An example [LuckyDip Ruleset](../docs/LuckyDip.json) which uses many differnt rules for rapid gameplay is provided as a guide to help you get started.

## Rules and Configurations

- __AbilityActionCostAdjustedRule__: Adjusts the casting costs for player abilitites.
  - Overrides the Ability.CostAP setting for player abilities.
  - `true` means the ability has a cost to cast, `false` means that it doesn't.
  - Config accepts Dictionary e.g. `{ "AbilityName1": bool, "AbilityName2": bool, }`
  
  ###### _Example JSON config for AbilityActionCostAdjustedRule_

  ```json
  {
    "Rule": "AbilityActionCostAdjusted",
    "Config": {
      "Zap": false,
      "StrengthenCourage": false,
      "Heal": true,
    }
  },
  ```

- __AbilityAoeAdjustedRule__: Adjusts the Area of Effect range(s) for abilities.
  - Does not work with all abilities.
  - Positive integers increase range, negative decrease. e.g. `"Fireball": 1` will increases the Fireball AOE from a 3x3 to 5x5
  - Config accepts Dictionary e.g. `{ "AbilityName1", int, "AbilityName2", int }`
  
  ###### _Example JSON config for AbilityAoeAdjustedRule_

  ```json
  {
    "Rule": "AbilityAoeAdjustedRule",
    "Config": {
      "StrengthenCourage": 1,
      "Strength": 1,
      "Speed": 1,
      "Heal": 1,
    }
  },
  ```

- __AbilityDamageAdjustedRule__: Ability damage is adjusted
  - Only functions for abilities which do damage. (You can't make a Heal hurt).
  - CriticalHitDamage is adjusted to double normal damage.
  - Positive numbers increase damage, negative decrease.
  - Config accepts Dictionary `{ "AbilityName1", int, "AbilityName2", int }`

  ###### _Example JSON config for AbilityDamageAdjustedRule_

  ```json
  {
      "Rule": "AbilityDamageAdjustedRule",
      "Config": { 
        "Zap": 1,
        "Whirlwind": 1,
      }
  },
  ```

- __AbilityRandomPieceListRule__: The randomPieceList for Abilities is adjusted
  - 🚧 _Skirmish-only - Does not work properly in multiplayer games._ 🚧
  - Some abilities (NaturesCall, RatBomb) have lists which are used to spawn random pieces.
  - This rule allows the list to be replaced with a different one.
  - Config accepts Dictionary e.g. `{ "AbilityName", BoardpieceId[], "AbilityName2", BoardpieceId[] }`  

  ###### _Example JSON config for AbilityRandomPieceListRule_

  ```json
  {
    "Rule": "AbilityRandomPieceList",
    "Config": {
      "NaturesCall": [
        "GoblinRanger",
        "Slime",
      ]
    }
  },
  ```

- __CardAdditionOverriddenRule__: Overrides the lists of cards which players receive from chests & karma
  - The default card allocation mechanism is intercepted changed to use a user-defined list of cards.
  - Config accepts Dictionary of PieceNames and lists of ability strings.. `{ "PieceName1": ["Ability1", "Ability2"], "PieceName2": ["Ability3", "Ability4"] }`  

  ###### _Example JSON config for CardAdditionOverridden_

  ```json
  {
    "Rule": "CardAdditionOverridden",
    "Config": {
        "HeroSorcerer": ["Strength", "Speed", "Bone", "Fireball", "Freeze", "SodiumHydroxide", "Teleport", "GodsFury", "RevealPath"],
        "HeroGuardian": ["Whirlwind", "Charge", "CallCompanion", "Heal"],
        }
  },
  ```

- __CardEnergyFromAttackMultipliedRule__: Card energy from attack is multiplied
  - Multiply how quickly the mana bar fills up when you attack enemies.
  - Config accepts float e.g. `1.3`  

  ###### _Example JSON config for CardEnergyFromAttackMultipliedRule_

  ```json
  {
    "Rule": "CardEnergyFromAttackMultipliedRule",
    "Config": 2.0
  },
  ```

- __CardEnergyFromRecyclingMultipliedRule__: Card energy from recycling is multiplied
  - Config accepts float e.g `1.4`  

  ###### _Example JSON config for CardEnergyFromRecyclingMultipliedRule_

  ```json
  {
    "Rule": "CardEnergyFromRecyclingMultipliedRule",
    "Config": 2.0
  },
  ```

- __CardLimitModifiedRule__: Card limit is modified
  - Change the size of the player's card hand from the default 10/11
  - Config accepts Int e.g `15`  

  ###### _Example JSON config for CardLimitModifiedRule_

  ```json
  {
    "Rule": "CardLimitModifiedRule",
    "Config": 20
  },
  ```

- __CardSellValueMultipliedRule__: Card sell values are multiplied
  - Increase card sale values in the shop. 
  - Config accepts float e.g `2.5`  

  ###### _Example JSON config for CardSellValueMultipliedRule_

  ```json
  {
    "Rule": "CardSellValueMultipliedRule",
    "Config": 2.0
  },
  ```

- __EnemyAttackScaledRule__: Enemy ⚔️attack⚔️ damage is scaled
  - Config accepts float e.g `0.85`  

  ###### _Example JSON config for EnemyAttackScaledRule_

  ```json
  {
    "Rule": "EnemyAttackScaledRule",
    "Config": 0.85
  },
  ```

- __EnemyDoorOpeningDisabledRule__: Enemy 🚪door🚪 opening ability disabled
  - Config accepts bool e.g `true`  

  ###### _Example JSON config for EnemyDoorOpeningDisabledRule_

  ```json
  {
  "Rule": "EnemyDoorOpeningDisabledRule",
  "Config": true
  },
  ```

- __EnemyHealthScaledRule__: Enemy health is scaled
  - Config accepts float e.g `0.85`  

  ###### _Example JSON config for EnemyHealthScaledRule_

  ```json
  {
    "Rule": "EnemyHealthScaledRule",
    "Config": 0.85
  },
  ```

- __EnemyRespawnDisabledRule__: Enemy respawns are disabled
  - Config accepts bool e.g `true`  

  ###### _Example JSON config for EnemyRespawnDisabledRule_

  ```json
  {
    "Rule": "EnemyRespawnDisabledRule",
    "Config": true
  },
  ```

- __GoldPickedUpMultipliedRule__: 💰Gold💰 picked up is multiplied
  - Config accepts float e.g `1.25`  

  ###### _Example JSON config for GoldPickedUpMultipliedRule_

  ```json
  {
    "Rule": "GoldPickedUpMultipliedRule",
    "Config": 1.25
  },
  ```

- __LevelExitLockedUntilAllEnemiesDefeatedRule__: The 🔒exit🔑 from each level will not open if any enemies remain.
  - This rule needs to be used in combination with other rules or it will not be possible to complete a level. (e.g. EnemyRespawnDisabledRule)

  ###### _Example JSON config for LevelExitLockedUntilAllEnemiesDefeatedRule_

  ```json
  {
    "Rule": "LevelExitLockedUntilAllEnemiesDefeatedRule",
    "Config": true
  },
  ```

- __LevelPropertiesModifiedRule__: Level properties are modified
  - Allows customisation Loot, Chests and HealingFountains on a per-floor basis
  - Config accepts Dictionary e.g. `{ "ParamName1", int, "ParamName2", int }`

  ###### _Example JSON config for LevelPropertiesModifiedRule_

  ```json
  {
  "Rule": "LevelPropertiesModifiedRule",
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

- __PetsFocusHunterMarkRule__: Pets focus on hunter marked enemies
  - Config accepts bool e.g `true`

  ###### _Example JSON config for PetsFocusHunterMarkRule_

  ```json
  {
    "Rule": "PetsFocusHunterMarkRule",
    "Config": true
  },
  ```

- __PieceConfigAdjustedRule__: Piece configuration is adjusted
  - See [PieceConfig.md](../docs/PieceConfig.md) for information about modifiable fields.
  - Allows customization of many of the properties for each game Piece. 🩺Health, 🎲ActionPoints, 🏃Movement, ⚔️MeleeDamage, etc
  - Config accepts List of Dicts e.g. `[ {}, {}, ]`
  - Only works for integer and float fields. The configured value replaces the default.

  ###### _Example JSON config for PieceConfigAdjustedRule_

  ```json
  {
    "Rule": "PieceConfigAdjustedRule",
    "Config": [
      { "Piece": "HeroSorcerer", "Property": "StartHealth", "Value": 20 },
      { "Piece": "HeroSorcerer", "Property": "MoveRange", "Value": 5 },
      { "Piece": "HeroSorcerer", "Property": "ActionPoint", "Value": 3 },
      { "Piece": "MonsterBait", "Property": "StartHealth", "Value": 30 },
      { "Piece": "BeaconOfSmite", "Property": "ActionPoint", "Value": 2 },
      { "Piece": "HeroSorcerer", "Property": "BerserkBelowHealth", "Value": 0.8 }
    ]
  },
  ```
  
- __PieceImmunityListAdjustedRule__: Piece ImmuneToStatusEffects list is adjusted
  - Allows customization of many the list of immunities for each game Piece. 🤢Diseased, 😵Stunned, 🤕Weakened, 🥶Frozen, 🧶Tangled, 💤Petrified , etc
  - Config accepts Dictionary e.g. `{ "HeroSorcerer", EventState[], "RatKing", EventState[], ... }`  

  ###### _Example JSON config for PieceImmunityListAdjustedRule_

  ```json
  {
    "Rule": "PieceImmunityListAdjusted",
    "Config": {
      "HeroSorcerer": [ "Diseased", "MarkOfAvalon", "Weaken", "Frozen", "Tangled", "Petrified" ],
      "HeroGuardian": [ "Frozen" ],
    }
  },
  ```

- __RatNestsSpawnGoldRule__: Rat nests spawn 💰gold💰
  - 🚧 _Skirmish-only - Does not work properly in multiplayer games._ 🚧
  - Config accepts bool e.g `true`  

  ###### _Example JSON config for RatNestsSpawnGoldRule_

  ```json
  {
    "Rule": "RatNestsSpawnGoldRule",
    "Config": true
  },
  ```

- __RoundCountLimitedRule__:  Sets a limit for the maximum number of rounds a game may take.
  - For ⏳ beat-the-clock ⏳ type gameplay.
  - Config accepts integer of number of rounds e.g 50  

  ###### _Example JSON config for RoundCountLimitedRule_

  ```json
  {
    "Rule": "RoundCountLimitedRule",
    "Config": 40
  },
  ```

- __SampleRule__: A [sample rule](Rules/SampleRule.cs) documenting the anatomy
  of a HouseRule rule.

- __StartCardsModifiedRule__: Player 🎴 starting cards 🎴 are modified
  - Removes all default cards from Player's hand and replaces them with custom ones.
  - Replenishable cards do not leave a players hand once cast (e.g. RepairArmor, HunterArrow or Zap)
  - Max of two replenishable cards per player.
  - Config accepts Dictionary of list of dicts e.g. `{ "<HeroName1": [ { "Card" : "<CardName>","isReplenishable": bool }, ... ], ...  }`

  ###### _Example JSON config for StartCardsModifiedRule_

  ```json
  {
     "Rule": "StartCardsModifiedRule",
     "Config": {
       "HeroGuardian": [
         { "Card": "Heal", "IsReplenishable": false },
         { "Card": "ReplenishArmor", "IsReplenishable": true },
         { "Card": "Whirlwind", "IsReplenishable": true },
         { "Card": "PiercingSpear", "IsReplenishable": false },
         { "Card": "CoinFlip", "IsReplenishable": false },
         { "Card": "BeaconOfSmite", "IsReplenishable": false },
         { "Card": "SwordOfAvalon", "IsReplenishable": false },
       ],
       "HeroHunter": [
         { "Card": "Heal", "IsReplenishable": false },
         { "Card": "HunterArrow", "IsReplenishable": true },
         { "Card": "HunterArrow", "IsReplenishable": true },
         { "Card": "CoinFlip", "IsReplenishable": false },
         { "Card": "DropChest", "IsReplenishable": false },
       ],
       "HeroSorcerer": [
         { "Card": "Heal", "IsReplenishable": false },
         { "Card": "Zap", "IsReplenishable": true },
         { "Card": "Whirlwind", "IsReplenishable": true },
         { "Card": "Freeze", "IsReplenishable": false },
         { "Card": "Fireball", "IsReplenishable": false },
         { "Card": "CallCompanion", "IsReplenishable": false },
       ],
     }
   },
   ```
- __StatusEffectConfigRule__: The parameters of different StatusEffects (🔥Torch, 🤢Poison, 🥶Frozen) can be overridden
- Accepts a list of overrides which take the place of the default config. 
  - If no override is specified, the default is used instead.
  - Default values can be found in `StatusEffectsConfig.effectsConfig` 
  - Config accepts list of dicts e.g. `[ {}, {}, ]`

  ###### _Example JSON config for StatusEffectConfigRule_

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
