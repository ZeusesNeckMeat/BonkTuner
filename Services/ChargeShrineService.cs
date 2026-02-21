using Assets.Scripts.UI.InGame.Rewards;

namespace BonkTuner.Services;

internal class ChargeShrineService : ServiceBase
{
    public static void TryUpdateShrineChargeTime(BaseInteractable baseInteractable)
    {
        if (!CanContinue() || !ConfigService.ChargeShrine.IsChargeRateMultiplierEnabled)
            return;

        if (baseInteractable == null || baseInteractable.WasCollected)
        {
            Main.Logger.LogWarning($"[{nameof(ChargeShrineService)}.{nameof(TryUpdateShrineChargeTime)}] BaseInteractable is null or was already collected. Cannot update shrine charge time.");
            return;
        }

        var chargeShrine = baseInteractable.TryCast<ChargeShrine>();
        if (chargeShrine == null)
            return;

        if (!StatService.TryGetStatValue(ConfigService.ChargeShrine.ChargeRateMultiplierFrequency, out var statValue))
            return;

        if (statValue == 0)
            return;

        var configMultiplier = ConfigService.ChargeShrine.ChargeRateMultiplier;
        var totalMultiplier = GetTotalMultiplier(configMultiplier, statValue);

        var originalChargeTime = chargeShrine.chargeTime;
        chargeShrine.currentChargeTime = originalChargeTime / totalMultiplier;

        Main.Logger.LogDebug($"[{nameof(ChargeShrineService)}.{nameof(TryUpdateShrineChargeTime)}] Updated shrine charge time. Original: {originalChargeTime:F2}s, New: {chargeShrine.currentChargeTime:F2}s, StatValue: {statValue}, ConfigMultiplier: {configMultiplier}, TotalMultiplier: {totalMultiplier:F2}x");
    }

    public static void TryUpdateShrineRewardOffers(Il2CppSystem.Collections.Generic.List<EncounterOffer> randomStatOffersResult, int amount, bool useShrineStats)
    {
        if (!useShrineStats || !CanContinue() || !ConfigService.ChargeShrine.IsRewardMultiplierEnabled)
            return;

        if (!StatService.TryGetStatValue(ConfigService.ChargeShrine.RewardMultiplierFrequency, out var statValue))
            return;

        if (statValue == 0)
            return;

        var configMultiplier = ConfigService.ChargeShrine.RewardMultiplier;
        var totalMultiplier = GetTotalMultiplier(configMultiplier, statValue);

        Main.Logger.LogDebug($"[{nameof(ChargeShrineService)}.{nameof(TryUpdateShrineRewardOffers)}] Applying reward multiplier: {totalMultiplier}x (frequency count: {statValue}, config multiplier: {configMultiplier}x)");

        for (var i = 0; i < amount; i++)
        {
            var offer = randomStatOffersResult[i];

            for (var j = 0; j < offer.effects.Count; j++)
            {
                var effect = offer.effects[j];
                var originalValue = effect.statModifier.modification;
                effect.statModifier.modification *= totalMultiplier;

                Main.Logger.LogDebug($"[{nameof(ChargeShrineService)}.{nameof(TryUpdateShrineRewardOffers)}] Effect {j + 1}: {originalValue:F2} -> {effect.statModifier.modification:F2}");
            }
        }
    }

    private static float GetTotalMultiplier(float configMultiplier, float statValue)
    {
        var bonusPercent = configMultiplier - 1f;
        var totalMultiplier = 1f + (statValue * bonusPercent);

        if (totalMultiplier <= 0.01f)
        {
            Main.Logger.LogWarning($"[{nameof(ChargeShrineService)}.{nameof(GetTotalMultiplier)}] Total multiplier too low ({totalMultiplier}), clamping to 0.01x");
            totalMultiplier = 0.01f;
        }

        return totalMultiplier;
    }
}