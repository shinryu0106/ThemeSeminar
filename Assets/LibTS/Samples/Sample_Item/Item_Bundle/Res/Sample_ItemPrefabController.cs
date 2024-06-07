using LibTS;
using UnityEngine;
using UnityEngine.UI;

public class Sample_ItemPrefabController : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private Text _categoryNameText;
    [SerializeField] private Text _itemIdText;
    [SerializeField] private Text _itemNameText;
    [SerializeField] private Text _itemCountText;
    private string _categoryName = "Item";
    private int _itemId = 0;

    public void SetItemInformation(string categoryName, int id, string name, int count)
    {
        _categoryName = categoryName;
        _itemId = id;

        ShowCategoryName(categoryName);
        ShowItemId(id);
        ShowItemName(name);
        ShowItemCount(count);
    }

    #region Category
    private void ShowCategoryName(string c) => _categoryNameText.text = c;
    #endregion

    #region ID
    public void DeleteItem()
    {
        ItemBundler.DeleteItem(_categoryName, _itemId);
        Destroy(gameObject);
    }

    private void ShowItemId(int i) => _itemIdText.text = "ID:" + i.ToString("D3");
    #endregion

    #region Name
    private void ShowItemName(string n) => _itemNameText.text = n;
    #endregion

    #region Count
    public void IncreaseItemCount()
    {
        var bI = ItemBundler.GetItem(_categoryName, _itemId);
        bI.Count++;
        ShowItemCount(bI.Count);
    }

    public void DecreaseItemCount()
    {
        var bI = ItemBundler.GetItem(_categoryName, _itemId);
        if (bI.Count > 0)
            bI.Count--;
        ShowItemCount(bI.Count);
    }

    private void ShowItemCount(int c) => _itemCountText.text = "個数:" + c.ToString("D3");
    #endregion
}
