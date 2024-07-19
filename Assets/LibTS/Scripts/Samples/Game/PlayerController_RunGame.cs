using System.Collections;
using LibTS;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController_RunGame : MonoBehaviour
{
    private static PlayerController_RunGame _playerControllerStatic = null;
    public static PlayerController_RunGame PlayerControllerStatic
    {
        get => _playerControllerStatic;
    }

    [Header("基本")]
    [SerializeField] private bool _allowMove = true;
    public bool AllowMove
    {
        set
        {
            _allowMove = value;
            foreach (var rotater in _rotaters)
            {
                rotater.AllowRotation = value;
            }
        }
    }
    [SerializeField] private float _moveSpeed = 6f;
    [SerializeField] private AnimationCurve _speedCurve = null;
    [SerializeField] private float _speedAdjustment = 0.1f;
    private float _startTime = 0f;
    [SerializeField] private Vector3 _direction = Vector3.forward;
    [SerializeField] private Vector3 _defaultPosition = new(0f, 0.25f, 0f);

    [Header("操作")]
    [SerializeField] private bool _allowOperation = true;
    public bool AllowOperation
    {
        set => _allowOperation = value;
    }
    private bool _isLeft = true;
    public bool IsLeft
    {
        set => _isLeft = value;
    }
    [SerializeField] private Transform _body = null;
    [SerializeField] private Vector3 _leftPosition = new(-2.5f, 0f, 0f);
    [SerializeField] private Vector3 _rightPosition = new(2.5f, 0f, 0f);
    [SerializeField] private float _animationTime = 1f;
    private Coroutine _animateCoroutine = null;
    [SerializeField] private Rotater_RunGame[] _rotaters;

    [Header("スコア")]
    [SerializeField] private Text _text = null;

    public void SetScoreText(int score)
    {
        _text.text = score.ToString("D7");
    }

    public void Init(bool isLeft)
    {
        AllowMove = true;
        _allowOperation = true;
        _startTime = 0f;

        transform.position = _defaultPosition;

        _isLeft = isLeft;
        _body.localPosition = isLeft ? _leftPosition : _rightPosition;
    }

    private void Operate()
    {
        if (!_allowOperation)
            return;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            AnimateLeft();
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            AnimateRight();
    }

    private void AnimateLeft()
    {
        if (_isLeft || _animateCoroutine != null)
            return;
        _isLeft = true;
    
        if (_animateCoroutine != null)
            StopCoroutine(_animateCoroutine);
        _animateCoroutine = StartCoroutine(AnimateCoroutine(_leftPosition));
    }

    private void AnimateRight()
    {
        if (!_isLeft || _animateCoroutine != null)
            return;
        _isLeft = false;
    
        if (_animateCoroutine != null)
            StopCoroutine(_animateCoroutine);
        _animateCoroutine = StartCoroutine(AnimateCoroutine(_rightPosition));
    }

    private IEnumerator AnimateCoroutine(Vector3 pos)
    {
        float t = Time.time;
        while (true)
        {
            float d = Time.time - t;
            _body.localPosition = Vector3.Lerp(_body.localPosition, pos, d / _animationTime);

            if (d >= _animationTime)
                break;
            yield return null;
        }
        _animateCoroutine = null;
    }

    private void Move()
    {
        if (!_allowMove)
            return;

        _moveSpeed = _speedCurve.Evaluate(_startTime * _speedAdjustment);
        transform.Forward(_direction, _moveSpeed, 0f);
    }

    private void Awake()
    {
        _playerControllerStatic = this;
        _startTime = 0f;
    }

    private void Update()
    {
        Operate();
        _startTime += Time.deltaTime;
        Move();
    }
}
