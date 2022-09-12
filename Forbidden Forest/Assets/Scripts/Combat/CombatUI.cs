using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour {
    public GameObject playerBanner;
    public GameObject enemyBanner;
    public Button endTurnButton;

    private void Start() {
        CombatSystem.Instance.OnPlayerTurn += OnPlayerTurn;
        CombatSystem.Instance.OnEnemyTurn += OnEnemyTurn;
    }

    private void OnEnemyTurn(object sender, EventArgs empty)
    {
        endTurnButton.enabled = false;
        StartCoroutine(SpawnBanner(enemyBanner));
    }

    private void OnPlayerTurn(object sender, EventArgs empty)
    {
        StartCoroutine(SpawnBanner(playerBanner));
        endTurnButton.enabled = true;
    }

    IEnumerator SpawnBanner(GameObject banner){
        banner.SetActive(true);
        yield return new WaitForSeconds(1);
        banner.SetActive(false);
    }
}
