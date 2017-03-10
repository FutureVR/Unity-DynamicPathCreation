using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vertex
{
    public Vector2 position;
    public List<Vertex> neighbors;

    public Vertex(Vector2 p)
    {
        position = p;
        neighbors = new List<Vertex>();
    }

}
