using BepInEx.Configuration;

namespace BonkTuner.Configs.MapConfigs;

internal abstract class MapConfigBase
{
    public MapConfigBase(
        float defaultStageDurationSeconds,
        float defaultChestSpawnMultiplier,
        float defaultShrineOnlySpawnMultiplier,
        float defaultShrineAndPotSpawnMultiplier)
    {
        DefaultStageDurationSeconds = defaultStageDurationSeconds;
        DefaultChestSpawnMultiplier = defaultChestSpawnMultiplier;
        DefaultShrineOnlySpawnMultiplier = defaultShrineOnlySpawnMultiplier;
        DefaultShrineAndPotSpawnMultiplier = defaultShrineAndPotSpawnMultiplier;
    }

    protected ConfigEntry<bool> IsEnabledEntry { get; set; }
    protected ConfigEntry<float> StageDurationSecondsEntry { get; set; }
    protected ConfigEntry<float> ChestSpawnMultiplierEntry { get; set; }
    protected ConfigEntry<float> ShrineOnlySpawnMultiplierEntry { get; set; }
    protected ConfigEntry<float> ShrineAndPotSpawnMultiplierEntry { get; set; }

    public string MapName { get; private set; }
    public bool IsEnabled => IsEnabledEntry?.Value ?? false;

    public float DefaultStageDurationSeconds { get; private set; }
    public float StageDurationSeconds => StageDurationSecondsEntry?.Value ?? DefaultStageDurationSeconds;

    public float DefaultChestSpawnMultiplier { get; private set; }
    public float ChestSpawnMultiplier => ChestSpawnMultiplierEntry?.Value ?? DefaultChestSpawnMultiplier;

    public float DefaultShrineOnlySpawnMultiplier { get; private set; }
    public float ShrineOnlySpawnMultiplier => ShrineOnlySpawnMultiplierEntry?.Value ?? DefaultShrineOnlySpawnMultiplier;

    public float DefaultShrineAndPotSpawnMultiplier { get; private set; }
    public float ShrineAndPotSpawnMultiplier => ShrineAndPotSpawnMultiplierEntry?.Value ?? DefaultShrineAndPotSpawnMultiplier;

    public abstract void Init(ConfigFile config);

    public void Disable() => IsEnabledEntry.Value = false;
    public void Enable() => IsEnabledEntry.Value = true;
    public void SetStageDurationSeconds(float seconds) => StageDurationSecondsEntry.Value = seconds;
    public void SetChestSpawnMultiplier(float multiplier) => ChestSpawnMultiplierEntry.Value = multiplier;
    public void SetShrineOnlySpawnMultiplier(float multiplier) => ShrineOnlySpawnMultiplierEntry.Value = multiplier;
    public void SetShrineAndPotSpawnMultiplier(float multiplier) => ShrineAndPotSpawnMultiplierEntry.Value = multiplier;

    public void ResetToDefaults()
    {
        StageDurationSecondsEntry.Value = DefaultStageDurationSeconds;
        ChestSpawnMultiplierEntry.Value = DefaultChestSpawnMultiplier;
        ShrineOnlySpawnMultiplierEntry.Value = DefaultShrineOnlySpawnMultiplier;
        ShrineAndPotSpawnMultiplierEntry.Value = DefaultShrineAndPotSpawnMultiplier;
    }
}