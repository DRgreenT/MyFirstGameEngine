# MyFirstGameEngine

**MyFirstGameEngine** is a simple 2D game engine with a WinForms-based interface, featuring a basic game loop, physics system (gravity & collision), sprites, shapes, and runtime logging via the console.

<img src="https://github.com/DRgreenT/MyFirstGameEngine/blob/master/docs/game_example.png" widht="100%"></img>

<img src="https://github.com/DRgreenT/MyFirstGameEngine/blob/master/docs/debugLog.png" widht="100%"></img>


## Component Overview

### `Program.cs`
- Entry point of the application.
- Initializes the game using `DemoGame`.
- Hides the cursor and sets up clean console output.

### `DemoGame.cs`
- Implements a basic 2D game using the engine.
- Loads a level from a map matrix (`g = Ground`, `e = Exit`).
- Creates the player as a `Sprite2D` and places it in the scene.
- `OnUpdate()` handles game logic including gravity, jumping, and movement.

### `Canvas.cs`
- The main WinForms window with a fixed size and disabled border resizing.
- Includes methods for initialization (`SetWindow`) and retrieving window size.
- Uses `DoubleBuffered` for smooth rendering.

### `Sprite2D.cs`
- Represents a visual game object with an image.
- Supports sprite caching via `Engine.CachedSprites`, loading images from `/Assets/Sprites/`.
- Can move (`Position`, `Velocity`) and detect collisions.
- Provides `Clone()` for simulating movement without modifying the original.

### `Shape2D.cs`
- Represents simple colored rectangle shapes (e.g., debug visuals).
- Automatically registered and managed in a central shape list.
- Supports bounding box collision detection with sprites.

### `Vector2.cs`
- A 2D vector class with `X` and `Y` components.
- Supports vector operations such as addition, subtraction, normalization, angle calculation, and directional movement (`Up`, `Down`, `Left`, `Right`).
- Used as the base for all positions, sizes, and velocities in the engine.

### `ConsoleLog.cs`
- Provides colored log output in the console (Info, Warning, Error, Message).
- Used by both the engine and the game for runtime diagnostics.

### `ConsoleStats.cs`
- Displays FPS and frame count in the console.
- Uses `Console.SetCursorPosition` for real-time updates.

---

## Game Features

- **Player movement** via `WASD` keys (`KeyDown`/`KeyUp` events)
- **Jumping** with the W key, only when `isGrounded == true`
- **Gravity** constantly pulls the player downward (with max fall speed limit)
- **Collision detection** with `Ground` objects (level structure)
- **Movement rollback on collision**: player position is reset if blocked

["Kenny´s" sprite pack](https://opengameart.org/content/platformer-art-complete-pack-often-updated)

---

## Level System

The level is defined using a simple character matrix (`string[,] Map`):  
- `"g"` places a ground platform
- `"e"` places an exit sign
- `"."` = empty space

---

## Execution

The app runs as a **WinForms application**, but the console window is visible by default.  
To remove the console window, modify your `.csproj` file:

```xml
<OutputType>WinExe</OutputType>
