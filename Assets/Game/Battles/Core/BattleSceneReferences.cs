using System;
using System.Collections.Generic;
using Game.Battles;
using Game.Battles.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BattleSceneReferences : MonoBehaviour
{
    public VitalDefinition energyVital;
    public VitalDefinition enduranceVital;

    
    public List<BotNamePlate> allyNamePlates;
    public List<BotNamePlate> enemyNamePlates;
    
    private List<BotSo> _allys;
    private List<BotSo> _enemys;

    public void Init(List<BotSo> allys, List<BotSo> enemies)
    {
        _allys = allys;
        _enemys = enemies;
    }

    public void UpdateUi()
    {
        for (int i = 0; i < allyNamePlates.Count; i++)
        {
            allyNamePlates[i].name.text = _allys[i].name;
            allyNamePlates[i].enduranceSlider.maxValue = _allys[i].Vitals[enduranceVital].Max;
            allyNamePlates[i].enduranceSlider.value = _allys[i].Vitals[enduranceVital].Current;
            allyNamePlates[i].energySlider.maxValue = _allys[i].Vitals[energyVital].Max;
            allyNamePlates[i].energySlider.value = _allys[i].Vitals[energyVital].Current;

        }
        for (int i = 0; i < enemyNamePlates.Count; i++)
        {
            enemyNamePlates[i].name.text = _enemys[i].name;
            enemyNamePlates[i].enduranceSlider.maxValue = _enemys[i].Vitals[enduranceVital].Max;
            enemyNamePlates[i].enduranceSlider.value = _enemys[i].Vitals[enduranceVital].Current;
            enemyNamePlates[i].energySlider.maxValue = _enemys[i].Vitals[energyVital].Max;
            enemyNamePlates[i].energySlider.value = _enemys[i].Vitals[energyVital].Current;

        }
    }
}

[Serializable]
public class BotNamePlate
{

    
    
    [SerializeField] public TextMeshProUGUI name;
    [SerializeField] public Slider enduranceSlider;
    [SerializeField] public Slider energySlider;

}
