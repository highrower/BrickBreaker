using UnityEngine;
using UnityEngine.UIElements;

public static class UIExtensions
{
    static readonly Vector2 ScreenSample = new(0f, 100f);
    const float Epsilon = 0.0001f;

    static bool IsFinite(float v) => !float.IsNaN(v) && !float.IsInfinity(v);
    static bool IsFiniteGt(float v, float min) => IsFinite(v) && v > min;

    public static bool TryPanelToScreenPixels(VisualElement element, out float screenPixels)
    {
        screenPixels = 0f;

        var panel = element?.panel;
        if (panel == null) return false;

        var panelUnits = element.resolvedStyle.height;
        if (!IsFiniteGt(panelUnits, 0f)) return false;

        var p0 = RuntimePanelUtils.ScreenToPanel(panel, Vector2.zero);
        var p1 = RuntimePanelUtils.ScreenToPanel(panel, ScreenSample);

        var panelDistance = Mathf.Abs(p1.y - p0.y);
        if (!IsFiniteGt(panelDistance, Epsilon)) return false;

        screenPixels = panelUnits * (ScreenSample.y / panelDistance);
        return IsFiniteGt(screenPixels, 0f);
    }
}