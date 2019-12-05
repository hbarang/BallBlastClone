using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject InGameScreenCanvas, StartScreenCanvas, GameWonScreenCanvas, GameLostScreenCanvas;
    public static UIManager Instance;
    private TextMeshProUGUI DamageDealtText, TotalHpText, LevelText, GameWonScore, GameLostScore;
    public Button TouchToPlayButton, GameWonReplayButton, GameLostReplayButton;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }

        else if (Instance != this)
        {
            Destroy(Instance);
        }

        DamageDealtText = InGameScreenCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TotalHpText = InGameScreenCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        LevelText = InGameScreenCanvas.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        TouchToPlayButton = StartScreenCanvas.transform.GetChild(0).GetComponent<Button>();

        GameWonReplayButton = GameWonScreenCanvas.transform.GetChild(0).GetComponent<Button>();
        GameWonScore = GameWonScreenCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        GameLostReplayButton = GameLostScreenCanvas.transform.GetChild(0).GetComponent<Button>();
        GameLostScore = GameLostScreenCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

    }

    void Start()
    {


        DamageDealtText.text = "0";
        TotalHpText.text = GameManager.Instance.CurrentLevelTotalHp.ToString();
        LevelText.text = GameManager.Instance.CurrentLevel.ToString();


        GameManager.Instance.CurrentLevelTotalHpChanged += ChangeHpText;
        GameManager.Instance.CurrentLevelDamageChanged += ChangeDamageDealtText;
        GameManager.Instance.LevelChangedEvent += ChangeLevelText;

        GameManager.Instance.GameStartedEvent += ActivateInGameScreen;
        GameManager.Instance.ActivateGameWonEvent += ActivateGameWonScreen;

        PlayerController.Instance.PlayerHitEvent += ActivateGameLostScreen;
        

        GameWonReplayButton.onClick.AddListener(GameWonReplayButtonClicked);
        GameLostReplayButton.onClick.AddListener(GameLostReplayButtonClicked);
    }


    void ChangeDamageDealtText(int hp)
    {
        DamageDealtText.text = hp.ToString();
    }

    void ChangeHpText(int hp)
    {
        TotalHpText.text = hp.ToString();
    }

    void ChangeLevelText()
    {
        LevelText.text = GameManager.Instance.CurrentLevel.ToString();
    }


    void OnDestroy()
    {

        GameManager.Instance.CurrentLevelDamageChanged -= ChangeDamageDealtText;
        GameManager.Instance.CurrentLevelTotalHpChanged -= ChangeHpText;
        GameManager.Instance.LevelChangedEvent -= ChangeLevelText;
        GameManager.Instance.GameStartedEvent -= ActivateInGameScreen;
        GameManager.Instance.ActivateGameWonEvent -= ActivateGameWonScreen;


    }

    void ActivateInGameScreen()
    {

        StartScreenCanvas.gameObject.SetActive(false);
        InGameScreenCanvas.gameObject.SetActive(!GameWonScreenCanvas.activeInHierarchy);

    }

    void ActivateGameWonScreen()
    {
        GameWonScore.text = DamageDealtText.text;
        InGameScreenCanvas.gameObject.SetActive(false);
        GameWonScreenCanvas.gameObject.SetActive(true);
    }

    void GameWonReplayButtonClicked()
    {
        Time.timeScale = 1f;
        InGameScreenCanvas.gameObject.SetActive(true);
        GameWonScreenCanvas.gameObject.SetActive(false);
    }

    void ActivateGameLostScreen()
    {
        GameLostScore.text = DamageDealtText.text;
        InGameScreenCanvas.gameObject.SetActive(false);
        GameLostScreenCanvas.gameObject.SetActive(true);
    }

    void GameLostReplayButtonClicked()
    {
        Time.timeScale = 1f;
        InGameScreenCanvas.gameObject.SetActive(true);
        GameLostScreenCanvas.gameObject.SetActive(false);
        GameManager.Instance.GameOver = false;

    }


}
