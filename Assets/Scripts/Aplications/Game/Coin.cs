using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameObject obj;
    public int index;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FactoryEventServices.GameAction.OpenCoin?.Invoke(index);
            obj.SetActive(false);
        }
           
    }
}
