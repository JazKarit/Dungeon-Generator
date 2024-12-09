using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class Graph
{
    private Dictionary<Guid, INode> nodes;
    public (int x, int z) Seed {get; set;}

    public Graph(int seedX, int seedZ)
    {
        nodes = new Dictionary<Guid, INode>();
        Seed = (seedX, seedZ);
    }


    public void RemoveNode(Guid id)
    {
        if (nodes.ContainsKey(id))
        {
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
    }

    public INode GetNode(Guid id)
    {
        return nodes.ContainsKey(id) ? nodes[id] : null;
    }

    public IEnumerable<INode> GetAllNodes()
    {
        return nodes.Values;
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
