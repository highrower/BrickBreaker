using System.Collections;
using UnityEngine;

public class BrickSpawner : MonoBehaviour
{
	[SerializeField] GameObject    brickPrefab;
	[SerializeField] int           gridSize = 6;
	[SerializeField] BrickSettings brickType;

	[SerializeField, Range(0.1f, 1f)] float heightRatio = 0.5f;
	[SerializeField, Range(0.8f, 1f)] float padding     = 0.95f;

	[SerializeField] RectReference brickBounds;

	Brick[,] _brickGrid;
	Coroutine _applyBoundsRoutine;

	void Awake()
	{
		_brickGrid = new Brick[gridSize, gridSize];
		SpawnBricks();
	}

	void OnEnable()
	{
		if (brickBounds != null && !brickBounds.useConstant && brickBounds.variable != null)
			brickBounds.variable.OnValueChanged += HandleBoundsChanged;

		KickApplyBounds();
	}
	
	void OnDisable()
	{
		if (brickBounds != null && !brickBounds.useConstant && brickBounds.variable != null)
			brickBounds.variable.OnValueChanged -= HandleBoundsChanged;

		if (_applyBoundsRoutine == null) return;
		StopCoroutine(_applyBoundsRoutine);
		_applyBoundsRoutine = null;
	}
	
	void HandleBoundsChanged(Rect bounds)
	{
		ApplyBounds(bounds);
	}

	void KickApplyBounds()
	{
		if (_applyBoundsRoutine != null)
			StopCoroutine(_applyBoundsRoutine);

		_applyBoundsRoutine = StartCoroutine(ApplyBoundsWhenValid());
	}
	
	IEnumerator ApplyBoundsWhenValid()
	{
		const int maxFrames = 30;

		for (var i = 0; i < maxFrames; i++)
		{
			var bounds = brickBounds.Value;
			if (bounds is { width: > 0f, height: > 0f })
			{
				ApplyBounds(bounds);
				_applyBoundsRoutine = null;
				yield break;
			}

			yield return null;
		}

		Debug.LogWarning(
			$"[BrickSpawner] Brick bounds never became valid. useConstant={brickBounds.useConstant}, " +
			$"hasVariable={brickBounds.variable} value={brickBounds.Value}"
		);

		_applyBoundsRoutine = null;
	}


	void SpawnBricks()
	{
		for (var y = 0; y < gridSize; y++)
		{
			for (var x = 0; x < gridSize; x++)
			{
				if (_brickGrid[x, y]) continue;

				var newBrick = Instantiate(brickPrefab, Vector3.zero, Quaternion.identity);
				newBrick.transform.SetParent(transform);
				var brickScript = newBrick.GetComponent<Brick>();
				brickScript.Initialize(brickType, x, y);
				_brickGrid[x, y] = brickScript;
			}
		}
	}
	
	void ApplyBounds(Rect bounds)
	{
		if (bounds.width <= 0f || bounds.height <= 0f) return;

		var leftBound  = bounds.xMin;
		var upperBound = bounds.yMax;

		var cellWidth  = bounds.width / gridSize;
		var cellHeight = cellWidth * heightRatio;

		var targetScaleX = cellWidth  * padding;
		var targetScaleY = cellHeight * padding;
		var finalScale   = new Vector3(targetScaleX, targetScaleY, 1f);

		foreach (var brick in _brickGrid)
		{
			if (!brick) continue;

			var brickXPos = leftBound  + (brick.GridX * cellWidth)  + (cellWidth  * 0.5f);
			var brickYPos = upperBound - (brick.GridY * cellHeight) - (cellHeight * 0.5f);

			brick.transform.position = new Vector2(brickXPos, brickYPos);
			brick.SetScale(finalScale);
		}
	}
}