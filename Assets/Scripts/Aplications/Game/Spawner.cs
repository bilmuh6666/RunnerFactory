using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawnObject;
    public int spawnCount;
    public float spawnZSize;

    public List<GameObject> objs;


    private void Start()
    {
        FactoryEventServices.GameAction.StopGame += Die;
        FactoryEventServices.GameAction.PlayGame += Play;
        FactoryEventServices.GameAction.PauseGame += Die;

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject obj = Instantiate(spawnObject, new Vector3(0, 0, i * spawnZSize), quaternion.identity);
            obj.GetComponent<Floor>().transportValue = ((spawnZSize / 2) + 10) * -1;
            obj.GetComponent<Floor>().tranportPos =
                new Vector3(0, 0, ((spawnZSize * spawnCount) - (spawnZSize / 2)) - 10);

            objs.Add(obj);
        }
    }

    private void OnDestroy()
    {
        FactoryEventServices.GameAction.StopGame -= Die;
        FactoryEventServices.GameAction.PlayGame -= Play;
        FactoryEventServices.GameAction.PauseGame -= Die;
    }

    public void Play()
    {
        foreach (var item in objs)
        {
            item.GetComponent<Floor>().speed = -75;
        }
    }

    public void Die()
    {
        foreach (var item in objs)
        {
            item.GetComponent<Floor>().speed = 0;
        }
    }
}