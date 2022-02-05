# RulesAPI

A framework allowing the definition of modular gameplay modifications (or
"rules") and the ability to group them to create custom gamemodes (or
"rulesets").

## Overview

The idea behind RulesAPI is:

There can be any number of custom "rules" available, where each rule introduces
some gameplay modification. E.g.:
- Ballistas shoot 2 additional times.
- Players move twice as fast.
- Players start with +1 strength.
- Etc.

Rules can be grouped into predefined sets.  Users (or other mods) can specify
which ruleset to load at the start of a game.  Thus, rulesets can also be
considered gamemodes.

RulesAPI provides the framework for defining custom rules and rulesets, and the
mechanisms by which they are patched into the game and activated/deactivated
during gameplay.

## Choosing a Ruleset

If installation instructions were followed (i.e., MelonLoader was installed),
the following file will appear in the Demeo game directory:
`UserData/MelonPreferences.cfg`.

After RulesAPI is installed and Demeo is run at least once, the following will
appear somewhere in that configuration file:

```toml
[RulesAPI]
ruleset = ""
```

Select the ruleset to use by typing its name within the quotes.  Alternatively,
use empty quotes `""` to specify no ruleset should be used.

For now, Demeo should not be running when the configuration file is modified.
Else your changes may not be saved.

A list of out-of-the-box ruleset names can be found in the
[Rules readme](../RulesAPI_Essentials/README.md).

## Rules vs Mods

A RulesAPI rule can be written with as few as a couple dozen lines of code,
and allows it to be loaded selectively by users, or other mods, at playtime.

There are several considerations to make when deciding between writing a
RulesAPI rule or a full-blown mod:
- **Gameplay Only**: Rules are activated only during gameplay and not, for
  example, in the main menu.
- **Runs on Host**: Rules are applied only host-side, where the majority of
  gameplay logic is resolved.

For everything else, developers are encouraged to write their own full-blown
mod, rather than try to jam their idea into the RulesAPI framework. 

## Behavior and Conditions

- Only the host of the game needs to have the mod installed.
- Rules are activated only for private or offline games where the user is the host.
- Rules must be registered with RulesAPI before they can be part of a ruleset.
- Rulesets must be registered before they can be selected for activation.
