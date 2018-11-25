using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traveler : MonoBehaviour {

    [SerializeField] private GameObject graph;
    [SerializeField] private Node[] clickedNodes = new Node[2];

    private List<Node> nodes = new List<Node>();
    private Stack<Node> path;
    private int clickIndex = 0;
    private int ClickIndex
    {
        get
        {
            if (clickIndex >= 2 )
            {
                clickIndex = 0;
            }
            return clickIndex++;
        }
    }

    private void Start()
    {
        GetNodes();
    }

    private void Update()
    {
        //Fill the start/goal array on node clicks
        if ( Input.GetMouseButtonDown(0) )
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            mouseWorldPos.z = 0f;
            Node node = NodeThatContains(mouseWorldPos);
            if (node != null)
            {
                clickedNodes[ClickIndex] = node;
            }
        }
        //Search a path if the start/goal array is full
        if (Input.GetMouseButtonDown(1))
        {
            if (clickedNodes[0] != null && clickedNodes[1] != null)
            {
                Pathfinder pathfinder = new Pathfinder();
                path = pathfinder.findPath(clickedNodes[0], clickedNodes[1]);
            }
        }
        //display the path if one has been found (visible in editor scene only)
        if ( path != null && path.Count > 0)
        {
            DrawPath(new Stack<Node>(new Stack<Node>(path)));
        }
    }

    private void DrawPath(Stack<Node> gameobjects) 
    {
        if ( gameobjects == null )
        {
            return;
        }
        GameObject lastGameobject = null;
        while (gameobjects.Count > 0)
        {
            GameObject obj = gameobjects.Pop().gameObject;

            if (lastGameobject != null)
            {
                Debug.DrawLine(lastGameobject.transform.position, obj.transform.position);
                
            }

            lastGameobject = obj;
        }
    }

    private void GetNodes()
    {
        nodes = new List<Node>();
        foreach (Transform child in graph.transform)
        {
            Node node = child.GetComponent<Node>();
            if (node != null)
            {
                nodes.Add(node);
            }
        }
    }

    private Node NodeThatContains(Vector3 worldPos)
    {
        worldPos.z = 0f;
        foreach (Node node in nodes)
        {
            if (node.Contains(worldPos))
            {
                return node;
            }
        }
        return null;
    }
}
