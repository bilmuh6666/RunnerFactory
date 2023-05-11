using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerSpawner : MonoBehaviour
{
    public List<GameObject> players;
    public GameObject activePlayer;
    public GameObject defaultPlayer;
    public GameObject instanceObj;
    private void Start()
    {
        activePlayer = PlayerPrefs.GetInt("ActivePlayer") == 0
            ? defaultPlayer
            : players.FirstOrDefault(x => x.GetComponent<Player>().id == PlayerPrefs.GetInt("ActivePlayer"));

        instanceObj = Instantiate(activePlayer);
        FactoryEventServices.GameAction.BuyPlayer += ChangePlayer;
        FactoryEventServices.GameAction.ChancePlayer += ChangePlayer;
    }

    private void OnDestroy()
    {
        FactoryEventServices.GameAction.BuyPlayer -= ChangePlayer;
        FactoryEventServices.GameAction.ChancePlayer -= ChangePlayer;
    }

    public void ChangePlayer(FactoryEnum type)
    {
        activePlayer = players.FirstOrDefault(x => x.GetComponent<Player>().type == type);
        PlayerPrefs.SetInt("ActivePlayer", activePlayer.GetComponent<Player>().id);
        Destroy(instanceObj);
        instanceObj = Instantiate(activePlayer);
        
    }
}