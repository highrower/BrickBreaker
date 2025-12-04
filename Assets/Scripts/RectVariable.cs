using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/RectVariable")]
public class RectVariable : ScriptableObject {

	[SerializeField] private Rect value;
	public                   Rect Value => value;

	public event Action<Rect> OnValueChanged;

	public void SetValue(Rect val) {
		value = val;
		OnValueChanged?.Invoke(value);
	}

	private void OnEnable() => value = Rect.zero;
}