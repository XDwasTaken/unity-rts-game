# World Generation System

## Overview

The world generation system creates a procedurally generated map with various terrain types using Perlin noise. It also manages resource placement and handles world initialization.

## Terrain Types

### 1. **Ocean** (Blue)
- Lowest elevation areas
- Not walkable
- High moisture content
- Temperate

### 2. **River** (Light Blue)
- Near sea level with high moisture
- Not walkable
- Critical moisture zones
- Used by ecosystems

### 3. **Plains** (Light Green)
- Low elevation, walkable terrain
- Moderate moisture
- Temperate climate
- Good for settlements

### 4. **Desert** (Gold)
- Low elevation but high temperature
- Very low moisture
- Walkable
- Harsh environment

### 5. **Forests** (Various Greens)
- **Birch Forest** (Light Green): Temperate forests with medium moisture
- **Spruce Forest** (Dark Green): Cold forests with high moisture
- **Oak Forest** (Medium Green): Warm forests with medium moisture
- All contain resources (wood, etc.)
- Walkable with difficulty

### 6. **Hills** (Tan)
- Medium elevation
- Low moisture
- Walkable
- Good defensive positions

### 7. **Rocky Mountains** (Gray)
- High elevation
- Not walkable
- Low moisture
- Natural barriers

### 8. **Snowy Peaks** (White)
- Highest elevation
- Not walkable
- Very cold
- Impassable terrain

## Components

### MapGenerator
Generates the procedural terrain map using Perlin noise.

**Features:**
- Multi-octave Perlin noise for realistic terrain
- Separate noise maps for height, moisture, and temperature
- Configurable map size and noise parameters
- Automatic visualization with colored tiles
- Editor support for noise map visualization

**Settings:**
```
Map Size: 100x100 (configurable)
Tile Size: 1.0 unit
Height Scale: 30.0
Noise Frequencies: Height (0.1), Moisture (0.05), Temperature (0.08)
Octaves: Height (4), Moisture (3)
```

### TerrainManager
Static manager that handles terrain configuration and type determination.

**Functions:**
- `GetTerrainConfig(TerrainType)` - Get configuration for a terrain type
- `GetTerrainTypeByHeight(height, moisture, temperature)` - Determine terrain from noise values

### ResourceManager
Manages resource nodes on the map.

**Resource Types:**
- Minerals (Cyan nodes)
- Gas (Yellow nodes)

**Features:**
- Random placement based on terrain type
- Resource harvesting system
- Area queries for nearby resources
- Visual debugging with gizmos

### WorldSpawner
Orchestrates the entire world initialization process.

**Responsibilities:**
- Initialize MapGenerator
- Generate ResourceManager
- Setup camera bounds
- Initialize game systems

## Usage

### Basic Setup

1. Create an empty GameObject called "World"
2. Add `MapGenerator` script
3. Add `ResourceManager` script
4. Add `WorldSpawner` script (optional, for automatic initialization)

### Configuration

In the Inspector:
```
MapGenerator:
  - Map Width: 100
  - Map Height: 100
  - Tile Size: 1.0
  - Height Scale: 30
  - Random Seed: 12345
  - Generate On Start: true

ResourceManager:
  - Minerals Per Node: 500
  - Gas Per Node: 300
  - Resource Density: 0.15 (15%)
```

### Procedural Generation

The terrain generation uses Perlin noise with multiple octaves:

```
Height Map: Determines elevation levels
  - Controls mountain, hill, and ocean placement
  - 4 octaves for varied features
  
Moisture Map: Controls water availability
  - Influences forest and desert placement
  - High moisture = forests/rivers
  - Low moisture = deserts
  
Temperature Map: Controls climate zones
  - Determines forest types
  - Spruce in cold areas, Oak in warm areas
  - Affects terrain selection
```

### Customization

#### Change Map Size
```csharp
mapGenerator.MapWidth = 200;
mapGenerator.MapHeight = 200;
mapGenerator.GenerateMap();
```

#### Use Fixed Seed
```csharp
mapGenerator.useRandomSeed = false;
mapGenerator.randomSeed = 12345;
mapGenerator.GenerateMap();
```

#### Adjust Noise Parameters
```csharp
mapGenerator.heightFrequency = 0.15f;  // More detailed terrain
mapGenerator.moistureFrequency = 0.08f;  // More forest/desert variation
mapGenerator.GenerateMap();
```

## Terrain Generation Rules

The system determines terrain based on height, moisture, and temperature:

```
IF height < 0.0:
  -> Ocean
ELSE IF height < 0.1 AND moisture > 0.8:
  -> River
ELSE IF height > 0.7:
  -> Snowy Peaks
ELSE IF height > 0.5:
  -> Rocky Mountains
ELSE IF height > 0.3:
  -> Hills
ELSE IF height > 0.1 AND moisture > 0.4:
  IF temperature < 0.35: -> Forest_Spruce
  ELSE IF temperature > 0.65: -> Forest_Oak
  ELSE: -> Forest_Birch
ELSE IF temperature > 0.8 AND moisture < 0.3:
  -> Desert
ELSE:
  -> Plains (default)
```

## Integration with Other Systems

### NavMesh
- Use `MapGenerator.IsWalkable(position)` to check terrain
- Only walkable terrains allow unit movement
- Rocky Mountains and Snowy Peaks block pathfinding

### Camera System
- `RTSCamera.SetMapBounds()` automatically set by WorldSpawner
- Camera can only pan within generated map bounds

### Unit Placement
- Spawn units only on walkable terrain
- Use `MapGenerator.GetTerrainAtPosition(worldPos)` to verify

### Resources
- Minerals and gas placed in forest areas
- Use `ResourceManager.GetResourcesInArea()` for gathering units

## Performance Considerations

- Map generation is CPU-intensive for large maps
- Tile mesh creation can be optimized with mesh batching
- Consider using LOD for large maps
- Resource queries are O(n) - cache for performance

## Future Enhancements

- Height-based mesh deformation (3D terrain)
- Weather system affecting terrain traversal
- Dynamic resource regeneration
- Biome modifiers for variety
- Island generation algorithms
- Cave systems
- Dynamic water flow simulation

---

**Version**: 1.0
**Last Updated**: 2026-09-05
