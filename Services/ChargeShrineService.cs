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
        var multiplier = statValue * configMultiplier;
        var originalChargeTime = chargeShrine.chargeTime;
        chargeShrine.currentChargeTime = originalChargeTime / multiplier;
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

        // Calculate the total multiplier based on frequency count
        // Example: 10 shrines charged with 1.1x config = 1 + (10 * 0.1) = 2.0x total
        var bonusPerCount = configMultiplier - 1f; // 1.1 - 1 = 0.1 (10% per shrine)
        var totalMultiplier = 1f + (statValue * bonusPerCount); // 1 + (10 * 0.1) = 2.0x

        Main.Logger.LogInfo($"[{nameof(ChargeShrineService)}.{nameof(TryUpdateShrineRewardOffers)}] Applying reward multiplier: {totalMultiplier}x (frequency count: {statValue}, config multiplier: {configMultiplier}x, bonus per count: {bonusPerCount:F2})");

        for (var i = 0; i < amount; i++)
        {
            var offer = randomStatOffersResult[i];

            for (var j = 0; j < offer.effects.Count; j++)
            {
                var effect = offer.effects[j];
                var originalValue = effect.statModifier.modification;
                effect.statModifier.modification *= totalMultiplier;

                Main.Logger.LogDebug($"[{nameof(ChargeShrineService)}.{nameof(TryUpdateShrineRewardOffers)}]   Effect {j + 1}: {originalValue:F2} -> {effect.statModifier.modification:F2}");
            }
        }
    }
}