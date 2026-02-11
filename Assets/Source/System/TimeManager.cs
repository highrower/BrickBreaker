using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
	[SerializeField] TwistComponent twistComponent;
	[SerializeField] float          duration;
	[SerializeField] float          timeScale;

	float _defaultFixedDeltaTime;

	void Awake() => _defaultFixedDeltaTime = Time.fixedDeltaTime;

	void OnEnable()
	{
		if (twistComponent)
			twistComponent.OnReleasingHit += StartTwistHitStop;
	}

	void OnDisable()
	{
		if (twistComponent)
			twistComponent.OnReleasingHit -= StartTwistHitStop;
	}

	void StartTwistHitStop()
	{
		StopAllCoroutines();
		StartCoroutine(HitStopRoutine());
	}

	IEnumerator HitStopRoutine()
	{
		Time.timeScale      = timeScale;
		Time.fixedDeltaTime = _defaultFixedDeltaTime * timeScale;

		yield return new WaitForSecondsRealtime(duration);

		Time.timeScale      = 1f;
		Time.fixedDeltaTime = _defaultFixedDeltaTime;
	}
}