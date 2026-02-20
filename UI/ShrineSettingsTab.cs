using BonkTuner.Services;

using System;

using UnityEngine;

namespace BonkTuner.UI;
internal class ShrineSettingsTab
{
    public static string TabName => "Shrine Settings";

    public static void Draw()
    {
        var isShrineRewardMultiplierEnabled = DrawHelper.DrawEnabledHeaderWithReset(
            label: "Shrine Reward Multiplier",
            isEnabled: ConfigService.ChargeShrine.IsRewardMultiplierEnabled,
            onEnabled: ConfigService.ChargeShrine.EnableRewardMultiplier,
            onDisabled: ConfigService.ChargeShrine.DisableRewardMultiplier,
            onReset: ConfigService.ChargeShrine.ResetRewardMultiplier);

        if (isShrineRewardMultiplierEnabled)
        {
            DrawFrequencySelector(
                label: "Frequency:",
                currentFreq: ConfigService.ChargeShrine.RewardMultiplierFrequency,
                onFreqChanged: freq => ConfigService.ChargeShrine.SetRewardMultiplierFrequency(freq));

            DrawHelper.DrawSliderWithSteps(
                label: $"Reward Multiplier: {ConfigService.ChargeShrine.RewardMultiplier:F2}x",
                currentValue: ConfigService.ChargeShrine.RewardMultiplier,
                min: 0f,
                max: 10f,
                step: 0.01f,
                onValueChanged: ConfigService.ChargeShrine.SetRewardMultiplier);
        }

        GUILayout.Label("", GUILayout.Height(18));

        var isChargeRateMultiplierEnabled = DrawHelper.DrawEnabledHeaderWithReset(
            label: "Charge Rate Multiplier",
            isEnabled: ConfigService.ChargeShrine.IsChargeRateMultiplierEnabled,
            onEnabled: ConfigService.ChargeShrine.EnableChargeRateMultiplier,
            onDisabled: ConfigService.ChargeShrine.DisableChargeRateMultiplier,
            onReset: ConfigService.ChargeShrine.ResetChargeRateMultiplier);

        if (isChargeRateMultiplierEnabled)
        {
            DrawFrequencySelector(
                label: "Frequency:",
                currentFreq: ConfigService.ChargeShrine.ChargeRateMultiplierFrequency,
                onFreqChanged: freq => ConfigService.ChargeShrine.SetChargeRateMultiplierFrequency(freq));

            DrawHelper.DrawSliderWithSteps(
                label: $"Charge Rate: {ConfigService.ChargeShrine.ChargeRateMultiplier:F2}x",
                currentValue: ConfigService.ChargeShrine.ChargeRateMultiplier,
                min: 0f,
                max: 10f,
                step: 0.01f,
                value => ConfigService.ChargeShrine.SetChargeRateMultiplier(value));
        }
    }

    private static void DrawFrequencySelector(string label, MultiplyStatFrequency currentFreq, Action<MultiplyStatFrequency> onFreqChanged)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label, Styles.SectionLabel, GUILayout.Width(110));
        GUILayout.EndHorizontal();
        GUILayout.Label("", GUILayout.Height(4));

        var frequencyNames = Enum.GetNames(typeof(MultiplyStatFrequency));
        var currentIndex = (int)currentFreq;

        const int columns = 3;
        var rows = (int)Math.Ceiling(frequencyNames.Length / (float)columns);

        for (var row = 0; row < rows; row++)
        {
            GUILayout.BeginHorizontal();

            for (var col = 0; col < columns; col++)
            {
                var index = row * columns + col;

                if (index < frequencyNames.Length)
                {
                    var isSelected = index == currentIndex;
                    var buttonStyle = isSelected ? Styles.ButtonActive : Styles.Button;

                    if (GUILayout.Button(frequencyNames[index], buttonStyle, GUILayout.ExpandWidth(true)))
                    {
                        onFreqChanged((MultiplyStatFrequency)index);
                    }
                }
                else
                {
                    // Empty space for incomplete last row
                    GUILayout.Label("", GUILayout.ExpandWidth(true));
                }
            }

            GUILayout.EndHorizontal();
        }

        GUILayout.Label("", GUILayout.Height(8));
    }
}