using UnityEditor;
using UnityEngine;

namespace Editor
{
[CustomPropertyDrawer(typeof(RectReference))]
public class ReferenceDrawer : PropertyDrawer
{
	readonly string[] popupOptions =
		{ "Use Constant", "Use Variable" };

	GUIStyle _popupStyle;

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		_popupStyle ??= new GUIStyle(GUI.skin.GetStyle("PaneOptions"))
		{
			imagePosition = ImagePosition.ImageOnly
		};

		label    = EditorGUI.BeginProperty(position, label, property);
		position = EditorGUI.PrefixLabel(position, label);

		EditorGUI.BeginChangeCheck();

		var useConstant   = property.FindPropertyRelative("useConstant");
		var constantValue = property.FindPropertyRelative("constantValue");
		var variable      = property.FindPropertyRelative("variable");

		var buttonRect = new Rect(position);
		buttonRect.yMin   += _popupStyle.margin.top;
		buttonRect.width  =  _popupStyle.fixedWidth + _popupStyle.margin.right;
		buttonRect.height =  EditorGUIUtility.singleLineHeight;
		position.xMin     =  buttonRect.xMax;

		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		var result = EditorGUI.Popup(buttonRect,
									 useConstant.boolValue ? 0 : 1,
									 popupOptions,
									 _popupStyle);

		useConstant.boolValue = result == 0;

		EditorGUI.PropertyField(position,
								useConstant.boolValue ? constantValue : variable,
								GUIContent.none);

		if (EditorGUI.EndChangeCheck())
			property.serializedObject.ApplyModifiedProperties();

		EditorGUI.indentLevel = indent;
		EditorGUI.EndProperty();
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		var useConstant   = property.FindPropertyRelative("useConstant");
		var constantValue = property.FindPropertyRelative("constantValue");

		return useConstant.boolValue ?
				   EditorGUI.GetPropertyHeight(constantValue, label) :
				   EditorGUIUtility.singleLineHeight;
	}
}
}