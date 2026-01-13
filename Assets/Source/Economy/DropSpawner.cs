using UnityEngine;
using UnityEngine.Pool;

public class DropSpawner : MonoBehaviour
{
	public static DropSpawner Instance { get; private set; }

	[SerializeField] Drop dropPrefab;
	[SerializeField] int  defaultCapacity = 10;
	[SerializeField] int  maxCapacity     = 20;

	IObjectPool<Drop> _pool;

	void Awake()
	{
		if (Instance != null && Instance != this)
		{
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

	Drop CreateDrop()
	{
		var drop = Instantiate(dropPrefab, transform, true);
		drop.SetPool(_pool);

		return drop;
	}

	static void OnTakeFromPool(Drop drop) => drop.gameObject.SetActive(true);

	static void OnReturnToPool(Drop drop) => drop.gameObject.SetActive(false);

	static void OnDestroyDrop(Drop drop) => Destroy(drop.gameObject);

	public void SpawnDrop(Vector3 position)
	{
		var drop = _pool.Get();
		drop.transform.position = position;
		drop.transform.rotation = Quaternion.identity;
	}
}