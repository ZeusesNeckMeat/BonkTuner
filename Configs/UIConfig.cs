using BepInEx.Configuration;

using UnityEngine;

namespace BonkTuner.Configs;

internal class UIConfig
{
    private UIConfig() { }

    private static UIConfig _instance;
    public static UIConfig Instance => _instance ??= new UIConfig();

    private ConfigEntry<KeyCode> _toggleKey;
    private ConfigEntry<bool> _startOpen;

    public KeyCode ToggleKey => _toggleKey.Value;
    public bool StartOpen => _startOpen.Value;

    public void SetToggleKey(KeyCode key) => _toggleKey.Value = key;
    public void DisableStartOpen() => _startOpen.Value = false;
    public void EnableStartOpen() => _startOpen.Value = true;

    public void Init(ConfigFile config)
    {
        _toggleKey = config.Bind(
            "UI",
            "Toggle Key",
            KeyCode.F5,
            "Key to toggle the configuration UI window.");

        _startOpen = config.Bind(
            "UI",
            "Start Open",
            false,
            "Whether the UI should be open when the game starts.");
    }
}