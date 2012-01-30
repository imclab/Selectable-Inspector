using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Transform inspector default class. Each type will require a default class with a selector.
/// </summary>
/// <remarks>
/// Note that this class should not inherit from the Editor class.
/// </remarks>
public class TransformInspectorDefault : ISelectableInspector
{
	#region ISelectableInspector implementation
	private bool mEnabled = false;
	
	public string InspectorName
	{
		get
		{
			return "Default";
		}
	}
	
	public System.Type InspectorType
	{
		get
		{
			return typeof(Transform);
		}
	}
	
	public bool InspectorEnabled
	{
		get
		{
			return mEnabled;
		}
		set
		{
			mEnabled = value;	
		}
	}
	
	/// In the default class, this should usually contain DrawDefaultInspector() to look like Unity defaults.
	public void DrawInspectorGUI(UnityEngine.Object target)
	{
		// DrawDefaultInspector();
		
		// Unity's default transform controls
		var transform = target as Transform;
		transform.localPosition = EditorGUILayout.Vector3Field("Position", transform.localPosition);
		transform.localEulerAngles = EditorGUILayout.Vector3Field("Rotation", transform.localEulerAngles);
		transform.localScale = EditorGUILayout.Vector3Field("Scale", transform.localScale);
	}
	#endregion
}


