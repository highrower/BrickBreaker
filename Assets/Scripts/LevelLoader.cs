using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour {

	[Header("Settings")]
	[SerializeField] float margin;

	[SerializeField] GameObject brickPrefab;

	[Header("Grid Config")]
	[SerializeField] List<BrickRow> grid;

	[SerializeField] GameObject leftWall;
	[SerializeField] GameObject rightWall;
	[SerializeField] GameObject upperWall;

	[System.Serializable]
	public struct BrickRow {
		public List<int> columns;
	}

	void Start() { GenerateLevel(); }

	void GenerateLevel() {
		var leftBound  = leftWall.GetComponent<SpriteRenderer>().bounds.max.x  + margin;
		var rightBound = rightWall.GetComponent<SpriteRenderer>().bounds.min.x - margin;
		var upperBound = upperWall.GetComponent<SpriteRenderer>().bounds.min.y - margin;

		if (grid.Count == 0) return;


		var gridWidth                 = grid[0].columns.Count - 1;
		if (gridWidth <= 0) gridWidth = 1;

		var spacePerBrick = (rightBound - leftBound) / gridWidth;

		for (var y = 0; y < grid.Count; y++) {
			for (var x = 0; x < grid[y].columns.Count; x++) {
				int brickHealth = grid[y].columns[x];

				if (brickHealth <= 0) continue;

				var brickXPos = x * spacePerBrick + leftBound;
				var brickYPos = upperBound        - (spacePerBrick * y);
				var spawnPos  = new Vector2(brickXPos, brickYPos);

				GameObject newBrick = Instantiate(brickPrefab, spawnPos, Quaternion.identity);
				newBrick.transform.SetParent(transform);

				Brick brickScript = newBrick.GetComponent<Brick>();
				brickScript.Initialize(brickHealth);
			}
		}
	}
}