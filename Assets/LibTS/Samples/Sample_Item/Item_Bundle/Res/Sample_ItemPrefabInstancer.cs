using LibTS;
using UnityEngine;
using UnityEngine.UI;

public class Sample_ItemPrefabInstancer : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private InputField _categoryNameField;
    [SerializeField] private InputField _itemIdField;
    [SerializeField] private InputField _itemNameField;
    [SerializeField] private InputField _itemCountField;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private Transform _itemPrefabParent;

    public void InstantiateItemPrefab()
    {
        string cN = _categoryNameField.text;
        string iI = _itemIdField.text;
        string iN = _itemNameField.text;
        string iC = _itemCountField.text;

        if (string.IsNullOrEmpty(cN) || string.IsNullOrEmpty(iI) ||
            string.IsNullOrEmpty(iN) || string.IsNullOrEmpty(iC))
        {
            Debug.LogWarning("未入力の項目があります。");
            return;
        }
        if (!int.TryParse(iI, out int id))
        {
            Debug.LogWarning("IDは数値で入力してください。");
            return;
        }
        if (!int.TryParse(iC, out int cnt))
        {
            Debug.LogWarning("個数は数値で入力してください。");
            return;
        }
        if (cnt < 0)
            cnt = 0;

        if (ItemBundler.ContainsItem(cN, id))
        {
            Debug.LogWarning("同じIDのアイテムが既に存在します。");
            return;
        }
        
        var itemPrefabInstance = Instantiate(_itemPrefab, _itemPrefabParent);
        itemPrefabInstance.GetComponent<Sample_ItemPrefabController>().SetItemInformation(cN, id, iN, cnt);

        ItemBundler.AddItem(cN, id, iN, cnt);
    }
}
