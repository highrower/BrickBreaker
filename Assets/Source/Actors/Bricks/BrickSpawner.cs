using UnityEngine;

public class BrickSpawner : MonoBehaviour {

	[SerializeField] GameObject    brickPrefab;
	[SerializeField] int           gridSize = 6;
	[SerializeField] BrickSettings brickType;

	[SerializeField, Range(0.1f, 1f)] float heightRatio = 0.5f;
	[SerializeField, Range(0.8f, 1f)] float padding     = 0.95f;

	[SerializeField] RectReference brickBounds;

	Brick[,] _brickGrid;


	void Start() {
		var tempRenderer = brickPrefab.GetComponent<SpriteRenderer>();

		_brickGrid = new Brick[gridSize, gridSize];
		SpawnBricks();
		brickBounds.variable.OnValueChanged += UpdateBrickPositions;
		UpdateBrickPositions(brickBounds.Value);
	}

	void OnDestroy() { brickBounds.variable.OnValueChanged -= UpdateBrickPositions; }

	void SpawnBricks() {
		for (var y = 0; y < gridSize; y++) {
			for (var x = 0; x < gridSize; x++) {
				if (_brickGrid[x, y]) continue;

				var newBrick = Instantiate(brickPrefab, Vector3.zero, Quaternion.identity);
				newBrick.transform.SetParent(transform);
				var brickScript = newBrick.GetComponent<Brick>();
				brickScript.Initialize(brickType, x, y);
				_brickGrid[x, y] = brickScript;
			}
		}
	}

	void UpdateBrickPositions(Rect bounds) {
		if (bounds.width <= 0 || bounds.height <= 0) return;

		var leftBound  = bounds.xMin;
		var upperBound = bounds.yMax;

		var cellWidth  = (bounds.xMax - leftBound) / gridSize;
		var cellHeight = cellWidth                 * heightRatio;

		var targetScaleX = (cellWidth  * padding);
		var targetScaleY = (cellHeight * padding);
		var finalScale   = new Vector3(targetScaleX, targetScaleY, 1f);

		foreach (var brick in _brickGrid) {
			if (brick == null) continue;

			var brickXPos = leftBound  + (brick.GridX * cellWidth)  + (cellWidth  / 2f);
			var brickYPos = upperBound - (brick.GridY * cellHeight) - (cellHeight / 2f);

			brick.transform.position = new Vector2(brickXPos, brickYPos);
			brick.SetScale(finalScale);
		}
	}

}