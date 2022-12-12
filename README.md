# DemeoMods

A collection of mods for Demeo.

Join the Demeo modding community if you are looking for support, other Demeo
mods, or are interested in learning to build your own.

[![Discord](https://img.shields.io/discord/841011788195823626?logo=discord&logoColor=fff&style=for-the-badge)](https://discord.gg/4BNSwmr784)

[![Latest Build](https://img.shields.io/github/workflow/status/orendain/demeomods/Build%20Mods/main?label=latest%20build&style=for-the-badge)](https://github.com/orendain/DemeoMods/actions/workflows/build.yml)

## Contents
- [Installation](#installation)
- [Mods](#mods)
  - [HouseRules](#houserules)
  - [SkipIntro](#skipintro)
  - [RoomFinder](#roomfinder)
  - [RoomCode](#roomcode)
- [For Developers](#for-developers)
- [Shoutouts](#shoutouts)

## Installation

> Note: Only the PCVR and PC Edition versions of Demeo are currently supported.
> E.g., playing on a Quest2 works, but only when linked to a PC.

1. Install [MelonLoader](https://github.com/LavaGang/MelonLoader#how-to-use-the-installer)
2. Download the latest [Nightly MelonLoader Alpha](https://nightly.link/LavaGang/MelonLoader/workflows/build/alpha-development) and unzip into the root of your Demeo game directory (allow overwrite).
   (must be 0.5.7 or later)
3. Download the latest version of the mod you want from the table below.
4. Unzip the downloaded files/folders into the root of your Demeo game directory.
5. Done. The mods will automatically load upon starting Demeo.

| Mod        | Latest Release                                                                 | Direct Download Link                                                                                                   |
|------------|--------------------------------------------------------------------------------|------------------------------------------------------------------------------------------------------------------------|
| HouseRules | [v1.5.0](https://github.com/orendain/DemeoMods/releases/tag/v1.5.0-houserules) | [HouseRules_1.5.0.zip](https://github.com/orendain/DemeoMods/releases/download/v1.5.0-houserules/HouseRules_1.5.0.zip) |
| SkipInto   | [v1.4.0](https://github.com/orendain/DemeoMods/releases/tag/v1.4.0-skipintro)  | [SkipIntro_1.4.0.zip](https://github.com/orendain/DemeoMods/releases/download/v1.4.0-skipintro/SkipIntro_1.4.0.zip)    |
| RoomFinder | [v1.7.0](https://github.com/orendain/DemeoMods/releases/tag/v1.7.0-roomfinder) | [RoomFinder_1.7.0.zip](https://github.com/orendain/DemeoMods/releases/download/v1.7.0-roomfinder/RoomFinder_1.7.zip) |
| RoomCode   | [v1.2.0](https://github.com/orendain/DemeoMods/releases/tag/v1.2.0-roomcode)   | [RoomCode_1.2.0.zip](https://github.com/orendain/DemeoMods/releases/download/v1.2.0-roomcode/RoomCode_1.2.0.zip)       |

## Mods

### HouseRules

![HouseRules Logo](docs/images/house-rules-logo2.png)

Set your own challenges and be the Dungeon Master of your own game. Make your own rules and challenge your friends.

HouseRules applies customisations to many settings, values and toggles used within the Demeo code.

![HouseRules Screenshot](docs/images/houserules_screenshot.jpg)

- Change the size of your card hand (Skirmish Mode Only)
- Adjust HP, AttackDamage, ActionPoints etc on a per-character basis
- Scale gold/mana/chests up/down
- Change starting cards dealt and max-cards allowed
- Add/Adjust AOE effects for abilities.
- Remove/add casting cost for cards
- Beat-the-clock Game Timer modes
- Specify cards distributed to players
- Change immunities for different pieces
- Prevent enemies respawing
- Keep the exit locked until all of the enemies are dead
- ... and many more.

This framework allows the definition of modular gameplay modifications (or
"rules") and the ability to group them to create custom gamemodes (or
"rulesets").

Rulesets can be configured as JSON files stored within the game's directory.

See the [HouseRules_Core readme](HouseRules_Core/README.md) for information about the
HouseRules framework.

See the [HouseRules_Essentials readme](HouseRules_Essentials/README.md) for a list of all predefined rules and rulesets.

#### How it works

HouseRules creates multiplayer games which are playable by ALL Demeo players (both Quest and PC), but the mod itself currently runs only on PC. In order to play with a modified ruleset, the player hosting the game must be on a PC running this mod. Unmodded clients are able to join the modded game as normal and play with new game rules.

🚨🛑 __IMPORTANT__ - During gameplay client machines will update their board state internally - They are sent frequent updates from the host to resynchronise board states. Some rules may cause temporary inconsistencies with clients seeing a different board view to the host. These inconsistencies are generally short lived and do not adversely affect gameplay.🛑🚨

#### Get in Touch

We have a dedicated [HouseRules Discord Channel ![Discord](https://img.shields.io/discord/841011788195823626.svg?label=&logo=discord&logoColor=ffffff&color=7389D8&labelColor=6A7EC2)](https://discord.gg/N9DZB5ebmj) to chat about gameplay, new rule ideas, report bugs or maybe get involved with writing some new rules. Come over and say 👋 'Hi' 👋

### SkipIntro

The Elven Necropolis is a very welcoming place... some might say it's too welcoming.

The SkipIntro mod skips the intro loading scene and takes you straight into the main menu.

You will never again need to ![SkipIntro Icon](docs/images/skipintro_icon.jpg)

### RoomFinder

Tired of Demeo's "Quickjoin" endlessly placing you into random games? This mod
lists all public rooms, along with their properties, so you can pick which one
to join.

![RoomFinder Screenshot](docs/images/roomfinder_screenshot.jpg)

### RoomCode

Set your own room code, skipping Demeo's random room code generation.

See the [RoomCode readme](RoomCode/README.md) for more information and configuration
options.

## For Developers

### `/Common`

A library shared by more than one mod. This is compiled as part of each mod.

Should there be any interest by other developers to reuse this library, the
author of this project can extract it into a common/util mod or
a NuGet.

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
