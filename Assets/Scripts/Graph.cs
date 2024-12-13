using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class Graph
{
    private Dictionary<Guid, INode> nodes;
    public (int x, int z) Seed {get; set;}

    public List<Graph> goalGraphs;

    public Graph(int seedX, int seedZ)
    {
        nodes = new Dictionary<Guid, INode>();
        Seed = (seedX, seedZ);
        goalGraphs = new List<Graph>();
    }

    public void AddGoalGraph(Graph graph)
    {
        goalGraphs.Add(graph);
    }

    public (int x, int z)? GoalSample()
    {
        if (goalGraphs.Count == 0)
        {
            return null;
        }
        var r = UnityEngine.Random.Range(0,goalGraphs.Count);
        
        return ((int)goalGraphs[r].GetRandomNode().GetPosition().x, (int)goalGraphs[r].GetRandomNode().GetPosition().z);
        //return goalGraphs[r].Seed;
    }


    public void RemoveNode(Guid id)
    {
        if (nodes.ContainsKey(id))
        {
            foreach(var neighbor in nodes[id].Neighbors)
            {
                if (neighbor.Id != id)
                {
                    neighbor.RemoveNeighbor(nodes[id]);
                }
            }
            nodes.Remove(id);
        }
    }

    public void AddNode(INode node)
    {
        if (!nodes.ContainsKey(node.Id))
        {
            nodes[node.Id] = node;
        }
    }

    public void AddEdge(Guid fromId, Guid toId)
    {
        if (nodes.ContainsKey(fromId) && nodes.ContainsKey(toId))
        {
            INode fromNode = nodes[fromId];
            INode toNode = nodes[toId];
            fromNode.AddNeighbor(toNode);
            toNode.AddNeighbor(fromNode); // For undirected graph
        }
        else
        {
            Debug.Log("Failed to add Edge");
        }
    }

    public INode GetNode(Guid id)
    {
        return nodes.ContainsKey(id) ? nodes[id] : null;
    }

    public IEnumerable<INode> GetAllNodes()
    {
        return nodes.Values;
    }

    public INode? GetRandomNode()
    {
        if (nodes.Count == 0)
        {
            return null;
        }
        var r = UnityEngine.Random.Range(0,nodes.Count);
        return nodes.Values.ToList()[r];
    }

    public void PrintGraph()
    {
        foreach (var node in nodes.Values)
        {
            UnityEngine.Debug.Log(node.ToStringWithNeighbors());
        }
    }

    // New method to print all self-loops
    public void PrintSelfLoops()
    {
        foreach (var node in nodes.Values)
        {
            if (node.Neighbors.Contains(node))
            {
                UnityEngine.Debug.Log($"Self-loop found: Node {node.Id} connects to itself.");
            }
        }
    }

    public void RemoveEdge(Guid fromId, Guid toId)
    {
        if (nodes.ContainsKey(fromId) && nodes.ContainsKey(toId))
        {
            INode fromNode = nodes[fromId];
            INode toNode = nodes[toId];

            fromNode.RemoveNeighbor(toNode);
            toNode.RemoveNeighbor(fromNode); // For undirected graph
        }
    }

    public List<INode> ReturnSelfLoopNodes()
    {
        List<INode> selfLoopNodes = new List<INode>();
        foreach (var node in nodes.Values)
        {
            if (node.Neighbors.Contains(node))
            {
                //UnityEngine.Debug.Log($"Self-loop found: Node {node.Id} connects to itself.");
                selfLoopNodes.Add(node);
            }
        }
        return selfLoopNodes;
    }
}
