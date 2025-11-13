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
using UnityEngine.UI;

public class BuyItemControl : MonoBehaviour {

    public int index = 1;
    public GameObject priceText;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start() {
        if (GameManager.Instance.IAPControl.controller != null) {
            if (this.index == 1) {
                priceText.GetComponent<Text>().text = GameManager.Instance.IAPControl.controller.products.WithID(GameManager.Instance.IAPControl.SKU_1000_COINS).metadata.localizedPriceString;
            } else if (this.index == 2) {
                priceText.GetComponent<Text>().text = GameManager.Instance.IAPControl.controller.products.WithID(GameManager.Instance.IAPControl.SKU_5000_COINS).metadata.localizedPriceString;
            } else if (this.index == 3) {
                priceText.GetComponent<Text>().text = GameManager.Instance.IAPControl.controller.products.WithID(GameManager.Instance.IAPControl.SKU_10000_COINS).metadata.localizedPriceString;
            } else if (this.index == 4) {
                priceText.GetComponent<Text>().text = GameManager.Instance.IAPControl.controller.products.WithID(GameManager.Instance.IAPControl.SKU_50000_COINS).metadata.localizedPriceString;
            } else if (this.index == 5) {
                priceText.GetComponent<Text>().text = GameManager.Instance.IAPControl.controller.products.WithID(GameManager.Instance.IAPControl.SKU_100000_COINS).metadata.localizedPriceString;
            }
        }

    }

    public void buyItem() {
        GameManager.Instance.IAPControl.OnPurchaseClicked(index);
    }
}
