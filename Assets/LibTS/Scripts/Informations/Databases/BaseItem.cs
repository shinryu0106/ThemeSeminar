using UnityEngine;

namespace LibTS
{
    [CreateAssetMenu(fileName = "New Base Item", menuName = "LibTS/Base Item")]
    public class BaseItem : ScriptableObject
    {
        public virtual BaseItem Set(int id = 0, string name = "New Item", int count = 0)
        {
            _id = id;
            _name = name;
            _count = count;
            
            return this;
        }

        [SerializeField] private int _id = 0;
        public int ID => _id;
        [SerializeField] private string _name = "New Item";
        public string Name => _name;
        [SerializeField] private int _count = 0;
        public int Count
        {
            get => _count;
            set => _count = value;
        }
    }
}