using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.AssetManagement;
using Infrastructure.Services.IAP;
using Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace UI.Windows.Shop
{
    public class ShopItemsContainer : MonoBehaviour
    {
        private const string ShopItemPath = "ShopItem";
        
        public GameObject[] ShopUnavailableObjects;
        public Transform Parent;
        
        private IIAPService _iapService;
        private IPersistentProgressService _progressService;
        private IAssets _assets;
        private readonly List<GameObject> _shopItems = new List<GameObject>();


        public void Construct(IIAPService iapService, IPersistentProgressService progressSrvice, IAssets assets)
        {
            _iapService = iapService;
            _progressService = progressSrvice;
            _assets = assets;
        }

        public void Initialize()
        {
            RefreshAvailableItems();
        }

        public void Subscribe()
        {
            _iapService.Initialized += RefreshAvailableItems;
            _progressService.Progress.PurchaseData.Changed += RefreshAvailableItems;
        }

        public void Cleanup()
        {
            _iapService.Initialized -= RefreshAvailableItems;
            _progressService.Progress.PurchaseData.Changed -= RefreshAvailableItems;
        }

        private async void RefreshAvailableItems()
        {
            UpdateShopUnavailableObjects();
            
            if(!_iapService.IsInitialized)
                return;

            ClearShopItems();

            await FillShopItems();
        }

        private void ClearShopItems()
        {
            foreach (GameObject shopItem in _shopItems)
                Destroy(shopItem.gameObject);
        }

        private async Task FillShopItems()
        {
            foreach (ProductDescription productDescription in _iapService.Products())
            {
                GameObject shopItemObject = await _assets.Instantiate(ShopItemPath, Parent);
                ShopItem shopItem = shopItemObject.GetComponent<ShopItem>();

                shopItem.Construct(_iapService, productDescription, _assets);
                shopItem.Initialize();

                _shopItems.Add(shopItemObject);
            }
        }

        private void UpdateShopUnavailableObjects()
        {
            foreach (GameObject shopUnavailableObject in ShopUnavailableObjects)
            {
                shopUnavailableObject.SetActive(!_iapService.IsInitialized);
            }
        }
    }
}