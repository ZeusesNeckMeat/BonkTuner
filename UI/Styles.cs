using UnityEngine;

namespace BonkTuner.UI;
internal class Styles
{
    public static Texture2D BackgroundTexture { get; private set; }
    public static Texture2D BoxTexture { get; private set; }
    public static Texture2D ButtonActiveTexture { get; private set; }
    public static Texture2D HeaderActiveTexture { get; private set; }
    public static Texture2D HeaderInactiveTexture { get; private set; }
    public static Texture2D DragHandleTexture { get; private set; }
    public static Texture2D SectionLabelTexture { get; private set; }
    public static Texture2D SliderBoxTexture { get; private set; }
    public static Texture2D SliderTrackTexture { get; private set; }
    public static Texture2D SliderThumbTexture { get; private set; }

    public static GUIStyle Window { get; private set; }
    public static GUIStyle Header { get; private set; }
    public static GUIStyle HeaderActive { get; private set; }
    public static GUIStyle HeaderInactive { get; private set; }
    public static GUIStyle Label { get; private set; }
    public static GUIStyle LabelBold { get; private set; }
    public static GUIStyle SectionLabel { get; private set; }
    public static GUIStyle Toggle { get; private set; }
    public static GUIStyle Button { get; private set; }
    public static GUIStyle ButtonActive { get; private set; }
    public static GUIStyle SmallButton { get; private set; }
    public static GUIStyle Slider { get; private set; }
    public static GUIStyle SliderThumb { get; private set; }
    public static GUIStyle Box { get; private set; }
    public static GUIStyle DragHandle { get; private set; }
    public static GUIStyle DragLabel { get; private set; }
    public static GUIStyle SliderBox { get; private set; }

    private static bool _isInitialized = false;

    public static void Initialize()
    {
        if (_isInitialized)
            return;

        // Background texture for window
        BackgroundTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        BackgroundTexture.SetPixel(0, 0, new Color(0.05f, 0.05f, 0.06f, 0.95f));
        BackgroundTexture.Apply();
        BackgroundTexture.hideFlags = HideFlags.HideAndDontSave;

        // Box texture for info panels
        BoxTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        BoxTexture.SetPixel(0, 0, new Color(0.1f, 0.1f, 0.12f, 0.8f));
        BoxTexture.Apply();
        BoxTexture.hideFlags = HideFlags.HideAndDontSave;

        // Slider box texture - distinct background for slider controls
        SliderBoxTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        SliderBoxTexture.SetPixel(0, 0, new Color(0.12f, 0.14f, 0.16f, 0.7f)); // Slightly lighter than Box
        SliderBoxTexture.Apply();
        SliderBoxTexture.hideFlags = HideFlags.HideAndDontSave;

        // Slider track texture - taller slider bar
        SliderTrackTexture = new Texture2D(1, 8, TextureFormat.RGBA32, false); // 8px tall
        for (var i = 0; i < 8; i++)
        {
            SliderTrackTexture.SetPixel(0, i, new Color(0.2f, 0.2f, 0.22f, 1f)); // Darker track
        }
        SliderTrackTexture.Apply();
        SliderTrackTexture.hideFlags = HideFlags.HideAndDontSave;

        // Slider thumb texture - larger thumb
        SliderThumbTexture = new Texture2D(12, 18, TextureFormat.RGBA32, false); // 12px wide, 18px tall
        for (var x = 0; x < 12; x++)
        {
            for (var y = 0; y < 18; y++)
            {
                // Create a rounded rectangle effect
                var isEdge = x == 0 || x == 11 || y == 0 || y == 17;
                var color = isEdge
                    ? new Color(0.5f, 0.7f, 0.5f, 1f) // Lighter green border
                    : new Color(0.3f, 0.6f, 0.3f, 1f); // Green fill
                SliderThumbTexture.SetPixel(x, y, color);
            }
        }
        SliderThumbTexture.Apply();
        SliderThumbTexture.hideFlags = HideFlags.HideAndDontSave;

        // SliderBox style - container for slider controls
        SliderBox = new GUIStyle(GUI.skin.box);
        SliderBox.normal.background = SliderBoxTexture;
        SliderBox.padding = new RectOffset
        {
            left = 10,
            right = 10,
            top = 8,
            bottom = 8
        };
        SliderBox.margin = new RectOffset
        {
            left = 0,
            right = 0,
            top = 4,
            bottom = 4
        };

        // Active button texture
        ButtonActiveTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        ButtonActiveTexture.SetPixel(0, 0, new Color(0.2f, 0.5f, 0.2f, 1f)); // Green tint
        ButtonActiveTexture.Apply();
        ButtonActiveTexture.hideFlags = HideFlags.HideAndDontSave;

        // Header active texture (enabled state)
        HeaderActiveTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        HeaderActiveTexture.SetPixel(0, 0, new Color(0.15f, 0.35f, 0.15f, 0.6f)); // Subtle green
        HeaderActiveTexture.Apply();
        HeaderActiveTexture.hideFlags = HideFlags.HideAndDontSave;

        // Header inactive texture (disabled state)
        HeaderInactiveTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        HeaderInactiveTexture.SetPixel(0, 0, new Color(0.25f, 0.1f, 0.1f, 0.4f)); // Subtle red
        HeaderInactiveTexture.Apply();
        HeaderInactiveTexture.hideFlags = HideFlags.HideAndDontSave;

        // Drag handle texture (distinct color to indicate draggable area)
        DragHandleTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        DragHandleTexture.SetPixel(0, 0, new Color(0.08f, 0.12f, 0.15f, 0.9f)); // Slightly lighter blue-grey
        DragHandleTexture.Apply();
        DragHandleTexture.hideFlags = HideFlags.HideAndDontSave;

        // Section label texture - subtle background for labels
        SectionLabelTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        SectionLabelTexture.SetPixel(0, 0, new Color(0.12f, 0.15f, 0.18f, 0.5f)); // Subtle blue-grey background
        SectionLabelTexture.Apply();
        SectionLabelTexture.hideFlags = HideFlags.HideAndDontSave;

        // Window style
        var windowPadding = new RectOffset
        {
            left = 12,
            right = 12,
            top = 28,
            bottom = 12
        };

        Window = new GUIStyle(GUI.skin.window);
        Window.normal.background = BackgroundTexture;
        Window.onNormal.background = BackgroundTexture;
        Window.active.background = BackgroundTexture;
        Window.focused.background = BackgroundTexture;
        Window.onFocused.background = BackgroundTexture;
        Window.normal.textColor = new Color(r: 1f, 1f, 0.9f);
        Window.fontSize = 19;
        Window.fontStyle = FontStyle.Bold;
        Window.padding = windowPadding;

        // DragHandle style - visual indicator for draggable area
        DragHandle = new GUIStyle(GUI.skin.box);
        DragHandle.normal.background = DragHandleTexture;
        DragHandle.alignment = TextAnchor.MiddleCenter;
        DragHandle.padding = new RectOffset
        {
            left = 4,
            right = 4,
            top = 6,
            bottom = 6
        };
        DragHandle.margin = new RectOffset
        {
            left = 0,
            right = 0,
            top = 0,
            bottom = 4
        };

        // DragLabel style - text shown in drag handle
        DragLabel = new GUIStyle(GUI.skin.label)
        {
            fontSize = 12,
            fontStyle = FontStyle.Italic,
            alignment = TextAnchor.MiddleCenter
        };
        DragLabel.normal.textColor = new Color(0.7f, 0.75f, 0.8f); // Subtle blue-grey text

        // Header style - for section headers WITH toggles
        Header = new GUIStyle(GUI.skin.label)
        {
            fontSize = 17,
            fontStyle = FontStyle.Bold
        };
        Header.normal.textColor = new Color(1f, 0.95f, 0.7f);
        Header.onNormal.textColor = new Color(1f, 0.95f, 0.7f);
        Header.alignment = TextAnchor.MiddleLeft;

        // HeaderActive style - clickable header when enabled
        HeaderActive = new GUIStyle(GUI.skin.button)
        {
            fontSize = 17,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleLeft
        };
        HeaderActive.normal.background = HeaderActiveTexture;
        HeaderActive.hover.background = HeaderActiveTexture;
        HeaderActive.active.background = HeaderActiveTexture;
        HeaderActive.normal.textColor = new Color(0.7f, 1f, 0.7f); // Bright green
        HeaderActive.hover.textColor = new Color(0.85f, 1f, 0.85f);
        HeaderActive.active.textColor = Color.white;
        HeaderActive.padding = new RectOffset
        {
            left = 8,
            right = 8,
            top = 6,
            bottom = 6
        };

        // HeaderInactive style - clickable header when disabled
        HeaderInactive = new GUIStyle(GUI.skin.button)
        {
            fontSize = 17,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleLeft
        };
        HeaderInactive.normal.background = HeaderInactiveTexture;
        HeaderInactive.hover.background = HeaderInactiveTexture;
        HeaderInactive.active.background = HeaderInactiveTexture;
        HeaderInactive.normal.textColor = new Color(1f, 0.6f, 0.6f); // Light red
        HeaderInactive.hover.textColor = new Color(1f, 0.75f, 0.75f);
        HeaderInactive.active.textColor = Color.white;
        HeaderInactive.padding = new RectOffset
        {
            left = 8,
            right = 8,
            top = 6,
            bottom = 6
        };

        // Label style - for regular labels WITHOUT toggles
        Label = new GUIStyle(GUI.skin.label)
        {
            fontSize = 14,
            alignment = TextAnchor.MiddleLeft
        };
        Label.normal.textColor = new Color(0.95f, 0.95f, 0.95f);

        // LabelBold style - for emphasized text
        LabelBold = new GUIStyle(GUI.skin.label)
        {
            fontSize = 14,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleLeft
        };
        LabelBold.normal.textColor = new Color(1f, 0.95f, 0.7f);

        // SectionLabel style - for labels like "Preset:" and "Frequency:" that precede button groups
        SectionLabel = new GUIStyle(GUI.skin.box)
        {
            fontSize = 16,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };
        SectionLabel.normal.background = SectionLabelTexture;
        SectionLabel.normal.textColor = new Color(0.85f, 0.9f, 1f); // Light blue tint
        SectionLabel.padding = new RectOffset
        {
            left = 8,
            right = 8,
            top = 4,
            bottom = 4
        };
        SectionLabel.margin = new RectOffset
        {
            left = 4,
            right = 4,
            top = 0,
            bottom = 0
        };

        // Toggle style with bright white text
        Toggle = new GUIStyle(GUI.skin.toggle)
        {
            fontSize = 13
        };
        Toggle.normal.textColor = new Color(0.95f, 0.95f, 0.95f);
        Toggle.onNormal.textColor = new Color(1f, 1f, 0.85f);

        // Button style with bright text
        Button = new GUIStyle(GUI.skin.button)
        {
            fontSize = 13,
            fontStyle = FontStyle.Bold
        };
        Button.normal.textColor = new Color(0.95f, 0.95f, 0.95f);
        Button.hover.textColor = new Color(0.5f, 1f, 0.5f);
        Button.active.textColor = Color.white;

        // ButtonActive style - for selected/active buttons
        ButtonActive = new GUIStyle(GUI.skin.button)
        {
            fontSize = 13,
            fontStyle = FontStyle.Bold
        };
        ButtonActive.normal.background = ButtonActiveTexture;
        ButtonActive.hover.background = ButtonActiveTexture;
        ButtonActive.active.background = ButtonActiveTexture;
        ButtonActive.normal.textColor = new Color(0.7f, 1f, 0.7f); // Bright green
        ButtonActive.hover.textColor = Color.white;
        ButtonActive.active.textColor = Color.white;

        // Small button style for +/- and reset buttons
        SmallButton = new GUIStyle(GUI.skin.button)
        {
            fontSize = 11,
            fontStyle = FontStyle.Bold
        };
        SmallButton.normal.textColor = new Color(0.9f, 0.9f, 0.9f);
        SmallButton.hover.textColor = new Color(0.5f, 1f, 0.5f);
        SmallButton.active.textColor = Color.white;

        // Box style - for info panels/containers
        Box = new GUIStyle(GUI.skin.box);
        Box.normal.background = BoxTexture;
        Box.padding = new RectOffset
        {
            left = 8,
            right = 8,
            top = 8,
            bottom = 8
        };
        Box.margin = new RectOffset
        {
            left = 4,
            right = 4,
            top = 4,
            bottom = 4
        };

        Slider = new GUIStyle(GUI.skin.horizontalSlider);
        Slider.normal.background = SliderTrackTexture;
        Slider.fixedHeight = 8;
        Slider.padding = new RectOffset
        {
            left = 0,
            right = 0,
            top = 0,
            bottom = 0
        };
        Slider.margin = new RectOffset
        {
            left = 0,
            right = 0,
            top = 12,
            bottom = 0
        };
        Slider.alignment = TextAnchor.MiddleCenter;

        SliderThumb = new GUIStyle(GUI.skin.horizontalSliderThumb);
        SliderThumb.normal.background = SliderThumbTexture;
        SliderThumb.fixedWidth = 12;
        SliderThumb.fixedHeight = 18;
        SliderThumb.padding = new RectOffset
        {
            left = 0,
            right = 0,
            top = 0,
            bottom = 0
        };
        SliderThumb.overflow = new RectOffset
        {
            left = 0,
            right = 0,
            top = 9,
            bottom = 0
        };

        _isInitialized = true;
    }
}