# Changelog

## [v1.6.0-houserules](https://github.com/orendain/DemeoMods/tree/v1.6.0-houserules) (2022-12-19)

[Full Changelog](https://github.com/orendain/DemeoMods/compare/v1.5.0-houserules...v1.6.0-houserules)

**Fixes:**

- Player view fixes [\#437](https://github.com/orendain/DemeoMods/pull/437)
- Fix a client lockup and some cleanup [\#435](https://github.com/orendain/DemeoMods/pull/435)

**Merged pull requests:**

- V1.25 fixes [\#442](https://github.com/orendain/DemeoMods/pull/442)
- Update README.md for HouseRules 1.5.0 [\#428](https://github.com/orendain/DemeoMods/pull/428)
- Allow people to mod again! [\#426](https://github.com/orendain/DemeoMods/pull/426)
- Fix exploit to run public games [\#422](https://github.com/orendain/DemeoMods/pull/422)
- V1.20 & 1.21 houserules fixes [\#418](https://github.com/orendain/DemeoMods/pull/418)

## [v1.5.0-houserules](https://github.com/orendain/DemeoMods/tree/v1.5.0-houserules) (2022-10-26)

[Full Changelog](https://github.com/orendain/DemeoMods/compare/v1.4.0-houserules...v1.5.0-houserules)

## [v1.4.0-houserules](https://github.com/orendain/DemeoMods/tree/v1.4.0-houserules) (2022-06-22)

[Full Changelog](https://github.com/orendain/DemeoMods/compare/v1.3.0-houserules...v1.4.0-houserules)

**Features/Enhancements:**

- Tag HouseRules games with a custom room property. [\#360](https://github.com/orendain/DemeoMods/pull/360)
- Improve performance of the board syncing feature. [\#310](https://github.com/orendain/DemeoMods/pull/310)
- Add a `ScheduleResync` method to allow rules to manually schedule board syncs. [\#306](https://github.com/orendain/DemeoMods/pull/306)
- Enable HouseRules to run on PC edition. [\#302](https://github.com/orendain/DemeoMods/pull/302)

**Fixes:**

- Update support for starting HouseRules games from Hangouts. [\#366](https://github.com/orendain/DemeoMods/pull/366)
- Typofix - ScheduleResync method got renamed. [\#316](https://github.com/orendain/DemeoMods/pull/316)

**Chores:**

- Update for V1.16 compatibility [\#349](https://github.com/orendain/DemeoMods/pull/349)
- Update HouseRules for V1.15 compatibility. [\#344](https://github.com/orendain/DemeoMods/pull/344)
- Apply maintenance across HouseRules: fix typos, apply auto-formatting, reduce nesting. [\#341](https://github.com/orendain/DemeoMods/pull/341)
- Perform maintenance on HouseRules modules. [\#312](https://github.com/orendain/DemeoMods/pull/312)

## [v1.3.0-houserules](https://github.com/orendain/DemeoMods/tree/v1.3.0-houserules) (2022-04-17)

[Full Changelog](https://github.com/orendain/DemeoMods/compare/v1.2.0-houserules...v1.3.0-houserules)

**Features/Enhancements:**

- Check if a new release of HouseRules is available during game startup. [\#292](https://github.com/orendain/DemeoMods/pull/292)
- Support loading rulesets when loading a save. [\#274](https://github.com/orendain/DemeoMods/pull/274)

## [v1.2.0-houserules](https://github.com/orendain/DemeoMods/tree/v1.2.0-houserules) (2022-03-31)

[Full Changelog](https://github.com/orendain/DemeoMods/compare/v1.1.0-houserules...v1.2.0-houserules)

**Features/Enhancements:**

- Enable HangoutsGameRacer optimizations only when a ruleset is selected and for non-table hosts. [\#269](https://github.com/orendain/DemeoMods/pull/269)
- Enable HouseRules users to race through the Hangout game starting procedure. [\#267](https://github.com/orendain/DemeoMods/pull/267)

## [v1.1.0-houserules](https://github.com/orendain/DemeoMods/tree/v1.1.0-houserules) (2022-03-21)

[Full Changelog](https://github.com/orendain/DemeoMods/compare/v1.0.0-houserules...v1.1.0-houserules)

**Features/Enhancements:**

- Update HouseRules for Demeo v1.13. [\#246](https://github.com/orendain/DemeoMods/pull/246)
- Trigger board sync only if active ruleset requires it. [\#242](https://github.com/orendain/DemeoMods/pull/242)
- Add an enum, representing specially synced data, that rules can declare they are modifying. [\#240](https://github.com/orendain/DemeoMods/pull/240)
- Resync board only at specific times, rather than as soon as a syncable change occurs. [\#238](https://github.com/orendain/DemeoMods/pull/238)

**Fixes:**

- Resync board with clients on two new conditions: Status effects added and status effect immunities checked. [\#239](https://github.com/orendain/DemeoMods/pull/239)

**Chores:**

- Encapsulate processes dealing with directing the lifecycle of a ruleset. [\#249](https://github.com/orendain/DemeoMods/pull/249)
- Move board syncing functionality to its own class. [\#237](https://github.com/orendain/DemeoMods/pull/237)
- Move images and rulesets to sub-directories to prepare for bundling rulesets with releases. [\#232](https://github.com/orendain/DemeoMods/pull/232)

## [v1.0.0-houserules](https://github.com/orendain/DemeoMods/tree/v1.0.0-houserules) (2022-02-21)

[Full Changelog](https://github.com/orendain/DemeoMods/compare/faa2e50c1fdc985e4bf0383f16ef8980eb1580b9...v1.0.0-houserules)

**Features/Enhancements:**

- Force board state resync in cases where a client may register a new spawn. [\#151](https://github.com/orendain/DemeoMods/pull/151)
- Add abstraction for labelling rules as being safe for multiplayer use. [\#147](https://github.com/orendain/DemeoMods/pull/147)
- Pass GameContext in as argument to hook calls. [\#116](https://github.com/orendain/DemeoMods/pull/116)
- Hook into a new game being started when in post-game state. [\#115](https://github.com/orendain/DemeoMods/pull/115)
- Modify the IConfigWritable interface. [\#103](https://github.com/orendain/DemeoMods/pull/103)
- Verify that rulesets do not include duplicate IPatchable rules when registering. [\#101](https://github.com/orendain/DemeoMods/pull/101)
- Migrate patchable rules to the IPatchable interface [\#98](https://github.com/orendain/DemeoMods/pull/98)
- Implement reading and writing rulesets, and their rule configurations, from configuration files. [\#94](https://github.com/orendain/DemeoMods/pull/94)
- Broadcast a welcome message with a description of the ruleset upon game creation. [\#77](https://github.com/orendain/DemeoMods/pull/77)
- Deactive ruleset at the end of a game or when the player disconnects. [\#64](https://github.com/orendain/DemeoMods/pull/64)
- Add hooks for pre game creation and post game creation. [\#63](https://github.com/orendain/DemeoMods/pull/63)
- Write mod preferences, saving/loading selected ruleset via config file. [\#53](https://github.com/orendain/DemeoMods/pull/53)
- Introduce RulesAPI. [\#28](https://github.com/orendain/DemeoMods/pull/28)

**Fixes:**

- Introduce `ISingular` interface and add `_global` variable to each IPatchable rule. [\#136](https://github.com/orendain/DemeoMods/pull/136)
- Check for selected ruleset before attempting to trigger callbacks. [\#70](https://github.com/orendain/DemeoMods/pull/70)
- Fix rule deactivation not setting activate flag off. [\#67](https://github.com/orendain/DemeoMods/pull/67)
- Activate rules later in the game initialization process. [\#56](https://github.com/orendain/DemeoMods/pull/56)

**Chores:**

- Perform maintenance. [\#153](https://github.com/orendain/DemeoMods/pull/153)
- Clean up welcome message. [\#148](https://github.com/orendain/DemeoMods/pull/148)
- Optimize HouseRules screenshot. [\#137](https://github.com/orendain/DemeoMods/pull/137)
- Rename Registrar to Rulebook. [\#130](https://github.com/orendain/DemeoMods/pull/130)
- Move HomeRules types \(e.g., Rule, all rule interfaces, Ruleset\) to 'Types' namespace. [\#129](https://github.com/orendain/DemeoMods/pull/129)
- Rename RulesAPI mods to HouseRules. [\#126](https://github.com/orendain/DemeoMods/pull/126)
- Perform RulesAPI-related renaming. [\#105](https://github.com/orendain/DemeoMods/pull/105)
- Consolodate recent new features into a ConfigManager class. [\#99](https://github.com/orendain/DemeoMods/pull/99)
- Perform maintenance on feature for reading/writing rules to configuration. [\#95](https://github.com/orendain/DemeoMods/pull/95)
- Update method names. [\#68](https://github.com/orendain/DemeoMods/pull/68)
- Flatten rule callback calls. [\#65](https://github.com/orendain/DemeoMods/pull/65)
- Find a better hook for recognizing when a game is started. [\#62](https://github.com/orendain/DemeoMods/pull/62)
- Allow rulesets to be created without having to implement a class. [\#52](https://github.com/orendain/DemeoMods/pull/52)
- Simply access for global RulesAPI state. [\#51](https://github.com/orendain/DemeoMods/pull/51)
- Load the first registered ruleset when a game is hosted. For development purposes. [\#37](https://github.com/orendain/DemeoMods/pull/37)



\* *This Changelog was automatically generated by [github_changelog_generator](https://github.com/github-changelog-generator/github-changelog-generator)*
