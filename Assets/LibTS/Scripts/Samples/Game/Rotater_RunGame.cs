using UnityEngine;

public class Rotater_RunGame : MonoBehaviour
{
    [Header("回転")]
    [SerializeField] private float _rotateSpeed = 1f;
    [SerializeField] private Vector3 _frontAxis = Vector3.right;
    [SerializeField] private bool _allowRotation = true;
    public bool AllowRotation
    {
        set => _allowRotation = value;
    }

    private void Rotate()
    {
        if (!_allowRotation)
            return;
        transform.Rotate(_frontAxis, _rotateSpeed * Time.deltaTime);
    }

    private void Update()
    {
        Rotate();
    }
}
