using Assets.Scripts._Data.MapsAndStages;
using Assets.Scripts.Managers;

using BonkTuner.Services;

using System;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace BonkTuner.UI;

internal class SettingsDisplayWindow : MonoBehaviour
{
    private Rect _windowRect = new(x: 50, y: 50, width: 280, height: 700);
    private bool _isMainWindowOpen = false;

    // Cache for performance
    private float _cachedRewardMultiplier = 0f;
    private float _cachedChargeMultiplier = 0f;
    private readonly float _cacheUpdateInterval = 2f;
    private float _nextCacheUpdate = 0f;

    private bool _isInGame = false;
    private EMap _map;

    public SettingsDisplayWindow(IntPtr ptr) : base(ptr) { }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        UpdateCache(); // Initialize cache
        Main.Logger.LogInfo($"[{nameof(SettingsDisplayWindow)}] SettingsDisplayWindow initialized.");
        SceneManager.sceneLoaded += new Action<Scene, LoadSceneMode>(SceneManager_sceneLoaded);
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu" || scene.name.Contains("Load", StringComparison.OrdinalIgnoreCase))
        {
            _isInGame = false;
            _map = EMap.None;
            return;
        }

        _isInGame = true;
        _map = MapController.currentMap.eMap;
    }

    public void SetMainWindowState(bool isOpen)
    {
        _isMainWindowOpen = isOpen;

        // Update cache immediately when opening main window
        if (isOpen)
        {
            UpdateCache();
        }
    }

    private void Update()
    {
        // Only update cache if display is visible and interval has passed
        if (ConfigService.UI.ShowSettingsDisplay && Time.unscaledTime >= _nextCacheUpdate)
        {
            UpdateCache();
            _nextCacheUpdate = Time.unscaledTime + _cacheUpdateInterval;
        }
    }

    private void UpdateCache()
    {
        _cachedRewardMultiplier = ChargeShrineService.GetTotalRewardMultiplier();
        _cachedChargeMultiplier = ChargeShrineService.GetTotalChargeMultiplier();
    }

    private void OnGUI()
    {
        Styles.Initialize();

        if (!ConfigService.UI.ShowSettingsDisplay)
            return;

        if (_isMainWindowOpen)
        {
            _windowRect = GUI.Window(234433, _windowRect, new Action<int>(DrawWindow), "Settings Display", Styles.Window);
        }
        else if (_isInGame)
        {
            DrawTextOnly();
        }
    }

    private void DrawWindow(int windowID)
    {
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal(Styles.DragHandle);
        GUILayout.Label("═══ Drag to Move ═══", Styles.DragLabel);
        GUILayout.EndHorizontal();
        GUI.DragWindow(new Rect(0, 0, 10000, 40));

        GUILayout.Label("", GUILayout.Height(8));
        DrawSettingsContent();
        GUILayout.EndVertical();
    }

    private void DrawTextOnly()
    {
        var textRect = new Rect(_windowRect.x, _windowRect.y, _windowRect.width, _windowRect.height);
        GUILayout.BeginArea(textRect);

        GUILayout.BeginVertical(Styles.TransparentOverlay);
        DrawSettingsContent();
        GUILayout.EndVertical();

        GUILayout.EndArea();
    }

    private void DrawSettingsContent()
    {
        // Core Settings
        GUILayout.Label("=== CORE ===", Styles.LabelBold);
        GUILayout.Label($"Mod: {GetEnabledText(ConfigService.Core.IsEnabled)}", Styles.Label);
        GUILayout.Label($"Challenges: {GetEnabledText(ConfigService.Core.IsEnabledDuringChallenges)}", Styles.Label);
        GUILayout.Label("", GUILayout.Height(8));

        // Map Modifiers
        GUILayout.Label("=== MAP MODIFIERS ===", Styles.LabelBold);

        switch (_map)
        {
            case EMap.Forest:
                DrawMapInfo("Forest", ConfigService.Forest);
                break;
            case EMap.Desert:
                DrawMapInfo("Desert", ConfigService.Desert);
                break;
            case EMap.Graveyard:
                DrawMapInfo("Graveyard", ConfigService.Graveyard);
                break;
        }

        GUILayout.Label("", GUILayout.Height(8));

        // Shrine Settings
        GUILayout.Label("=== SHRINE ===", Styles.LabelBold);
        if (ConfigService.ChargeShrine.IsRewardMultiplierEnabled)
        {
            GUILayout.Label($"Reward: {ConfigService.ChargeShrine.RewardMultiplier:F2}x", Styles.Label);
            GUILayout.Label($"  Freq: {ConfigService.ChargeShrine.RewardMultiplierFrequency}", Styles.Label);
            GUILayout.Label($"  Total: {_cachedRewardMultiplier:F2}x", Styles.Label);
        }
        else
        {
            GUILayout.Label("Reward: Disabled", Styles.Label);
        }

        if (ConfigService.ChargeShrine.IsChargeRateMultiplierEnabled)
        {
            GUILayout.Label($"Charge Rate: {ConfigService.ChargeShrine.ChargeRateMultiplier:F2}x", Styles.Label);
            GUILayout.Label($"  Freq: {ConfigService.ChargeShrine.ChargeRateMultiplierFrequency}", Styles.Label);
            GUILayout.Label($"  Total: {_cachedChargeMultiplier:F2}x", Styles.Label);
        }
        else
        {
            GUILayout.Label("Charge Rate: Disabled", Styles.Label);
        }
        GUILayout.Label("", GUILayout.Height(8));

        // Enemy Modifiers
        GUILayout.Label("=== ENEMIES ===", Styles.LabelBold);

        // Drop Chance
        if (ConfigService.EnemyModifiers.IsEnemyDropChanceModifierEnabled)
        {
            GUILayout.Label($"Drop: {ConfigService.EnemyModifiers.EnemyChestDropChance * 100:F4}%", Styles.Label);
        }
        else
        {
            GUILayout.Label("Drop: Disabled", Styles.Label);
        }

        // Spawn Preset
        if (ConfigService.EnemyModifiers.IsEnemySpawnModifiersEnabled)
        {
            var preset = ConfigService.EnemyModifiers.GetCurrentSpawnModifiers();
            GUILayout.Label($"Preset: {preset.Preset}", Styles.Label);
            GUILayout.Label($"  Target: {preset.TargetSpawnsMultiplier:F1}x", Styles.Label);
            GUILayout.Label($"  Credit: {preset.CreditMultiplier:F1}x", Styles.Label);
            GUILayout.Label($"  Interval: {preset.SpawnIntervalMultiplier:F1}x", Styles.Label);
        }
        else
        {
            GUILayout.Label("Spawns: Disabled", Styles.Label);
        }
    }

    private static void DrawMapInfo(string mapName, Configs.MapConfigs.MapConfigBase config)
    {
        if (config.IsEnabled)
        {
            GUILayout.Label($"{mapName}:", Styles.Label);
            GUILayout.Label($"  Duration: {config.StageDurationSeconds:F0}s", Styles.Label);
            GUILayout.Label($"  Chest: {config.ChestSpawnMultiplier:F2}x", Styles.Label);
            GUILayout.Label($"  Shrine: {config.ShrineOnlySpawnMultiplier:F2}x", Styles.Label);
            GUILayout.Label($"  S&P: {config.ShrineAndPotSpawnMultiplier:F2}x", Styles.Label);
        }
        else
        {
            GUILayout.Label($"{mapName}: Disabled", Styles.Label);
        }
    }

    private static string GetEnabledText(bool isEnabled)
    {
        return isEnabled ? "Enabled" : "Disabled";
    }
}