using UnityEngine;
using System.Collections;

public class Edge
{
    public Vertex vertex1;
    public Vertex vertex2;
    //public LineRenderer myRenderer = new LineRenderer();

    public Edge(Vertex v1, Vertex v2)
    {
        vertex1 = v1;
        vertex2 = v2;

        //myRenderer.SetVertexCount(2);
    }

}
