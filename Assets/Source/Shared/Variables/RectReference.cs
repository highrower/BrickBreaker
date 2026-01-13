using System;
using UnityEngine;

[Serializable]
public class RectReference
{
	public bool         useConstant = true;
	public Rect         constantValue;
	public RectVariable variable;

	public Rect Value => useConstant ? constantValue : variable.Value;

	public RectReference() { }

	public RectReference(Rect value)
	{
		useConstant   = true;
		constantValue = value;
	}

	public static implicit operator Rect(RectReference reference) { return reference.Value; }
}