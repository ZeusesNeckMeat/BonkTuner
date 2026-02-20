using BepInEx.Configuration;

namespace BonkTuner.Configs.MapConfigs;

internal class ForestConfig : MapConfigBase
{
    private ForestConfig() : base(600f, 1f, 1f, 1f) { }

    private static ForestConfig _instance;
    public static ForestConfig Instance => _instance ??= new ForestConfig();

    public override void Init(ConfigFile config)
    {
        IsEnabledEntry = config.Bind(
            "Forest Map Modifiers",
            "Is Enabled",
            true,
            "Enable or disable Forest map modifiers.");

        StageDurationSecondsEntry = config.Bind(
            "Forest Map Modifiers",
            "Stage Duration Seconds",
            DefaultStageDurationSeconds,
            "Duration of each stage in seconds.");

        ChestSpawnMultiplierEntry = config.Bind(
            "Forest Map Modifiers",
            "Chest Spawn Multiplier",
            DefaultChestSpawnMultiplier,
            "Multiplier for chest spawn rates.");

        ShrineOnlySpawnMultiplierEntry = config.Bind(
            "Forest Map Modifiers",
            "Shrine Only Spawn Multiplier",
            DefaultShrineOnlySpawnMultiplier,
            "Multiplier for shrine spawn rates.");

        ShrineAndPotSpawnMultiplierEntry = config.Bind(
            "Forest Map Modifiers",
            "Shrine and Pot Spawn Multiplier",
            DefaultShrineAndPotSpawnMultiplier,
            "Multiplier for shrine and pot spawn rates.");
    }
}