using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class VoxelEdit : Editor
{
    private static Vector2 position;

    public static Vector2 Position
    {
        get { return position; }
    }

    private static void UpdateView(SceneView sceneView)
    {
        if (Event.current != null)
        {
            position = new Vector2(Event.current.mousePosition.x + sceneView.position.x, Event.current.mousePosition.y + sceneView.position.y);
            Debug.Log(position);
        }
            
    }
}
