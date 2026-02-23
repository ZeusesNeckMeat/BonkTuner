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

        var totalMultiplier = GetTotalChargeMultiplier();
        var originalChargeTime = chargeShrine.chargeTime;
        chargeShrine.currentChargeTime = originalChargeTime / totalMultiplier;
    }

    public static void TryUpdateShrineRewardOffers(Il2CppSystem.Collections.Generic.List<EncounterOffer> randomStatOffersResult, int amount, bool useShrineStats)
    {
        if (!useShrineStats || !CanContinue() || !ConfigService.ChargeShrine.IsRewardMultiplierEnabled)
            return;

        var totalMultiplier = GetTotalRewardMultiplier();
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

    public static float GetTotalRewardMultiplier()
    {
        return !StatService.TryGetStatValue(ConfigService.ChargeShrine.RewardMultiplierFrequency, out var statValue)
            ? 1
            : GetTotalMultiplier(ConfigService.ChargeShrine.RewardMultiplier, statValue);
    }

    public static float GetTotalChargeMultiplier()
    {
        return !StatService.TryGetStatValue(ConfigService.ChargeShrine.ChargeRateMultiplierFrequency, out var statValue)
            ? 1
            : GetTotalMultiplier(ConfigService.ChargeShrine.ChargeRateMultiplier, statValue);
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