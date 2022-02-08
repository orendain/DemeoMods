# Ability Configuration

> Note: There is no guarantee that setting a particular field will result in
> the changes that you might expect.
>
> Demeo will conditionally ignore several fields in different situations.
> It is out of scope of this documentation to list what all can actually be done
> by setting these fields.

The following is parsed from Demeo version `1.10.135079`.

Ability fields:
```
// "----------------------------- GENERAL -----------------------------------------"

AbilityKey abilityKey


// ----------------------------- ENEMY AI -----------------------------------------

// This is used by enemy AI to describe the ability so it can randomize from several different of the same type.
AbilityAttribute[] abilityAttributes

// EnemyAI: Is ability only allowed if enemy is berserk? Or only allowed if NOT berserk?
BerserkModifier abilityBerserkModifier

// Instructions for the AI how we would prefer this ability to be used, only for enemies.
// (note: enemyTargetStrategy is CURRENTLY NOT IN USE!!) Only for enemy abilities. Ignore for player abilities.
EnemyTargetStrategy enemyTargetStrategyNotInUse

// Cooldown only affects enemies. -1 is no cooldown
// Determines how often an enemy can use an ability. -1 is always
int cooldown

// Probability only affects enemies. 1 is always!
// To be able to use this ability the enemy must get past a randomness check
float probability

// Enemy can only use this if it can see a player before first action
bool onlyIfCanSeePlayerAtStartOfTurn


// --------------------------- LOCALIZATION ---------------------------------------
// Localized key for the title, if this ability is represented on a card or in some manual somewhere
string titleLocalizedKey

// Localized key for the description text, if this ability is represented on a card or in some manual somewhere
string descrLocalizedKey


// ------------------------------- SOUND ------------------------------------------
SoundTriggers soundTriggers


// ----------------------------- ANIMATION -----------------------------------------
// Should caste turn to face the tile where we placed the ability before actually casting it?
bool faceTargetTile

// If >0 we'll add a pause after [possible] rotation/movement before we execute the gameplay sequences
float pauseBeforeExecutingSequence

// Animation trigger to invoke for this ability (optional)
string defaultAnimation

// Animation trigger to invoke if ability result is a critical hit (optional)
string criticalAnimation


// ------------------------------- DAMAGE -------------------------------------------
DamageMode damageMode
DiceType diceType
DamageTag[] damageTags
AbilityDamage abilityDamage
int stealthBonusDamage
bool muteTextOutputIfZeroDamage
bool enableWeakenPenalty
bool enableStrengthBonus
bool enableBackstabBonus
bool dealExtraDamageOnFrozenTargets
CritFailType critFail


// ------------------------------- HEAL -------------------------------------------
AbilityHeal abilityHeal


// ------------------------------- TARGETING ------------------------------------------

// Special case where you can place an ability on any tile as long as it is marked as walkable. Teleport. Eye of avalon.
TargetAnyTileMode canTargetAnyTile

// This ability can only be placed on pieces with these pieceTypes
List<PieceType> validAbilityTargets

// This ability can be placed on an empty tile that has this specific TileEffect on it
TileEffect[] validTileEffectTargets

// Can place ability on one self regardless of valid ability targets
bool mayTargetSelf

// Can ability be placed on a tile with no pieces on it?
bool mayTargetEmptyTiles

// If true then we cannot place this card on pressure plates
bool blockedByPressurePlate

// What piece types can be affected by the effect of the ability. Configures friendly fire for example.
PieceType[] validEffectTargets

bool usableWhenDowned


// --------------------------------- RANGE ------------------------------------------

// This is only used for enemies right now to figure out if they are too close to their target
int minRange

// How far away from the caster you are allowed to place the card
int maxRange

// Will this ability modify the position of the caster?
MovesCaster movesCaster


// ---------------------------------- AOE ---------------------------------------------
AreaOfEffect.Shape areaOfEffectShape
int areaOfEffectAngle
int areaOfEffectRange
int areaOfEffectCritRange
bool isCritting


// ---------------------------------- MISC ---------------------------------------------

EffectStateType[] disableStatusesOnTarget
TileEffect addTileEffectToTile
float addTileEffectProbabilityPerTile

// Effect states applied to each defender
EffectStateType[] targetEffects

float applyAndDisableTargetEffectsProbability

// If we have Stealthed statusEffect, will it dissipate when using this ability?
bool breaksStealth

// Some abilities does not cost an action - potions for example.
bool costActionPoint

// How far the Ability should push back an enemy
int pushbackTiles

bool ignitesGas


// -------------------------------- SPAWNING ---------------------------------------------
SpawnMode usePieceToSpawn

[DrawIf("usePieceToSpawn", SpawnMode.UsePieceToSpawn, DrawIfAttribute.DisablingType.ReadOnly)]
BoardPieceId pieceToSpawn

BoardPieceId[] randomPieceList
PieceSpawnSettings.SpawnParticleEffect spawnVFX
SpawnTargetMode OLD_spawnTargetMode
bool spawnFillAOE

[DrawIf("spawnFillAOE", false, DrawIfAttribute.DisablingType.ReadOnly)]
SpawnAmount spawnAmount

[DrawIf("spawnFillAOE", false, DrawIfAttribute.DisablingType.ReadOnly)]
SpawnDistanceStrategy spawnDistanceStrategy

// In seconds, should probably not be more than 0.1
[DrawIf("spawnAmount", SpawnAmount.RandomAmount, DrawIfAttribute.DisablingType.DontDraw)]
float delayBetweenSpawns

EffectStateType[] effectStatesTypeToApplyToSpawnedPiece
int spawnAliveForRounds
int spawnMaxDistance

[DrawIf("spawnAmount", SpawnAmount.RandomAmount, DrawIfAttribute.DisablingType.ReadOnly)]
int spawnMinAmount

[DrawIf("spawnAmount", SpawnAmount.RandomAmount, DrawIfAttribute.DisablingType.ReadOnly)]
int spawnMaxAmount

float spawnProbabilityInFOW
bool useRandomRotation
bool useBloodhoundOnSpawnedUnit
bool wakeUpOnSpawn

// -------------------------------- TEXT ON HIT -------------------------------------------
bool displayTextOnHit
string textOnHitLocalizedKey
Color textOnHitColor
float textOnHitDuration
AbilitySequenceBuilder[] abilitySequenceBuilders
AreaOfEffect areaOfEffect
AreaOfEffect areaOfEffectCrit
```

Some corresponding enums and structures types for the above fields:
```
...
```
