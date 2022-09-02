using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour{
    [SerializeField] private GameObject mapObject;
    [SerializeField] private GameObject rewardObject;
    private void Start() {
        EventManager.Instance.OnEncouterDone += OnEncounterDone;
        EventManager.Instance.OnNextRoom += OnNextRoom;
        EventManager.Instance.OnWin += OnWin;
    }

    private void OnWin(object sender, EventArgs empty) {
        rewardObject.SetActive(true);
    }

    private void OnNextRoom(object sender, EventArgs empty) {
        mapObject.SetActive(false);
    }

    private void OnEncounterDone(object sender, EventArgs empty) {
        mapObject.SetActive(true);
        rewardObject.SetActive(false);
    }
}
