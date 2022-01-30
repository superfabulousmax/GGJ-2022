using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _totalKilled;
    [SerializeField] private TMP_Text _totalHealed;
    [SerializeField] private TMP_Text _totalDamage;
    [SerializeField] private DataManager _dataManager;
    void Start()
    {
        _totalKilled.text = PlayerPrefs.GetInt("totalKilled").ToString();
        _totalHealed.text = PlayerPrefs.GetInt("totalHealed").ToString();
        _totalDamage.text = PlayerPrefs.GetInt("totalDamage").ToString();
    }

}
