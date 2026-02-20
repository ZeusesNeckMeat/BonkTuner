using BonkTuner.Services;

using UnityEngine;

namespace BonkTuner.UI;
internal class EnemyModifiersTab
{
    public static string TabName => "Enemy Modifiers";

    public static void Draw()
    {
        var isDropEnabled = DrawHelper.DrawEnabledHeaderWithReset(
            label: "Enemy Drop Chance",
            isEnabled: ConfigService.EnemyModifiers.IsEnemyDropChanceModifierEnabled,
            onEnabled: ConfigService.EnemyModifiers.EnableEnemyDropChanceModifier,
            onDisabled: ConfigService.EnemyModifiers.DisableEnemyDropChanceModifier,
            onReset: ConfigService.EnemyModifiers.ResetEnemyChestDropChance);

        if (isDropEnabled)
        {
            GUILayout.Label("", GUILayout.Height(8));

            DrawHelper.DrawSliderWithSteps(
                label: $"Drop Chance: {ConfigService.EnemyModifiers.EnemyChestDropChance * 100:F4}%",
                currentValue: ConfigService.EnemyModifiers.EnemyChestDropChance,
                min: 0f,
                max: 0.1f,
                step: 0.0001f,
                onValueChanged: ConfigService.EnemyModifiers.SetEnemyChestDropChance);

            GUILayout.Label("", GUILayout.Height(12));
            GUILayout.Label($"Actual: {ConfigService.EnemyModifiers.EnemyChestDropChance:F6} ({ConfigService.EnemyModifiers.EnemyChestDropChance * 100:F4}%)", Styles.Label);
        }

        GUILayout.Label("", GUILayout.Height(18));

        var isPresetEnabled = DrawHelper.DrawEnabledHeaderWithReset(
            label: "Enemy Spawn Presets",
            isEnabled: ConfigService.EnemyModifiers.IsEnemySpawnModifiersEnabled,
            onEnabled: ConfigService.EnemyModifiers.EnableEnemySpawnModifiers,
            onDisabled: ConfigService.EnemyModifiers.DisableEnemySpawnModifiers,
            onReset: ConfigService.EnemyModifiers.ResetEnemySpawnPreset);

        if (isPresetEnabled)
        {
            GUILayout.Label("", GUILayout.Height(8));

            // Preset selector
            GUILayout.BeginHorizontal();
            GUILayout.Label("Preset:", Styles.SectionLabel, GUILayout.Width(60));

            var currentPreset = ConfigService.EnemyModifiers.GetCurrentSpawnModifiers();
            var presetNames = System.Enum.GetNames(typeof(EnemySpawnPreset));
            var currentIndex = (int)currentPreset.Preset;

            for (var i = 0; i < presetNames.Length; i++)
            {
                var isSelected = i == currentIndex;
                var buttonStyle = isSelected ? Styles.ButtonActive : Styles.Button;

                if (GUILayout.Button(presetNames[i], buttonStyle, GUILayout.ExpandWidth(true)))
                {
                    ConfigService.EnemyModifiers.SetEnemySpawnPreset((EnemySpawnPreset)i);
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Label("", GUILayout.Height(8));
            GUILayout.Label(currentPreset.Description, Styles.Label);
            GUILayout.Label("", GUILayout.Height(12));

            // Show current settings
            GUILayout.BeginVertical(Styles.Box);
            GUILayout.Label("Preset Effects:", Styles.LabelBold);
            GUILayout.Label($"  • Target Enemy Count: {currentPreset.TargetSpawnsMultiplier:F1}x", Styles.Label);
            GUILayout.Label($"  • Credit Gain Speed: {currentPreset.CreditMultiplier:F1}x", Styles.Label);
            GUILayout.Label($"  • Spawn Interval: {currentPreset.SpawnIntervalMultiplier:F1}x", Styles.Label);
            GUILayout.EndVertical();
        }
    }
}