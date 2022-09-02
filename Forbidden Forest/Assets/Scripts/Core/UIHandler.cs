using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour{
    [SerializeField] private GameObject mapObject;
    private void Start() {
        EventManager.Instance.OnEncouterDone += OnEncounterDone;
        EventManager.Instance.OnNextRoom += OnNextRoom;
    }

    private void OnNextRoom(object sender, EventArgs empty) {
        mapObject.SetActive(false);
    }

    private void OnEncounterDone(object sender, EventArgs empty) {
        mapObject.SetActive(true);
    }
}
