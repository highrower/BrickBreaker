using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour {

	[Header("Settings")]
	[SerializeField] private float margin;

	[SerializeField] private GameObject brickPrefab;

	[Header("Grid Config")]
	[SerializeField]
	private List<BrickRow> grid;

	private readonly List<Brick> _spawnedBricks  = new List<Brick>();
	private          bool        _isLevelSpawned = false;

	[System.Serializable]
	public struct BrickRow {
		public List<int> columns;
	}

	private void Start() {
		if (PlayArea.Instance == null)
			return;

		PlayArea.Instance.OnBoundsChanged += RefreshLevelPositioning;
		if (PlayArea.Instance.WorldBounds.width > 0)
			RefreshLevelPositioning();
	}

	private void OnDestroy() {
		if (PlayArea.Instance != null)
			PlayArea.Instance.OnBoundsChanged -= RefreshLevelPositioning;
	}

	private void RefreshLevelPositioning() {
		var bounds = PlayArea.Instance.WorldBounds;
		if (bounds.width <= 0 || bounds.height <= 0) return;

		if (!_isLevelSpawned) {
			SpawnBricks();
			_isLevelSpawned = true;
		}

		UpdateBrickPositions(bounds);
	}

	private void SpawnBricks() {
		_spawnedBricks.Clear();

		for (var y = 0; y < grid.Count; y++) {
			for (var x = 0; x < grid[y].columns.Count; x++) {
				var brickHealth = grid[y].columns[x];
				if (brickHealth <= 0) continue;

				var newBrick = Instantiate(brickPrefab, Vector3.zero, Quaternion.identity);
				newBrick.transform.SetParent(transform);

				var brickScript = newBrick.GetComponent<Brick>();
				brickScript.Initialize(brickHealth, x, y);
				_spawnedBricks.Add(brickScript);
			}
		}
	}

	private void UpdateBrickPositions(Rect bounds) {
		if (grid.Count == 0) return;

		var leftBound  = bounds.xMin + margin;
		var rightBound = bounds.xMax - margin;
		var upperBound = bounds.yMax - margin;

		var gridWidth = grid[0].columns.Count - 1;
		if (gridWidth <= 0)
			gridWidth = 1;

		var spacePerBrick = (rightBound - leftBound) / gridWidth;

		foreach (var brick in _spawnedBricks) {
			if (brick == null) continue;
			var brickXPos = leftBound  + brick.GridX * spacePerBrick;
			var brickYPos = upperBound - brick.GridY * spacePerBrick;
			brick.transform.position = new Vector2(brickXPos, brickYPos);
			brick.GridX              = brickXPos;
			brick.GridY              = brickYPos;
		}
	}
}