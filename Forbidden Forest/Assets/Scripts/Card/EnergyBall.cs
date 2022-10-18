using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class EnergyBall : MonoBehaviour
{
    public static EnergyBall Instance {get; private set;}
    [SerializeField] private TextMeshPro text;
    [SerializeField] private int maxEnergy = 3;
    private int currentEnergy;

    private void Awake() {
        if(Instance != null){
            Debug.LogError("EnergyBall is already used → " + gameObject);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start() {
        CombatSystem.Instance.OnPlayerTurn += OnPlayerTurn;
    }

    private void OnPlayerTurn(object sender, EventArgs e)
    {
        currentEnergy = maxEnergy;
        adjustEnergy();
    }

    public bool tryToPlay(int consume){
        if(currentEnergy - consume >= 0){
            currentEnergy -= consume;
            adjustEnergy();
            return true;
        }
        return false;
    }

    private void adjustEnergy(){
        text.text = currentEnergy + " / " + maxEnergy;
    }
}
