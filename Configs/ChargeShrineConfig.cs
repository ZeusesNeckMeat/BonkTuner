using BepInEx.Configuration;

namespace BonkTuner.Configs;

internal class ChargeShrineConfig
{
    private ChargeShrineConfig() { }

    private static ChargeShrineConfig _instance;
    public static ChargeShrineConfig Instance => _instance ??= new ChargeShrineConfig();

    private ConfigEntry<bool> _isRewardMultiplierEnabled;
    private ConfigEntry<MultiplyStatFrequency> _rewardMultiplierFrequency;
    private ConfigEntry<float> _rewardMultiplier;

    private ConfigEntry<bool> _isChargeRateMultiplierEnabled;
    private ConfigEntry<MultiplyStatFrequency> _chargeRateMultiplierFrequency;
    private ConfigEntry<float> _chargeRateMultiplier;

    public bool IsRewardMultiplierEnabled => _isRewardMultiplierEnabled.Value;
    public MultiplyStatFrequency RewardMultiplierFrequency => _rewardMultiplierFrequency.Value;
    public float RewardMultiplier => _rewardMultiplier.Value;

    public bool IsChargeRateMultiplierEnabled => _isChargeRateMultiplierEnabled.Value;
    public MultiplyStatFrequency ChargeRateMultiplierFrequency => _chargeRateMultiplierFrequency.Value;
    public float ChargeRateMultiplier => _chargeRateMultiplier.Value;

    public void DisableRewardMultiplier() => _isRewardMultiplierEnabled.Value = false;
    public void EnableRewardMultiplier() => _isRewardMultiplierEnabled.Value = true;
    public void SetRewardMultiplierFrequency(MultiplyStatFrequency frequency) => _rewardMultiplierFrequency.Value = frequency;
    public void SetRewardMultiplier(float multiplier) => _rewardMultiplier.Value = multiplier;
    public void ResetRewardMultiplier() => _rewardMultiplier.Value = (float)_rewardMultiplier.DefaultValue;

    public void DisableChargeRateMultiplier() => _isChargeRateMultiplierEnabled.Value = false;
    public void EnableChargeRateMultiplier() => _isChargeRateMultiplierEnabled.Value = true;
    public void SetChargeRateMultiplierFrequency(MultiplyStatFrequency frequency) => _chargeRateMultiplierFrequency.Value = frequency;
    public void SetChargeRateMultiplier(float multiplier) => _chargeRateMultiplier.Value = multiplier;
    public void ResetChargeRateMultiplier() => _chargeRateMultiplier.Value = (float)_chargeRateMultiplier.DefaultValue;

    public void Init(ConfigFile config)
    {
        _isRewardMultiplierEnabled = config.Bind(
            "Charge Shrine Modifiers",
            "Is Reward Multiplier Enabled",
            true,
            "Enable or disable the charge shrine reward multiplier.");

        _rewardMultiplierFrequency = config.Bind(
            "Charge Shrine Modifiers",
            "Reward Multiplier Frequency",
            MultiplyStatFrequency.PerShrineCharged,
            "Frequency at which the reward multiplier is applied.");

        _rewardMultiplier = config.Bind(
            "Charge Shrine Modifiers",
            "Reward Multiplier",
            1.0f,
            "Multiplier for shrine reward quality. The frequency stat value is multiplied by this multiplier. (1.0 = no change, 2.0 = double per frequency count, 0.5 = half per frequency count)");

        _isChargeRateMultiplierEnabled = config.Bind(
            "Charge Shrine Modifiers",
            "Is Charge Rate Multiplier Enabled",
            true,
            "Enable or disable the charge shrine charge rate multiplier.");

        _chargeRateMultiplierFrequency = config.Bind(
            "Charge Shrine Modifiers",
            "Charge Rate Multiplier Frequency",
            MultiplyStatFrequency.PerShrineCharged,
            "Frequency at which the charge rate multiplier is applied.");

        _chargeRateMultiplier = config.Bind(
            "Charge Shrine Modifiers",
            "Charge Rate Multiplier",
            1.0f,
            "Multiplier for shrine charge speed. The frequency stat value is multiplied by this multiplier. (1.0 = no change, 2.0 = charges twice as fast per frequency count, 0.5 = charges twice as slow per frequency count)");
    }
}