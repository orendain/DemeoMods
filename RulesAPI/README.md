# RulesAPI

A framework allowing the definition of modular gameplay modifications ("rules")
and the selection of them.

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
considered "game modes".

RulesAPI provides the framework for defining custom rules and rulesets, and the
mechanisms by which they are patched into the game and activated/deactivated
during gameplay.

## Rules vs Mods

A RulesAPI rule can be written with as few as a couple dozen lines of code,
and allows it to be loaded selectively by users, or other mods, at playtime.

There are several considerations to make when deciding between writing a
RulesAPI rule or a full-blown mod:
- **Gameplay Only**: Rules are activate only during gameplay and not, for
  example, in the main menu.
- **Runs on Host**: Rules are applied only host-side, where the majority of
  gameplay logic is resolved.

For everything else, developers are encouraged to write their own full-blown
mod, rather than try to jam their idea into the RulesAPI framework. 
