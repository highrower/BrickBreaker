using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class BrickManager : MonoBehaviour {

	[SerializeField] private GameObject      brickPrefab;
	[SerializeField] private float           margin    = 0.5f;
	[SerializeField] private int             gridSize  = 6;
	[SerializeField] private Brick.BrickType brickType = Brick.BrickType.Dirt;

	[SerializeField] private float         spawnChance        = 0.5f;
	[SerializeField] private float         spawnRatePerSecond = 0.5f;
	[SerializeField] private RectReference brickBounds;

	private Brick[,] _brickGrid;
	private bool     _isLevelSpawned = false;

	private void Start() {
		_brickGrid                          =  new Brick[gridSize, gridSize];
		brickBounds.variable.OnValueChanged += RefreshLevelPositioning;
		RefreshLevelPositioning(brickBounds.Value);
		StartCoroutine(SpawnRoutine());
	}

	private void OnDestroy() {
		brickBounds.variable.OnValueChanged -= RefreshLevelPositioning;
	}

	private IEnumerator SpawnRoutine() {
		var waitTime = 1f / spawnRatePerSecond;
		var wait     = new WaitForSeconds(waitTime);
		while (true) {
			SpawnBricks();
			UpdateBrickPositions(brickBounds.Value); // TODO: make it only on new bounds
			yield return wait;
		}
	}

	private void SpawnBricks() {
		for (var y = 0; y < gridSize; y++) {
			for (var x = 0; x < gridSize; x++) {
				if (_brickGrid[x, y]) continue;
				if (Random.value > spawnChance) continue;

				var newBrick = Instantiate(brickPrefab, Vector3.zero, Quaternion.identity);
				newBrick.transform.SetParent(transform);
				var brickScript = newBrick.GetComponent<Brick>();
				brickScript.Initialize(brickType, x, y);
				_brickGrid[x, y] = brickScript;
			}
		}
	}

	private void RefreshLevelPositioning(Rect bounds) {
		if (brickBounds.Value.width <= 0 || brickBounds.Value.height <= 0) return;

		if (!_isLevelSpawned) {
			SpawnBricks();
			_isLevelSpawned = true;
		}

		UpdateBrickPositions(brickBounds.Value);
	}

	private void UpdateBrickPositions(Rect bounds) {
		var leftBound  = bounds.xMin;
		var upperBound = bounds.yMax;

		var spacePerBrick = (bounds.xMax - leftBound) / gridSize;
		var halfStep      = spacePerBrick             / 2f;

		foreach (var brick in _brickGrid) {
			if (brick == null) continue;
			var brickXPos = leftBound  + brick.GridX * spacePerBrick + halfStep;
			var brickYPos = upperBound - brick.GridY * spacePerBrick - halfStep;
			brick.transform.position = new Vector2(brickXPos, brickYPos);
		}
	}

}