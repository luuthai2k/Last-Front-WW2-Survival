using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.UI;

namespace Samples.Purchasing.Core.IAPManager
{
    public class IAPManager : Singleton<IAPManager>, IDetailedStoreListener
    {
        IStoreController m_StoreController; // The Unity Purchasing system.
        private static IExtensionProvider m_StoreExtensionProvider;
        //Your products IDs. They should match the ids of your products in your store.
        public string removeads = "weup.ww2.duty.frontline.zone.removeads";
        public string ddayoffer = "weup.ww2.duty.frontline.zone.ddayoffer";
        public string sgt = "weup.ww2.duty.frontline.zone.sgt";

        public string ak47golden = "weup.ww2.duty.frontline.zone.ak47golden";
        public string starterpack = "weup.ww2.duty.frontline.zone.starterpack";

        public string cashoffer1 = "weup.ww2.duty.frontline.zone.cashoffer1";
        public string cashoffer2 = "weup.ww2.duty.frontline.zone.cashoffer2";
        public string cashoffer3 = "weup.ww2.duty.frontline.zone.cashoffer3";
        public string cashoffer4 = "weup.ww2.duty.frontline.zone.cashoffer4";
        public string cashoffer5 = "weup.ww2.duty.frontline.zone.cashoffer5";
        public string goldoffer1 = "weup.ww2.duty.frontline.zone.goldoffer1";
        public string goldoffer2 = "weup.ww2.duty.frontline.zone.goldoffer2";
        public string goldoffer3 = "weup.ww2.duty.frontline.zone.goldoffer3";
        public string weaponoffer1 = "weup.ww2.duty.frontline.zone.weaponoffer1";
        public string weaponoffer2 = "weup.ww2.duty.frontline.zone.weaponoffer2";
        public string weaponoffer3 = "weup.ww2.duty.frontline.zone.weaponoffer3";
        public string weaponoffer4 = "weup.ww2.duty.frontline.zone.weaponoffer4";
        public string weaponoffer5 = "weup.ww2.duty.frontline.zone.weaponoffer5";
         public string weaponoffer6 = "weup.ww2.duty.frontline.zone.weaponoffer6";

        public string endlessoffer1 = "weup.ww2.duty.frontline.zone.endlessoffer";
        public string endlessoffer2 = "weup.ww2.duty.frontline.zone.endlessoffer1";
        public string endlessoffer3 = "weup.ww2.duty.frontline.zone.endlessoffer2";

        public string battlepasspremium = "weup.ww2.duty.frontline.zone.battlepasspremium";




        void Start()
        {
            InitializePurchasing();
        }

        void InitializePurchasing()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            //Add products that will be purchasable and indicate its type.
            builder.AddProduct(removeads, ProductType.Consumable);
            builder.AddProduct(ddayoffer, ProductType.Consumable);
            builder.AddProduct(sgt, ProductType.Consumable);

            builder.AddProduct(ak47golden, ProductType.Consumable);
            builder.AddProduct(starterpack, ProductType.Consumable);

            builder.AddProduct(cashoffer1, ProductType.Consumable);
            builder.AddProduct(cashoffer2, ProductType.Consumable);
            builder.AddProduct(cashoffer3, ProductType.Consumable);
            builder.AddProduct(cashoffer4, ProductType.Consumable);
            builder.AddProduct(cashoffer5, ProductType.Consumable);

            builder.AddProduct(goldoffer1, ProductType.Consumable);
            builder.AddProduct(goldoffer2, ProductType.Consumable);
            builder.AddProduct(goldoffer3, ProductType.Consumable);

            builder.AddProduct(weaponoffer1, ProductType.Consumable);
            builder.AddProduct(weaponoffer2, ProductType.Consumable);
            builder.AddProduct(weaponoffer3, ProductType.Consumable);
            builder.AddProduct(weaponoffer4, ProductType.Consumable);
            builder.AddProduct(weaponoffer5, ProductType.Consumable);
            builder.AddProduct(weaponoffer6, ProductType.Consumable);

            builder.AddProduct(endlessoffer1, ProductType.Consumable);
            builder.AddProduct(endlessoffer2, ProductType.Consumable);
            builder.AddProduct(endlessoffer3, ProductType.Consumable);

            builder.AddProduct(battlepasspremium, ProductType.Consumable);
            UnityPurchasing.Initialize(this, builder);
        }
        public Action<string> onPurchaseComplete,onPurchaseFailed;
        public void BuyPack(string id, Action<string> onPurchaseComplete, Action<string> onPurchaseFailed)
        {
            m_StoreController.InitiatePurchase(id);
            this.onPurchaseComplete = onPurchaseComplete;
            this.onPurchaseFailed = onPurchaseFailed;
        }
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log("In-App Purchasing successfully initialized");
            m_StoreController = controller;
            m_StoreExtensionProvider = extensions;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            OnInitializeFailed(error, null);
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            var errorMessage = $"Purchasing failed to initialize. Reason: {error}.";

            if (message != null)
            {
                errorMessage += $" More details: {message}";
            }

            Debug.Log(errorMessage);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            //Retrieve the purchased product
            var product = args.purchasedProduct;

            //Add the purchased product to the players inventory
           
            onPurchaseComplete?.Invoke(product.definition.id);
            Debug.Log($"Purchase Complete - Product: {product.definition.id}");

            //We return Complete, informing IAP that the processing on our side is done and the transaction can be closed.
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            onPurchaseFailed?.Invoke(product.definition.id);
            Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            onPurchaseFailed?.Invoke(product.definition.id);
            Debug.Log($"Purchase failed - Product: '{product.definition.id}'," +
                $" Purchase failure reason: {failureDescription.reason}," +
                $" Purchase failure details: {failureDescription.message}");
        }
        public string GetLocalPrice(string id)
        {
            if (m_StoreController == null)
            {
                return null;
            }
            Product product = m_StoreController.products.WithID(id);
            if (product != null && product.metadata != null)
            {
                return product.metadata.localizedPriceString;
            }
            return null;
        }
        public string GetLocalPriceWithCurrencyCode(string id)
        {
            if (m_StoreController == null)
                return null;
            Product product = m_StoreController.products.WithID(id);
            if (product != null && product.metadata != null)
            {
                decimal price = product.metadata.localizedPrice;
                string currencyCode = product.metadata.isoCurrencyCode;
                return $"{price} {currencyCode}";
            }
            return null;
        }
        public void RestorePurchases()
        {
            if (m_StoreController != null && m_StoreExtensionProvider != null)
            {
                if (Application.platform == RuntimePlatform.IPhonePlayer ||
                    Application.platform == RuntimePlatform.OSXPlayer)
                {
                    Debug.Log("Restore Purchases started ...");
                    var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
                    apple.RestoreTransactions((result,info) =>
                    {
                        if (result)
                        {
                            Debug.Log("Restore Successful");
                            // Handle restored purchases (e.g., unlock content)
                            foreach (var product in m_StoreController.products.all)
                            {
                                if (product.hasReceipt)
                                {
                                    HandleRestoredPurchase(product);
                                }
                            }
                        }
                        else
                        {
                            Debug.Log("Restore Failed");
                        }
                    });
                }
                else if (Application.platform == RuntimePlatform.Android)
                {
                    Debug.Log("Restore Purchases are not necessary on Android");
                    //Android handles restores automatically.
                }
                else
                {
                    Debug.Log("Restore Purchases are not implemented on this platform");
                }
            }
            else
            {
                Debug.Log("Restore Purchases called before IAP is initialized");
            }
        }

        private void HandleRestoredPurchase(Product product)
        {
            //Unlock the content related to the restored purchase.
            Debug.Log("Restored purchase: " + product.definition.id);
            //Example: unlock a level.
            var packData = IAPPackHelper.GetPack(product.definition.id);
            UIManager.Instance. RestorePurchase(packData);
            DataController.Instance.RemoveAllListIAP();
            //if (product.definition.id == "your_non_consumable_id")
            //{
            //    //your code to unlock level.
            //}
        }
        public void RestorePurchase(Pack packData)
        {
            if (packData.Weapons.Length > 0)
            {
                foreach (var wp in packData.Weapons)
                {
                    var data = wp.Key.Split('_');
                    var wpid = DataController.Instance.GetWeaponIngameData((WeaponType)int.Parse(data[0]), int.Parse(data[1]));
                    wpid.isOwned = false;
                }
            }

            if (packData.Items.Length > 0)
            {

                foreach (var item in packData.Items)
                {
                    if (item.Key == "RemoveAds")
                    {

                        PlayerPrefs.SetInt("RemoveAds", 0);
                        ManagerAds.ins.ShowBanner();
                    }
                }
            }
        }


    }
}
