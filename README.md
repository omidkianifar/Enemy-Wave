# Enemy Wave System

A Unity-based enemy wave system that manages enemy spawning, pathfinding, and wave progression.

> **Note**: Open the sample scene located at `_Project/Scenes` to see the system in action.

> **Setup Instructions**: 
> 1. Adjust the Game view window to 16:9 aspect ratio for proper display
> 2. Click the "Spawn" button in the scene to start a new wave

> **Data Structure Note**: Enemy data is stored in ScriptableObjects for easy configuration and reuse, while wave data is implemented as a regular class since it needs to maintain references to scene-specific path objects.

## Class Diagram

The project includes a PlantUML class diagram (`class-diagram.puml`) that shows the relationships between all components. To view the diagram:

1. Use the PlantUML online server: http://www.plantuml.com/plantuml/uml/
2. Install a PlantUML extension in your IDE:
   - VS Code: "PlantUML" extension
   - IntelliJ IDEA: "PlantUML Integration" plugin
   - Visual Studio: "PlantUML" extension

The diagram shows:
- All interfaces and their implementations
- Class relationships and dependencies
- Data models and their connections
- Core system components and their interactions

## TODO Items

### High Priority
1. Separate player health from GameplayManager and store it in a data model
2. Implement object pooling for better performance with large waves
3. Add wave progression system (difficulty scaling, rewards, etc.)
4. Implement save/load system for game progress

### Medium Priority
1. Add visual feedback for enemy damage and death
2. Create a wave preview system
3. Add sound effects and background music
4. Implement different path types (circular, zigzag, etc.)

### Low Priority
1. Add particle effects for enemy spawn/death
2. Create a wave editor tool
3. Add statistics tracking (enemies killed, waves completed, etc.)
4. Implement different enemy behaviors (ranged, melee, etc.)

## Project Structure

```
Assets/
└── _Project/
    └── Scripts/
        ├── Enemies/
        │   ├── Controllers/
        │   │   └── EnemyController.cs       # Controls enemy behavior and movement
        │   ├── Factories/
        │   │   └── EnemyFactory.cs          # Creates enemy instances
        │   ├── Managers/
        │   │   ├── EnemySpawner.cs          # Handles enemy spawning
        │   │   ├── IEnemySpawner.cs         # Interface for enemy spawning
        │   │   ├── WaveManager.cs           # Manages wave progression
        │   │   └── IWaveManager.cs          # Interface for wave management
        │   ├── Models/
        │   │   ├── EnemyProperties.cs       # Enemy configuration data
        │   │   ├── EnemyType.cs             # Enum for enemy types
        │   │   ├── EnemyWave.cs             # Wave configuration
        │   │   ├── EnemyRepository.cs       # Stores enemy properties
        │   │   └── IEnemyRepository.cs      # Interface for enemy data access
        │   └── Views/
        │       └── EnemyView.cs             # Handles enemy visuals and animations
        ├── Pathfinding/
        │   ├── PathFinder.cs                # Manages enemy paths
        │   └── IPathFinder.cs               # Interface for pathfinding
        └── Common/
            └── Interfaces/                  # Common interfaces
```

## Key Components

### Enemy System
- **EnemyController**: Core component that manages enemy behavior, movement, and state
- **EnemyFactory**: Creates enemy instances with proper dependencies
- **EnemySpawner**: Handles spawning of enemies at specified positions
- **EnemyRepository**: Stores and provides access to enemy properties
- **EnemyView**: Manages enemy visuals and animations

### Wave System
- **WaveManager**: Controls wave progression and timing
- **EnemyWave**: Configuration for wave composition and timing
- **EnemyType**: Enum defining different enemy types

### Pathfinding
- **PathFinder**: Manages enemy movement paths
- **IPathFinder**: Interface for pathfinding functionality

## Setup Instructions

1. Clone the repository
2. Open the project in Unity (recommended version: 2022.3 LTS or newer)
3. Open the main scene
4. Configure the following in the Unity Editor:
   - Set up enemy prefabs with required components
   - Configure wave settings in the WaveManager
   - Set up path points for enemy movement

## Dependencies

- Unity 2022.3 LTS or newer
- VContainer (for dependency injection)

## Usage

1. Create enemy prefabs with:
   - EnemyView component
   - Required visual components
   - Proper colliders and rigidbodies

2. Configure enemy properties in the EnemyRepository:
   - Health
   - Speed
   - Damage
   - Other relevant stats

3. Set up wave configurations:
   - Define wave composition
   - Set spawn timing
   - Configure enemy types and counts

4. Configure paths:
   - Set up path points in the scene
   - Configure path properties in the PathFinder

## Development Guidelines

1. **Dependency Injection**
   - Use VContainer for dependency management
   - Follow interface-based design
   - Keep components loosely coupled

2. **Code Organization**
   - Follow the existing folder structure
   - Place new components in appropriate directories
   - Maintain separation of concerns

3. **Testing**
   - Test new features in isolation
   - Verify wave progression
   - Check enemy behavior and pathfinding

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request

## License

This project is licensed under the MIT License - see the LICENSE file for details. 