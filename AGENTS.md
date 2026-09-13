# AGENTS.md

## Project

Unity top-down 2D pixel-art exploration/progression game for an FYP.

Core loop:
`Explore -> Objective -> Coins + Score -> Upgrades -> Higher Capability -> Higher-Value Objectives -> Leaderboard`

The game must work as a normal progression game while supporting study of recommendation behaviour and information sharing.

## Non-Negotiable Game Rules

- Progression is open-ended; there is no final objective.
- Bosses are optional/high-value objectives, not an ending.
- **Coins** are spendable progression currency.
- **Score** is persistent, non-spendable leaderboard progress.
- Players can always explore and choose objectives without recommendations.
- Players only directly know objectives/resources inside their local perception.
- Player-local knowledge and recommendation-system knowledge are separate.
- Information sharing is an explicit player choice; unshared local information remains local.

### Objective Data

Keep objectives data-driven with stable IDs. Typical fields:
- location
- coin reward
- score reward
- difficulty
- capability requirements
- objective type
- availability

Distance should normally be derived from player/objective positions rather than duplicated as mutable state.

## Recommendation System

System knowledge follows:

`SystemQueue = (CurrentQueue U SacrificedQueue) - UtilisedQueue`

- **CurrentQueue**: information already known and still usable.
- **SacrificedQueue**: newly shared local information from players.
- **UtilisedQueue**: completed, claimed, expired, invalid, or otherwise unavailable information.

Keep **knowledge management** separate from **recommendation ranking**.

Pipeline:
1. Update `SystemQueue`.
2. Remove unavailable objectives.
3. Filter candidates incompatible with the player's capabilities.
4. Rank eligible candidates.
5. Return the recommendation and enough information to explain it when required.

Conceptually:

`Recommendation(player) = argmax U(player, objective)`

`U` may use distance, coin reward, score reward, difficulty, capability match, progression value, and player preferences.

Keep recommendation weights/configuration adjustable for experimentation. Do not hard-code one permanent formula into UI or objective scripts.

## Unity Constraints

- Unity **6000.3.22f1**.
- Use the installed **Unity Input System**.
- Prefer the existing Unity 2D, Tilemap, URP, and Unity Test Framework tooling.
- Do not add packages/frameworks without a demonstrated need.
- Preserve Unity `.meta` files/GUIDs; avoid filesystem asset moves that break references.

## Engineering Rules

### KISS

Implement the smallest clean solution that satisfies the current requirement.

Prefer:
- small focused components
- explicit dependencies
- straightforward C#
- reusable prefabs
- data-driven configuration
- plain C# classes for logic that does not require Unity lifecycle APIs

Avoid unless clearly justified:
- global singleton networks
- service-locator-heavy designs
- custom DI frameworks
- large generic base-class hierarchies
- complex event buses
- ECS/DOTS
- speculative multiplayer architecture
- abstractions created only for hypothetical future use

### Separation of Concerns

Keep these responsibilities separate where practical:
- input
- movement
- animation/presentation
- combat
- objectives
- rewards/progression
- perception
- system knowledge/queues
- recommendation filtering/ranking
- UI
- persistence
- networking

UI presents state and sends player intent; it must not own authoritative coins, score, objective, or recommendation state.

### Data

Use ScriptableObjects where useful for shared/static definitions such as objectives, upgrades, enemies, and recommendation tuning.

Keep mutable runtime/player state separate from definition assets. Do not mutate shared ScriptableObject assets as per-player state.

## Top-Down 2D Unity Practices

### Input / Movement / Physics

- Read input in `Update`.
- Perform Rigidbody2D physics movement in `FixedUpdate` when physics is involved.
- Do not move a physics-driven player by directly changing `transform.position`.
- Use 2D physics APIs consistently (`Rigidbody2D`, `Collider2D`, Physics2D).
- Configure collision layers intentionally.
- Cache component references used repeatedly.
- Avoid repeated scene searches or physics queries in hot loops.

### Pixel Art / World

- Keep pixels-per-unit and sprite import settings consistent.
- Use appropriate pixel-art filtering/compression settings.
- Define sorting layers deliberately for ground, actors, effects, foreground, etc.
- Prefer Tilemaps for terrain/environment where appropriate.
- Separate visual and collision tilemaps when it improves maintainability.
- Keep gameplay/objective logic out of visual tile assets.
- Each tile is 32x32 pixels.
- Character sprites will be maximum 32 pixels wide and 64 pixels tall.

### Animation / Camera / UI

- Animation reflects gameplay state; it should not own core game rules.
- Keep Animator parameters small and meaningful.
- Avoid critical gameplay logic in animation events when normal code is clearer.
- Keep camera code independent from player game rules.
- Avoid `Find*`-style scene searches from UI/gameplay code when references can be assigned or injected.

### Performance

Optimize measured problems, but avoid obvious per-frame waste:
- `Find*` calls
- repeated `GetComponent`
- LINQ/allocations in hot `Update`/`FixedUpdate` paths
- unnecessary instantiate/destroy churn

Use pooling only when repeated creation/destruction actually warrants it.

## Suggested Structure

Use a simple feature-oriented layout as the project grows:

```text
Assets/
  Art/
  Audio/
  Prefabs/
  Scenes/
  ScriptableObjects/
  Scripts/
    Core/
    Player/
    Objectives/
    Progression/
    Perception/
    Recommendations/
    Combat/
    UI/
  Tests/
```

Do not reorganize working assets merely to match this example.

## C# Style

- One primary type per file; file name matches type name.
- `PascalCase`: types, methods, properties.
- `camelCase`: locals and parameters.
- `_camelCase`: private fields.
- Prefer `[SerializeField] private` over public mutable Inspector fields.
- Avoid magic numbers/strings; use serialized config, constants, enums, or typed IDs.
- Use stable IDs for players/objectives/items that may later cross multiplayer boundaries.
- Validate required references/config and fail clearly.
- Comments explain **why**, constraints, or non-obvious choices—not what obvious code does.

## Testing

Use Unity Test Framework for meaningful game logic.

Prioritize tests for:
- coins/score calculations
- upgrade eligibility/purchases
- capability checks
- queue merge/removal behaviour
- recommendation candidate filtering
- recommendation ranking
- objective state transitions

Prefer EditMode tests for pure logic. Use PlayMode tests when scene, lifecycle, animation, or physics behaviour is genuinely required.

For bug fixes: reproduce -> add/update focused test when practical -> make smallest fix -> verify regression risk.

## Multiplayer Readiness

Single-player comes first. Do not implement networking early, but avoid blocking it unnecessarily.

- Keep authoritative gameplay state separate from presentation/UI.
- Model player-specific state explicitly; avoid static global mutable player state.
- Use stable IDs for shared entities/records.
- Keep queue/recommendation logic deterministic where practical.
- Do not assume there will only ever be one player object in domain logic.

## Codex Change Discipline

Before editing:
1. inspect relevant scripts, prefabs, scenes, and configuration;
2. understand the existing pattern;
3. identify the smallest necessary change.

While editing:
- solve the requested task, not hypothetical future tasks;
- preserve working behaviour unless the requirement changes it;
- reuse sound existing patterns;
- avoid unrelated refactors;
- keep serialized changes backward-compatible where practical;
- never silently change the game rules defined here.

After editing:
- ensure scripts compile;
- run relevant tests when available;
- check Unity references/serialization implications;
- report assumptions and important design decisions.

When requirements are ambiguous, choose the simplest implementation consistent with this file and the existing codebase.
