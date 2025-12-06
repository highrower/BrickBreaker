using UnityEngine;

public class BrickSpawner : MonoBehaviour {

	[SerializeField] private GameObject    brickPrefab;
	[SerializeField] private float         margin   = 0.5f;
	[SerializeField] private int           gridSize = 6;
	[SerializeField] private BrickSettings brickType;

	[SerializeField] private RectReference brickBounds;

	private Brick[,] _brickGrid;

	private void Start() {
		_brickGrid = new Brick[gridSize, gridSize];
		SpawnBricks();
		brickBounds.variable.OnValueChanged += UpdateBrickPositions;
		UpdateBrickPositions(brickBounds.Value);
	}

	private void OnDestroy() { brickBounds.variable.OnValueChanged -= UpdateBrickPositions; }

	private void SpawnBricks() {
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

	private void UpdateBrickPositions(Rect bounds) {
		if (brickBounds.Value.width <= 0 || brickBounds.Value.height <= 0) return;
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