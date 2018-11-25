using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pathfinder {

    public Stack<Node> findPath(Node startNode, Node goalNode)
    {
        Stack<Node> path = new Stack<Node>();
        List<NodePath> OpenSet = new List<NodePath>();
        List<NodePath> ClosedSet = new List<NodePath>();
        System.Func<NodePath, float> getGScore = (node) => node.gScore;
        System.Func<NodePath, float> getFScore = (node) => node.fScore;
        System.Func<NodePath, float> getHScore = (node) => node.hScore;

        NodePath start = new NodePath(startNode);
        NodePath goal = new NodePath(goalNode);

        //Check if start == goal 
        if ( start.Equals(goal) )
        {
            return path;
        }

        NodePath current;
        start.gScore = 0f;
        start.fScore = Distance(start, goal);

        OpenSet.Add(start);
        
        int stopFlood = 0;
        int maxIter = 1000;
        while ( OpenSet.Count > 0 && stopFlood < maxIter )
        {
            stopFlood++;

            current = GetMinScore(OpenSet, getHScore);

            if ( current.Equals(goal) )
            {
                do
                {
                    path.Push(current.Node);
                    current = current.Parent;
                } while (current != null);
                return path;
            }

            OpenSet.Remove(current);
            ClosedSet.Add(current);

            //We add all neighbors to OpenSet :
            foreach (Node neighbor in current.Node.Neighbors)
            {
                if (neighbor == null)
                    continue;
                NodePath nodePath = new NodePath(neighbor);

                nodePath.gScore = current.gScore + Distance(current, nodePath);
                nodePath.fScore = Distance(nodePath, goal);
                nodePath.Parent = current;

                if ( !Contains(OpenSet, nodePath ))
                {
                    OpenSet.Add(nodePath);
                }
                else if ( Contains(ClosedSet, nodePath) )
                {
                    //We check if this path to that node is better than the registered one :
                    NodePath savedNodePath = GetFrom(ClosedSet, nodePath);
                    if ( savedNodePath.gScore > nodePath.gScore )
                    {
                        savedNodePath.gScore = nodePath.gScore;
                        savedNodePath.Parent = current;
                    }
                }
            }
        }

        Debug.LogWarning("No path found");
        return null;
    }

    private class NodePath
    {
        public Node Node;
        public NodePath Parent = null;
        public float gScore; //heurestic to start
        public float fScore; //heurestic to goal

        public NodePath(Node node)
        {
            Node = node;
        }

        public float hScore
        {
            get
            {
                return gScore + fScore;
            }
        }

        public override bool Equals(object obj)
        {
            NodePath node = (NodePath)obj;
            return node.Node == Node;
        }

        public override int GetHashCode()
        {
            return Node.GetHashCode();
        }
    }

    private NodePath GetMinScore(List<NodePath> nodes, System.Func<NodePath, float> getScore)
    {
        float minScore = Mathf.Infinity;
        NodePath minNode = null;
        foreach (NodePath node in nodes)
        {
            if ( getScore(node) < minScore )
            {
                minScore = getScore(node);
                minNode = node;
            }
        }
        return minNode;
    }

    private bool Contains(List<NodePath> nodes, NodePath node)
    {
        foreach (NodePath nodePath in nodes)
        {
            if ( nodePath.Equals(node) )
            {
                return true;
            }
        }
        return false;
    }
    
    private NodePath GetFrom(List<NodePath> nodes, NodePath node)
    {

        foreach (NodePath nodePath in nodes)
        {
            if (nodePath.Equals(node))
            {
                return nodePath;
            }
        }
        return null;
    }


    private float Distance(NodePath nodeA, NodePath nodeB)
    {
        return Vector3.Distance(nodeA.Node.transform.position, nodeB.Node.transform.position);
    }
}
