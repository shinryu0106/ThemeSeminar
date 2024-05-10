using LibTS;
using UnityEngine;

public class Sample_ForwardConstantly_Transform : MonoBehaviour
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
}