/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;
using System.Collections;
using UnityEngine.Purchasing;
using System;

public class IAPController : MonoBehaviour, IStoreListener
{

    public string SKU_1000_COINS = "com.5000coins";
    public string SKU_5000_COINS = "com.10000coins";
    public string SKU_10000_COINS = "com.25000coins";
    public string SKU_50000_COINS = "com.75000coins";
    public string SKU_100000_COINS = "com.200000coins";

    public IStoreController controller;
    private IExtensionProvider extensions;

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
        GameManager.Instance.IAPControl = this;
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(SKU_1000_COINS, ProductType.Consumable);
        builder.AddProduct(SKU_5000_COINS, ProductType.Consumable);
        builder.AddProduct(SKU_10000_COINS, ProductType.Consumable);
        builder.AddProduct(SKU_50000_COINS, ProductType.Consumable);
        builder.AddProduct(SKU_100000_COINS, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("IAP Initizalization complete!!");
        this.controller = controller;
        this.extensions = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("IAP Initizalization FAILED!! " + error.ToString());
    }

    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
        Debug.Log("IAP purchase FAILED!! " + p.ToString());
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {

        if (e.purchasedProduct.definition.id == SKU_1000_COINS)
        {
            GameManager.Instance.playfabManager.addCoinsRequest(5000);
        }
        else if (e.purchasedProduct.definition.id == SKU_5000_COINS)
        {
            GameManager.Instance.playfabManager.addCoinsRequest(10000);
        }
        else if (e.purchasedProduct.definition.id == SKU_10000_COINS)
        {
            GameManager.Instance.playfabManager.addCoinsRequest(25000);
        }
        else if (e.purchasedProduct.definition.id == SKU_50000_COINS)
        {
            GameManager.Instance.playfabManager.addCoinsRequest(75000);
        }
        else if (e.purchasedProduct.definition.id == SKU_100000_COINS)
        {
            GameManager.Instance.playfabManager.addCoinsRequest(200000);
        }
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseClicked(int productId)
    {
        if (controller != null)
        {
            if (productId == 1)
            {
                controller.InitiatePurchase(SKU_1000_COINS);
            }
            else if (productId == 2)
            {
                controller.InitiatePurchase(SKU_5000_COINS);
            }
            else if (productId == 3)
            {
                controller.InitiatePurchase(SKU_10000_COINS);
            }
            else if (productId == 4)
            {
                controller.InitiatePurchase(SKU_50000_COINS);
            }
            else if (productId == 5)
            {
                controller.InitiatePurchase(SKU_100000_COINS);
            }
        }
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new NotImplementedException();
    }
}
