using BepInEx.Configuration;

namespace BonkTuner.Configs.MapConfigs;

internal class DesertConfig : MapConfigBase
{
    private DesertConfig() : base(600f, 1f, 1f, 1f) { }

    private static DesertConfig _instance;
    public static DesertConfig Instance => _instance ??= new DesertConfig();

    public override void Init(ConfigFile config)
    {
        IsEnabledEntry = config.Bind(
            "Desert Map Modifiers",
            "Is Enabled",
            true,
            "Enable or disable Desert map modifiers.");

        StageDurationSecondsEntry = config.Bind(
            "Desert Map Modifiers",
            "Stage Duration Seconds",
            DefaultStageDurationSeconds,
            "Duration of each stage in seconds.");

        ChestSpawnMultiplierEntry = config.Bind(
            "Desert Map Modifiers",
            "Chest Spawn Multiplier",
            DefaultChestSpawnMultiplier,
            "Multiplier for chest spawn rates.");

        ShrineOnlySpawnMultiplierEntry = config.Bind(
            "Desert Map Modifiers",
            "Shrine Only Spawn Multiplier",
            DefaultShrineOnlySpawnMultiplier,
            "Multiplier for shrine spawn rates.");

        ShrineAndPotSpawnMultiplierEntry = config.Bind(
            "Desert Map Modifiers",
            "Shrine and Pot Spawn Multiplier",
            DefaultShrineAndPotSpawnMultiplier,
            "Multiplier for shrine and pot spawn rates.");
    }
}