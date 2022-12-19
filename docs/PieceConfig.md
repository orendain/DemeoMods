# Piece Configuration

> Note: There is no guarantee that setting a particular field will result in
> the changes that you might expect.
>
> Demeo will conditionally ignore several fields in different situations.
> It is out of scope of this documentation to list what all can actually be done
> by setting these fields.

The following is parsed from Demeo version `1.10.135079`.

Piece fields with primitive types:
```
float hitParticleScale
bool fogOfWarEnabled
string PieceName
string Lore
string PieceNameLocalizedKey
string LoreLocalizedKey
int StartHealth
int StartArmor
int BarkArmor
int AliveForRounds
int VisionRange
int ActionPoint
int MoveRange
int AttackDamage
int PowerIndex
int TurnPriority
bool CanOpenDoor
float AcidSlimeTrailChance
float WaterTrailChance
float SpiderWebChance
int WaterTrailTiles
float ChanceOfDeathPanic
float ChanceOfFirePanic
float ChanceofPanicWarcry
float BerserkBelowHealth
float EliteChance
int CriticalHitChance
int CriticalHitDamage
bool DontConsumeAPOnCrit
int MinHealthPotions
int MaxHealthPotions
```

Piece fields with non-primitive types:
```
SoundTriggers soundTriggers
Vector2Int tileSize
AnimationHitTime[] animationHitTimes
PieceType[] PieceType
List<AbilityKey> UseWhenCreated
List<AbilityKey> Abilities
List<AbilityKey> UseWhenKilled
Behaviour[] Behaviours
EffectStateType[] ImmuneToStatusEffects
IntPoint2D TileSize
```
