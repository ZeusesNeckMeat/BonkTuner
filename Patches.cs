using Assets.Scripts.Game.Other;
using Assets.Scripts.Game.Spawning.New;
using Assets.Scripts.Game.Spawning.New.Summoners;
using Assets.Scripts.Inventory__Items__Pickups.Upgrades;
using Assets.Scripts.Managers;
using Assets.Scripts.UI.InGame.Rewards;

using BonkTuner.Services;

using HarmonyLib;

namespace BonkTuner;

[HarmonyPatch(typeof(EffectManager), nameof(EffectManager.Awake))]
internal class EffectManager_Awake_Patch
{
    [HarmonyPostfix]
    private static void Postfix(EffectManager __instance)
    {
        EnemyModifierService.TrySetEnemyChestDropChance(__instance);
    }
}

[HarmonyPatch(typeof(MapController), nameof(MapController.StartNewMap))]
internal static class MapGenerator_GenerateMap_2_Patch
{
    [HarmonyPrefix]
    public static void Prefix(RunConfig newRunConfig)
    {
        MapDataService.TrySetMapDataOnNewMap(newRunConfig);
    }
}

[HarmonyPatch(typeof(ChargeShrine), nameof(ChargeShrine.OnTriggerEnter))]
internal static class ChargeShrine_OnTriggerEnter_Patch
{
    [HarmonyPostfix]
    public static void Postfix(BaseInteractable __instance)
    {
        ChargeShrineService.TryUpdateShrineChargeTime(__instance);
    }
}

[HarmonyPatch(typeof(EncounterUtility), nameof(EncounterUtility.GetRandomStatOffers))]
internal class EncounterUtility_GetRandomStatOffers_Patch
{
    [HarmonyPostfix]
    public static void EncounterUtility_GetRandomStatOffers_Postfix(Il2CppSystem.Collections.Generic.List<EncounterOffer> __result, int amount, bool forceLegendary, bool useShrineStats)
    {
        ChargeShrineService.TryUpdateShrineRewardOffers(__result, amount, useShrineStats);
    }
}

[HarmonyPatch(typeof(StageSummoner), nameof(StageSummoner.GetNumTargetEnemies))]
internal static class StageSummoner_GetNumTargetEnemies_Patch
{
    [HarmonyPostfix]
    public static void Postfix(ref int __result)
    {
        __result = EnemyModifierService.GetModifiedTargetEnemies(__result);
    }
}

[HarmonyPatch(typeof(SummonerController), nameof(SummonerController.AddSummoner))]
internal static class SummonerController_AddSummoner_Patch
{
    [HarmonyPostfix]
    public static void Postfix(BaseSummoner summoner)
    {
        EnemyModifierService.TrySetBaseCreditsPerSecond(summoner);
    }
}

[HarmonyPatch(typeof(StageSummoner), nameof(StageSummoner.GetSummonInterval))]
internal static class StageSummoner_GetSummonInterval_Patch
{
    [HarmonyPostfix]
    public static void Postfix(ref float __result)
    {
        __result = EnemyModifierService.GetModifiedSpawnInterval(__result);
    }
}