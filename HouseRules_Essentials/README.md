# HouseRules Essentials

A collection of predefined rules and rulesets for HouseRules_Core

See the [HouseRules_Core readme](../HouseRules_Core/README.md) for more information about
RulesAPI.

The game is is played using a board, a number of __Pieces__ and a number of __Abilities__

## Rulesets

### Built-in Rulesets

- __SampleRuleset__: A [sample ruleset](https://github.com/orendain/DemeoMods/blob/045aec568fdddb95b63a1ed34abcb64065e4ca99/Rules/RulesMod.cs#L27-L28)
  for the purposes of documenting how to create a ruleset (and for testing during development).

### JSON Rulesets

Rulesets may also be defined in JSON within the MelonLoader config file. A sample [MelonLoader config with JSON ruleset](sample-json-ruleset.txt) is provided to help you get started. 

## Rules

- __SampleRule__: A [sample rule](Rules/SampleRule.cs) documenting the anatomy
  of a HouseRule rule.
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
      "Heal": 1
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
        "Whirlwind": 1
      }
  },
  ```

- __AbilityActionCostAdjustedRule__: Ability Action Cost is adjusted
  - Set whether abilities cost AP to cast or not.
  - `true` means the ability has a cost to cast, `false` means that it doesn't.
  - Config accepts Dictionary e.g. `{ "AbilityName1", bool, "AbilityName2", bool }`  

  ###### _Example JSON config for AbilityActionCostAdjustedRule_

  ```json
  {
    "Rule": "AbilityActionCostAdjustedRule",
    "Config": {
      "Zap": false,
      "StrengthenCourage": false
    }
  },
  ```

- ___ActionPointsAdjustedRule__: Action points are adjusted_
  - No longer needed? PieceConfig can do this too.
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

- __EnemyAttackScaledRule__: Enemy attack damage is scaled
  - Config accepts float e.g `0.85`  

  ###### _Example JSON config for EnemyAttackScaledRule_

  ```json
  {
    "Rule": "EnemyAttackScaledRule",
    "Config": 0.85
  },
  ```

- __EnemyDoorOpeningDisabledRule__: Enemy door opening ability disabled
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

- __GoldPickedUpMultipliedRule__: Gold picked up is multiplied
  - Config accepts float e.g `1.25`  

  ###### _Example JSON config for GoldPickedUpMultipliedRule_

  ```json
  {
    "Rule": "GoldPickedUpMultipliedRule",
    "Config": 1.25
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
    "FloorThreeLootChests": 9
  },
  ```

- __PieceConfigAdjustedRule__: Piece configuration is adjusted
  - See [PieceConfig.md](../docs/PieceConfig.md) for information about modifiable fields.
  - Allows customization of many of the properties for each game Piece. Health, ActionPoints, Movement, MeleeDamage, etc
  - Config accepts List of Lists e.g. `[ [ "PieceName1", "ParamName1", int ], [ "PieceName1", "ParamName1", int ], ... ]`   

  ###### _Example JSON config for PieceConfigAdjustedRule_

  ```json
  {
    "Rule": "PieceConfigAdjustedRule",
    "Config": [
      [ "HeroSorcerer", "StartHealth", "10" ],
      [ "HeroSorcerer", "MoveRange", "1" ],
      [ "HeroSorcerer", "ActionPoint", "1" ],
      [ "WolfCompanion", "StartHealth", "20" ],
      [ "SwordOfAvalon", "StartHealth", "10" ],
      [ "BeaconOfSmite", "StartHealth", "10" ],
      [ "BeaconOfSmite", "ActionPoint", "1" ],
      [ "MonsterBait", "StartHealth", "20" ]
    ]
  },
  ```

- __RatNestsSpawnGoldRule__: Rat nests spawn gold
  - Config accepts bool e.g `true`  

  ###### _Example JSON config for RatNestsSpawnGoldRule_

  ```json
  {
    "Rule": "RatNestsSpawnGoldRule",
    "Config": true
  },
  ```

- __StartCardsModifiedRule__: Hero start cards are modified
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
         { "Card": "SwordOfAvalon", "IsReplenishable": false }
       ],
       "HeroHunter": [
         { "Card": "Heal", "IsReplenishable": false },
         { "Card": "HunterArrow", "IsReplenishable": true },
         { "Card": "HunterArrow", "IsReplenishable": true },
         { "Card": "CoinFlip", "IsReplenishable": false },
         { "Card": "DropChest", "IsReplenishable": false }
       ],
       "HeroSorcerer": [
         { "Card": "Heal", "IsReplenishable": false },
         { "Card": "Zap", "IsReplenishable": true },
         { "Card": "Whirlwind", "IsReplenishable": true },
         { "Card": "Freeze", "IsReplenishable": false },
         { "Card": "Fireball", "IsReplenishable": false },
         { "Card": "CallCompanion", "IsReplenishable": false }
       ],
     }
   },
   ```

- ___StartHealthAdjustedRule__: Starting Health is adjusted_
  - Same settings handled by piececonfig. Rule not needed anymore?
