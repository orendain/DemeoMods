# Custom Turn Order

Define your own custom turn order.

## Configuration

After installing the mod, run the game once. A configuration file will be
created in your Demeo game directory. Specifically, at:
`<Demeo_Game_Directory>/UserData/MelonPreferences.cfg`

You should see something like the following in that file.

```toml
[TurnOrderCustomizer]
# Whether or not (true/false) to override the game's default turn order.
enabled = false
assassin = 0
bard = 0
guardian = 0
hunter = 0
sorcerer = 0
# Downed players.
downed = 0
# Players with the javelin.
javelin = 0
```

Add values to each of the listed attributes. Players with higher totals go first
in the turn order.

Make sure to set `enabled` to `true` if you want your custom turn order to
take effect.  Keep in mind that this will only apply in multiplayer games when
you are the host of the game.

## Example Configuration

```toml
[TurnOrderCustomizer]
# Whether or not (true/false) to override the game's default turn order.
enabled = true
assassin = 8
bard = 2
guardian = 20
hunter = 6
sorcerer = 4
# Downed players.
downed = 10
# Players with the Sigataurian Javelin.
javelin = 10
```

In the example above:
- When no player is downed or has a Sigataurian Javelin, guardians would go
  first, followed by assassins, then hunters, then sorcerers, and finally bards.
- Downed players will almost certainly go first, except when there's a guardian
  in the group or someone has a Sigataurian Javelin.
- A downed sorcerer with a Sigataurian Javelin will go before a downed hunter
  that doesn't have a Sigataurian Javelin.
