using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour{
    public Button button;
    public void EndTurn(){
        button.enabled = false;
        CombatSystem.Instance.EndPhase();
    }
}
