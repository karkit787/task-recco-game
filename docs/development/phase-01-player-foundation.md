# Phase 1 — Basic Playable Character

## Status

**Completed**

## Goal

Establish the minimum playable character foundation for the game.

The Phase 1 target was:

> **Launch game → move around map → camera follows → collision works.**

Directional character animation was also implemented as part of this phase.

---

## Implemented Features

### 1. Player Movement

Basic top-down 2D player movement has been implemented using Unity's **Input System** and `Rigidbody2D`.

Supported controls:

* `W` / `Up Arrow` — move up
* `S` / `Down Arrow` — move down
* `A` / `Left Arrow` — move left
* `D` / `Right Arrow` — move right

Movement behaviour includes:

* four-directional movement;
* diagonal movement;
* normalized movement input to prevent faster diagonal movement;
* configurable movement speed through the Unity Inspector;
* physics-based movement using `Rigidbody2D.MovePosition`;
* input reading during `Update`;
* physics movement during `FixedUpdate`.

The movement implementation is contained in:

```text
Assets/Scripts/Player/PlayerMovement.cs
```

---

## 2. Unity Input System

The project uses Unity's **new Input System** rather than the legacy Input Manager.

The existing input actions asset is:

```text
Assets/Settings/InputSystem_Actions.inputactions
```

The player movement system currently uses:

```text
Player
└── Move
```

The `Move` action outputs a `Vector2`.

Existing keyboard bindings include:

```text
W / Up Arrow
A / Left Arrow
S / Down Arrow
D / Right Arrow
```

The movement script receives this through an `InputActionReference`.

---

## 3. Player Physics

The Player GameObject uses a `Rigidbody2D`.

Current intended Rigidbody configuration:

```text
Body Type: Dynamic
Gravity Scale: 0
Interpolate: Interpolate

Constraints:
    Freeze Rotation Z
```

`Gravity Scale` is set to `0` because this is a top-down game and the player should not be affected by platformer-style vertical gravity.

Normal movement is performed through the Rigidbody rather than directly changing the player's Transform.

---

## 4. Player Collision

The Player has a 2D collider attached.

A temporary wall using a `BoxCollider2D` was created to verify collision behaviour.

Testing confirmed that:

* the player cannot move through the wall;
* collision works while approaching the obstacle;
* the player's Rigidbody interacts correctly with static 2D colliders;
* the player does not rotate after colliding with an obstacle.

The temporary wall exists only as a development/testing object and is not final game content.

---

## 5. Camera Follow

A basic top-down camera-follow system has been implemented.

The camera:

* follows the player's X and Y position;
* retains its own Z position;
* updates after player movement;
* remains independent of the player movement logic.

Current behaviour keeps the player around the centre of the visible game area while moving through the environment.

The camera is currently configured as an **Orthographic Camera**, appropriate for the game's 2D top-down presentation.

Camera smoothing, map boundaries, and pixel-perfect camera behaviour have not yet been implemented.

---

## 6. Character Animation

Basic directional player animation has been implemented.

The current character supports:

```text
Idle Up
Idle Down
Idle Left
Idle Right

Walk Up
Walk Down
Walk Left
Walk Right
```

Walking animation clips were created using sliced frames from the current character spritesheet.

The Animator uses directional information from player movement to determine which animation should play.

Animator parameters currently include:

```text
MoveX
MoveY
Speed
```

Direction values follow the convention:

```text
Up      = ( 0,  1)
Down    = ( 0, -1)
Left    = (-1,  0)
Right   = ( 1,  0)
```

`Speed` determines whether the player should use an idle or walking animation.

---

## 7. Directional Blend Trees

Two directional animation groups are currently used:

```text
Idle
Walk
```

Each uses a **2D Simple Directional Blend Tree** based on:

```text
MoveX
MoveY
```

The walking Blend Tree contains:

```text
Walk Up
Walk Down
Walk Left
Walk Right
```

The idle Blend Tree contains:

```text
Idle Up
Idle Down
Idle Left
Idle Right
```

The Animator switches between the two states using `Speed`.

Conceptually:

```text
Speed > 0
Idle ─────────────→ Walk

Speed = 0
Idle ←───────────── Walk
```

The transitions do not use Exit Time so movement animation responds immediately to player input.

---

## 8. Last-Facing Direction

When the player stops moving, the character remains facing the direction of their last movement.

For example:

```text
Walk Left
    ↓
Release A
    ↓
Idle Left
```

The animation system only updates `MoveX` and `MoveY` while movement input is non-zero.

When movement stops:

```text
Speed = 0
```

but the previous directional values are retained.

This allows the appropriate directional idle animation to play.

---

## 9. Initial Facing Direction

When the game begins, the player defaults to:

```text
Idle Down
```

The animation system initializes the Animator with:

```text
MoveX = 0
MoveY = -1
Speed = 0
```

This prevents the Blend Tree from starting at an unintended direction such as Idle Right.

---

## 10. Player Component Structure

The Player GameObject currently contains approximately:

```text
Player
├── Transform
├── Sprite Renderer
├── Animator
├── Rigidbody 2D
├── Collider 2D
├── PlayerMovement
└── PlayerAnimation
```

Responsibilities are intentionally separated.

### `PlayerMovement`

Responsible for:

* reading movement input;
* storing movement direction;
* controlling Rigidbody2D movement;
* exposing current movement input to other presentation systems.

### `PlayerAnimation`

Responsible for:

* reading movement state;
* updating Animator parameters;
* selecting the appropriate direction;
* preserving the player's last-facing direction.

Movement logic does not directly control animation clips.

---

# Current Scene Foundation

The current development scene contains approximately:

```text
Scene
├── Main Camera
├── BaseMap
├── Player
└── Test / Temporary Collision Objects
```

This is currently being used as a development sandbox for implementing gameplay systems.

---

# Placeholder Asset Notice

**All visual assets currently used during Phase 1 are placeholders.**

This includes:

* the current player character spritesheet;
* the current base map/background;
* temporary environmental objects;
* temporary collision test objects;
* any current visual presentation used for testing.

These assets are being used only to validate gameplay functionality and technical implementation.

They are **not considered final game art or final world design**.

The current focus is on building and validating gameplay systems before investing time in final visual assets.

Future development may replace:

```text
Character sprites
Map artwork
Environment objects
Animations
UI
NPC artwork
Objective markers
Effects
```

without requiring the underlying gameplay systems to be rewritten.

---

# Phase 1 Verification

The following functionality has been manually tested:

* [x] Game launches successfully
* [x] Player appears in the scene
* [x] Player moves using WASD
* [x] Player moves using arrow keys
* [x] Player can move diagonally
* [x] Diagonal movement is normalized
* [x] Player stops when input is released
* [x] Movement speed can be configured
* [x] Rigidbody2D movement works correctly
* [x] Player is not affected by gravity
* [x] Player does not rotate during collision
* [x] Player collides with test obstacles
* [x] Camera follows the player
* [x] Walking animations respond to movement direction
* [x] Idle animations respond to facing direction
* [x] Player retains their last-facing direction
* [x] Player starts facing downward
* [x] No major movement, camera, collision, or animation issues remain

---

# Deliberately Deferred Features

The following systems were intentionally **not implemented during Phase 1**:

* objective/task system;
* player interaction system;
* coins;
* score;
* upgrades;
* capability system;
* recommendation system;
* objective discovery/perception;
* NPCs;
* combat;
* inventory;
* character customization;
* final world/map construction;
* final environmental collision;
* final visual assets;
* UI;
* audio;
* saving/loading;
* multiplayer/networking;
* camera boundaries;
* camera smoothing;
* final pixel-perfect camera configuration.

These will be developed incrementally in later phases.

---

# Phase 1 Result

Phase 1 establishes a working player foundation that can now support gameplay systems.

The current playable loop is:

```text
Launch Game
    ↓
Control Player
    ↓
Move Around Environment
    ↓
Directional Animation Plays
    ↓
Camera Follows Player
    ↓
Player Interacts Correctly With Physics Colliders
```

This provides the foundation required to begin implementing actual game objectives.

---

# Next Phase

## Phase 2 — Objective Foundation

Target milestone:

> **Walk around → encounter objective → interact → objective completes.**

Initial development should focus on one simple objective lifecycle before introducing additional objective types.

The first objective can be a placeholder interaction such as:

```text
Walk to Objective Marker
        ↓
Enter Interaction Range
        ↓
Press Interact
        ↓
Objective Completes
        ↓
Coin + Score Reward Produced
```

The objective system should be implemented independently of final visual assets so that placeholder markers and test objects can continue to be used during development.
