using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FactoryEventServices : MonoBehaviour
{
    public static FactoryEventServices Instance;
    public static bool isPlay;
    private void Awake()
    {
        Instance ??= this;
    }


    [HideInInspector]
    public class GameAction
    {
        public static Action StopGame;
        public static Action<FactoryEnum> BuyPlayer;
        public static Action BuyCoin;
        public static Action<FactoryEnum> ChancePlayer;
        public static Action PlayGame;
        public static Action<int> OpenCoin;
        public static Action PauseGame;
    }

    [HideInInspector]
    public class UIAction
    {
        public static Action PlayGame;
    }
}