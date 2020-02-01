using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomSpawner : MonoBehaviour
{
    public int ItemsAtStart;
    public int timeBetweenItem;
    public List<GameObject> items;

    private float realTime = 0;
    private float lastSpawnTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i <= ItemsAtStart; i++)
            SpawnItem();
    }

    // Update is called once per frame
    void Update()
    {
        if (realTime >= lastSpawnTime + timeBetweenItem)
            SpawnItem();
        realTime += Time.deltaTime;
    }

    private void SpawnItem()
    {
        lastSpawnTime = realTime;
        Instantiate(items[Random.Range(0, items.Count)], GetRandomLocation(), Quaternion.identity);
    }

    private Vector3 GetRandomLocation()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        // Pick the first indice of a random triangle in the nav mesh
        int t = Random.Range(0, navMeshData.indices.Length);

        // Select a random point on it
        Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t + 1]], Random.value);
        Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value);

        return point;
    }
}
