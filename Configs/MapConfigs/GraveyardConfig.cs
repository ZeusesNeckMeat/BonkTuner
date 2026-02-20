using BepInEx.Configuration;

namespace BonkTuner.Configs.MapConfigs;

internal class GraveyardConfig : MapConfigBase
{
    private GraveyardConfig() : base(960f, 1.5f, 2f, 1.5f) { }

    private static GraveyardConfig _instance;
    public static GraveyardConfig Instance => _instance ??= new GraveyardConfig();

    public override void Init(ConfigFile config)
    {
        IsEnabledEntry = config.Bind(
            "Graveyard Map Modifiers",
            "Is Enabled",
            true,
            "Enable or disable Graveyard map modifiers.");

        StageDurationSecondsEntry = config.Bind(
            "Graveyard Map Modifiers",
            "Stage Duration Seconds",
            DefaultStageDurationSeconds,
            "Duration of each stage in seconds.");

        ChestSpawnMultiplierEntry = config.Bind(
            "Graveyard Map Modifiers",
            "Chest Spawn Multiplier",
            DefaultChestSpawnMultiplier,
            "Multiplier for chest spawn rates.");

        ShrineOnlySpawnMultiplierEntry = config.Bind(
            "Graveyard Map Modifiers",
            "Shrine Only Spawn Multiplier",
            DefaultShrineOnlySpawnMultiplier,
            "Multiplier for shrine spawn rates.");

        ShrineAndPotSpawnMultiplierEntry = config.Bind(
            "Graveyard Map Modifiers",
            "Shrine and Pot Spawn Multiplier",
            DefaultShrineAndPotSpawnMultiplier,
            "Multiplier for shrine and pot spawn rates.");
    }
}