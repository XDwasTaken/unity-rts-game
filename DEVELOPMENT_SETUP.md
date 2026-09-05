# Development Setup Guide

## Prerequisites

- **Unity**: 2022 LTS or newer (https://unity.com/download)
- **Visual Studio** or **Rider**: For C# scripting
- **Git**: For version control

## Project Setup

### 1. Clone the Repository

```bash
git clone https://github.com/XDwasTaken/unity-rts-game.git
cd unity-rts-game
```

### 2. Open in Unity

1. Open **Unity Hub**
2. Click **Add** and select the cloned folder
3. Select a compatible Unity version and open the project

### 3. Project Structure

The `Assets/` folder contains:

```
Assets/
├── Scripts/
│   ├── Core/              # GameManager, game state
│   ├── Units/             # Unit logic and behaviors
│   ├── Buildings/         # Building system
│   ├── Resources/         # Resource management
│   ├── UI/                # UI controllers and managers
│   ├── Camera/            # Camera systems
│   └── Managers/          # Various managers
├── Scenes/                # Game scenes (to be created)
├── Prefabs/               # Reusable GameObjects
├── Materials/             # Materials and shaders
├── Audio/                 # Sound and music
└── Documentation/         # Design docs
```

## Coding Standards

### Naming Conventions

- **Classes**: PascalCase (e.g., `GameManager`, `Unit`)
- **Methods**: PascalCase (e.g., `MoveTo()`, `TakeDamage()`)
- **Variables**: camelCase (e.g., `moveSpeed`, `currentHealth`)
- **Constants**: UPPER_SNAKE_CASE (e.g., `MAX_UNITS = 100`)
- **Private fields**: Prefix with underscore (e.g., `_maxHealth`)

### Namespaces

Organize code into namespaces based on functionality:

```csharp
namespace RTS.Core { }
namespace RTS.Units { }
namespace RTS.Buildings { }
namespace RTS.UI { }
```

### Code Style

- Use **curly braces** on new lines
- Add **XML documentation** for public methods
- Keep methods **focused and small**
- Use **meaningful variable names**

### Example

```csharp
using UnityEngine;

namespace RTS.Units
{
    /// <summary>
    /// Base unit class for all controllable units
    /// </summary>
    public class Unit : MonoBehaviour
    {
        [SerializeField] private float _maxHealth = 100f;
        private float _currentHealth;

        /// <summary>
        /// Deal damage to this unit
        /// </summary>
        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}
```

## Git Workflow

### Branch Naming

- `feature/unit-selection` - New features
- `bugfix/camera-clipping` - Bug fixes
- `refactor/unit-movement` - Refactoring

### Commit Messages

```
[FEATURE] Add unit selection system
[BUGFIX] Fix camera boundary clipping
[REFACTOR] Improve movement pathfinding
```

### Pull Requests

1. Create a branch from `main`
2. Implement your feature/fix
3. Create a pull request with clear description
4. Request review from team members
5. Merge after approval

## Unity Best Practices

### Scene Management
- Keep scenes organized and clear
- Use meaningful scene names
- Avoid overly large scenes

### Prefabs
- Create prefabs for units, buildings, UI elements
- Keep prefab hierarchies clean
- Use prefab variants for different unit types

### Performance
- Use object pooling for frequently created/destroyed objects
- Batch rendering when possible
- Profile regularly with the Profiler window

### Physics
- Use appropriate collision layers
- Optimize collider complexity
- Consider using NavMesh for pathfinding

## Testing

### Manual Testing
- Test unit selection and movement
- Test unit combat
- Test camera controls
- Test UI responsiveness

### Debugging
- Use `Debug.Log()` for logging
- Use breakpoints in Visual Studio/Rider
- Use the Profiler for performance issues

## Common Issues & Solutions

### NavMesh Not Baking
- Ensure you have terrain or static geometry
- Mark objects as "Static" before baking
- Check NavMesh settings in the scene

### Scripts Not Compiling
- Check for syntax errors in IDE
- Ensure correct namespaces
- Check for missing using statements

### Object Pool Efficiency
- Use object pooling for frequently spawned objects
- Preload pools at game start
- Disable rather than destroy objects

## Resources

- [Unity Documentation](https://docs.unity.com/)
- [C# Language Documentation](https://docs.microsoft.com/en-us/dotnet/csharp/)
- [NavMesh Guide](https://docs.unity.com/Manual/nav-BuildingNavMesh.html)
- [UI Toolkit Documentation](https://docs.unity.com/Manual/UIE-index.html)

---

**Last Updated**: 2026-09-05
