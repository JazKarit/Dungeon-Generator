using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class Graph
{
    private Dictionary<Guid, INode> nodes;

    public Graph()
    {
        nodes = new Dictionary<Guid, INode>();
    }


    public void RemoveNode(Guid id)
    {
        nodes.Remove(id);
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
}
