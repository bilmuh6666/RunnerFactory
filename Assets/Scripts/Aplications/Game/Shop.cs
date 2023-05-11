using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Button coinButton;
    public List<GameObject> playerButtons;
    public TMP_Text coinText;
    public TMP_Text coinPlayerText;
    public int playerChip = 600;
    public GameObject finishPanel;
    public Button watchButton;
    private void Start()
    {
        if (PlayerPrefs.HasKey(FactoryEnum.Player3.ToString()))
            PlayerPrefs.SetInt(FactoryEnum.Player3.ToString(), 0);

        coinPlayerText.text = $"Buy:{playerChip}";
        coinText.text = $"Coin:{PlayerPrefs.GetInt(FactoryEnum.Coin.ToString())}";
        FactoryEventServices.GameAction.BuyPlayer += BuyedPlayer;
        FactoryEventServices.GameAction.BuyCoin += BuyedCoin;
        FactoryEventServices.GameAction.StopGame += StopGame;
        BuyButtonControl();
    }

    private void OnDestroy()
    {
        FactoryEventServices.GameAction.BuyPlayer -= BuyedPlayer;
        FactoryEventServices.GameAction.BuyCoin -= BuyedCoin;
        FactoryEventServices.GameAction.StopGame -= StopGame;
    }

    public void BuyedCoin()
    {
        coinText.text = coinText.text = $"Coin:{PlayerPrefs.GetInt(FactoryEnum.Coin.ToString())}";
    }

    public void BuyedPlayer(FactoryEnum id)
    {
        FactoryEventServices.GameAction.ChancePlayer?.Invoke(id);
        BuyButtonControl();
    }

    public void SelectButtonClick(GameObject e)
    {
        var a = e.GetComponent<MyButton>().type;
        FactoryEventServices.GameAction.ChancePlayer?.Invoke(a);
    }

    public void PlayGame()
    {
        FactoryEventServices.GameAction.PlayGame?.Invoke();
        FactoryEventServices.isPlay = true;
        watchButton.interactable = false;
    }
    public void Replay()
    {
        SceneManager.LoadScene(0);
        watchButton.interactable = true;
    }

    public void StopGame()
    {
        watchButton.interactable = true;
        finishPanel.SetActive(true);
    }

    public void BuyPlayerWithCoin()
    {
        if (PlayerPrefs.GetInt(FactoryEnum.Player3.ToString()) == 0 &&
            PlayerPrefs.GetInt(FactoryEnum.Coin.ToString()) >= playerChip)
        {
            PlayerPrefs.SetInt(FactoryEnum.Player3.ToString(), 1);
            PlayerPrefs.SetInt(FactoryEnum.Coin.ToString(), PlayerPrefs.GetInt(FactoryEnum.Coin.ToString()) - playerChip);

            FactoryEventServices.GameAction.BuyPlayer?.Invoke(FactoryEnum.Player3);
        }
    }

    public void BuyButtonControl()
    {
        foreach (var VARIABLE in playerButtons)
        {
            if (PlayerPrefs.HasKey(VARIABLE.GetComponent<MyButton>().type.ToString()))
            {
                VARIABLE.gameObject.SetActive(false);
            }
            else
            {
                VARIABLE.gameObject.SetActive(true);
            }
        }
    }
}