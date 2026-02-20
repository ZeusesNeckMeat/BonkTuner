using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;

using BonkTuner.Services;
using BonkTuner.UI;

using HarmonyLib;

using Il2CppInterop.Runtime.Injection;

using UnityEngine;

namespace BonkTuner;

[BepInPlugin(Constants.GUID, Constants.MODNAME, Constants.VERSION)]
public class Main : BasePlugin
{
    private static ManualLogSource _logger;
    public static ManualLogSource Logger => _logger;

    public Main()
    {
        _logger = base.Log;
    }

    public override void Load()
    {
        _logger.LogInfo($"Loading {Constants.MODNAME} v{Constants.VERSION} by {Constants.AUTHOR}");

        var harmony = new Harmony(Constants.GUID);
        harmony.PatchAll();

        ConfigService.InitializeConfigs(Config);

        try
        {
            ClassInjector.RegisterTypeInIl2Cpp<MainWindow>();
            _logger.LogInfo("MainWindow type registered with IL2CPP");
        }
        catch (System.Exception ex)
        {
            _logger.LogError($"Failed to register MainWindow type: {ex.Message}");
            return;
        }

        // Initialize UI
        var uiObject = new GameObject("BonkTunerUI");
        uiObject.AddComponent<MainWindow>();
        UnityEngine.Object.DontDestroyOnLoad(uiObject);
        _logger.LogInfo("UI initialized");
    }
}