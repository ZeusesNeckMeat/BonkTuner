using Assets.Scripts._Data.MapsAndStages;

using BonkTuner.Configs.MapConfigs;
using BonkTuner.Services;

using UnityEngine;

namespace BonkTuner.UI;
internal class MapModifiersTab
{
    public static string TabName => "Map Modifiers";

    public static void Draw()
    {
        DrawMapModifier("Desert", ConfigService.GetMapModifiers(EMap.Desert));

        GUILayout.Label("", GUILayout.Height(18));
        DrawMapModifier("Forest", ConfigService.GetMapModifiers(EMap.Forest));

        GUILayout.Label("", GUILayout.Height(18));
        DrawMapModifier("Graveyard", ConfigService.GetMapModifiers(EMap.Graveyard));
    }

    private static void DrawMapModifier(string mapName, MapConfigBase modifiers)
    {
        if (modifiers == null)
            return;

        // Header with toggle and reset button
        var isEnabled = DrawHelper.DrawEnabledHeaderWithReset(
            label: $" {mapName} Settings",
            isEnabled: modifiers.IsEnabled,
            onEnabled: modifiers.Enable,
            onDisabled: modifiers.Disable,
            onReset: modifiers.ResetToDefaults);

        if (isEnabled)
        {
            GUILayout.Label("", GUILayout.Height(8));

            // Stage Duration with step controls (0 to 3600 seconds, step 10)
            DrawHelper.DrawSliderWithSteps(
                label: $"Stage Duration: {modifiers.StageDurationSeconds:F0}s",
                currentValue: modifiers.StageDurationSeconds,
                min: 0f,
                max: 3600f,
                step: 10f,
                onValueChanged: modifiers.SetStageDurationSeconds);

            // Chest Spawn Multiplier with step controls (0 to 20x, step 0.1)
            DrawHelper.DrawSliderWithSteps(
                label: $"Chest Spawn: {modifiers.ChestSpawnMultiplier:F2}x",
                currentValue: modifiers.ChestSpawnMultiplier,
                min: 0f,
                max: 20f,
                step: 0.1f,
                onValueChanged: modifiers.SetChestSpawnMultiplier);

            // Shrine Only Multiplier with step controls (0 to 20x, step 0.1)
            DrawHelper.DrawSliderWithSteps(
                label: $"Shrine Spawn: {modifiers.ShrineOnlySpawnMultiplier:F2}x",
                currentValue: modifiers.ShrineOnlySpawnMultiplier,
                min: 0f,
                max: 20f,
                step: 0.1f,
                onValueChanged: modifiers.SetShrineOnlySpawnMultiplier);

            // Shrine and Pot Multiplier with step controls (0 to 20x, step 0.1)
            DrawHelper.DrawSliderWithSteps(
                label: $"Shrine & Pot: {modifiers.ShrineAndPotSpawnMultiplier:F2}x",
                currentValue: modifiers.ShrineAndPotSpawnMultiplier,
                min: 0f,
                max: 20f,
                step: 0.1f,
                onValueChanged: modifiers.SetShrineAndPotSpawnMultiplier);
        }
    }
}