using LibTS;
using UnityEngine;
using UnityEngine.UI;

public class ScoreChecker_RunGame : CollisionChecker
{
    [Header("計算")]
    [SerializeField] private MathematicalSymbols _symbol = MathematicalSymbols.Plus;
    [SerializeField] private int _coefficient = 1;

    [Header("見た目")]
    [SerializeField] private MeshRenderer _meshRenderer = null;
    [SerializeField] private Material _positiveMaterial = null;
    [SerializeField] private Material _negativeMaterial = null;
    [SerializeField] private Material _normalMaterial = null;
    [SerializeField] private Text _viewText = null;

    public void Init(MathematicalSymbols symbol, int coefficient)
    {
        _symbol = symbol;
        _coefficient = coefficient;

        switch (_symbol)
        {
            case MathematicalSymbols.Plus:
                _meshRenderer.sharedMaterial = _positiveMaterial;
                _viewText.text = "+" + _coefficient;
                break;
            case MathematicalSymbols.Minus:
                _meshRenderer.sharedMaterial = _negativeMaterial;
                _viewText.text = "-" + _coefficient;
                break;
            case MathematicalSymbols.Multiply:
                _meshRenderer.sharedMaterial = _positiveMaterial;
                _viewText.text = "×" + _coefficient;
                break;
            case MathematicalSymbols.Divide:
                _meshRenderer.sharedMaterial = _negativeMaterial;
                _viewText.text = "÷" + _coefficient;
                break;
            case MathematicalSymbols.Equal:
                _meshRenderer.sharedMaterial = _normalMaterial;
                _viewText.text = "=" + _coefficient;
                break;
        }
    }

    private void Calculate()
    {
        switch (_symbol)
        {
            case MathematicalSymbols.Plus:
                GameManager_RunGame.GameManagerStatic.Score += _coefficient;
                break;
            case MathematicalSymbols.Minus:
                GameManager_RunGame.GameManagerStatic.Score -= _coefficient;
                break;
            case MathematicalSymbols.Multiply:
                GameManager_RunGame.GameManagerStatic.Score *= _coefficient;
                break;
            case MathematicalSymbols.Divide:
                GameManager_RunGame.GameManagerStatic.Score /= _coefficient;
                break;
            case MathematicalSymbols.Equal:
                GameManager_RunGame.GameManagerStatic.Score = _coefficient;
                break;
        }
        GameManager_RunGame.GameManagerStatic.CheckGameEnd();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CollisionCheck(other))
        {
            Calculate();
            GateInstancer_RunGame.GateInstancerStatic.UpdateScoreChecker();
        }
    }
}

public enum MathematicalSymbols
{
    Plus,
    Minus,
    Multiply,
    Divide,
    Equal
}