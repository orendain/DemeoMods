# Changelog

## [v1.6.0-houserules](https://github.com/orendain/DemeoMods/tree/v1.6.0-houserules) (2022-12-19)

[Full Changelog](https://github.com/orendain/DemeoMods/compare/v1.5.0-houserules...v1.6.0-houserules)

**Features/Enhancements:**

- Reposition and resize elements for a cleaner look. [\#400](https://github.com/orendain/DemeoMods/pull/400)

**Fixes:**

- Rulesets should not be exported by default...  [\#451](https://github.com/orendain/DemeoMods/pull/451)

**Chores:**

- Apply maintenance to VR and NonVR UIs. [\#387](https://github.com/orendain/DemeoMods/pull/387)

**Merged pull requests:**

- Update README.md for HouseRules 1.5.0 [\#428](https://github.com/orendain/DemeoMods/pull/428)
- V1.20 & 1.21 houserules fixes [\#418](https://github.com/orendain/DemeoMods/pull/418)

## [v1.5.0-houserules](https://github.com/orendain/DemeoMods/tree/v1.5.0-houserules) (2022-10-26)

[Full Changelog](https://github.com/orendain/DemeoMods/compare/v1.4.0-houserules...v1.5.0-houserules)

## [v1.4.0-houserules](https://github.com/orendain/DemeoMods/tree/v1.4.0-houserules) (2022-06-22)

[Full Changelog](https://github.com/orendain/DemeoMods/compare/v1.3.0-houserules...v1.4.0-houserules)

**Features/Enhancements:**

- Add NonVr UI for HouseRules. [\#373](https://github.com/orendain/DemeoMods/pull/373)
- Add NonVR utilities to the common module. [\#372](https://github.com/orendain/DemeoMods/pull/372)
- Modify HouseRules to account for the recent `common/ui` refactor. [\#371](https://github.com/orendain/DemeoMods/pull/371)
- Display HouseRules version on the UI. [\#308](https://github.com/orendain/DemeoMods/pull/308)
- Enable HouseRules to run on PC edition. [\#302](https://github.com/orendain/DemeoMods/pull/302)

**Chores:**

- Adjust RoomFinder to be compatible with the latest `common/ui` refactor. [\#374](https://github.com/orendain/DemeoMods/pull/374)
- Update HouseRules for V1.15 compatibility. [\#344](https://github.com/orendain/DemeoMods/pull/344)
- Apply maintenance across HouseRules: fix typos, apply auto-formatting, reduce nesting. [\#341](https://github.com/orendain/DemeoMods/pull/341)
- Perform maintenance on HouseRules modules. [\#312](https://github.com/orendain/DemeoMods/pull/312)
- Apply partial semver checking when determining if an online version is an update. [\#309](https://github.com/orendain/DemeoMods/pull/309)

## [v1.3.0-houserules](https://github.com/orendain/DemeoMods/tree/v1.3.0-houserules) (2022-04-17)

[Full Changelog](https://github.com/orendain/DemeoMods/compare/v1.2.0-houserules...v1.3.0-houserules)

**Features/Enhancements:**

- Check if a new release of HouseRules is available during game startup. [\#292](https://github.com/orendain/DemeoMods/pull/292)

## [v1.2.0-houserules](https://github.com/orendain/DemeoMods/tree/v1.2.0-houserules) (2022-03-31)

[Full Changelog](https://github.com/orendain/DemeoMods/compare/v1.1.0-houserules...v1.2.0-houserules)

## [v1.1.0-houserules](https://github.com/orendain/DemeoMods/tree/v1.1.0-houserules) (2022-03-21)

[Full Changelog](https://github.com/orendain/DemeoMods/compare/v1.0.0-houserules...v1.1.0-houserules)

**Features/Enhancements:**

- Paginate HouseRules' Ruleset selection UI. [\#253](https://github.com/orendain/DemeoMods/pull/253)
- Update HouseRules for Demeo v1.13. [\#246](https://github.com/orendain/DemeoMods/pull/246)
- Provide visual feedback of selected ruleset on UI. [\#228](https://github.com/orendain/DemeoMods/pull/228)

## [v1.0.0-houserules](https://github.com/orendain/DemeoMods/tree/v1.0.0-houserules) (2022-02-21)

[Full Changelog](https://github.com/orendain/DemeoMods/compare/faa2e50c1fdc985e4bf0383f16ef8980eb1580b9...v1.0.0-houserules)

**Features/Enhancements:**

- Display ingame notification if custom ruleset issues are encountered during startup. [\#200](https://github.com/orendain/DemeoMods/pull/200)
- Import all JSON rulesets on game load. [\#181](https://github.com/orendain/DemeoMods/pull/181)
- Modify HouseRules UI. [\#167](https://github.com/orendain/DemeoMods/pull/167)
- Add in-game UI for HouseRules. [\#144](https://github.com/orendain/DemeoMods/pull/144)
- Shorten rule names when writing rulesets to config. [\#141](https://github.com/orendain/DemeoMods/pull/141)
- Implement full JSON ruleset export/import. [\#134](https://github.com/orendain/DemeoMods/pull/134)
- Add ConfigManager functionality allowing loading rulesets specified from config. [\#119](https://github.com/orendain/DemeoMods/pull/119)
- Serialize enums as strings instead of as ints. [\#108](https://github.com/orendain/DemeoMods/pull/108)
- Add RulesAPI\_ConfigurationRulesets mod. [\#104](https://github.com/orendain/DemeoMods/pull/104)

**Fixes:**

- Use Newtonsoft.Json instead of System.Text.Json for configuration serialization. [\#125](https://github.com/orendain/DemeoMods/pull/125)
- Fix spelling in demo rule [\#121](https://github.com/orendain/DemeoMods/pull/121)

**Chores:**

- Moves temporary example code to a separate file. [\#202](https://github.com/orendain/DemeoMods/pull/202)
- Move Import/Export ruleset methods to be non-static, to depend on serialization settings of the ConfigManager instance. [\#180](https://github.com/orendain/DemeoMods/pull/180)
- Consolidate configuration functions to RulesAPI: Configuration mod. [\#107](https://github.com/orendain/DemeoMods/pull/107)
- Perform RulesAPI-related renaming. [\#105](https://github.com/orendain/DemeoMods/pull/105)



\* *This Changelog was automatically generated by [github_changelog_generator](https://github.com/github-changelog-generator/github-changelog-generator)*
