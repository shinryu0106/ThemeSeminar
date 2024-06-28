using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace LibTS
{
    [Serializable]
    public class UiMover
    {
        private readonly RectTransform _rectTransform = null;
        private readonly Selectable _selectable = null;
        [SerializeField] private List<Info_TransformUi> _info_Transforms = new();
        private int _nowPoint = 0;
        public int NowPoint { get { return _nowPoint; } }

        public UiMover(RectTransform rectTransform)
        {
            _rectTransform = rectTransform;
            _rectTransform.TryGetComponent(out _selectable);
        }

        #region List
        /// <summary>
        /// 新しい動作演出ポイントを追加
        /// </summary>
        /// <remarks>
        /// positionはVector3型、rotationはQuaternion型、scaleはVector3型で指定
        /// </remarks>
        public void Add(Vector3 position = default, Quaternion rotation = default, Vector3 scale = default) => _info_Transforms.Add(new Info_TransformUi(position, rotation, scale));

        /// <summary>
        /// 新しい動作演出ポイントを追加
        /// </summary>
        /// <remarks>
        /// positionはVector3型、eulerAnglesはVector3型、scaleはVector3型で指定
        /// </remarks>
        public void Add(Vector3 position = default, Vector3 eulerAngles = default, Vector3 scale = default) => _info_Transforms.Add(new Info_TransformUi(position, eulerAngles, scale));

        /// <summary>
        /// 新しい動作演出ポイントを追加
        /// </summary>
        /// <remarks>
        /// Transform型で指定
        /// </remarks>
        public void Add(Transform transform) => _info_Transforms.Add(new Info_TransformUi(transform));

        /// <summary>
        /// 新しい動作演出ポイントを追加
        /// </summary>
        /// <remarks>
        /// Info_TransformUi型で指定
        /// </remarks>
        public void Add(Info_TransformUi info_TransformUi) => _info_Transforms.Add(info_TransformUi);

        /// <summary>
        /// 新しい動作演出ポイントを追加（指定インデックスに挿入）
        /// </summary>
        /// <remarks>
        /// positionはVector3型、rotationはQuaternion型、scaleはVector3型で指定
        /// </remarks>
        public void Insert(int index, Vector3 position = default, Quaternion rotation = default, Vector3 scale = default) => _info_Transforms.Insert(index, new Info_TransformUi(position, rotation, scale));

        /// <summary>
        /// 新しい動作演出ポイントを追加（指定インデックスに挿入）
        /// </summary>
        /// <remarks>
        /// positionはVector3型、eulerAnglesはVector3型、scaleはVector3型で指定
        /// </remarks>
        public void Insert(int index, Vector3 position = default, Vector3 eulerAngles = default, Vector3 scale = default) => _info_Transforms.Insert(index, new Info_TransformUi(position, eulerAngles, scale));

        /// <summary>
        /// 新しい動作演出ポイントを追加（指定インデックスに挿入）
        /// </summary>
        /// <remarks>
        /// Transform型で指定
        /// </remarks>
        public void Insert(int index, Transform transform) => _info_Transforms.Insert(index, new Info_TransformUi(transform));

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
        /// 次の動作演出ポイントに移動（現在位置から指定時間で移動）
        /// </summary>
        public void Next(float time, bool allowInteractable = false)
        {
            Move(
                _nowPoint + 1 >= _info_Transforms.Count ? 0 : _nowPoint + 1,
                time,
                allowInteractable
            );
        }

        /// <summary>
        /// 次の動作演出ポイントに移動（現在位置から強制的に移動）
        /// </summary>
        public void Next()
        {
            Move(
                _nowPoint + 1 >= _info_Transforms.Count ? 0 : _nowPoint + 1
            );
        }

        /// <summary>
        /// 前の動作演出ポイントに移動（現在位置から指定時間で移動）
        /// </summary>
        public void Back(float time, bool allowInteractable = false)
        {
            Move(
                _nowPoint - 1 < 0 ? _info_Transforms.Count - 1 : _nowPoint - 1,
                time,
                allowInteractable
            );
        }

        /// <summary>
        /// 前の動作演出ポイントに移動（現在位置から強制的に移動）
        /// </summary>
        public void Back()
        {
            Move(
                _nowPoint - 1 < 0 ? _info_Transforms.Count - 1 : _nowPoint - 1
            );
        }

        /// <summary>
        /// 指定したインデックスの動作演出ポイントに移動（現在位置から指定時間で移動）
        /// </summary>
        public async void Move(int point, float time, bool allowInteractable = false)
        {
            if (point < 0 || point >= _info_Transforms.Count)
                return;

            if (!allowInteractable && _selectable != null)
                _selectable.interactable = false;
            
            int prePoint = _nowPoint;
            _nowPoint = point;
            float preTime = Time.time;
            // Debug.Log(prePoint + " -> " + _nowPoint);

            while (Time.time - preTime < time)
            {
                float rate = (Time.time - preTime) / time;
                _rectTransform.localPosition = Vector3.Lerp(_info_Transforms[prePoint].Position, _info_Transforms[point].Position, rate);
                _rectTransform.localRotation = Quaternion.Lerp(_info_Transforms[prePoint].Rotation, _info_Transforms[point].Rotation, rate);
                _rectTransform.localScale = Vector3.Lerp(_info_Transforms[prePoint].Scale, _info_Transforms[point].Scale, rate);
                
                await Task.Yield();
            }
            _rectTransform.localPosition = _info_Transforms[point].Position;
            _rectTransform.localRotation = _info_Transforms[point].Rotation;
            _rectTransform.localScale = _info_Transforms[point].Scale;

            if (_selectable != null)
                _selectable.interactable = true;
        }

        /// <summary>
        /// 指定したインデックスの動作演出ポイントに移動（現在位置から強制的に移動）
        /// </summary>
        public void Move(int point)
        {
            if (point < 0 || point >= _info_Transforms.Count)
                return;

            _rectTransform.position = _info_Transforms[point].Position;
            _rectTransform.rotation = _info_Transforms[point].Rotation;
            _rectTransform.localScale = _info_Transforms[point].Scale;
        }
        #endregion
    }
}