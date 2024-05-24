using System.Collections.Generic;
using UnityEngine;

namespace LibTS
{
    public static class ItemBundler
    {
        private static readonly Dictionary<string, Dictionary<int, BaseItem>> _bundle_items_libts = new();

        #region Bundle
        /// <summary>
        /// カテゴリー内のアイテム存在確認
        /// </summary>
        /// <remarks>
        /// IDで確認
        /// </remarks>
        public static bool ContainsItem(string categoryName, int id)
        {
            if (!_bundle_items_libts.ContainsKey(categoryName))
                return false;
            return _bundle_items_libts[categoryName].ContainsKey(id);
        }

        /// <summary>
        /// カテゴリー内のアイテム存在確認
        /// </summary>
        /// <remarks>
        /// 名前で確認
        /// </remarks>
        public static bool ContainsItem(string categoryName, string name)
        {
            if (!_bundle_items_libts.ContainsKey(categoryName))
                return false;
            foreach (var item in _bundle_items_libts[categoryName])
                if (item.Value.Name == name)
                    return true;
            return false;
        }

        /// <summary>
        /// カテゴリー内のアイテムを取得
        /// </summary>
        /// <remarks>
        /// IDで取得
        /// </remarks>
        public static BaseItem GetItem(string categoryName, int id)
        {
            if (!_bundle_items_libts.ContainsKey(categoryName))
            {
                Debug.LogWarning($"Bundle '{categoryName}' は存在しません。");
                return null;
            }
            if (!_bundle_items_libts[categoryName].ContainsKey(id))
            {
                Debug.LogWarning($"Item ID '{id}' は存在しません。");
                return null;
            }
            return _bundle_items_libts[categoryName][id];
        }

        /// <summary>
        /// カテゴリー内のアイテムを取得
        /// </summary>
        /// <remarks>
        /// 名前で取得
        /// </remarks>
        public static BaseItem GetItem(string categoryName, string name)
        {
            if (!_bundle_items_libts.ContainsKey(categoryName))
            {
                Debug.LogWarning($"Bundle '{categoryName}' は存在しません。");
                return null;
            }
            foreach (var item in _bundle_items_libts[categoryName])
            {
                if (item.Value.Name == name)
                {
                    return item.Value;
                }
            }
            Debug.LogWarning($"Item '{name}' は存在しません。");
            return null;
        }

        /// <summary>
        /// カテゴリーの追加
        /// </summary>
        public static void AddBundle(string categoryName)
        {
            if (_bundle_items_libts.ContainsKey(categoryName))
            {
                Debug.LogWarning($"Bundle '{categoryName}' は既に存在します。");
                return;
            }
            _bundle_items_libts.Add(categoryName, new Dictionary<int, BaseItem>());
        }

        /// <summary>
        /// カテゴリーの削除
        /// </summary>
        public static void DeleteBundle(string categoryName)
        {
            if (!_bundle_items_libts.ContainsKey(categoryName))
            {
                Debug.LogWarning($"Bundle '{categoryName}' は存在しません。");
                return;
            }
            _bundle_items_libts.Remove(categoryName);
        }
        #endregion

        #region Item
        /// <summary>
        /// カテゴリー内にアイテムを追加
        /// </summary>
        /// <remarks>
        /// BaseItemクラスを直接追加
        /// </remarks>
        public static void AddItem(string categoryName, BaseItem item)
        {
            if (!_bundle_items_libts.ContainsKey(categoryName))
                AddBundle(categoryName);
            if (_bundle_items_libts[categoryName].ContainsKey(item.ID))
            {
                Debug.LogWarning($"Item '{item.Name}' が既に存在しているか、IDが重複しています。");
                return;
            }
            _bundle_items_libts[categoryName].Add(item.ID, item);
        }

        /// <summary>
        /// カテゴリー内にアイテムを追加
        /// </summary>
        /// <remarks>
        /// ID, 名前, 個数を指定して追加
        /// </remarks>
        public static void AddItem(string categoryName, int id, string name, int count = 0)
        {
            AddItem(
                categoryName,
                ScriptableObject.CreateInstance<BaseItem>().Set(id, name, count)
            );
        }

        /// <summary>
        /// カテゴリー内にアイテムを追加
        /// </summary>
        /// <remarks>
        /// 名前, 個数を指定して追加（IDは自動生成）
        /// </remarks>
        public static int AddItem(string categoryName, string name, int count = 0)
        {
            int id = 0;
            while (_bundle_items_libts[categoryName].ContainsKey(id))
            {
                id++;
                if (id >= int.MaxValue)
                {
                    Debug.LogWarning("IDが枯渇しました。");
                    return -1;
                }
            }
            AddItem(
                categoryName,
                ScriptableObject.CreateInstance<BaseItem>().Set(id, name, count)
            );
            return id;
        }

        /// <summary>
        /// カテゴリー内のアイテムを削除
        /// </summary>
        /// <remarks>
        /// IDで削除
        /// </remarks>
        public static bool DeleteItem(string categoryName, int id)
        {
            if (!_bundle_items_libts.ContainsKey(categoryName))
            {
                Debug.LogWarning($"Bundle '{categoryName}' は存在しません。");
                return false;
            }
            if (!_bundle_items_libts[categoryName].ContainsKey(id))
            {
                Debug.LogWarning($"Item ID '{id}' は存在しません。");
                return false;
            }
            return _bundle_items_libts[categoryName].Remove(id);
        }

        /// <summary>
        /// カテゴリー内のアイテムを削除
        /// </summary>
        /// <remarks>
        /// 名前で削除
        /// </remarks>
        public static bool DeleteItem(string categoryName, string name)
        {
            if (!_bundle_items_libts.ContainsKey(categoryName))
            {
                Debug.LogWarning($"Bundle '{categoryName}' は存在しません。");
                return false;
            }
            foreach (var item in _bundle_items_libts[categoryName])
            {
                if (item.Value.Name == name)
                {
                    return _bundle_items_libts[categoryName].Remove(item.Key);
                }
            }
            Debug.LogWarning($"Item '{name}' は存在しません。");
            return false;
        }
        #endregion
    }
}
