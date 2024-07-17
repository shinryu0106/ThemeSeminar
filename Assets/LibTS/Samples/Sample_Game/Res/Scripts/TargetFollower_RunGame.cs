using UnityEngine;

public class TargetFollower_RunGame : MonoBehaviour
{
    [Header("基本")]
    [SerializeField] private Transform _targetTransform = null;

    [Header("位置")]
    [SerializeField] private bool _allowPosition = true;
    [SerializeField] private Vector3 _adjustPosition = Vector3.zero;
    [SerializeField] private Vector3 _myOffset = Vector3.zero;

    [Header("回転")]
    [SerializeField] private bool _allowRotation = true;
    [SerializeField] private bool _isInverse = false;

    private void FollowPosition()
    {
        if (!_allowPosition)
            return;

        transform.position = _targetTransform.position + _myOffset;
    }

    private void FollowRotation()
    {
        if (!_allowRotation)
            return;

        Quaternion r = Quaternion.LookRotation(_targetTransform.position + _adjustPosition - transform.position);
        if (_isInverse)
            r *= Quaternion.Euler(0, 180, 0);
        transform.rotation = r;
    }

    private void Update()
    {
        if (_targetTransform == null)
            return;
            
        FollowPosition();
        FollowRotation();
    }
}
