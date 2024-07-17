using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateInstancer_RunGame : MonoBehaviour
{
    private static GateInstancer_RunGame _gateInstancerStatic = null;
    public static GateInstancer_RunGame GateInstancerStatic
    {
        get => _gateInstancerStatic;
    }

    [Header("基本")]
    [SerializeField] private GameObject _gatePrefab = null;
    [SerializeField] private Vector3 _leftPosition = new(-2.5f, 2f, 0f);
    [SerializeField] private Vector3 _rightPosition = new(2.5f, 2f, 0f);
    [SerializeField] private float _space = 15f;
    private float _nowSpace = 0f;
    private List<ScoreChecker_RunGame> _scoreCheckers = new();
    [SerializeField] private int _count = 8;
    [SerializeField] private float _updateWaitTime = 1.5f;
    
    [Header("確率")]
    [SerializeField] private ScoreInformation _plus = new();
    [SerializeField] private ScoreInformation _minus = new();
    [SerializeField] private ScoreInformation _multiply = new();
    [SerializeField] private ScoreInformation _divide = new();
    [SerializeField] private ScoreInformation _equal = new();
    [Serializable]
    private class ScoreInformation
    {
        [Range(1, 9999999)] public int _minCoefficient = 1;
        [Range(1, 9999999)] public int _maxCoefficient = 10;
        [Range(0f, 1f)] public float _probability = 0.2f;

        public void OnValidate()
        {
            if (_minCoefficient > _maxCoefficient)
                _minCoefficient = _maxCoefficient;
        }
    }

    private void OnValidate()
    {
        _plus.OnValidate();
        _minus.OnValidate();
        _multiply.OnValidate();
        _divide.OnValidate();
        _equal.OnValidate();
    }

    private void Instantiate()
    {
        Instantiate(new Vector3(_leftPosition.x, _leftPosition.y, _nowSpace));
        Instantiate(new Vector3(_rightPosition.x, _rightPosition.y, _nowSpace));
        _nowSpace += _space;
    }

    private void Instantiate(Vector3 position)
    {
        GameObject gate = Instantiate(_gatePrefab, position, Quaternion.identity);

        _scoreCheckers.Add(gate.GetComponentInChildren<ScoreChecker_RunGame>());
        if (_scoreCheckers[^1] != null)
        {
            MathematicalSymbols symbol = GetRandomSymbol();
            _scoreCheckers[^1].Init(symbol, GetRandomCoefficient(symbol));
        }
    }

    private MathematicalSymbols GetRandomSymbol()
    {
        float random = UnityEngine.Random.value;
        if (random < _plus._probability)
            return MathematicalSymbols.Plus;
        if (random < _plus._probability + _minus._probability)
            return MathematicalSymbols.Minus;
        if (random < _plus._probability + _minus._probability + _multiply._probability)
            return MathematicalSymbols.Multiply;
        if (random < _plus._probability + _minus._probability + _multiply._probability + _divide._probability)
            return MathematicalSymbols.Divide;
        return MathematicalSymbols.Equal;
    }

    private int GetRandomCoefficient(MathematicalSymbols symbol)
    {
        return symbol switch
        {
            MathematicalSymbols.Plus => UnityEngine.Random.Range(_plus._minCoefficient, _plus._maxCoefficient + 1),
            MathematicalSymbols.Minus => UnityEngine.Random.Range(_minus._minCoefficient, _minus._maxCoefficient + 1),
            MathematicalSymbols.Multiply => UnityEngine.Random.Range(_multiply._minCoefficient, _multiply._maxCoefficient + 1),
            MathematicalSymbols.Divide => UnityEngine.Random.Range(_divide._minCoefficient, _divide._maxCoefficient + 1),
            MathematicalSymbols.Equal => UnityEngine.Random.Range(_equal._minCoefficient, _equal._maxCoefficient + 1),
            _ => 1,
        };
    }

    public void UpdateScoreChecker()
    {
        StartCoroutine(UpdateScoreCheckerCoroutine());
    }

    private IEnumerator UpdateScoreCheckerCoroutine()
    {
        yield return new WaitForSeconds(_updateWaitTime);
        for (int i = 0; i < 2; i++)
        {
            ScoreChecker_RunGame temp = _scoreCheckers[0];
            MathematicalSymbols symbol = GetRandomSymbol();
            temp.Init(symbol, GetRandomCoefficient(symbol));
            temp.transform.root.position = new Vector3(temp.transform.root.position.x, temp.transform.root.position.y, _nowSpace);
            
            _scoreCheckers.RemoveAt(0);
            _scoreCheckers.Add(temp);
        }
        _nowSpace += _space;
    }

    public void Reset()
    {
        _nowSpace = _space;
        foreach (ScoreChecker_RunGame scoreChecker in _scoreCheckers)
            Destroy(scoreChecker.transform.root.gameObject);
        _scoreCheckers.Clear();
        for (int i = 0; i < _count; i++)
            Instantiate();
    }

    private void Awake()
    {
        _gateInstancerStatic = this;
    }
}
