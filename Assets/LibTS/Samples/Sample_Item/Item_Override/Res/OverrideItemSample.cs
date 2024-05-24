using UnityEngine;
using LibTS;

[CreateAssetMenu(fileName = "New Override Item", menuName = "LibTS/Override Item")]
public class OverrideItemSample : BaseItem
{
    public BaseItem Set(int id = 0, string name = "New Item", int count = 0, string description = "New Description")
    {
        base.Set(id, name, count);
        _description = description;
        return this;
    }

    [SerializeField] private string _description = "New Description";
    public string Description => _description;
}
