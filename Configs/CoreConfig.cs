using BepInEx.Configuration;

namespace BonkTuner.Configs;

internal class CoreConfig
{
    private CoreConfig() { }

    private static CoreConfig _instance;
    public static CoreConfig Instance => _instance ??= new CoreConfig();

    private ConfigEntry<bool> _isEnabled;
    private ConfigEntry<bool> _isEnabledDuringChallenges;

    public bool IsEnabled => _isEnabled.Value;
    public bool IsEnabledDuringChallenges => _isEnabledDuringChallenges.Value;

    public void DisableMod() => _isEnabled.Value = false;
    public void EnableMod() => _isEnabled.Value = true;

    public void EnableDuringChallenges() => _isEnabledDuringChallenges.Value = true;
    public void DisableDuringChallenges() => _isEnabledDuringChallenges.Value = false;

    public void Init(ConfigFile config)
    {
        _isEnabled = config.Bind(
            "Core",
            "Is Enabled",
            true,
            "Enable or disable the BonkTuner mod.");

        _isEnabledDuringChallenges = config.Bind(
            "Core",
            "Is Enabled During Challenges",
            false,
            "Allow the mod's effects to be active during challenge runs. Enabling this may cause balance issues in certain challenges.");
    }
}