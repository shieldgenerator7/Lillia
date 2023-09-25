using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class Spawner : Resettable
{
    public GameObject prefab;

    private List<GameObject> spawnedObjects = new();

    public T SpawnObject<T>(Vector3 position) where T : MonoBehaviour
    {
        GameObject go = getNextObject();
        go.transform.position = position;
        T comp = go.GetComponent<T>();
        return comp;
    }

    private GameObject getNextObject()
    {
        GameObject go = spawnedObjects.FirstOrDefault(go => !go.activeSelf);
        if (!go)
        {
            go = Instantiate(prefab);
            spawnedObjects.Add(go);
        }
        go.SetActive(true);
        return go;
    }

    public void DestroyObject(GameObject go)
    {
        if (spawnedObjects.Contains(go))
        {
            go.SetActive(false);
        }
    }

    public override void recordInitialState()
    {
    }

    public override void reset()
    {
        spawnedObjects.ForEach(go => go.SetActive(false));
    }
}
