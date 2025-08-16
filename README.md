# ComBots

ComBots is a modular, scalable Unity project for single-player robot combat. Players, as Meisters, build, customize, and battle Bots against AI opponents in a physics-driven arena. The project features a robust stat system, a quest system with dynamic triggers, a dialogue system with instruction bubbles, and a designer/developer-friendly architecture optimized for Unity 6. All terminology and formulas are derived from the official ComBots documentation.

---

## Glossary & Core Formulas

### Glossary
- **Bot**: A player-controlled or AI combatant in a battle.
- **Rogue**: A wild Bot without a Meister.
- **Meister**: A Bot’s operator, whose Rank influences Bot capabilities.
- **Upgrade**: Transforming a Basic Bot into a Combat Bot using Omega Energy.
- **Awaken**: Transforming a Combat Bot into an Awakened Bot using Omega Energy.
- **Basic Bot**: The initial form of a Bot with limited capabilities.
- **Combat Bot**: An upgraded Bot form, unlocked via Blueprint Experience.
- **Awakened Bot**: The highest Bot form, unlocked after all Combat Bots for a Blueprint are available.
- **RAM**: Resource required to install Software on a Bot, tied to Meister Rank.
- **Load**: Resource required to equip Hardware on a Bot, tied to Meister Rank.
- **Software**: Attacks or moves used by Bots, consuming Energy or Omega Energy.
- **Hardware**: Equipment (e.g., Internal, Headgear, Armor, Arm, Boot) affecting stats and abilities.
- **Ability**: A passive skill assigned to a Bot, one of three unlocked via Blueprint Experience.
- **Rank**: A Meister’s level, determining Bot stats and resource limits.
- **Omega Energy**: A resource used for Upgrades and Awakening Bots.
- **Condition**: Status effects (e.g., Shock, Rust, Overload) impacting Bots in and out of battle.

### Core Formulas
- **Damage Calculation**:  
  `FinalDamage = BaseDamage × WeaponMultiplier × (1 - TargetArmor/100)`
- **Energy Consumption**:  
  `EnergyUsed = ActionCost × EfficiencyMultiplier`
- **Movement Speed**:  
  `Speed = BaseSpeed × MobilityTypeMultiplier × (1 - WeightPenalty)`
- **Knockback**:  
  `KnockbackForce = WeaponForce × (1 - TargetMass/MaxMass)`
- **Variable-Power Software (Examples)**:  
  - *Exhaust Blast*: `Power = 2 × OmegaEnergyConsumed`  
  - *Panic*: `Power = 10 to 100` (based on Endurance ratio, see documentation)  
  - *Topspin*: `Power = round(25 × UserSpeed / TargetSpeed)`
- **Rank Experience Gain**:  
  `Yield_o = round_up[(l × Rank_o / 5) × m × ((2 × Rank_o + 10) / (Rank_o + Rank_p + 10))^2.5]`  
  Where `l` varies by Bot form (60, 120, 240) and `m` adjusts for battle conditions.

For detailed terms and formulas, refer to `Documentation/0 ComBots Glossary + Formulas.pdf` and `Documentation/3 Formulas for Variable-Power Software.pdf`.

---

## Table of Contents
- [Features](#features)
- [Technical Stack](#technical-stack)
- [Project Organization](#project-organization)
- [Development Team](#development-team)
- [Development Process](#development-process)
- [Getting Started](#getting-started)
- [Usage](#usage)
- [Known Issues](#known-issues)
- [Contributing](#contributing)
- [License](#license)

---

## Features
- **Modular Robot Building**: Customize Bots with Hardware and Software, driven by a stat system.
- **AI Combat**: Battle AI-controlled Bots with physics-based mechanics (damage, knockback, movement).
- **Bot Progression**: Upgrade Basic Bots to Combat Bots and Awaken them using Omega Energy.
- **Quest System**: Dynamic triggers (e.g., dialogue, area entry, battle end) for quests (see `Documentation/4 Quest Triggers.pdf`).
- **Dialogue System**: Instruction bubbles (e.g., TALK, QUEST, REPAIR) for NPC interactions (see `Documentation/5 Dialog Instruction Bubbles.pdf`).
- **Variable-Power Software**: Moves with dynamic power based on conditions (e.g., Endurance, Speed, Hardware).
- **Designer-Friendly Architecture**: Modular assets and code optimized for Unity 6.
- **Performance Optimizations**: ECS and object pooling for efficient gameplay.


---

## Project Organization

This project uses a domain-driven folder structure for clarity and scalability:

```bash
Assets/
├── @Core/               # Core systems (managers, utilities)
├── @Art/                # All visual/audio assets
├── @Scripts/            # Codebase
├── @Prefabs/            # Prefabs organized by domain
├── @Scenes/             # Scene files
├── @ThirdParty/         # Store-bought assets (clearly isolated)
└── @Sandbox/            # Temporary/experimental assets (excluded from VCS)
```

**Domain-First Subfolders:**
- `@Art/Environment/Forest/Textures`
- `@Prefabs/Bots/Heavy/Crusher`
- `@Scripts/Gameplay/Combat`

**Asset Prefixes:**
- `MAT_BotArmor_Titanium` (Material)
- `TEX_BotChassis_Carbon` (Texture)
- `FBX_Bot_HeavyCrusher` (Model)
- `S_BotWeapon_Laser` (Shader)

**Critical Changes:**
...existing code...
- Replace `_Project` with `@Core` for engine-agnostic systems.
- Use `@` prefix for top folders (prevents naming conflicts).
- Delete `__NoVersionControl` → Use `.gitignore` for `@Sandbox` instead.

---

## Scene Hierarchy & Management

*Optimized for multi-designer workflows in Unity 6.*

### Hierarchy Conventions:
```bash
- [SCENE_ROOT]           # Empty GameObject at (0,0,0)
  ├── #Managers          # Persistent systems (GameManager, AudioManager)
  ├── #Environment       # Static world geometry
  ├── #Dynamic           # Runtime-spawned objects (enemies, items)
  ├── #UI_Canvas         # All UI elements (World Space UI separate)
  └── #Lighting          # Light probes, reflection probes, global lights
```

### Scene Workflow:
- **Modular Scene Loading**:
  - Split levels into `SceneName_Main`, `SceneName_Lighting`, `SceneName_Props`.
  - Load additive scenes via Addressables (not `LoadSceneAsync`).

- **Designer Safety**:
  - Use `[ExecuteInEditMode]` scripts for designer tools (e.g., object placers).
  - Never use `DontDestroyOnLoad` → Inject managers via `@Core` prefabs.

---

## Code Organization

Enforce strict boundaries for designers/developers.

### Structural Standards:
```csharp
// Namespace mirrors folder path:
namespace Project.Core.Audio { ... }  
namespace Project.Gameplay.Combat { ... }  
```

### Designer-Accessible Patterns:
- **ScriptableObject Data Containers**: `SO_EnemyStats`, `SO_LevelConfig`
- **Event Channels**: `VoidEventChannel` for decoupled interactions (e.g., UI → gameplay)

### Code Conventions:
- `private Rigidbody _rigidbody;` (camelCase + `_` prefix)
- `public float MaxHealth { get; private set; }` (properties > public fields)

---

## Version Control & Collaboration

*Git-centric workflow for 50+ member teams.*

### Essential Practices:
- Git LFS for art assets (`.fbx`, `.psd`, `.wav`)
- Perforce for projects >100GB

### .gitignore must exclude:
```bash
/[Ll]ibrary/
/[Tt]emp/
/@Sandbox/
*.assetbundle
```

### Designer Workflow:
- Prefab Variants for level props (designers tweak without breaking prefabs)
- UnityYAMLMerge for scene/resolving conflicts

---

## Efficiency & Designer Tooling

*Unity 6-specific optimizations.*

### Designer-Centric Tools:
- **Custom Editors**: Create Inspector tools for artists (e.g., terrain painters)
- **Addressable Asset System**: Isolate platform-specific assets (designers tag via GUI)

### Performance:
- **Entity Component System (ECS)**: For CPU-intensive features (physics, AI)
- **Presets**: Enforce texture/material settings automatically

---

## Core Principles for Robust Unity Architecture

### Component-Based Architecture
**Principle**: Build small, reusable MonoBehaviors with single responsibilities

**Implementation**:
```csharp
// Instead of monolithic Player class
public class CharacterMovement : MonoBehaviour { ... }
public class CharacterCombat : MonoBehaviour { ... }
public class CharacterInventory : MonoBehaviour { ... }
```

### Decoupled Systems
**Principle**: Isolate systems using interfaces and dependency inversion

**Implementation**:
```csharp
public interface IInventoryService {
    void AddItem(Item item);
    void RemoveItem(Item item);
}

public class PlayerController : MonoBehaviour {
    [SerializeField] private IInventoryService _inventory;
}
```

### Event-Driven Communication
**Principle**: Replace direct references with event channels

**Implementation**:
```csharp
[CreateAssetMenu(menuName = "Events/ItemEvent")]
public class ItemEventChannel : ScriptableObject {
    public event Action<Item> OnEventRaised;
    public void Raise(Item item) => OnEventRaised?.Invoke(item);
}
```

---

## Advanced Architectural Patterns

### 1. Single Entry Point
Use one single entry point in the scene.
```csharp
public interface IInitializable
{
    void Initialize();
}

public class SceneEntryPoint : MonoBehaviour
{
    [Tooltip("Drag all initializable MonoBehaviours here")]
    [SerializeField] private MonoBehaviour[] _initializables;

    private void Awake()
    {
        foreach (var mono in _initializables)
        {
            if (mono is IInitializable system)
            {
                system.Initialize();
            }
            else
            {
                Debug.LogWarning($"{mono.name} does not implement IInitializable");
            }
        }
    }
}
```

### 2. Enable designer-configurable behaviors
Use sriptable objects for behaviour configurations
```csharp
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyBehavior", menuName = "Config/Enemy Behavior")]
public class EnemyBehaviorConfig : ScriptableObject
{
    public float moveSpeed = 2f;
    public int health = 100;
    public string attackType = "Melee";
}
```

---

## Dependency Management Techniques

### 1. Service Locator Pattern
```csharp
public static class ServiceLocator {
    private static readonly Dictionary<Type, object> _services = new();

    public static void Register<T>(T service) {
        _services[typeof(T)] = service;
    }

    public static T Get<T>() {
        return (T)_services[typeof(T)];
    }
}

// Usage in systems:
var audioService = ServiceLocator.Get<IAudioService>();
audioService.PlaySFX("explosion");
```

### 2. Dependency Injection (via Zenject)
```csharp
public class GameInstaller : MonoInstaller {
    public override void InstallBindings() {
        Container.Bind<ISaveSystem>().To<JsonSaveSystem>().AsSingle();
        Container.Bind<IAnalytics>().To<UnityAnalyticsAdapter>().AsSingle();
    }
}

public class PlayerProgression : MonoBehaviour {
    [Inject] private readonly ISaveSystem _saveSystem;
}
```

---

## Performance-Oriented Design

### Object Pooling System
```csharp
public class GameObjectPool {
    private Queue<GameObject> _pool = new();
    private GameObject _prefab;

    public GameObjectPool(GameObject prefab, int initialSize) {
        _prefab = prefab;
        for (int i = 0; i < initialSize; i++) {
            ReturnToPool(CreateNew());
        }
    }

    public GameObject Get() => _pool.Count > 0 ? _pool.Dequeue() : CreateNew();

    private GameObject CreateNew() {
        var obj = Instantiate(_prefab);
        obj.AddComponent<Poolable>().Initialize(this);
        return obj;
    }

    public void ReturnToPool(GameObject obj) {
        obj.SetActive(false);
        _pool.Enqueue(obj);
    }
}
```

### Data-Oriented Design
- Use `NativeArray` for intensive calculations
- Employ Burst-compiled jobs
- Structure data in contiguous memory

---

## Testing & Maintenance

### Unit Test Framework
```csharp
[TestFixture]
public class InventoryTests {
    [Test]
    public void AddItem_IncreasesCount() {
        var inventory = new InventorySystem();
        inventory.AddItem(new Item("Sword"));
        Assert.AreEqual(1, inventory.ItemCount);
    }
}
```

### Editor Debugging Tools
```csharp
[CustomEditor(typeof(PathfindingSystem))]
public class PathfindingEditor : Editor {
    void OnSceneGUI() {
        var system = target as PathfindingSystem;
        Handles.color = Color.green;
        foreach (var node in system.Nodes) {
            Handles.DrawWireCube(node.Position, Vector3.one);
        }
    }
}
```