using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    [SerializeField] private List<Node> neighbors = new List<Node>();

    private float radius;

    public List<Node> Neighbors
    {
        get
        {
            return neighbors;
        }
    }

    private void OnDrawGizmos()
    {

        //Draw a sphere to represent the current node.
        Gizmos.color = Color.white;
        radius = GetRadius(); //The size depend on the gameobject's size.
        Gizmos.DrawSphere(transform.position, radius);

        //Draw edges to neighbors
        Gizmos.color = Color.green;
        foreach (Node neighbor in Neighbors)
        {
            if ( neighbor == null )
            {
                continue;
            }
            Vector3 start = transform.position;
            Vector3 goal = neighbor.transform.position;
            float distance = Vector3.Distance(start, goal);

            //We draw a small sphere at the edge of the current node spehre to mark the starting point.
            float smallRadius = radius / 4f;
            Vector3 nodeBorder = Vector3.Lerp(start, goal, radius / distance);
            Vector3 neighborBorder = Vector3.Lerp(goal, start, neighbor.GetRadius() / distance);
            Gizmos.DrawSphere(nodeBorder, smallRadius);

            //We draw a line linking to the other node
            Gizmos.DrawLine(nodeBorder, neighborBorder);
        }
    }

    public float GetRadius()
    {
        float spriteRadius = GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2f;
        float smallestLocalScale = Mathf.Min(transform.localScale.x, transform.localScale.y);
        return spriteRadius * smallestLocalScale;
    }

    public bool Contains(Vector3 worldPos)
    {
        /*Debug.Log("Distance = " + Vector3.Distance(worldPos, transform.position) + " , radius = " + GetRadius());
        Debug.Log(worldPos);
        Debug.Log("transform.pos " + transform.position);*/
        return Vector3.Distance(worldPos, transform.position) < GetRadius();
    }
    
}
