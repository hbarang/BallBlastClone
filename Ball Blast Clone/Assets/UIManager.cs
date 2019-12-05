using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject InGameScreenCanvas, StartScreenCanvas;
    public static UIManager Instance;
    private TextMeshProUGUI DamageDealtText, TotalHpText, LevelText;
    public Button TouchToPlayButton;

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

    }

    void ActivateInGameScreen()
    {

        StartScreenCanvas.gameObject.SetActive(false);
        InGameScreenCanvas.gameObject.SetActive(true);

    }


}
