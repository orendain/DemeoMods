# HouseRules Essentials

A collection of predefined rules and rulesets for HouseRules_Core

![HouseRules Logo](../docs/images/house-rules-logo1.png)

See the [HouseRules_Core readme](../HouseRules_Core/README.md) for more information about
HouseRules API.

## Rulesets

### Built-in Rulesets

- __Earth Wind & Fire__ : Not the band. Let's get Elemental.
- __🕷️Arachnophobia🕷️__ : Offers a fresh adventure to be played on the RootsOfEvil Map.
  Chased by violent thugs from their ancestral homes in Sunderhaven, the King and Queen flee into the woods.
  Befriended by money spiders, they hatch a plan to rebuild their fallen empires, but first they're going to need some cash.
- __🎲LuckyDip🎲__ : Players each start with two 'Drop Chest' cards instead of their normal
  starting cards, meaning that no two games start the same. Many potions have AOE effect, because it's rude not to share.
  Many other changes included for faster gameplay with an aim of around 90 minutes per game.
- __💣It's A Trap💣__ : Build fiendiesh traps for your enemies and lure them to their deaths, but do try not to kill your friends. Lamps and BoobyTraps aplenty. Enemies cannot open doors, DetectEnemies/EyeOfAvalon & Torch will not be attacked, Stealth & TileEffect durations extended.
- __The Swirl__ : Only poison, fireballs and vortexes. Health and POIs aplenty, but must defeat all enemies to escape.
- __Beat The Clock__ : Ultra health. Ultra card recycling. Only 15 rounds to escape...
- __Hunter's Paradise__ : Pets, pets, pets! And hunter's mark.
- __Demeo Reloaded__ : The Gray Alien's ruleset. MANY class changes. NEW enemies. BETTER loot. No respawns. Yet somehow challenging...
- __Difficulty Easy__ : Decreased game difficulty for a more casual playstyle.
- __Difficulty Hard__ : Increased game difficulty for a greater challenge.
- __Difficulty Legendary__ : Increased game difficulty for those who want to be a legend.
- __3x3 Potions and Buffs__ : Heal, Strength, Speed, Adamant, Antitoxin, RepairArmor and Bard buffs are 3x3 AOE.
- __Better Sorcerer__ : 0 Action Cost for Sorcerer's Zap - No other changes. #STS
- __No Surprises__ :  No surprises in the dark or coming through doors.
- __Quick and the Dead__ : A mode with a small hand but fast turnaround time on cards means you need to not hesitate.
- __Flipping Out__ : Coin Flips ONLY! BIG ENEMIES! Heads... or tails?
### JSON Rulesets

Rulesets may also be configured as JSON files and stored within the game directory `<GAME_DIR>/UserData/HouseRules/<rulesetname>.json`

A copy of all built-in rulesets are included as JSON files and, after
HouseRules installation, can be found in the game directory
`<GAME_DIR>/UserData/HouseRules/ExampleRulesets/` so you can modify them
to your heart's content, or use them as a starting point for your own completely
custom ruleset.

Additionally, [those JSON rulesets can be found within this repository](../docs/rulesets).

The [Settings Reference](../docs/SettingsReference.md) contains lists of all different BehaviourIDs, AbilityKeys and other data types used by the Rules.

## Rules and Configurations

#### __AbilityActionCostAdjusted__: Adjusts the casting costs for player abilities.
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
      "HealingPotion": true
    }
  },
  ```

#### __AbilityAoeAdjusted__: Adjusts the Area of Effect range(s) for abilities.
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
      "HealingPotion": 1
    }
  },
  ```

#### __AbilityBackstabAdjusted__: Adjusts the enableBackstabBonus setting for abilities.
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
      "Arrow": true,
      "PiercingArrow": true,
      "PoisonedTip": true,
      "Fireball": true,
      "Freeze": true
    }
  },
  ```

#### __AbilityDamageOverridden__: Ability targetDamage and critDamage are adjusted
  - Only functions for abilities which do damage. (You can't make a HealingPotion hurt).
  - CriticalHitDamage is adjusted to double normal damage.
  - To configure:
    - Specify the [AbilityKey](../docs/SettingsReference.md#abilitykeys) of the ability to modify.
    - Specify a positive integers for targetDamage and critDamage respectively E.g.: `"Zap": [ 2, 5 ]` will set Zap targetDamage to 2 and critDmage to 5.

  ###### _Example JSON config for AbilityDamageOverridden_

  ```json
  {
    "Rule": "AbilityDamageOverridden",
    "Config": {
      "Zap": [ 2, 4 ],
      "Whirlwind": [ 4, 8 ]
    }
  },
  ```

#### __AbilityHealOverridden__: Ability which heal may have the healAmount overridden
  - Only functions for abilities which do heal. (You can't make a Fireball heal).
  - To configure:
    - Specify the [AbilityKey](../docs/SettingsReference.md#abilitykeys) of the ability to modify.
    - Specify the new integer for healAmount to use.

  ###### _Example JSON config for AbilityHealOverridden_

  ```json
  {
    "Rule": "AbilityHealOverridden",
    "Config": {
      "HealingPotion": 3
    }
  },
  ```

#### __AbilityRandomPieceList__: The randomPieceList for Abilities is adjusted
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
        "Slime"
      ]
    }
  },
  ```

#### __AbilityStealthDamageOverridden__: Ability stealthBonusDamage is overridden
  - Can function for abilities which don't do damage. (You can make a FlashBomb hurt).
  - To configure:
    - Specify the [AbilityKey](../docs/SettingsReference.md#abilitykeys) of the ability to modify.
    - Specify a positive integer for stealthBonusDamage E.g.: `"PlayerMelee": 2` adds 2 damage to a normal attack if stealthed.

  ###### _Example JSON config for AbilityStealthDamageOverridden_

  ```json
  {
    "Rule": "AbilityStealthDamageOverridden",
    "Config":
    {
      "Blink": 4,
      "DiseasedBite": 2,
      "PoisonBomb": 1,
      "CursedDagger": 3,
      "PlayerMelee": 2
    }
  },
  ```
#### __BackstabConfigOverridden__: A list of Pieces may use 🔪Backstab🔪 instead of just the Assassin
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

#### __CardAdditionOverridden__: Overrides the lists of cards which players receive from chests & card energy.
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
      "HeroGuardian": ["WhirlwindAttack", "Charge", "CallCompanion", "HealingPotion"]
    }
  },
  ```

#### __CardEnergyFromAttackMultiplied__: Card energy from attack is multiplied
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

#### __CardEnergyFromRecyclingMultiplied__: Card energy from recycling is multiplied
  - To configure:
    - Specify a decimal number representing how the energy is multiplied.

  ###### _Example JSON config for CardEnergyFromRecyclingMultiplied_

  ```json
  {
    "Rule": "CardEnergyFromRecyclingMultiplied",
    "Config": 2.0
  },
  ```

#### __CardLimitModified__: Card limit is modified
  - 🚧 _Skirmish-only - Does not work properly in multiplayer games._ 🚧
  - Change the size of the player's card hand from the default 9 plus replenishable cards.
  - To configure:
    - Specify an integer representing the total size of the player's hand minus replenishable cards.

  ###### _Example JSON config for CardLimitModified_

  ```json
  {
    "Rule": "CardLimitModified",
    "Config": 18
  },
  ```

#### __CardSellValueMultiplied__: Card sell values are multiplied
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

#### __CardClassRestrictionOverridden__: Overrides Character Class assignments for cards.
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
        "Zap": "Hunter"
      }
    },
  ```

#### __CourageShantyAddsHp__: In addition to normal effects, Courage Shanty also adds HP
  - To configure:
    - Specify a decimal number for how many HP to add to the target each time CourageShanty is used.

  ###### _Example JSON config for CourageShantyAddsHp_

  ```json
  {
    "Rule": "CourageShantyAddsHp",
    "Config": 2
  },
  ```

#### __EnemyAttackScaled__: Enemy ⚔️attack⚔️ damage is scaled
  - To configure:
    - Specify a decimal number representing how enemy attack damage is multiplied.

  ###### _Example JSON config for EnemyAttackScaled_

  ```json
  {
    "Rule": "EnemyAttackScaled",
    "Config": 0.85
  },
  ```

#### __EnemyCooldownOverridden__: Enemy ability cooldowns are overridden
  - Lets you set the number of turns before enemies can use specified abilities again.
  - To configure:
    - Specify the [AbilityKeys](../docs/SettingsReference.md#abilitykeys) of the ability to modify.
    - Specify the new integer for cooldown to use.

  ###### _Example JSON config for EnemyDoorOpeningDisabled_

  ```json
  {
    "Rule": "EnemyCooldownOverridden",
    "Config": {
      "LeapHeavy": 2,
      "EnemyFrostball": 2,
      "Shockwave": 3,
      "EnemyFireball": 2,
      "EnemyFlashbang": 2
    }
  },
  ```

#### __EnemyDoorOpeningDisabled__: Enemy 🚪door🚪 opening ability disabled
  - To configure:
    - Specify `true` to disable enemies opening doors.

  ###### _Example JSON config for EnemyDoorOpeningDisabled_

  ```json
  {
    "Rule": "EnemyDoorOpeningDisabled",
    "Config": true
  },
  ```

#### __EnemyHealthScaled__: Enemy health is scaled
  - To configure:
    - Specify a decimal number representing how enemy health is multiplied.

  ###### _Example JSON config for EnemyHealthScaled_

  ```json
  {
    "Rule": "EnemyHealthScaled",
    "Config": 0.85
  },
  ```

#### __EnemyRespawnDisabled__: Enemy respawns are disabled
  - To configure:
    - Specify `true` to disable enemy respawns.

  ###### _Example JSON config for EnemyRespawnDisabled_

  ```json
  {
    "Rule": "EnemyRespawnDisabled",
    "Config": true
  },
  ```

#### __FreeAbilityOnCrit__: A Critical Hit rewards you with a free ability.
  - Whenever you score a critical hit, a user-configured card is added to your inventory.
  - Allows configuration of different abilities on a per-hero basis.
  - To configure:
    - Specify a Dictionary of [BoardPieceIds](../docs/SettingsReference.md#boardpieceids) and abilities.

  ###### _Example JSON config for FreeAbilityOnCrit_

  ```json
  {
    "Rule": "FreeAbilityOnCrit",
    "Config": {
      "HeroBard": "OneMoreThing",
      "HeroHunter": "PoisonedTip",
      "HeroSorcerer": "Fireball",
      "HeroGuardian": "Bone",
      "HeroRogue": "PoisonBomb"
    }
  },
```  

#### __GoldPickedUpMultiplied__: 💰Gold💰 picked up is multiplied
  - To configure:
    - Specify a decimal number representing how gold is multiplied.

  ###### _Example JSON config for GoldPickedUpMultiplied_

  ```json
  {
    "Rule": "GoldPickedUpMultiplied",
    "Config": 1.25
  },
  ```

#### __LevelExitLockedUntilAllEnemiesDefeated__: The 🔒exit🔑 from each level will not open if any enemies remain.
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

#### __LampTypesOverridden__: The types of lamps spawned on each floor are overridden.
  - Lamps are spawned in pre-set locations on each map, but the list of available lamps can be configured.
  - Lamps are spawned at random, so specifying multiples of some lamp types can be used to get a desired ratio.
  - Boardpieces other than lamps (e.g. SporeFungus) can also be spawned.
  - To configure:
    - Specify a list of [BoardPieceIds](../docs/SettingsReference.md#boardpieceids) for each Floor level.
    - Floor level names must be Floor1Lamps, Floor2Lamps and Floor3Lamps

  ###### _Example JSON config for LampTypesOverridden_

  ```json
  {
    "Rule": "LampTypesOverridden",
    "Config": {
      "1": [
        "OilLamp",
        "OilLamp",
        "OilLamp",
        "VortexLamp"
      ],
      "2": [
        "GasLamp",
        "GasLamp",
        "GasLamp",
        "VortexLamp"
      ],
      "3": [
        "IceLamp",
        "IceLamp",
        "IceLamp",
        "VortexLamp"
      ]
    }
  },
  ```

#### __LevelPropertiesModified__: Level properties are modified
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
      "FloorThreeLootChests": 9
    }
  },
  ```

#### __LevelSequenceOverridden__: The Level Sequence of dungeon floors is overridden.
  - List of levels must be exactly five items long. The game will crash at the end if the list is any longer.
  - Shop levels can be replaced with game levels.
  - It is possible to use levels from any book (Elven, Sewers, Forest) together in a single list
  - Level soundtracks may not match the played level or adventure (e.g. The shop "Ah Customers, Welcome" will always play on 2nd and 4th levels)
  - Level names are ElvenFloor01-17, SewersFloor01-12, ForestFloor01-03, ForestFloor05-09, ShopFloor02, SewersShopFloor & ForestShopFloor
  - To configure:
    - Specify a list of [LevelNames](../docs/LevelNames.md).

  ###### _Example JSON config for LevelPropertiesModified_

  ```json
  {
    "Rule": "LevelSequenceOverridden",
    "Config": [ "ElvenFloor01", "SewersFloor07", "ForestFloor09", "ForestShopFloor", "ElvenFloor08" ]
  },
  ```

#### __MonsterDeckOverridden__: The MonsterDeck which is used for spawning monsters is overridden.
  - This rule is a more advanced implementation of SpawnCategoriesOverridden, and will directly configure the MonsterDeck from lists.
  - Within the game the `AIDirectorController` deals Monsters from the MonsterDeck when populating the levels.
  - Two subdecks are used for each floor. One subdeck is used when players are near the Entrance, and another nearer the exit.
  - The subdecks are called 'standard' and 'spike' within the game code, but we're callling them 'Entrance' and 'Exit' for simplicity.
  - The final 'Boss' level only has a single subDeck
  - In addition to the MonsterDeck, configuration for the KeyHolder for each floor and the Boss is also required.
  - To configure:
    - Specify a lists of [BoardPieceIds](../docs/SettingsReference.md#boardpieceids) for each of the five subdecks.
    - The subdecks must be named `EntranceDeckFloor1`, `ExitDeckFloor1`, `EntranceDeckFloor2`, `ExitDeckFloor2`, `BossDeck`
    - Specify single [BoardPieceId](../docs/SettingsReference.md#boardpieceids) for each of `KeyHolderFloor1`, `KeyHolderFloor2`, and `Boss`

  ###### _Example JSON config for MonsterDeckOverridden_

  ```json
  {
    "Rule": "MonsterDeckOverridden",
    "Config": {
      "EntranceDeckFloor1": {
        "Spider": 0,
        "IceElemental": 2,
        "ChestGoblin": 3,
        "FireElemental": 2
      },
      "ExitDeckFloor1": {
        "Rat": 20,
        "Spider": 20,
        "IceElemental": 2,
        "ChestGoblin": 3,
        "Mimic": 1,
        "GoblinMadUn": 1,
        "DruidArcher": 1
      },
      "EntranceDeckFloor2": {
        "Spider": 10,
        "GoblinFighter": 0,
        "SporeFungus": 10,
        "SpiderEgg": 3,
        "FireElemental": 2,
        "ElvenArcher": 2
      },
      "ExitDeckFloor2": {
        "Spider": 20,
        "Rat": 30,
        "Bandit": 2,
        "ChestGoblin": 3,
        "ElvenPriest": 4,
        "ElvenMarauder": 2
      },
      "BossDeck": {
        "SpiderEgg": 10,
        "TheUnseen": 0,
        "TheUnheard": 0,
        "TheUnspoken": 0,
        "Slimeling": 0,
        "ElvenSkirmisher": 2
      },
      "KeyHolderFloor1": "Cavetroll",
      "KeyHolderFloor2": "Sigataur",
      "Boss": "Brookmare"
    }
  },
  ```

#### __PartyElectricityDamageOverridden__: Electrical damage between players is zeroed
  - Electricity Damage from friendly fire is zeroed
  - To configure:
    - Specify `true` to remove player on player electrical damage.

  ###### _Example JSON config for PartyElectricityDamageOverridden_

  ```json
    {
      "Rule": "PartyElectricityDamageOverridden",
      "Config": true
    },
  ```

#### __PetsFocusHunterMark__: Pets focus on hunter marked enemies
  - To configure:
    - Specify `true` to have pets focus on enemies that are marked.

  ###### _Example JSON config for PetsFocusHunterMark_

  ```json
  {
    "Rule": "PetsFocusHunterMark",
    "Config": true
  },
  ```

#### __PieceAbilityListOverridden__: The list of abilities for a ♟️BoardPiece is overridden.
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
      "GoblinFighter": [ "SpawnCultists", "EnemyStealGold", "DropChest", "EnemyStealCard", "EnemyStealGold" ]
    }
  },
  ```

#### __PieceBehavioursListOverridden__: The list of behaviours that a ♟️BoardPiece behaves is overridden.
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
      "GoblinFighter": [ "FollowPlayerRangedAttacker", "RangedSpellCaster" ]
    }
  },
  ```

#### __PieceConfigAdjusted__: Allows customization of any numeric field for any ♟️BoardPiece
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
      { "Piece": "SmiteWard", "Property": "ActionPoint", "Value": 2 },
      { "Piece": "HeroSorcerer", "Property": "BerserkBelowHealth", "Value": 0.8 }
    ]
  },
  ```
  
#### __PieceImmunityListAdjusted__: Allows the list of immunities for any ♟️BoardPiece to be overridden
  - Allows customization of many the list of immunities for each game Piece. 🤢Diseased, 😵Stunned, 🤕Weaken1Turn, 🥶Frozen, 🧶Tangled, 💤Petrified , etc
  - To configure:
    - Specify the [BoardPieceId](../docs/SettingsReference.md#boardpieceids) of the piece to modify.
    - Specify the list of [EffectStates](../docs/SettingsReference.md#effectstatetypes) that the piece should be immune to.

  ###### _Example JSON config for PieceImmunityListAdjusted_

  ```json
  {
    "Rule": "PieceImmunityListAdjusted",
    "Config": {
      "HeroSorcerer": [ "Diseased", "HuntersMark", "Weaken1Turn", "Frozen", "Tangled", "Petrified" ],
      "Spider": [ "Weaken2Turns", "Panic" ]
    }
  },
  ```

#### __PiecePieceTypeListOverridden__: Allows the list of PieceTypes for a ♟️BoardPiece to be overridden.
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
      "GoblinFighter": [ "Enemy", "Goblin", "Thief", "Canine" ]
    }
  },
  ```
  
#### __PieceUseWhenKilledOverridden__: Allows the list of UseWhenKilled abilities for any ♟️BoardPiece to be overridden
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
      "CaveTroll": [ "Rejuvenation" ]
    }
  },
  ```

#### __RatNestsSpawnGold__: Rat nests spawn 💰gold💰
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

#### __RegainAbilityIfMaxxedOutOverridden__: Controls whether you get a potion back when you cast it on someone who is already at max.
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

#### __RoundCountLimited__:  Sets a limit for the maximum number of rounds a game may take.
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

#### __SampleRule__: A [sample rule](Rules/SampleRule.cs) documenting the anatomy of a HouseRule rule.

#### __SpawnCategoryOverridden__:  Overrides the Spawn Categories which control distribution of pieces in each map.
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
        "ElvenQueen": [ 1, 1, 2 ]
      }
    },
  ```

#### __StartCardsModified__: Player 🎴 starting cards 🎴 are modified
  - Removes all default cards from Player's hand and replaces them with custom ones.
  - Replenishable cards do not leave a players hand once cast (e.g. RepairArmor, Arrow or Zap).
  - Max of two replenishable cards per player.
  - To configure:
    - Specify the [BoardPieceId](../docs/SettingsReference.md#boardpieceids) of the piece to replace the starting card hand of.
    - Specify the [AbilityKey](../docs/SettingsReference.md#boardpieceids) of the cards to add to the piece's hand.
    - Specify integer value for replenish frequency. 0=NoReplenish, 1=EveryTurn, 2=Every 2 Turns etc
    - Max replenish frequency value is 7 

  ###### _Example JSON config for StartCardsModified_

  ```json
  {
    "Rule": "StartCardsModified",
    "Config": {
      "HeroGuardian": [
        { "Card": "HealingPotion", "ReplenishFrequency": 0 },
        { "Card": "ReplenishArmor", "ReplenishFrequency": 1 },
        { "Card": "WhirlwindAttack", "ReplenishFrequency": 1 },
        { "Card": "PiercingThrow", "ReplenishFrequency": 0 },
        { "Card": "CoinFlip", "ReplenishFrequency": 0 },
        { "Card": "TheBehemoth", "ReplenishFrequency": 0 },
        { "Card": "SwordOfAvalon", "ReplenishFrequency": 0 }
      ],
      "HeroHunter": [
        { "Card": "HealingPotion", "ReplenishFrequency": 0 },
        { "Card": "Arrow", "ReplenishFrequency": 1 },
        { "Card": "Arrow", "ReplenishFrequency": 1 },
        { "Card": "CoinFlip", "ReplenishFrequency": 0 },
        { "Card": "DropChest", "ReplenishFrequency": 0 }
      ],
      "HeroSorcerer": [
        { "Card": "HealingPotion", "ReplenishFrequency": 0 },
        { "Card": "Zap", "ReplenishFrequency": 1 },
        { "Card": "WhirlwindAttack", "ReplenishFrequency": 1 },
        { "Card": "Freeze", "ReplenishFrequency": 0 },
        { "Card": "Fireball", "ReplenishFrequency": 0 },
        { "Card": "CallCompanion", "ReplenishFrequency": 0 }
      ]
    }
  },
  ```

#### __StatModifiersOverridden__: The additiveBonus parameters of StatModifiers are overridden.
  - There are only six different StatModifiers in the game. They are used by 💪StrengthPotion, 🦶SwiftnessPotion, 🛡️ReplenishArmor, HuntersMark, etc.
  - These modifiers control the power of each corresponding ability.  E.g., by default the stat modifier for SongOfResilience is 5, as it grants 5 units of armor.  
  - To configure:
    - Specify the [AbilityKey](../docs/SettingsReference.md#boardpieceids) of the ability whose stat modifer should be replaced.
    - Specify an integer representing the new value of the stat modifier.

  ###### _Example JSON config for StatModifiersOverridden_

  ```json
  {
    "Rule": "StatModifiersOverridden",
    "Config": {
      "StrengthPotion": 2,
      "SwiftnessPotion": 2,
      "HuntersMark": -4,
      "ReplenishBarkArmor": 4,
      "SongOfResilience": 6,
      "ReplenishArmor": 4
    }
  },
  ```

#### __StatusEffectConfig__: The parameters of different StatusEffects (🔥Torch, 🤢Poison, 🥶Frozen) can be overridden
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

#### __TileEffectDurationOverridden__: Overrides TileEffect durations settings for gas, acid, web etc.
  - There are five different TileEffects in the game. Gas, Acid, Web, Water and Target.
  - Overriding the durations allows for TileEffects to last for longer or shorter times.
  - It is necessary to specify all five effects in the config for this rule, or they will assume a default duration of 9999 rounds.
  - To configure:
    - Specify the `TileEfect` (Gas, Acidm etc) of the effect whose duration should be replaced.
    - Specify an integer representing the new duration in turns.

  ###### _Example JSON config for TileEffectDurationOverridden_

  ```json
  {
    "Rule": "TileEffectDurationOverridden",
    "Config": {
      "Gas": 3,
      "Acid": 4,
      "Web": 2,
      "Water": 2,
      "Target": 0
    }
  },
  ```

#### __TurnOrderOverridden__: Override the player turn order. 
  - The game will compute the turn order at the start of every turn.
  - For each attribute that a player satisfies, they get a corresponding value added to their initiative score. Players with higher initiative scores go first.
  - The `Downed` attribute is considered when players are downed, and the `Javelin` attribute is considered when players have the Sigataurian Javelin in their inventory.
  - To configure:
    - Specify a value for any attribute that should add to a player's initiative score.

  ###### _Example JSON config for TurnOrderOverridden_

  ```json
  {
    "Rule": "TurnOrderOverridden",
    "Config": {
      "Assassin": 8,
      "Bard": 2,
      "Guardian": 20,
      "Hunter": 6,
      "Sorcerer": 4,
      "Downed": 10,
      "Javelin": 10
    }
  },
  ```
