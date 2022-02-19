# RoomCode

Set your own room code.

After installing the mod, run the game once. A configuration file will be
created in your Demeo game directory. Specifically, at:
`<Demeo_Game_Directory>/UserData/MelonPreferences.cfg`

You should see something like the following in that file.

```toml
[RoomCode]
enabled = true
codes = [ ]
```

`enabled`: Set to `true` to enable the mod, or `false` to disable it.
`codes`: List all room codes you'd like to use, in order of preference.

If none of the room codes are available, the mod will fall back to Demeo's
random room code generation.

**Example configuration:**
```toml
[RoomCode]
enabled = true
codes = ["8888", "7777", "1234"]
```
