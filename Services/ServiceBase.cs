using Assets.Scripts.Game.Other;
using Assets.Scripts.Managers;

namespace BonkTuner.Services;

internal abstract class ServiceBase
{
    public static bool CanContinue()
    {
        return ConfigService.Core.IsEnabled && (ConfigService.Core.IsEnabledDuringChallenges || MapController.runConfig?.challenge == null);
    }

    public static bool CanContinue(RunConfig newRunConfig)
    {
        return ConfigService.Core.IsEnabled && (ConfigService.Core.IsEnabledDuringChallenges || newRunConfig?.challenge == null);
    }
}