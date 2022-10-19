using UnityEngine;

public class GameManager : MonoBehaviour
{
    Player player;
    int score;

    void Start()
    {
        ResumeGame();
        player = GameObject.FindObjectOfType<Player>();
        SubscribeListeners();
        initScore();
    }

    void initScore()
    {
        score = 0;
        GlobalEventManager.OnScoreInit.Fire(score);
    }

    public void SubscribeListeners() {
        GlobalEventManager.OnRewardedEnemyDeath.AddListener(OnRewardedEnemyDeath);
        GlobalEventManager.OnExtraDeath.AddListener(OnExtraDeath);
        GlobalEventManager.OnEndgame.AddListener(PauseGame);
    }
    
    public void OnRewardedEnemyDeath(Enemy enemy, float reward){
        player.ApplyStrenghtChanges(-reward);
        score++;
        GlobalEventManager.OnScoreChange.Fire(score);
    }

    public void OnExtraDeath(){
        if(player.Health < player.MaxHealth){
            player.ApplyHealthChanges(-(player.MaxHealth / 2));
        } else{
            player.ApplyStrenghtChanges(-(player.MaxStrenght / 2));
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    private void OnDisable()
    {
        GlobalEventManager.OnRewardedEnemyDeath.RemoveListener(OnRewardedEnemyDeath);
        GlobalEventManager.OnExtraDeath.RemoveListener(OnExtraDeath);
        GlobalEventManager.OnEndgame.RemoveListener(PauseGame);
    }
}
