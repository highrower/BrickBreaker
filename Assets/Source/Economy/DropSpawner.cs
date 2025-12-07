using UnityEngine;
using UnityEngine.Pool;

public class DropSpawner : MonoBehaviour {
	public static DropSpawner Instance { get; private set; }

	[SerializeField] private Drop dropPrefab;
	[SerializeField] private int  defaultCapacity = 10;
	[SerializeField] private int  maxCapacity     = 20;

	private IObjectPool<Drop> _pool;

	private void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(this);
			return;
		}

		Instance = this;
		_pool = new ObjectPool<Drop>(createFunc: CreateDrop,
		                             actionOnGet: OnTakeFromPool,
		                             actionOnRelease: OnReturnToPool,
		                             actionOnDestroy: OnDestroyDrop,
		                             collectionCheck: true,
		                             defaultCapacity: defaultCapacity,
		                             maxSize: maxCapacity);
	}

	private Drop CreateDrop() {
		var drop = Instantiate(dropPrefab, transform, true);
		drop.SetPool(_pool);
		return drop;
	}

	private static void OnTakeFromPool(Drop drop) => drop.gameObject.SetActive(true);

	private static void OnReturnToPool(Drop drop) => drop.gameObject.SetActive(false);

	private static void OnDestroyDrop(Drop drop) => Destroy(drop.gameObject);

	public void SpawnDrop(Vector3 position) {
		var drop = _pool.Get();
		drop.transform.position = position;
		drop.transform.rotation = Quaternion.identity;
	}
}