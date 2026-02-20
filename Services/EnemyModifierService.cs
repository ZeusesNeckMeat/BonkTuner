using Assets.Scripts.Game.Spawning.New;
using Assets.Scripts.Game.Spawning.New.Summoners;

using UnityEngine;

namespace BonkTuner.Services;

internal class EnemyModifierService : ServiceBase
{
    public static void TrySetEnemyChestDropChance(EffectManager effectManager)
    {
        if (!CanContinue() || !ConfigService.EnemyModifiers.IsEnemyDropChanceModifierEnabled)
            return;

        if (effectManager == null || effectManager.WasCollected)
        {
            Main.Logger.LogWarning($"[{nameof(EnemyModifierService)}.{nameof(TrySetEnemyChestDropChance)}] EffectManager is null or was already collected. Cannot set chest drop chance.");
            return;
        }

        var originalDropChance = effectManager.baseChestDropChance;
        var newDropChance = ConfigService.EnemyModifiers.EnemyChestDropChance;
        Main.Logger.LogInfo($"[{nameof(EnemyModifierService)}.{nameof(TrySetEnemyChestDropChance)}] Updating chest drop chance from {originalDropChance} to {newDropChance}");

        effectManager.baseChestDropChance = newDropChance;
    }

    public static int GetModifiedTargetEnemies(int originalTarget)
    {
        if (!CanContinue() || !ConfigService.EnemyModifiers.IsEnemySpawnModifiersEnabled)
            return originalTarget;

        var spawnSettings = ConfigService.EnemyModifiers.GetCurrentSpawnModifiers();
        var newTarget = Mathf.RoundToInt(originalTarget * spawnSettings.TargetSpawnsMultiplier);

        return newTarget;
    }

    public static void TrySetBaseCreditsPerSecond(BaseSummoner newSummoner)
    {
        if (!CanContinue() || !ConfigService.EnemyModifiers.IsEnemySpawnModifiersEnabled)
            return;

        if (newSummoner == null || newSummoner.WasCollected)
        {
            Main.Logger.LogWarning($"[{nameof(EnemyModifierService)}.{nameof(TrySetBaseCreditsPerSecond)}] BaseSummoner is null or was already collected. Cannot set base credits per second.");
            return;
        }

        var stageSummoner = newSummoner.TryCast<StageSummoner>();
        if (stageSummoner == null)
            return;

        var baseCreditsPerSecond = stageSummoner.capReduction;
        var creditsModifier = ConfigService.EnemyModifiers.GetCurrentSpawnModifiers().CreditMultiplier;
        var newCreditsPerSecond = baseCreditsPerSecond * creditsModifier;

        Main.Logger.LogInfo($"[{nameof(EnemyModifierService)}.{nameof(TrySetBaseCreditsPerSecond)}] Updating base credits per second from {baseCreditsPerSecond} to {newCreditsPerSecond} (Modifier: {creditsModifier}x)");

        stageSummoner.capReduction = newCreditsPerSecond;
    }

    public static float GetModifiedSpawnInterval(float originalInterval)
    {
        if (!CanContinue() || !ConfigService.EnemyModifiers.IsEnemySpawnModifiersEnabled)
            return originalInterval;

        var spawnSettings = ConfigService.EnemyModifiers.GetCurrentSpawnModifiers();
        var newInterval = originalInterval * spawnSettings.SpawnIntervalMultiplier;

        return newInterval;
    }
}