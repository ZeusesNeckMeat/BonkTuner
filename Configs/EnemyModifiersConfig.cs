using BepInEx.Configuration;

using BonkTuner.Configs.EnemySpawnModifiers;

using Il2CppSystem.Text;

using System.Collections.Generic;

namespace BonkTuner.Configs;

internal class EnemyModifiersConfig
{
    private EnemyModifiersConfig() { }

    private static EnemyModifiersConfig _instance;
    public static EnemyModifiersConfig Instance => _instance ??= new EnemyModifiersConfig();

    private ConfigEntry<bool> _isEnemyDropChanceModifierEnabled;
    private ConfigEntry<float> _enemyChestDropChance;

    private ConfigEntry<bool> _isEnemySpawnModifiersEnabled;
    private ConfigEntry<EnemySpawnPreset> _enemySpawnPreset;

    private static readonly Dictionary<EnemySpawnPreset, SpawnModifiersBase> _enemySpawnModifiers = new()
    {
        { EnemySpawnPreset.Normal, new NormalSpawnModifiers() },
        { EnemySpawnPreset.Casual, new CasualSpawnModifiers() },
        { EnemySpawnPreset.Intense, new IntenseSpawnModifiers() },
        { EnemySpawnPreset.Swarm, new SwarmSpawnModifiers() },
        { EnemySpawnPreset.Apocalypse, new ApocalypseSpawnModifiers() }
    };

    public bool IsEnemyDropChanceModifierEnabled => _isEnemyDropChanceModifierEnabled.Value;
    public float EnemyChestDropChance => _enemyChestDropChance.Value;

    public bool IsEnemySpawnModifiersEnabled => _isEnemySpawnModifiersEnabled.Value;
    public EnemySpawnPreset SpawnPreset => _enemySpawnPreset.Value;

    public void DisableEnemyDropChanceModifier() => _isEnemyDropChanceModifierEnabled.Value = false;
    public void EnableEnemyDropChanceModifier() => _isEnemyDropChanceModifierEnabled.Value = true;
    public void SetEnemyChestDropChance(float chance) => _enemyChestDropChance.Value = chance;
    public void ResetEnemyChestDropChance() => _enemyChestDropChance.Value = (float)_enemyChestDropChance.DefaultValue;

    public void DisableEnemySpawnModifiers() => _isEnemySpawnModifiersEnabled.Value = false;
    public void EnableEnemySpawnModifiers() => _isEnemySpawnModifiersEnabled.Value = true;
    public void SetEnemySpawnPreset(EnemySpawnPreset preset) => _enemySpawnPreset.Value = preset;
    public void ResetEnemySpawnPreset() => _enemySpawnPreset.Value = EnemySpawnPreset.Normal;

    public void Init(ConfigFile config)
    {
        _isEnemyDropChanceModifierEnabled = config.Bind(
            "Enemy Modifiers",
            "Is Enemy Drop Chance Modifier Enabled",
            true,
            "Enable or disable the enemy chest drop chance modifier.");

        _enemyChestDropChance = config.Bind(
            "Enemy Modifiers",
            "Enemy Chest Drop Chance",
            0.0005f,
            "Base chance for enemies to drop chests (0.01 = 1%).");

        var stringBuilder = new StringBuilder("Preset options for enemy spawn rates:");
        stringBuilder.AppendLine();
        foreach (var settings in _enemySpawnModifiers.Values)
        {
            stringBuilder.AppendLine($" => {settings.Preset}: {settings.Description}");
        }

        _isEnemySpawnModifiersEnabled = config.Bind(
            "Enemy Modifiers",
            "Is Enemy Spawn Modifiers Enabled",
            true,
            "Enable or disable the enemy spawn modifiers.");

        _enemySpawnPreset = config.Bind(
            "Enemy Modifiers",
            "Enemy Spawn Preset",
            EnemySpawnPreset.Normal,
            stringBuilder.ToString());
    }

    public SpawnModifiersBase GetCurrentSpawnModifiers()
    {
        return _enemySpawnModifiers[SpawnPreset];
    }
}