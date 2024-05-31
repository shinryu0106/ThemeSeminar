using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace LibTS
{
    public class UiMover
    {
        private RectTransform _rectTransform = null;
        private List<Info_Transform> _info_Transforms = new();
        private int _nowPoint = 0;
        public int NowPoint { get { return _nowPoint; } }

        #region List
        /// <summary>
        /// 新しい動作演出ポイントを追加
        /// </summary>
        public void Add(Vector3 position = default, Quaternion rotation = default, Vector3 scale = default)
        {
            _info_Transforms.Add(new Info_Transform(
                position == default ? _rectTransform.position : position,
                rotation == default ? _rectTransform.rotation : rotation,
                scale == default ? _rectTransform.localScale : scale
            ));
        }

        /// <summary>
        /// 指定したインデックスの動作演出ポイントを削除
        /// </summary>
        public int Remove(int index)
        {
            if (index >= 0 && index < _info_Transforms.Count)
                _info_Transforms.RemoveAt(index);
            return _info_Transforms.Count;
        }
        #endregion

        #region Move
        /// <summary>
        /// 指定したインデックスの動作演出ポイントに移動（現在位置から強制的に移動）
        /// </summary>
        public void ForceMove(int index)
        {
            if (index >= 0 && index < _info_Transforms.Count)
            {
                _rectTransform.position = _info_Transforms[index].Position;
                _rectTransform.rotation = _info_Transforms[index].Rotation;
                _rectTransform.localScale = _info_Transforms[index].Scale;
            }
        }
        #endregion
    }
}