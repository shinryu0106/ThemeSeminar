using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager_RunGame : MonoBehaviour
{
    private static GameManager_RunGame _gameManagerStatic = null;
    public static GameManager_RunGame GameManagerStatic
    {
        get => _gameManagerStatic;
    }

    [Header("基本")]
    [SerializeField] private UnityEvent _onGameEnd = null;

    [Header("スコア")]
    [SerializeField] private int _score = 1;
    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            if (_score < 0)
                _score = 0;
            else if (_score > 9999999)
                _score = 9999999;
            PlayerController_RunGame.PlayerControllerStatic.SetScoreText(_score);
        }
    }
    [SerializeField] private Text _finalScoreText = null;

    public void CheckGameEnd()
    {
        if (_score <= 0)
        {
            Debug.Log("Game Over...");
            EndGame();
        }
        else if (_score >= 9999999)
        {
            Debug.Log("Game Clear!!!");
            EndGame();
        }
    }

    public void InitGame()
    {
        Score = 1;
        PlayerController_RunGame.PlayerControllerStatic.Init(Random.Range(0, 2) == 0); // 0:Left, 1:Right
    }

    public void EndGame()
    {
        PlayerController_RunGame.PlayerControllerStatic.AllowMove = false;
        PlayerController_RunGame.PlayerControllerStatic.AllowOperation = false;

        _finalScoreText.text = _score.ToString("D7");
        _onGameEnd.Invoke();
    }

    private void Awake()
    {
        _gameManagerStatic = this;
    }
}
