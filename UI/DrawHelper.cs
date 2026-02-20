using System;

using UnityEngine;

namespace BonkTuner.UI;
internal class DrawHelper
{
    public static void DrawSliderWithSteps(string label, float currentValue, float min, float max, float step, Action<float> onValueChanged)
    {
        GUILayout.BeginVertical(Styles.SliderBox);
        GUILayout.BeginHorizontal();
        GUILayout.Label(label, Styles.Label, GUILayout.Width(240));

        // Minus button - larger and more visible
        if (GUILayout.Button("-", Styles.SmallButton, GUILayout.Width(30), GUILayout.Height(24)))
        {
            // Calculate the floor value based on step
            var steppedValue = Mathf.Floor(currentValue / step) * step;

            // If already on a step, go to previous step
            if (Mathf.Abs(currentValue - steppedValue) < 0.00001f)
            {
                steppedValue -= step;
            }
            var newValue = Mathf.Max(min, steppedValue);
            onValueChanged(newValue);
        }

        var sliderValue = GUILayout.HorizontalSlider(
            currentValue,
            min,
            max,
            Styles.Slider,
            Styles.SliderThumb,
            GUILayout.Height(24));

        if (Math.Abs(sliderValue - currentValue) > 0.00001f)
        {
            onValueChanged(sliderValue);
        }

        if (GUILayout.Button("+", Styles.SmallButton, GUILayout.Width(30), GUILayout.Height(24)))
        {
            // Calculate the ceiling value based on step
            var steppedValue = Mathf.Ceil(currentValue / step) * step;

            // If already on a step, go to next step
            if (Mathf.Abs(currentValue - steppedValue) < 0.00001f)
            {
                steppedValue += step;
            }
            var newValue = Mathf.Min(max, steppedValue);
            onValueChanged(newValue);
        }

        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    public static bool DrawEnabledHeaderWithReset(string label, bool isEnabled, Action onEnabled, Action onDisabled, Action onReset)
    {
        GUILayout.BeginHorizontal();

        var statusIcon = isEnabled ? "✓" : "✗";
        var statusColor = isEnabled ? "ENABLED" : "DISABLED";
        var displayLabel = $"{statusIcon} {label} [{statusColor}]";

        if (GUILayout.Button(displayLabel, isEnabled ? Styles.HeaderActive : Styles.HeaderInactive, GUILayout.ExpandWidth(true)))
        {
            if (!isEnabled)
                onEnabled();
            else
                onDisabled();
        }

        if (GUILayout.Button("Reset", Styles.SmallButton, GUILayout.Width(60), GUILayout.Height(33)))
        {
            onReset();
            Main.Logger.LogInfo($"{label} reset to default");
        }

        GUILayout.EndHorizontal();

        return isEnabled;
    }
}