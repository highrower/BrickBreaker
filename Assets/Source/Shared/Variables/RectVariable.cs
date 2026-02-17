using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Rect Variable")]
public class RectVariable : ScriptableObject
{
	[SerializeField] Rect value;
	public           Rect Value => value;

	public event Action<Rect> OnValueChanged;

	public void SetValue(Rect val)
	{
		value = val;
		OnValueChanged?.Invoke(value);
	}

}