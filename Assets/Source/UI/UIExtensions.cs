using UnityEngine;
using UnityEngine.UIElements;

public class UIExtensions {
	static readonly Vector2 ScreenSample = new Vector2(0, 100f);

	public static float PanelToScreenPixels(VisualElement element) {
		var panel      = element.panel;
		var panelUnits = element.resolvedStyle.height;

		if (panel == null) return panelUnits;

		var panelZero   = RuntimePanelUtils.ScreenToPanel(panel, Vector2.zero);
		var panelSample = RuntimePanelUtils.ScreenToPanel(panel, ScreenSample);

		var panelDistance = Mathf.Abs(panelSample.y - panelZero.y);

		if (Mathf.Approximately(panelDistance, 0)) return 0;

		var pixelsPerUnit = ScreenSample.y / panelDistance;
		return panelUnits * pixelsPerUnit;
	}
}