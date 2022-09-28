using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(20)]
public class CombatSystem : MonoBehaviour
{
    public static CombatSystem Instance { get; private set; }
    public event EventHandler OnPlayerTurn;
    public event EventHandler OnEnemyTurn;
    public event EventHandler OnDrawCard;

    private enum Phase
    {
        Player,
        Enemy
    }

    private Phase phase;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError(("CombatSytem already exist → " + gameObject));
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        StartPhase(Phase.Player);
    }

    private void StartPhase(Phase phase)
    {
        this.phase = phase;
        switch (phase)
        {
            case Phase.Player:
                OnPlayerTurn?.Invoke(this, EventArgs.Empty);
                break;
            case Phase.Enemy:
                OnEnemyTurn?.Invoke(this, EventArgs.Empty);
                EnemyPhase();
                break;
        }
    }

    private void EnemyPhase()
    {
        StartCoroutine(WaitEnemyTurn());
    }

    IEnumerator WaitEnemyTurn()
    {
        yield return new WaitForSeconds(2);
        EndPhase();
    }

    public void EndPhase()
    {
        switch (phase)
        {
            case Phase.Player:
                StartPhase(Phase.Enemy);
                break;

            case Phase.Enemy:
                StartPhase(Phase.Player);
                break;
        }
    }

    public void OnDrawCardTrigger(CardScriptableObject card)
    {
        OnDrawCard?.Invoke(this, EventArgs.Empty);
    }
}
