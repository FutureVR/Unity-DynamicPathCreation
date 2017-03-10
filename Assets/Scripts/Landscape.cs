using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Landscape : MonoBehaviour
{

    public enum blockNames {empty, wall};
    public GameObject[] blocks;

    Camera cam;
    public Transform bottomRightCorner;

    List<List<int>> mapOfInts;
    List<List<GameObject>> mapOfObjects;

    public GameObject terrain;
    public Vector3 blockSize;

    private Vector3 worldSize;
    public int pathWidth = 2;
    int x_size;
    int y_size;

    Graph graph;

    void Start()
    {
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        graph = GetComponent<Graph>();

        worldSize = terrain.transform.localScale * 10;
        x_size = (int)(worldSize.x / blockSize.x);
        y_size = (int)(worldSize.y / blockSize.y);

        foreach (GameObject b in blocks) b.transform.localScale = blockSize;

        initializeMaps();
        graph.createVertices();
        if (graph.vertices.Count >= 2) graph.createEdges();
        graph.printVertices();
        //Debug.Log(graph.edges.Count);
        placeBlocksFromEdges();
    }

    Vector3 initial_indexed_pos;
    Vector3 final_indexed_pos;
    Vector3 initial_pos;
    Vector3 final_pos;

    void Update()
    {

    }

    void placeBlocksFromEdges()
    {
        //Test if this fuction works
        //placeBlockLine(new Vector3(0, 0 , 0), new Vector3(5, 3, 0));

        foreach (Edge e in graph.edges)
        {
            Vector3 pos1 = e.vertex1.position;
            Vector3 pos2 = e.vertex2.position;

            for (int i = 0; i < pathWidth; i++)
            {
                /*Vector3 newVec = pos2 - pos1;
                Vector3 offset = Vector3.Cross(newVec, Vector3.forward);
                Vector3.Normalize(offset);
                offset = offset;
                offset = offset * (1 / blockSize.x); */
                Vector3 offset = new Vector3(i*blockSize.x/2, i * blockSize.y/2, 0);
                placeBlockLine(pos1 + offset, pos2 + offset, 1);
            }
        }
    }


    void placeBlockLine(Vector3 initial_pos, Vector3 final_pos, int block_index)
    {
        initial_indexed_pos = initial_pos - bottomRightCorner.position;
        final_indexed_pos = final_pos - bottomRightCorner.position;
        //Debug.Log(final_indexed_pos);

        float increment_distance = blockSize.x;
        Vector3 increment_vector = final_indexed_pos - initial_indexed_pos;
        int blockTotal = Mathf.FloorToInt(Vector3.Magnitude(increment_vector) / increment_distance) + 1;
        increment_vector = Vector3.Normalize(increment_vector) * increment_distance;

        Vector3 curr_vector = initial_indexed_pos;

        for (int i = 0; i < blockTotal; i++)
        {
            placeBlockAtPoint(curr_vector, block_index);
            curr_vector += increment_vector;
        }
    }

    void placeBlockAtPoint(Vector3 indexed_pos, int block_index)
    {
        int index_x = Mathf.FloorToInt(indexed_pos.x / blockSize.x);
        int index_y = Mathf.FloorToInt(indexed_pos.y / blockSize.y);

        placeBlockAtPoint(index_x, index_y, block_index);
        
    }

    void placeBlockAtPoint(int index_x, int index_y, int block_index)
    {
        //else if (mapOfInts[index_z][index_x] != currBlock)
        if (0 <= index_x && index_x < mapOfInts[0].Count)
        {
            if (0 <= index_y && index_y < mapOfInts.Count)
            {
                Vector3 worldPos = blockSize.x * new Vector3(index_x, index_y, 0) + bottomRightCorner.position + new Vector3(blockSize.x / 2, blockSize.y / 2, 0);
                GameObject newBlock = (GameObject)Instantiate(blocks[block_index], worldPos, Quaternion.identity);

                //Destroy the old block
                //GameObject oldBlock = mapOfObjects[index_y][index_x];
                //if (oldBlock == null) Debug.Log("NULL");
                //GameObject.Destroy(oldBlock);
                

                //Place down the new block
                mapOfInts[index_y][index_x] = block_index;
                mapOfObjects[index_y][index_x] = newBlock;
            }
        }
    }

    void initializeMaps()
    {
        mapOfInts = new List<List<int>>();
        mapOfObjects = new List<List<GameObject>>();
        for (int y = 0; y <= y_size; y++)
        {
            mapOfInts.Add(new List<int>());
            mapOfObjects.Add(new List<GameObject>());

            for (int x = 0; x <= x_size; x++)
            {
                mapOfInts[y].Add(0);
                mapOfObjects[y].Add(blocks[0]);

                placeBlockAtPoint(x, y, 0);
            }
        }
    }

}
