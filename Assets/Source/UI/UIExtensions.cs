using UnityEngine;
using UnityEngine.UIElements;

public static class UIExtensions
{
    static readonly Vector2 ScreenSample = new(0f, 100f);
    const float Epsilon = 0.0001f;

    static bool IsFinitePositive(float v) => v > 0f && !float.IsNaN(v) && !float.IsInfinity(v);

    public static bool TryPanelToScreenPixels(VisualElement element, out float screenPixels)
    {
        screenPixels = 0f;

        var panel = element?.panel;
        if (panel == null) return false;

        var panelUnits = element.resolvedStyle.height;
        if (!IsFinitePositive(panelUnits)) return false;

        var p0 = RuntimePanelUtils.ScreenToPanel(panel, Vector2.zero);
        var p1 = RuntimePanelUtils.ScreenToPanel(panel, ScreenSample);

        var panelDistance = Mathf.Abs(p1.y - p0.y);

        if (float.IsNaN(panelDistance) || float.IsInfinity(panelDistance) || panelDistance <= Epsilon)
            return false;

        var pixelsPerUnit = ScreenSample.y / panelDistance;
        var result = panelUnits * pixelsPerUnit;
        
        if (!IsFinitePositive(pixelsPerUnit) || !IsFinitePositive(result)) return false;

        screenPixels = result;
        return true;
    }
}