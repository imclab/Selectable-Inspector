using UnityEngine;
using UnityEditor;

// Transform selector
[CustomEditor(typeof(Transform))]
public class TransformSelector : InspectorSelectBase
{
	public override System.Type GetInspectorType()
	{
		return typeof(Transform);
	}
}

