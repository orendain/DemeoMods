# Release

This page describes the release process.

## Prepare for release

### (HouseRules only) Generate JSON for built-in rulesets

Uncomment the relevant line from `ExampleRulesetExporter.cs`. Build and run the
mod so it generates JSON files for built-in rulesets during game startup.

Open a PR with these JSON files added to `docs/rulesets` for documentation and
reference purposes.

## Cutting a release

### Tag the release

Every release is tagged with `v<major>.<minor>.<patch>-<mod-name>`,
e.g. `v1.0.0-houserules`. Note the `v` prefix.

The commit, on the `main` branch, from which to cut a release must have this
tag. The following example assumes the semantic version "1.0.0" and the mod
"HouseRules".

```shell
git tag -s "v1.0.0-houserules" -m "HouseRules v1.0.0"
git push origin "v1.0.0-houserules"
```

If multiple mod are released simultaneously, it is acceptable to apply both tags
to the same commit.

### Bundle the release

#### 1. Build the mod DLL(s)

Build the mod using the "Release" (and not "Debug") configuration.

#### 2. Bundle all required files into a ZIP

Files must be bundled in a way that users can extract the archive into the
top-level Demeo game directory and all mod files fall into their correct place.

The ZIP file must be named `<mod-name>_<semenatic-version>.zip`,
e.g. `HouseRules_1.3.0.zip`.

Example 1:

```
RoomFinder_1.4.0.zip
└── Mods/
    └── RoomFinder.dll
```

Example 2:

```
HouseRules_1.3.0.zip
├── Mods/
│   ├── HouseRules_Configuration.dll
│   ├── HouseRules_Core.dll
│   └── HouseRules_Essentials.dll
├── UserData/
│   └── HouseRules/
│       └── ExampleRulesets/
│           ├── (Custom) LuckyDip.json
│           └── (Custom) The Swirl.json
└── UserLibs/
    └── Newtonsoft.Json.dll
```

### Update the changelog

#### 1. Ensure each PR is properly tagged

The PRs listed in a mod's changelog are selected based on GitHub labels. Each
PR:

- Must have at least one `area: <mod>` label (more than one if the PR crosses
  mods).
- Must have exactly one `type: <type>` label.
- Must have a title that describes the change.
    - ✅ `Add a new rule for healing all players`
    - ❌ `Rule heals players`
- Should have its title written in the active voice.
    - ✅ `Add a new rule for healing all players`
    - ❌ `Heal all player rule added`

> ☝️ Note: Generating a preliminary draft of the changelog may help to spot PR changes to make.

#### 2. Generate the changelog

The `DemeoMods` project uses the
the [github-changelog-generator](https://github.com/github-changelog-generator/github-changelog-generator)
for generating the contents of each mod's `CHANGELOG.md`. Each mod directory
hosts it's own `.github_changelog_generator` config file, as well as its
generated changelog.

```shell
cd <mod>
github_changelog_generator
```

> Note: Unless the `--token <personal_access_token>` argument is specified, the
> GitHub rate limits will be hit pretty quickly.

### Publish the release

#### GitHub

1. Create
   a [release in GitHub](https://docs.github.com/en/repositories/releasing-projects-on-github/managing-releases-in-a-repository#creating-a-release)
   using the tag created above.
2. Include a copy-paste of the generated changelog.
3. Optionally include a top-level message highlighting the most important points
   of the release.
4. Include the bundled ZIP file as a release asset.

#### CurseForge

1. Upload the mod's ZIP file
   as [a file to the relevant CurseForge project](https://support.curseforge.com/en/support/solutions/articles/9000197242-file-project-types-and-additional-fields)
   (see the section labeled `Adding Files to a Project`).
2. Mark the file as with the `Release Type` "Release".
3. The `Display Name` may be left blank so as to take the name of the ZIP file.
4. Include a copy-paste of the generated changelog, with images removed as
   they're not supported by CurseForge markdown.
