using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-20)]
public class CardHandler : MonoBehaviour
{
    [SerializeField] private float cardSpeed = 35f;
    [SerializeField] private float handWidth = 12f;

    [Header("Transform")]
    [SerializeField] private Transform deckPosition;
    [SerializeField] private Transform handParent;
    [SerializeField] private Transform transitionParent;
    [SerializeField] private Transform graveyardParent;

    [Header("Prefabs")]
    [SerializeField] private CardObject cardPrefabs;

    private List<CardObject> cardsObj;
    private List<CardObject> movingToHand;
    private List<CardObject> movingToGraveyard;

    void Start()
    {
        cardsObj = new List<CardObject>();
        movingToHand = new List<CardObject>();
        movingToGraveyard = new List<CardObject>();
        CombatSystem.Instance.OnDrawCard += OnDrawCard;
        CombatSystem.Instance.OnEnemyTurn += OnEnemyTurn;
    }

    private void Update()
    {
        MovingCardFromDeckToHand();
        MovingCardFromHandToGraveyard();
    }

    private void OnEnemyTurn(object sender, EventArgs e)
    {
        while (cardsObj.Count > 0)
        {
            CardObject card = cardsObj[0];
            movingToGraveyard.Add(card);
            cardsObj.RemoveAt(0);
            card.transform.SetParent(transitionParent);
        }

        movingToHand = new List<CardObject>();
    }

    private void OnDrawCard(object sender, EventArgs e)
    {
        CardObject card = Instantiate(cardPrefabs, deckPosition.position, Quaternion.identity, transitionParent);
        movingToHand.Add(card);
        cardsObj.Add(card);
    }

    private void MovingCardFromDeckToHand()
    {
        for(int i = 0; i < movingToHand.Count; i++)
        {
            CardObject card = movingToHand[i];
            float step = cardSpeed * Time.deltaTime;
            card.transform.position = Vector3.MoveTowards(card.transform.position, handParent.position, step);

            if (Vector3.Distance(card.transform.position, handParent.position) < 0.001f)
            {
                movingToHand.RemoveAt(i);
                card.transform.SetParent(handParent);
                i--;
            }
        }
    }

    private void MovingCardFromHandToGraveyard()
    {
        for(int i = 0; i < movingToGraveyard.Count; i++)
        {
            CardObject card = movingToGraveyard[i];
            float step = cardSpeed * Time.deltaTime;
            card.transform.position = Vector3.MoveTowards(card.transform.position, graveyardParent.position, step);

            if (Vector3.Distance(card.transform.position, graveyardParent.position) < 0.001f)
            {
                movingToGraveyard.RemoveAt(i);
                Destroy(card.gameObject);
                i--;
            }
        }
    }
}
