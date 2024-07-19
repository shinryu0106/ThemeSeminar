using UnityEngine;
using LibTS;
using UnityEngine.Events;
using System.Collections;

public class MoveButton_RunGame : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private Vector3[] _positions;
    [SerializeField] private Vector3[] _scales;
    [SerializeField] private float[] _moveTimes;
    private RectTransform _rectTransform = null;
    private UiMover _mover = null;
    [SerializeField] private UnityEvent _onMoveEnd = null;

    public void Init()
    {
        StartCoroutine(InitCoroutine());
    }

    private IEnumerator InitCoroutine()
    {
        _mover.Move(0);
        Next();
        yield return new WaitForSeconds(_moveTimes[0]);
        Next();
    }

    public void Next()
    {
        _mover.Next(_moveTimes[_mover.NowPoint]);

        if (_mover.NowPoint == _positions.Length - 1)
            StartCoroutine(NextCoroutine());
    }

    private IEnumerator NextCoroutine()
    {
        yield return new WaitForSeconds(_moveTimes[2]);
        _onMoveEnd.Invoke();
    }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _mover = new UiMover(_rectTransform);
        for (int i = 0; i < _positions.Length; i++)
            _mover.Add(_positions[i], Quaternion.identity, _scales[i]);

        Init();
    }
}
