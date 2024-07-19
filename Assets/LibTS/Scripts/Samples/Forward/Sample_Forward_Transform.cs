using LibTS;
using UnityEngine;

public class Sample_Forward_Transform : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _rotateSpeed = 1f;
    [SerializeField] private TimeType _timeType = TimeType.Default;

    [Header("参照")]
    [SerializeField] private Transform _target = null;
    [SerializeField] private Vector3 _direction = Vector3.forward;

    [Header("デバッグ")]
    [SerializeField] private bool _allowDebug = true;

    private void Update()
    {
        Info_Forward i = default;
        if (_target != null)
        {
            i = transform.Forward(_target, _moveSpeed, _rotateSpeed, _timeType);
            if (_allowDebug)
            {
                Debug.Log($"移動距離: {i.Distance}, 回転角度: {i.Angle}");
                Debug.Log($"移動方向: {i.Direction}");
                Debug.Log($"目標地点に到達: {i.IsGoal}");
            }
        }
        else
        {
            i = transform.Forward(_direction, _moveSpeed, _rotateSpeed, _timeType);
            if (_allowDebug)
                Debug.Log($"移動距離: {i.Distance}, 回転角度: {i.Angle}");
        }
    }
}