using System;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

[UxmlElement]
partial class UpgradeGraph : VisualElement
{
	Dictionary<ShopUpgrade, VisualElement> nodeMap     = new();
	List<ShopUpgrade>                      allUpgrades = new();

	public UpgradeGraph() => generateVisualContent += OnGenerateVisualContent;

	public void Initialize(List<ShopUpgrade>                  upgrades,
						   VisualTreeAsset                    nodeTemplate,
						   Action<VisualElement, ShopUpgrade> onNodeCreated)
	{
		allUpgrades = upgrades;
		nodeMap.Clear();
		Clear();

		foreach (var upgrade in upgrades)
		{
			var nodeInstance = nodeTemplate.Instantiate();
			var root         = nodeInstance.contentContainer;

			root.style.position = Position.Absolute;
			root.style.left     = upgrade.gridPosition.x * 150;
			root.style.top      = upgrade.gridPosition.y * 150;
			root.userData       = upgrade;

			Add(root);
			nodeMap[upgrade] = root;

			onNodeCreated?.Invoke(root, upgrade);
		}

		MarkDirtyRepaint();
	}

	void OnGenerateVisualContent(MeshGenerationContext context)
	{
		var painter = context.painter2D;
		painter.lineWidth   = 4f;
		painter.strokeColor = Color.gray;

		foreach (var upgrade in allUpgrades)
		{
			if (!nodeMap.TryGetValue(upgrade, out var targetNode)) continue;

			foreach (var req in upgrade.prereqs)
				DrawLine(targetNode, req, painter);
		}
	}

	void DrawLine(VisualElement end, ShopUpgrade start, Painter2D painter)
	{
		if (!nodeMap.TryGetValue(start, out var source)) return;

		painter.BeginPath();
		painter.MoveTo(source.layout.center + new Vector2(source.layout.x, source.layout.y));
		painter.LineTo(end.layout.center    + new Vector2(end.layout.x,    end.layout.y));
		painter.Stroke();
	}
}