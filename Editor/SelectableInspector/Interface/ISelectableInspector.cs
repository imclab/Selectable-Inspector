using UnityEngine;
using System.Collections;

/// <summary>
/// Selectable inspector interface. Selectable inspectors must implement this class.
/// </summary>
public interface ISelectableInspector
{
	// <summary>
	// The name of the custom inspector
	/// </summary>
	string InspectorName
	{
		get;
	}
	
	/// <summary>
	/// The type of object to inspect (Transform, Rigidbody, etc)
	/// </summary>
	System.Type InspectorType
	{
		get;
	}
	
	/// <summary>
	/// Whether the inspector is enabled or not - called by the selector. Can be used to enable/disable other functionality such as OnSceneGUI
	/// </summary>
	bool InspectorEnabled
	{
		get;
		set;
	}
	
	/// <summary>
	/// Main draw function. This replaces OnInspectorGUI.
	/// </summary>
	void DrawInspectorGUI(UnityEngine.Object target);
}
