using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
public interface INode
{
    public Guid Id { get;  } // Unique identifier for the node
    public List<INode> Neighbors { get; set; } // Connections to other nodes

    public int IterationCreated {get; set; }

    public void AddNeighbor(INode neighbor);

    // Output node and its neighbors as a string
    public string ToStringWithNeighbors();
    
    public int GetNumberOfExpansions();
    public void AddExpansion();
    public float GetCoverage();
}