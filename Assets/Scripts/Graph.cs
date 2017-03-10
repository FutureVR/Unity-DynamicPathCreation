using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Graph : MonoBehaviour
{
    private Vector2 worldSize;
    public int numOfVertices;
    public int maxEdgesPerNode;

    [HideInInspector] public List<Vertex> vertices = new List<Vertex>();
    [HideInInspector] public List<Edge> edges = new List<Edge>();
    public GameObject[] blocks;

    public GameObject terrain;
    public float exclusionRadius = 1;
    public int triesLimit = 5;

    void Start()
    {
        printVertices();
    }

    public void createVertices()
    {
        worldSize = terrain.transform.localScale * 10;

        for (int i = 0; i < numOfVertices; i++)
        {
            bool posFound = false;
            int tries = 0;

            while (posFound == false && tries < triesLimit)
            {
                tries++;
                //Debug.Log(tries);
                Vector2 newVec = new Vector2(Random.Range(-worldSize.x / 2, worldSize.x / 2),
                                             Random.Range(-worldSize.y / 2, worldSize.y / 2));

                if (vertices.Count > 0)
                {
                    posFound = true;
                    foreach (Vertex v in vertices)
                    {
                        if (Vector2.Distance(newVec, v.position) < exclusionRadius)
                        {
                            posFound = false;
                        }
                    }

                    if (posFound)
                    {
                        Vertex newVertex = new Vertex(newVec);
                        vertices.Add(newVertex);
                    }
                }
                else
                {
                    Vertex newVertex = new Vertex(newVec);
                    vertices.Add(newVertex);
                }

            }
        }
    }

    public void createEdges()
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            while (vertices[i].neighbors.Count < maxEdgesPerNode)
            {
                //Find the closest vertex
                bool otherEdgeFound = false;
                Vertex closestVertex = null;
                float closestDistance = Mathf.Infinity;

                for (int j = 0; j < vertices.Count; j++)
                {
                    //If the two vertices are the same, continue
                    if (i == j) continue;

                    //If the two vertices are already connected, continue
                    if (vertices[i].neighbors.Contains(vertices[j])) continue;

                    //Else, calculate if this new edge is closer than the old one
                    otherEdgeFound = true;
                    if (closestVertex == null) closestVertex = vertices[j];
                    else
                    {
                        float dist = Vector2.Distance(vertices[i].position, vertices[j].position);
                        if (dist < closestDistance)
                        {
                            closestDistance = dist;
                            closestVertex = vertices[j];
                        }
                    }
                }

                //if (otherEdgeFound)
                {
                    //Create an edge between this vertex and the closest one
                    Edge newEdge = new Edge(vertices[i], closestVertex);
                    edges.Add(newEdge);

                    //Add each vertex to each other's neighbors
                    vertices[i].neighbors.Add(closestVertex);
                    closestVertex.neighbors.Add(vertices[i]);
                }
            }
        }
    }

    public void printVertices()
    {
        foreach (Vertex v in vertices)
        {
            //GameObject tempBlock = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //tempBlock.transform.position = v.position;
            GameObject newBlock = (GameObject)Instantiate(blocks[0], v.position, Quaternion.identity);
            //Debug.Log(v.position);
            newBlock.transform.position += new Vector3(0, newBlock.transform.localScale.y / 2, 0);
        }
    }

    void printEdges()
    {
        foreach (Edge e in edges)
        {
            LineRenderer lr = new LineRenderer();

            Vector3 v1 = e.vertex1.position;
            Vector3 v2 = e.vertex2.position;
            Vector3[] points = { v1, v2 };

            lr.SetVertexCount(1);
            lr.SetPosition(0, v1);
            lr.SetPosition(1, v2);
        }
        
            
    }
}
    


