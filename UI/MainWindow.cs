using BonkTuner.Services;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace BonkTuner.UI;

internal class MainWindow : MonoBehaviour
{
    private static bool _isGuiOpen = false;

    private Rect _windowRect = new(x: 500, y: 250, width: 650, height: 750);
    private Vector2 _scrollPosition = Vector2.zero;

    private int _selectedTab = 0;
    private readonly string[] _tabNames = new[]
    {
        CoreSettingsTab.TabName,
        MapModifiersTab.TabName,
        ShrineSettingsTab.TabName,
        EnemyModifiersTab.TabName
    };

    private readonly Dictionary<int, Action> _tabDrawActions = new()
    {
        { 0, CoreSettingsTab.Draw },
        { 1, MapModifiersTab.Draw },
        { 2, ShrineSettingsTab.Draw },
        { 3, EnemyModifiersTab.Draw }
    };

    public MainWindow(IntPtr ptr) : base(ptr) { }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        _isGuiOpen = ConfigService.UI.StartOpen;

        Main.Logger.LogInfo($"[{nameof(MainWindow)}] MainWindow initialized and set to not destroy on load.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(ConfigService.UI.ToggleKey))
        {
            _isGuiOpen = !_isGuiOpen;
            Main.Logger.LogInfo($"[{nameof(MainWindow)}] UI toggled: {_isGuiOpen}");
        }
    }

    private void OnGUI()
    {
        Styles.Initialize();

        if (!_isGuiOpen)
            return;

        _windowRect = GUI.Window(234432, _windowRect, new Action<int>(DrawWindow), "BonkTuner Configuration", Styles.Window);
    }

    private void DrawWindow(int windowID)
    {
        GUILayout.BeginVertical();

        // Draggable header area with visual indicator
        GUILayout.BeginHorizontal(Styles.DragHandle);
        GUILayout.Label("═══ Drag to Move ═══", Styles.DragLabel);
        GUILayout.EndHorizontal();

        GUI.DragWindow(new Rect(0, 0, 10000, 40));

        // Tab selection with ButtonActive for selected tab
        GUILayout.BeginHorizontal();
        for (var i = 0; i < _tabNames.Length; i++)
        {
            var isSelected = _selectedTab == i;
            var buttonStyle = isSelected ? Styles.ButtonActive : Styles.Button;

            if (GUILayout.Button(_tabNames[i], buttonStyle))
            {
                _selectedTab = i;
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.Label("", GUILayout.Height(12));

        // Scrollable content area
        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Height(600));
        if (_tabDrawActions.TryGetValue(_selectedTab, out var drawAction))
            drawAction();

        GUILayout.EndScrollView();

        // Close button with spacer
        GUILayout.Label("", GUILayout.Height(12));
        if (GUILayout.Button("Close", Styles.Button))
        {
            _isGuiOpen = false;
        }

        GUILayout.EndVertical();
    }
}