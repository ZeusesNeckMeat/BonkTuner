using Assets.Scripts.Actors.Player;
using Assets.Scripts.Managers;
using Assets.Scripts.Saves___Serialization.Progression.Stats;

namespace BonkTuner.Services;

internal class StatService
{
    public static bool TryGetStatValue(MultiplyStatFrequency stat, out int value)
    {
        value = 0;

        switch (stat)
        {
            case MultiplyStatFrequency.PerPlayerLevel:
                value = MyPlayer.Instance.inventory.GetCharacterLevel();
                break;
            case MultiplyStatFrequency.PerShrineCharged:
                value = RunStats.GetStat(EMyStat.shrineCharge);
                break;
            case MultiplyStatFrequency.PerStageCompleted:
                value = MapController.GetStageIndex();
                break;
            case MultiplyStatFrequency.PerBossDefeated:
                value = RunStats.GetStat(EMyStat.bossKills);
                break;
            case MultiplyStatFrequency.PerMiniBossAndBossDefeated:
                value = (RunStats.GetStat(EMyStat.bossKills) + RunStats.GetStat(EMyStat.minibossKills));
                break;
            case MultiplyStatFrequency.PerEliteKilled:
                value = RunStats.GetStat(EMyStat.eliteKills);
                break;
            case MultiplyStatFrequency.PerPowerupsUsed:
                value = RunStats.GetStat(EMyStat.powerupsUsed);
                break;
            case MultiplyStatFrequency.PerPotsBroken:
                value = RunStats.GetStat(EMyStat.potsBroken);
                break;
            case MultiplyStatFrequency.PerChestsOpened:
                value = RunStats.GetStat(EMyStat.chestsOpened);
                break;
            case MultiplyStatFrequency.PerSilverEarnedInRun:
                value = RunStats.GetStat(EMyStat.silverEarned);
                break;
            case MultiplyStatFrequency.Disabled:
            default:
                return false;
        }

        return true;
    }
}