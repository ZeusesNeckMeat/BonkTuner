using BonkTuner.Services;

using UnityEngine;

namespace BonkTuner.UI;
internal class CoreSettingsTab
{
    public static string TabName => "Core";

    public static void Draw()
    {
        GUILayout.Label("Core Settings", Styles.Header);
        GUILayout.Label("", GUILayout.Height(8));

        var isEnabled = ConfigService.Core.IsEnabled;
        var newIsEnabled = GUILayout.Toggle(isEnabled, " Mod Enabled", Styles.Toggle);
        if (newIsEnabled != isEnabled)
        {
            if (newIsEnabled)
                ConfigService.Core.EnableMod();
            else
                ConfigService.Core.DisableMod();

            Main.Logger.LogInfo($"Mod enabled changed to: {newIsEnabled}");
        }

        var isDuringChallenges = ConfigService.Core.IsEnabledDuringChallenges;
        var newIsDuringChallenges = GUILayout.Toggle(isDuringChallenges, " Enable During Challenges", Styles.Toggle);
        if (newIsDuringChallenges != isDuringChallenges)
        {
            if (newIsDuringChallenges)
                ConfigService.Core.EnableDuringChallenges();
            else
                ConfigService.Core.DisableDuringChallenges();

            Main.Logger.LogInfo($"Enable during challenges changed to: {newIsDuringChallenges}");
        }

        GUILayout.Label("", GUILayout.Height(12));
        GUILayout.Label("Press F5 to toggle this menu", Styles.Label);
    }
}