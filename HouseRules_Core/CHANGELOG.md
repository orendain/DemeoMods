# Changelog

## [v1.1.0-houserules](https://github.com/orendain/demeomods/tree/v1.1.0-houserules) (2022-03-21)

[Full Changelog](https://github.com/orendain/demeomods/compare/v1.0.0-houserules...v1.1.0-houserules)

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

## [v1.0.0-houserules](https://github.com/orendain/demeomods/tree/v1.0.0-houserules) (2022-02-21)

[Full Changelog](https://github.com/orendain/demeomods/compare/faa2e50c1fdc985e4bf0383f16ef8980eb1580b9...v1.0.0-houserules)

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
