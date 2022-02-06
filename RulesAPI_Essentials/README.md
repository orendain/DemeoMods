# RulesAPI Rules

A collection of predefined rules and rulesets for RulesAPI.

See the [RulesAPI readme](../RulesAPI_Core/README.md) for more information about
RulesAPI.

## Rules

- **SampleRule**: A [sample rule](Rules/SampleRule.cs) documenting the anatomy
  of a RulesAPI rule.
- **AbilityAOEAdjustedRule**: Ability AOEs are adjsuted
- **AbilityDamageAdjustedRule**: Ability damage is adjusted
- **AbilityActionCostAdjustedRule**: Ability Action Cost is adjusted
- **ActionPointsAdjustedRule**: Action points are adjusted
- **CardEnergyFromAttackMultipliedRule**: Card energy from attack is multiplied
- **CardEnergyFromRecyclingMultipliedRule**: Card energy from recycling is multiplied
- **CardSellValueMultipliedRule**: Card sell values are multiplied
- **EnemyAttackScaledRule**: Enemy attack damage is scaled
- **EnemyDoorOpeningDisabledRule**: Enemy door opening ability disabled
- **EnemyHealthScaledRule**: Enemy health is scaled
- **EnemyRespawnDisabledRule**: Enemy respawns are disabled
- **GoldPickedUpMultipliedRule**: Gold picked up is multiplied
- **LevelPropertiesModifiedRule**: Level properties are modified
- **PieceConfigAdjustedRule**: Piece configuration is adjusted
  - See [PieceConfig.md](../docs/PieceConfig.md) for information about modifiable fields.
- **RatNestsSpawnGoldRule**: Rat nests spawn gold
- **SorcererStartCardsModifiedRule**: Sorcerer start cards are modified
- **StartCardsModifiedRule**: Hero start cards are modified
- **StartHealthAdjustedRule**: Starting Health is adjusted
- **ZapStartingInventoryAdjustedRule**: Zap starting inventory is adjusted

## Rulesets

- **SampleRuleset**: A [sample ruleset](https://github.com/orendain/DemeoMods/blob/045aec568fdddb95b63a1ed34abcb64065e4ca99/Rules/RulesMod.cs#L27-L28)
  for the purposes of documenting how to create a ruleset (and for testing during development).
