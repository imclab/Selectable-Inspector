using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Inspector select base class. Each type needs a selector inheriting from this class.
/// </summary>
public abstract class InspectorSelectBase : Editor
{
	private Dictionary<string, ISelectableInspector> inspectors = new Dictionary<string, ISelectableInspector>();
	private ISelectableInspector currentInspector;
	private bool initialized = false;
	private string[] inspectorList;
	private int[] inspectorIndices;
	private int currentIndex;
	private System.Type inspectorType;
	
	public abstract System.Type GetInspectorType();
		
	private void FindInspectors()
	{
		inspectors.Clear();
		
		var type = typeof(ISelectableInspector);
		var types = System.AppDomain.CurrentDomain.GetAssemblies().ToList()
			.SelectMany(s => s.GetTypes())
			.Where(p => type.IsAssignableFrom(p) && p!=type);
		
		foreach(var plugin in types)
		{
			try
			{
				ISelectableInspector inspector = System.Activator.CreateInstance(plugin) as ISelectableInspector;
				inspectorType = inspector.InspectorType;
				if (inspectorType == GetInspectorType())
				{
					inspectors.Add(inspector.InspectorName, inspector);
				}
			}
			catch (System.Exception e)
			{
				Debug.LogError(e.Message);
			}
		}
	}
	// Populate the selector list
	void Initialize ()
	{
		FindInspectors ();
		string currentString = EditorPrefs.GetString ("SelectableInspector." + EditorApplication.applicationPath + "." + inspectorType.ToString (), "Default");
		if (!inspectors.ContainsKey (currentString))
		{
			return;
		}
		currentInspector = inspectors[currentString];
		currentInspector.InspectorEnabled = true;
		
		inspectorList = inspectors.Keys.ToArray();
		inspectorIndices = new int[inspectorList.Length];
		for(int i = 0; i < inspectorIndices.Length; i++)
		{
			inspectorIndices[i] = i;
			if (inspectorList[i] == currentInspector.InspectorName)
				currentIndex = i;
		}
		initialized = true;
	}
	
	// Reinitialize with OnEnable
	void OnEnable()
	{
		Initialize();
	}
	
	// Display code
	public override void OnInspectorGUI ()
	{
		if (!initialized)
		{
			Initialize ();
		}
		
		if (!initialized || inspectors.Count == 0)
		{
			// Fallback
			GUI.color = Color.red;
			GUILayout.Label("No Custom Inspectors Found");
			GUI.color = Color.white;
			this.DrawDefaultInspector();
			return;
		}
		
		currentIndex = EditorGUILayout.IntPopup("Inspector", currentIndex, inspectorList, inspectorIndices);
		
		try
		{
			if (inspectors[inspectorList[currentIndex]] != currentInspector)
			{
				currentInspector.InspectorEnabled = false;
				currentInspector = inspectors[inspectorList[currentIndex]];
				currentInspector.InspectorEnabled = true;
				EditorPrefs.SetString("SelectableInspector." + inspectorType.ToString(), currentInspector.InspectorName);
			}
		}
		catch
		{
			// Fallback
			this.DrawDefaultInspector();
		}
		
		if (currentInspector != null)
		{
			currentInspector.DrawInspectorGUI(target);
		}
		else
		{
			// Fallback
			this.DrawDefaultInspector();
		}
	}
}
