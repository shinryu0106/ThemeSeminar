using LibTS;
using UnityEngine;

public class Sample_Forward_Transform : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _rotateSpeed = 1f;
    [SerializeField] private TimeType _timeType = TimeType.Default;
    [SerializeField] private float _error = 0.1f;

    [Header("参照")]
    [SerializeField] private Transform _target = null;
    [SerializeField] private Vector3 _direction = Vector3.forward;

    [Header("デバッグ")]
    [SerializeField] private bool _allowDebug = true;

    private void Start()
    {
        this.ForwardConstantly(_direction, 3f, _moveSpeed, _rotateSpeed, _timeType);
    }

    // private void Update()
    // {
    //     if (_target != null)
    //     {
    //         var t = transform.Forward(_target, out Vector3 direction, _moveSpeed, _rotateSpeed, _error, _timeType);
    //         if (_allowDebug)
    //         {
    //             Debug.Log($"移動距離: {t.Item1}, 回転角度: {t.Item2}");
    //             Debug.Log($"移動方向: {direction}");
    //         }
    //     }
    //     else
    //     {
    //         var t = transform.Forward(_direction, _moveSpeed, _rotateSpeed, _timeType);
    //         if (_allowDebug)
    //             Debug.Log($"移動距離: {t.Item1}, 回転角度: {t.Item2}");
    //     }
    // }
}