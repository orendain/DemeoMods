# DemeoMods

A collection of mods for Demeo.

Join the Demeo modding community if you are looking for support, other Demeo
mods, or are interested in learning to build your own.

[![Discord](https://img.shields.io/discord/841011788195823626?logo=discord&logoColor=fff&style=for-the-badge)](https://discord.gg/4BNSwmr784)

## Mods

### SkipIntro

Skips the intro loading scene.

### HouseRules

Set your own challenges and be the Dungeon Master of your own game. Make your own rules and challenge your friends.

HouseRules applies customisations to many settings, values and toggles used within the Demeo code.

- Adjust HP, AttackDamage, ActionPoints etc on a per-character basis
- Scale gold/mana/chests up/down
- Change starting cards dealt and max-cards allowed
- Add/Adjust AOE effects for abilities.
- Remove/add casting cost for cards
- .. and many more

This framework allows the definition of modular gameplay modifications (or
"rules") and the ability to group them to create custom gamemodes (or
"rulesets").

Rulesets can be configured as JSON files stored within the game's directoy.
It is currently necessary to select the ruleset by editing the `MelonloaderPreferences.cfg` file before starting the game.

See the [HouseRules_Core readme](HouseRules_Core/README.md) for information about the
HouseRules framework.

See the [HouseRules_Essentials readme](HouseRules_Essentials/README.md) for a list of all predefined RulesAPI
rules and rulesets.

### RoomFinder

Tired of Demeo's "Quickjoin" endlessly placing you into random games? This mod
lists all public rooms, along with their properties, so you can pick which one
to join.

![RoomFinder Screenshot](docs/roomfinder_screenshot.jpg)

### Hmm ...

A handful of other mods are in use/development privately, but it is unknown if
any will be cleaned up enough to see the light of OSS.

- PlayerBerserk, allows players to become berserk/enraged.
- RoomCode, allows players to set their own room code instead of one being
  randomly generated.
- Highlighter, ...
- DungeonMasterView, ...

## Installation

> Note: Only PCVR versions of games are currently supported.
> E.g., playing on a Quest2 works, but only when linked to a PC.

1. Install [MelonLoader](https://github.com/LavaGang/MelonLoader#how-to-use-the-installer)
   (must be version `0.5.3` or later).
2. Download the the mods that you would like to use from
   the [releases page](https://github.com/orendain/DemeoMods/releases).
3. Place the mod DLL files in the `/Mods` folder (created by MelonLoader) in
   your game directory.
4. Done. The mods will load automatically upon starting the game.

## For Developers

### `/Common`

A library of entities shared by more than one mod.

This is compiled as part of each dependent mod, so as to not generate a
common/util mod that needs to be included separately by each user.

Should there be any interest by other developers to reuse this library, the
author of this project can extract it into a common/util mod or
a [NuGet](https://www.nuget.org/).

Please file an issue if interested.

### `/Common/UI`

The start of a UI library, before the author decided too much time was being
spent on it :wink:. Now a set of helpers to make development of Unity views for
Demeo significantly easier.

## Shoutouts

- Thanks to [PyrrhaDevs](https://github.com/PyrrhaDevs) for fostering the Demeo
  modding community.
- Thanks to [Pokachi](https://github.com/Pokachi) for heavy early exploration of
  Demeo+Unity in the context of modding, and making those findings available to
  the community.
