using System;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopPurchaser : MonoBehaviour, IStoreListener
{
    public IStoreController Control;

    public TMP_Text coinText;
    public TMP_Text player1Text;
    public TMP_Text player2Text;
    public static ShopPurchaser Instance;
    private void Awake() => Instance = this;

    public const string ID1 = "player1";
    public const string ID2 = "player2";
    public const string ID3 = "coin1";

    async Task Start()
    {
        DontDestroyOnLoad(this);
        Debug.Log("Purchaser: Start");
        var options = new InitializationOptions().SetEnvironmentName("test");
        UnityServices.InitializeAsync(options);
        Initialize();
    }

    public async Task Initialize()
    {
        Debug.Log("Purchaser: UnityServices.InitializeAsync");

        var module = StandardPurchasingModule.Instance();
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(module);
        builder.AddProduct(ID1, ProductType.NonConsumable);
        builder.AddProduct(ID2, ProductType.NonConsumable);
        builder.AddProduct(ID3, ProductType.Consumable);
        Debug.Log("Purchaser: UnityServices.builder");
        UnityPurchasing.Initialize(this, builder);
        Debug.Log("Purchaser: UnityServices.Initialize");
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        throw new NotImplementedException();
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new NotImplementedException();
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        Debug.Log("buyed");
        if (string.Equals(purchaseEvent.purchasedProduct.definition.id, ID1, StringComparison.Ordinal))
        {
            PlayerPrefs.SetInt(FactoryEnum.Player1.ToString(), 1);
            FactoryEventServices.GameAction.BuyPlayer?.Invoke(FactoryEnum.Player1);
            return PurchaseProcessingResult.Complete;
        }

        if (string.Equals(purchaseEvent.purchasedProduct.definition.id, ID2, StringComparison.Ordinal))
        {
            PlayerPrefs.SetInt(FactoryEnum.Player2.ToString(), 1);
            FactoryEventServices.GameAction.BuyPlayer?.Invoke(FactoryEnum.Player2);
            return PurchaseProcessingResult.Complete;
        }

        if (string.Equals(purchaseEvent.purchasedProduct.definition.id, ID3, StringComparison.Ordinal))
        {
            PlayerPrefs.SetInt(FactoryEnum.Coin.ToString(), 100 + PlayerPrefs.GetInt(FactoryEnum.Coin.ToString()));
            FactoryEventServices.GameAction.BuyCoin?.Invoke();
            return PurchaseProcessingResult.Complete;
        }

        return PurchaseProcessingResult.Pending;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        throw new NotImplementedException();
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("Purchaser: OnInitialized");

        Control = controller;
        Write();
    }

    public void Buy(String id)
    {
        Product prod = Control.products.WithID(id);
        if (prod is { availableToPurchase: true })
        {
            Debug.Log("Buying");
            Control.InitiatePurchase(prod);
        }
        else
        {
            Debug.Log("Purchase failed");
        }
    }

    public void Write()
    {
        coinText.text = Control.products.WithID(ID3).metadata.localizedPriceString;
        player1Text.text = Control.products.WithID(ID1).metadata.localizedPriceString;
        player2Text.text = Control.products.WithID(ID2).metadata.localizedPriceString;
    }

    // IStoreController control;
    // private string id1;
    // private string id2;
    // private string id3;
    //
    // public Text text1;
    // public Text text2;
    // public Text text3;
    //
    //
    // void Start()
    // {
    //     Debug.Log("NewShop: Start");
    //     id1 = "player1";
    //     id2 = "player2";
    //     id3 = "chip1";
    //     Initalize();
    // }
    //
    //
    // public void Initalize()
    // {
    //     var options = new InitializationOptions().SetEnvironmentName("test");
    //        UnityServices.InitializeAsync(options);
    //     
    //     var module = StandardPurchasingModule.Instance();
    //     ConfigurationBuilder builder = ConfigurationBuilder.Instance(module);
    //     
    //     builder.AddProduct(id1, ProductType.NonConsumable);
    //     builder.AddProduct(id2, ProductType.NonConsumable);
    //     builder.AddProduct(id3, ProductType.Consumable);
    //     UnityPurchasing.Initialize(this, builder);
    //     Debug.Log("NewShop: Initialize");
    // }
    //
    //
    // public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    // {
    //     Debug.Log("NewShop: OnInitialized");
    //
    //     this.control = controller;
    //     //  yaz();
    // }
    //
    // public void OnInitializeFailed(InitializationFailureReason error)
    // {
    //     Debug.Log("Error:" + error.ToString());
    // }
    //
    // public void OnInitializeFailed(InitializationFailureReason error, string message)
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    // {
    //     Debug.Log("Error while buying" + p.ToString());
    // }
    //
    //
    // public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    // {
    //     if (string.Equals(e.purchasedProduct.definition.id, id1, StringComparison.Ordinal))
    //     {
    //         BuyComplete(FactoryEnum.Player1);
    //         return PurchaseProcessingResult.Complete;
    //     }
    //     else if (string.Equals(e.purchasedProduct.definition.id, id2, StringComparison.Ordinal))
    //     {
    //         BuyComplete(FactoryEnum.Player2);
    //         return PurchaseProcessingResult.Complete;
    //     }
    //     else if (string.Equals(e.purchasedProduct.definition.id, id3, StringComparison.Ordinal))
    //     {
    //         BuyCoin(100, FactoryEnum.Player1);
    //         return PurchaseProcessingResult.Complete;
    //     }
    //     else
    //     {
    //         return PurchaseProcessingResult.Pending;
    //     }
    // }
    //
    // public void Butonatikla(String id)
    // {
    //     Product prod = control.products.WithID(id);
    //     if (prod != null && prod.availableToPurchase)
    //     {
    //         Debug.Log("Buying");
    //         control.InitiatePurchase(prod);
    //     }
    //     else
    //     {
    //         Debug.Log("satin alma basarisiz");
    //     }
    // }
    //
    //
    // private void BuyComplete(FactoryEnum id)
    // {
    //     if (PlayerPrefs.HasKey(id.ToString()) == false)
    //     {
    //         PlayerPrefs.SetInt(id.ToString(), 1);
    //         FactoryEventServices.GameAction.BuyPlayer?.Invoke(id);
    //     }
    // }
    //
    // public void BuyCoin(int value, FactoryEnum id)
    // {
    //     PlayerPrefs.SetInt(id.ToString(), value + PlayerPrefs.GetInt(id.ToString()));
    //     FactoryEventServices.GameAction.BuyCoin?.Invoke(id);
    // }
    //
    //
    // public void yaz()
    // {
    //     Debug.Log("yaz");
    //     foreach (var product in control.products.all)
    //     {
    //         Debug.Log($"prudct: {product.definition.id} {product.metadata.localizedPriceString}");
    //     }
    //
    //     text1.text = control.products.WithID("chip_1m").metadata.localizedPriceString;
    //     text2.text = control.products.WithID("chip_2.5m").metadata.localizedPriceString;
    //     text3.text = control.products.WithID("chip_7.5m").metadata.localizedPriceString;
    // }
}