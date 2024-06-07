using UnityEngine;
using LibTS;

public class Sample_MoveUi : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private RectTransform _rectTransform = null;
    [SerializeField] private Vector3[] _positions;
    [SerializeField] private Vector3[] _rotations;
    [SerializeField] private Vector3[] _scales;
    [SerializeField] private float[] _moveTimes;
    private UiMover _mover = null;

    public void Move()
    {
        _mover.Next(_moveTimes[_mover.NowPoint]);
    }

    private void Awake()
    {
        _mover = new UiMover(_rectTransform);
        for (int i = 0; i < _positions.Length; i++)
            _mover.Add(_positions[i], _rotations[i], _scales[i]);

        _rectTransform.localPosition = _positions[0];
        _rectTransform.localEulerAngles = _rotations[0];
        _rectTransform.localScale = _scales[0];
    }
}
