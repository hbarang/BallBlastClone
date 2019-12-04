using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject InGameScreenCanvas;
    public static UIManager Instance;
    private TextMeshProUGUI DamageDealtText, TotalHpText, LevelText;
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

    }

    void Start()
    {
        DamageDealtText = InGameScreenCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TotalHpText = InGameScreenCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        LevelText = InGameScreenCanvas.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        DamageDealtText.text = "0";
        TotalHpText.text = GameManager.Instance.CurrentLevelTotalHp.ToString();
        LevelText.text = GameManager.Instance.CurrentLevel.ToString();


        GameManager.Instance.CurrentLevelTotalHpChanged += ChangeHpText;
        GameManager.Instance.CurrentLevelDamageChanged += ChangeDamageDealtText;
        GameManager.Instance.LevelChangedEvent += ChangeLevelText;
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


    private void OnDestroy()
    {
        GameManager.Instance.CurrentLevelDamageChanged -= ChangeDamageDealtText;
        GameManager.Instance.CurrentLevelTotalHpChanged -= ChangeHpText;
        GameManager.Instance.LevelChangedEvent -= ChangeLevelText;
    }


}
