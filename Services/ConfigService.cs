using Assets.Scripts._Data.MapsAndStages;

using BepInEx.Configuration;

using BonkTuner.Configs;
using BonkTuner.Configs.MapConfigs;

using System.Collections.Generic;

namespace BonkTuner.Services;

internal static class ConfigService
{
    private static readonly Dictionary<EMap, MapConfigBase> _mapModifiers = new();

    public static void InitializeConfigs(ConfigFile config)
    {
        UIConfig.Instance.Init(config);
        CoreConfig.Instance.Init(config);
        EnemyModifiersConfig.Instance.Init(config);
        ChargeShrineConfig.Instance.Init(config);
        DesertConfig.Instance.Init(config);
        ForestConfig.Instance.Init(config);
        GraveyardConfig.Instance.Init(config);

        _mapModifiers[EMap.Desert] = DesertConfig.Instance;
        _mapModifiers[EMap.Forest] = ForestConfig.Instance;
        _mapModifiers[EMap.Graveyard] = GraveyardConfig.Instance;
    }

    public static UIConfig UI => UIConfig.Instance;
    public static CoreConfig Core => CoreConfig.Instance;
    public static EnemyModifiersConfig EnemyModifiers => EnemyModifiersConfig.Instance;
    public static ChargeShrineConfig ChargeShrine => ChargeShrineConfig.Instance;
    public static DesertConfig Desert => DesertConfig.Instance;
    public static ForestConfig Forest => ForestConfig.Instance;
    public static GraveyardConfig Graveyard => GraveyardConfig.Instance;

    public static MapConfigBase GetMapModifiers(EMap map)
    {
        return _mapModifiers.TryGetValue(map, out var modifiers) ? modifiers : null;
    }
}