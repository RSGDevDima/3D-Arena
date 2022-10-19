using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameUI _gameUI;

    private Player _player;
    private int _score;
    private bool _isPaused;

    private void Start()
    {
        ResumeGame();
        _player = GameObject.FindObjectOfType<Player>();
        SubscribeListeners();
        InitScore();
    }

    private void Update()
    {
        CheckCursor();
    }

    private void InitScore()
    {
        _score = 0;
        GlobalEventManager.OnScoreInit.Fire(_score);
    }

    public void SubscribeListeners()
    {
        GlobalEventManager.OnRewardedEnemyDeath.AddListener(OnRewardedEnemyDeath);
        GlobalEventManager.OnExtraDeath.AddListener(OnExtraDeath);
        GlobalEventManager.OnEndgame.AddListener(StopGame);
    }

    public void OnRewardedEnemyDeath(Enemy enemy, float reward)
    {
        _player.ApplyStrenghtChanges(-reward);
        _score++;
        GlobalEventManager.OnScoreChange.Fire(_score);
    }

    public void OnExtraDeath()
    {
        if (_player.Health < _player.MaxHealth)
        {
            _player.ApplyHealthChanges(-(_player.MaxHealth / 2));
        }
        else
        {
            _player.ApplyStrenghtChanges(-(_player.MaxStrenght / 2));
        }
    }

    private void OnDisable()
    {
        GlobalEventManager.OnRewardedEnemyDeath.RemoveListener(OnRewardedEnemyDeath);
        GlobalEventManager.OnExtraDeath.RemoveListener(OnExtraDeath);
        GlobalEventManager.OnEndgame.RemoveListener(StopGame);
    }

    public void StopGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void CheckCursor()
    {
        if (Time.timeScale == 1 && Input.touchCount == 0){
            Cursor.lockState = CursorLockMode.Locked;
        } else 
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
