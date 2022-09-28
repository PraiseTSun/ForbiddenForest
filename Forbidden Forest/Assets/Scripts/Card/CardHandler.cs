using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-20)]
public class CardHandler : MonoBehaviour
{
    [Header("Transform")]
    [SerializeField] private Transform deckPosition;

    [Header("Prefabs")]
    [SerializeField] private CardObject cardPrefabs;

    private List<CardObject> cardsObj;

    void Start()
    {
        cardsObj = new List<CardObject>();
        CombatSystem.Instance.OnDrawCard += OnDrawCard;        
        CombatSystem.Instance.OnEnemyTurn += OnEnemyTurn;
    }

    private void OnEnemyTurn(object sender, EventArgs e)
    {
        while(cardsObj.Count > 0){
            CardObject card = cardsObj[0];
            cardsObj.RemoveAt(0);
            Destroy(card.gameObject);
        }
    }

    private void OnDrawCard(object sender, EventArgs e)
    {
        CardObject card = Instantiate(cardPrefabs, deckPosition.position, Quaternion.identity);
        cardsObj.Add(card);
    }
}
