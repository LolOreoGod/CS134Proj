# CS134Proj - Stealth Action Game (Unity)
## Swindling Stealth Spree

## Overview
This project is a stealth-based action game built in Unity where the player navigates through levels equipped with a smoke grenade, avoiding enemy detection, eliminating guards, and reaching the exit.

The core gameplay focuses on:
- Enemy AI with patrol, chase, and investigation behaviors
- Stealth mechanics (line-of-sight raycast detection, smoke grenades)
- Melee combat and takedowns
- Level progression system with UI feedback

---

## Features

### Enemy AI
- Finite State Machine with:
  - Patrol
  - Chase
  - Investigate
  - Stunned
- Vision system using:
  - Distance checks
  - Angle (field of view)
  - Raycasting for line-of-sight
- Detection affected by:
  - Obstacles (walls)
  - Smoke grenades
- Shared detection system for UI feedback

---

### Combat System
- Enemy melee attacks with timed hit detection
- Animation-driven damage using collider activation
- Player health system with damage handling and death state

---

### Player Mechanics
- First-person / character controller-based movement
- Interaction system (doors)
- Health system with HUD display
- Death system triggering Game Over UI

---

### UI Systems
- **HUD**
  - Health bar (slider-based)
  - Dynamic health text
  - Detection indicator ("DETECTED")

- **Level Complete Screen**
  - Displays time spent in level
  - Next Level button (disabled on final level)
  - Quit option

- **Game Over Screen**
  - Retry level
  - Return to main menu

---

### Audio
- Footstep sounds based on movement
- Detection sound cues
- Background music:
  - Normal exploration music
  - Chase music (shared across enemies)

---

### Level System
- Multiple levels managed through Unity Build Settings
- Scene progression using:
  - Index-based loading
  - Modulo logic to return to main menu after final level

---

## Technical Highlights

### AI Detection
- Uses `Physics.RaycastAll` for accurate obstruction handling
- Sorts ray hits by distance to determine visibility
- Supports environmental effects like smoke blocking vision

### State Management
- Enemy behavior implemented using a custom state machine
- Smooth transitions between patrol, chase, and investigate

### UI Integration
- Real-time updates via `PlayerHUD`
- Centralized control via `LevelManager`
- Time-based tracking for level completion

### Game Flow
- Managed through `LevelManager`
- Handles:
  - Level completion
  - Game over
  - Scene transitions
  - Cursor state and time scaling

---

## Controls
- **Movement:** (depends on your setup)
- **Interact:** Assigned via Input System (`interactAction`)
- **Combat:** Automatic when enemies are in range

---

## Known Issues / Limitations
- Lighting does not affect enemy detection (purely physics-based)
- Audio does not affect enemy detection

---

## Future Improvements
- Add lighting-based stealth mechanics
- Add audio-based stealth mechanics
- Improve enemy coordination and group behavior
- Add animations and transitions for UI (fade in/out)
- Add save/load system

---

## Technologies Used
- Unity Engine
- C#
- Unity NavMesh (AI navigation)
- Unity Input System
- TextMeshPro (UI)

---

## Author
Mengting Chang
Jamie O'neill
