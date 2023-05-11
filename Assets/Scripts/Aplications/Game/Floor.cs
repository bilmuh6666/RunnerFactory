using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public float speed;
    public float transportValue;
    public Vector3 tranportPos;
    public List<GameObject> coins;

    private void Start()
    {
        FactoryEventServices.GameAction.OpenCoin += OpenCoin;
    }

    private void OnDestroy()
    {
        FactoryEventServices.GameAction.OpenCoin -= OpenCoin;
    }

    private void Update()
    {
        transform.position += new Vector3(0, 0, speed) * Time.deltaTime;
        if (transform.position.z <= transportValue)
        {
            Trasport((-1 * transportValue) + transform.position.z);
        }
    }

    public void Trasport(float value)
    {
        transform.position = tranportPos + new Vector3(0, 0, value);
    }

    public void OpenCoin(int i)
    {
        StartCoroutine(Open(i));
    }

    public IEnumerator Open(int i)
    {
        yield return new WaitForSeconds(0.2f);
        coins[i].SetActive(true);
    }
}