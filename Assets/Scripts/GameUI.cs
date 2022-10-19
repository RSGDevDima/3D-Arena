using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private GameObject _hitIndicatorObject;
    [SerializeField] private float _hitIndicatorTime = 1f;
    [SerializeField] private TMP_Text _killsText;
    [SerializeField] private TMP_Text _hpText;
    [SerializeField] private TMP_Text _strengthText;
    [SerializeField] private TMP_Text _endgameKillsText;
    [SerializeField] private GameObject _pauseScreen;
    [SerializeField] private GameObject _endgameScreen;
    [SerializeField] private GameObject _ultraButton;

    private Coroutine _hitIndicatorProcess;
    private float _hitTimeLeft = 0;

    private void OnEnable()
    {
        GlobalEventManager.OnScoreChange.AddListener(SetKills);
        GlobalEventManager.OnHealthChange.AddListener(SetHP);
        GlobalEventManager.OnStrenghtChange.AddListener(SetStrength);
        GlobalEventManager.OnEndgame.AddListener(ActivateEndgameScreen);
        GlobalEventManager.OnPlayerInit.AddListener(InitPlayerValues);
        GlobalEventManager.OnScoreInit.AddListener(InitScore);
        GlobalEventManager.OnUltraStateChanged.AddListener(ToggleUltraButton);
        GlobalEventManager.OnEnemyDamage.AddListener(ShowHitIndicator);
    }

    private void Update()
    {
        // pause
        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();
    }

    private void InitPlayerValues(float health, float maxHealth, float strength, float maxStrength)
    {
        SetHP(health, maxHealth);
        SetStrength(strength, maxStrength);
    }

    private void InitScore(int score)
    {
        SetKills(score);
    }

    private void SetKills(int score)
    {
        _killsText.text = score.ToString();
    }

    private void SetHP(float currentHp, float maxHp)
    {
        _hpText.text = currentHp + "/" + maxHp;
    }

    private void SetStrength(float currentStrength, float maxStrngth)
    {
        _strengthText.text = currentStrength + "/" + maxStrngth;
    }

    private void ActivateEndgameScreen()
    {
        SetEndgameKillsText(_killsText.text);
        OpenEndgame();
    }

    private void SetEndgameKillsText(string text)
    {
        _endgameKillsText.text = text;
    }

    public void CloseScreens()
    {
        _pauseScreen.active = false;
        _endgameScreen.active = false;
    }

    public void OpenEndgame()
    {
        _endgameScreen.active = true;
    }

    public void Pause()
    {
        _gameManager.StopGame();
        OpenPause();
    }

    public void OpenPause()
    {
        _pauseScreen.active = true;
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void ToggleUltraButton(bool ultraState)
    {
        if (ultraState)
        {
            _ultraButton.active = true;
        }
        else
        {
            _ultraButton.active = false;
        }
    }

    private void ShowHitIndicator(){
        Debug.Log($"Show hit...");
        _hitTimeLeft += _hitIndicatorTime;

        if(_hitIndicatorProcess == null){
            _hitIndicatorProcess = StartCoroutine(HitIndicatorProcess());
        }
    }

    private IEnumerator HitIndicatorProcess(){
        _hitIndicatorObject.SetActive(true);

        while(_hitTimeLeft > 0){
            _hitTimeLeft -= Time.deltaTime;
            yield return null;
        }

        _hitIndicatorProcess = null;
        _hitIndicatorObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GlobalEventManager.OnScoreChange.RemoveListener(SetKills);
        GlobalEventManager.OnHealthChange.RemoveListener(SetHP);
        GlobalEventManager.OnStrenghtChange.RemoveListener(SetStrength);
        GlobalEventManager.OnEndgame.RemoveListener(ActivateEndgameScreen);
        GlobalEventManager.OnPlayerInit.RemoveListener(InitPlayerValues);
        GlobalEventManager.OnScoreInit.RemoveListener(InitScore);
        GlobalEventManager.OnUltraStateChanged.RemoveListener(ToggleUltraButton);
        GlobalEventManager.OnEnemyDamage.RemoveListener(ShowHitIndicator);
    }
}
