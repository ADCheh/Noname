using Infrastructure.AssetManagement;
using Infrastructure.Services.IAP;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Shop
{
    public class ShopItem : MonoBehaviour
    {
        public Button BuyItemButton;
        public TextMeshProUGUI PriceText;
        public TextMeshProUGUI QuantityText;
        public TextMeshProUGUI AvailableItemsLeftText;
        public Image Icon;
        
        private ProductDescription _productDescription;
        private IIAPService _iapService;
        private IAssets _assets;

        public void Construct(IIAPService iapService, ProductDescription productDescription, IAssets assets)
        {
            _iapService = iapService;
            _productDescription = productDescription;
            _assets = assets;
        }

        public async void Initialize()
        {
            BuyItemButton.onClick.AddListener(OnBuyItemClick);

            PriceText.text = _productDescription.Config.Price;
            QuantityText.text = _productDescription.Config.Quantity.ToString();
            AvailableItemsLeftText.text = _productDescription.AvailablePurchasesLeft.ToString();
            Icon.sprite = await _assets.Load<Sprite>(_productDescription.Config.Icon);
        }

        private void OnBuyItemClick()
        {
            _iapService.StartPurchase(_productDescription.Id);
        }
    }
}