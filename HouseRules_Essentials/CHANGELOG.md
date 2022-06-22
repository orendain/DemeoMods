# Changelog

## [v1.4.0-houserules](https://github.com/orendain/demeomods/tree/v1.4.0-houserules) (2022-04-17)

[Full Changelog](https://github.com/orendain/demeomods/compare/v1.3.0-houserules...v1.4.0-houserules)

**Features/Enhancements:**

- Fix resetting the original value for adjusted AoE abilities. [\#304](https://github.com/orendain/DemeoMods/pull/304)
- Modify Lamp Rule to support arbitrary number of levels. [\#311](https://github.com/orendain/DemeoMods/pull/311)
- Replace `AbilityDamageAdjustedRule` with an improved `AbilityDamageOverriddenRule`. [\#318](https://github.com/orendain/DemeoMods/pull/318)
- New 'It's a Trap!' ruleset added - Build elaborate traps for your enemies (and friends). [\#320](https://github.com/orendain/DemeoMods/pull/320)
- Enable configuraable `replenishFrequency` in `StartCardsOverridden`. [\#319](https://github.com/orendain/DemeoMods/pull/319)
- Fix `LevelSequenceOverridden` bug with 'Play Again' games [\#328](https://github.com/orendain/DemeoMods/pull/328)
- New 'Demeo Reloaded' ruleset (submitted by The Gray Alien). [\#329](https://github.com/orendain/DemeoMods/pull/329)
- Update `CardHandLimit` for compatibility with Demeo versions later than v1.14 [\#333](https://github.com/orendain/DemeoMods/pull/333)
- New `TurnOrderOverridden` rule for dyanmically controlling the Heroes turn order. [\#336](https://github.com/orendain/DemeoMods/pull/336)
- New 'FreeAbilityOnCrit` rule to give a Hero a card whenever they score a Crit. [\#345](https://github.com/orendain/DemeoMods/pull/345)
- Remove limit on number of Replenishables in `StartCardsModified` as the hard limits are no more. [\#355](https://github.com/orendain/DemeoMods/pull/355)
- New `AbilityStealthDamageOverridden` rule for configuring extra damage when stealthed (submitted by The Gray Alien) [\#359](https://github.com/orendain/DemeoMods/pull/359)
- New 'Flipping Out' ruleset (idea by GQJenn, Ruleset by The Gray Alien) [\#368](https://github.com/orendain/DemeoMods/pull/369)
- Add configuration for the new Warlock character to existing rulesets. [\#369](https://github.com/orendain/DemeoMods/pull/369)

## [v1.3.0-houserules](https://github.com/orendain/demeomods/tree/v1.3.0-houserules) (2022-04-17)

[Full Changelog](https://github.com/orendain/demeomods/compare/v1.2.0-houserules...v1.3.0-houserules)

**Features/Enhancements:**

- Check if a new release of HouseRules is available during game startup. [\#292](https://github.com/orendain/DemeoMods/pull/292)
- Add a new rule for controlling the duration of TileEffects. [\#291](https://github.com/orendain/DemeoMods/pull/291)
- Extend LevelSequence rule to allow arbitrarily long sequences. [\#275](https://github.com/orendain/DemeoMods/pull/275)
- Add a new rule for overriding the LevelSequence used in a game session. [\#272](https://github.com/orendain/DemeoMods/pull/272)

## [v1.2.0-houserules](https://github.com/orendain/demeomods/tree/v1.2.0-houserules) (2022-03-31)

[Full Changelog](https://github.com/orendain/demeomods/compare/v1.1.0-houserules...v1.2.0-houserules)

**Features/Enhancements:**

- Add three new rules \(`MonsterDeckOverridden`, `LampTypesOverridden`, `AbilityHealOverridden`\), improve `Arachnophobia` ruleset, and introduce new `Earth, Wind and Fire` ruleset. [\#264](https://github.com/orendain/DemeoMods/pull/264)

**Fixes:**

- Improve board syncronization between hosts and other players. [\#265](https://github.com/orendain/DemeoMods/pull/265)

## [v1.1.0-houserules](https://github.com/orendain/demeomods/tree/v1.1.0-houserules) (2022-03-21)

[Full Changelog](https://github.com/orendain/demeomods/compare/v1.0.0-houserules...v1.1.0-houserules)

**Features/Enhancements:**

- Codify JSON rulesets. [\#257](https://github.com/orendain/DemeoMods/pull/257)
- Update HouseRules for Demeo v1.13. [\#246](https://github.com/orendain/DemeoMods/pull/246)
- Modify PieceConfigDTOdict directly, and new PieceUseWhenKilled rule [\#245](https://github.com/orendain/DemeoMods/pull/245)
- Update The Swirl ruleset. [\#241](https://github.com/orendain/DemeoMods/pull/241)
- Add two new rulesets: `Better Sorcerer` and `3x3 Potions and Buffs` [\#234](https://github.com/orendain/DemeoMods/pull/234)
- Adjust difficulty rulesets. [\#231](https://github.com/orendain/DemeoMods/pull/231)
- Add rule to control the enableBackstabBonus toggle on Abilities. [\#226](https://github.com/orendain/DemeoMods/pull/226)
- Allow backstab to be configured for multiple pieces. [\#219](https://github.com/orendain/DemeoMods/pull/219)
- Improve StatusEffectsConfig rule. [\#218](https://github.com/orendain/DemeoMods/pull/218)
- Control whether cards are returned to players hand when cast on someone already at max. [\#216](https://github.com/orendain/DemeoMods/pull/216)
- Add `SpawnCategoryOverridden` and `CardClassRestrictionOverridden`  rules and the `Arachnophobia` ruleset. [\#211](https://github.com/orendain/DemeoMods/pull/211)

**Fixes:**

- Fix Arachnophobia ruleset after StatModifiers got renamed in error. [\#250](https://github.com/orendain/DemeoMods/pull/250)
- Fix certain replaced values not being restored on rule deactivation. [\#247](https://github.com/orendain/DemeoMods/pull/247)
- Revert renaming required-named parameter. [\#244](https://github.com/orendain/DemeoMods/pull/244)
- Consider enemy friendliness in LevelExitLockedUntilAllEnemiesDefeatedRule. [\#243](https://github.com/orendain/DemeoMods/pull/243)

**Chores:**

- Perform a lookup against an AbilityKey to find a corresponding StatModifier to modify. [\#251](https://github.com/orendain/DemeoMods/pull/251)
- Modify ability rules to perform abilityKey-to-ability lookups. [\#248](https://github.com/orendain/DemeoMods/pull/248)
- Perform some maintenance on rules. [\#230](https://github.com/orendain/DemeoMods/pull/230)
- Clean up unused 'lookupstring' var. [\#215](https://github.com/orendain/DemeoMods/pull/215)

## [v1.0.0-houserules](https://github.com/orendain/demeomods/tree/v1.0.0-houserules) (2022-02-21)

[Full Changelog](https://github.com/orendain/demeomods/compare/faa2e50c1fdc985e4bf0383f16ef8980eb1580b9...v1.0.0-houserules)

**Features/Enhancements:**

- A rule for adjusting the StatModifiers [\#198](https://github.com/orendain/DemeoMods/pull/198)
- Three experimental rules for modifying Piece Abilities, Behaviours and PieceType lists [\#197](https://github.com/orendain/DemeoMods/pull/197)
- Add support for PieceConfigAdjustedRule to set float properties. [\#193](https://github.com/orendain/DemeoMods/pull/193)
- Add 'Hunters Paradise' ruleset. [\#188](https://github.com/orendain/DemeoMods/pull/188)
- Add PetsFocusHunterMarkRule. [\#187](https://github.com/orendain/DemeoMods/pull/187)
- Added 'The Swirl' ruleset. [\#183](https://github.com/orendain/DemeoMods/pull/183)
- Add rule to keep exit locked until all enemies are defeated. [\#182](https://github.com/orendain/DemeoMods/pull/182)
- PieceConfigAdjustedRule config uses desired values instead of +- modifiers. [\#173](https://github.com/orendain/DemeoMods/pull/173)
- Tweak BeatTheClock ruleset. [\#171](https://github.com/orendain/DemeoMods/pull/171)
- Additional Rulesets and corrections [\#170](https://github.com/orendain/DemeoMods/pull/170)
- Add rule to override StatusEffectConfig  [\#164](https://github.com/orendain/DemeoMods/pull/164)
- Add rule to adjust PieceConfig ImmuneToStatusEffects lists. [\#162](https://github.com/orendain/DemeoMods/pull/162)
- Updated Difficulty Rulesets and added ruleset [\#160](https://github.com/orendain/DemeoMods/pull/160)
- Add "Beat the clock!" ruleset. [\#156](https://github.com/orendain/DemeoMods/pull/156)
- Add rule limiting the total number of rounds allowed to play. [\#155](https://github.com/orendain/DemeoMods/pull/155)
- Add rule that allows to override cards that players receive. [\#154](https://github.com/orendain/DemeoMods/pull/154)
- Created additional core ruleset [\#146](https://github.com/orendain/DemeoMods/pull/146)
- Add Rule to modify the randomPieceList used by a few abilities. [\#143](https://github.com/orendain/DemeoMods/pull/143)
- Add rulesets for easy/hard/legendary difficulty. [\#133](https://github.com/orendain/DemeoMods/pull/133)
- Add rule modifying card limit. [\#120](https://github.com/orendain/DemeoMods/pull/120)
- Add rule modifying level properties. [\#114](https://github.com/orendain/DemeoMods/pull/114)
- Add start card rule affecting all heroes. [\#106](https://github.com/orendain/DemeoMods/pull/106)
- Make even more rules config writeable [\#102](https://github.com/orendain/DemeoMods/pull/102)
- Make more rules config writable. [\#100](https://github.com/orendain/DemeoMods/pull/100)
- A rule for adjusting the AOE range of abilities [\#93](https://github.com/orendain/DemeoMods/pull/93)
- Add rule for setting Sorcerer start cards. [\#92](https://github.com/orendain/DemeoMods/pull/92)
- AbilityActionCostRule - A rule for adjusting whether specific cards have a cost to cast or not. [\#89](https://github.com/orendain/DemeoMods/pull/89)
- Add rule for scaling all enemy attack damage. [\#88](https://github.com/orendain/DemeoMods/pull/88)
- Add rule for scaling all enemy health. [\#87](https://github.com/orendain/DemeoMods/pull/87)
- Add rule disallowed enemies from opening doors. [\#86](https://github.com/orendain/DemeoMods/pull/86)
- Add rule for multiplying recycled card energy. [\#85](https://github.com/orendain/DemeoMods/pull/85)
- Added rule for multiplying card energy gained from attacks. [\#84](https://github.com/orendain/DemeoMods/pull/84)
- A generic PiceConfig Adjuster Rule which can set any named integer field. [\#74](https://github.com/orendain/DemeoMods/pull/74)
- Add rule for adjusting StartHealth [\#72](https://github.com/orendain/DemeoMods/pull/72)
- Introduce generic ability damage rule. [\#69](https://github.com/orendain/DemeoMods/pull/69)
- Introduce ActionPointsAdjustedRule. [\#59](https://github.com/orendain/DemeoMods/pull/59)
- Add rule which increases card sell values. [\#40](https://github.com/orendain/DemeoMods/pull/40)
- Add rule for multiplying gold picked up. [\#39](https://github.com/orendain/DemeoMods/pull/39)
- Add rule for disabling enemy respawns. [\#38](https://github.com/orendain/DemeoMods/pull/38)
- Add rule for adjusting starting count of Zap cards. [\#36](https://github.com/orendain/DemeoMods/pull/36)
- Add rule for adjusting Zap damage. [\#34](https://github.com/orendain/DemeoMods/pull/34)
- Add rule for making rat nests spawn gold. [\#32](https://github.com/orendain/DemeoMods/pull/32)
- Add ballista damage and action points adjustment rules. [\#31](https://github.com/orendain/DemeoMods/pull/31)
- Create basic rule and ruleset for RulesAPI. [\#30](https://github.com/orendain/DemeoMods/pull/30)

**Fixes:**

- Use item's key when intepolating strings. [\#172](https://github.com/orendain/DemeoMods/pull/172)
- Add QuickAndDeadRuleset to csproj. [\#163](https://github.com/orendain/DemeoMods/pull/163)
- Fix card limit rule assuming the card limit was 0 by default. [\#127](https://github.com/orendain/DemeoMods/pull/127)
- Fix level properties rule so deactivation works. [\#118](https://github.com/orendain/DemeoMods/pull/118)
- Fix zap patching. [\#55](https://github.com/orendain/DemeoMods/pull/55)

**Chores:**

- Deregulation - Remove duplicated rule functionality.  [\#184](https://github.com/orendain/DemeoMods/pull/184)
- Fix indentation warnings. [\#165](https://github.com/orendain/DemeoMods/pull/165)
- Remove sample rule and ruleset from being registered. [\#157](https://github.com/orendain/DemeoMods/pull/157)
- Mark more rules as being multiplayer safe. [\#152](https://github.com/orendain/DemeoMods/pull/152)
- Label rules that have been tested to be safe for multiplayer. [\#150](https://github.com/orendain/DemeoMods/pull/150)
- Establish how to separate ruleset definitions from one another. [\#132](https://github.com/orendain/DemeoMods/pull/132)
- Remove unneeded \(?\) rules. [\#128](https://github.com/orendain/DemeoMods/pull/128)
- Perform RulesAPI-related renaming. [\#105](https://github.com/orendain/DemeoMods/pull/105)
- actionPoints varialbe name was out of context. [\#82](https://github.com/orendain/DemeoMods/pull/82)



\* *This Changelog was automatically generated by [github_changelog_generator](https://github.com/github-changelog-generator/github-changelog-generator)*
