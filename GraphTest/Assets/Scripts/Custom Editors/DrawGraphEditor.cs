using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Graph))]
public class DrawGraphEditor : Editor {

    private void OnSceneGUI()
    {
        Graph graph = target as Graph;

        if (graph == null || graph.Nodes == null )
        {
            return;
        }

        //Foreach node
        foreach (Node node in graph.Nodes)
        {
            if (node == null)
                continue;

            //We draw a line from it to each of its neighbors
            Vector3 center = node.transform.position;
            foreach (Node neighbor in node.Neighbors)
            {
                if (neighbor == null)
                    continue;

                Handles.DrawLine(center, neighbor.transform.position);
            }
        }
    }
}
