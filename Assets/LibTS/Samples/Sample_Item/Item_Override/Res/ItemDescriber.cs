using UnityEngine;
using LibTS;
using UnityEngine.UI;

public class ItemDescriber : MonoBehaviour
{
    const string CATEGORY = "Sample";

    [Header("設定")]
    [SerializeField] private BaseItem _item;
    [SerializeField] private OverrideItemSample _overrideItem;
    private int _id_base;
    private int _id_override;
    [SerializeField] private Text _text;

    public void Describe(bool isOverride = false)
    {
        if (isOverride)
            _text.text = ItemBundler.GetItem(CATEGORY, _id_override).Name +
                         " : " +
                         ((OverrideItemSample)ItemBundler.GetItem(CATEGORY, _id_override)).Description;
        else
            _text.text = ItemBundler.GetItem(CATEGORY, _id_base).Name +
                         " : " +
                         "No Description";
    }

    private void Awake()
    {
        ItemBundler.AddBundle(CATEGORY);
        ItemBundler.AddItem(CATEGORY, _item);
        ItemBundler.AddItem(CATEGORY, _overrideItem);
        _id_base = _item.ID;
        _id_override = _overrideItem.ID;
    }
}
