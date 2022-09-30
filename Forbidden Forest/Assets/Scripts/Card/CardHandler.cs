using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-20)]
public class CardHandler : MonoBehaviour
{
    [SerializeField] private float cardSpeed = 35f;
    [SerializeField] private float distanceBetweenCard = 1.25f;

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
    private List<CardObject> cardInHand;
    private List<CardObject> cardToMoveInHand;

    void Start()
    {
        cardsObj = new List<CardObject>();
        movingToHand = new List<CardObject>();
        movingToGraveyard = new List<CardObject>();
        cardInHand = new List<CardObject>();
        cardToMoveInHand = new List<CardObject>();

        CombatSystem.Instance.OnDrawCard += OnDrawCard;
        CombatSystem.Instance.OnEnemyTurn += OnEnemyTurn;
    }

    private void Update()
    {
        MovingCardFromDeckToHand();
        MovingCardFromHandToGraveyard();
        MovingCardInHand();
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
        cardInHand = new List<CardObject>();
        cardToMoveInHand = new List<CardObject>();
    }

    private void OnDrawCard(object sender, EventArgs e)
    {
        CardObject card = Instantiate(cardPrefabs, deckPosition.position, Quaternion.identity, transitionParent);
        movingToHand.Add(card);
        cardsObj.Add(card);
    }

    private void MovingCardFromDeckToHand()
    {
        for (int i = 0; i < movingToHand.Count; i++)
        {
            CardObject card = movingToHand[i];
            if (MoveCardToTarget(card, handParent.transform.position))
            {
                movingToHand.RemoveAt(i);
                AddCardToHand(card);
                card.transform.SetParent(handParent);
                i--;
            }
        }
    }

    private void MovingCardFromHandToGraveyard()
    {
        for (int i = 0; i < movingToGraveyard.Count; i++)
        {
            CardObject card = movingToGraveyard[i];
            if (MoveCardToTarget(card, graveyardParent.transform.position))
            {
                movingToGraveyard.RemoveAt(i);
                Destroy(card.gameObject);
                i--;
            }
        }
    }

    private void MovingCardInHand(){
        if(cardToMoveInHand.Count == 0)
            return;

        Vector3 move = Vector3.left * -distanceBetweenCard; 
        Vector3 initPos= new Vector3((int)(cardInHand.Count / 2) * distanceBetweenCard, 0f);
        if(cardInHand.Count % 2 == 0) initPos.x -= distanceBetweenCard / 2; 
        initPos += handParent.transform.position; 

        for(int i = 0; i < cardToMoveInHand.Count; i++){
            CardObject card = cardToMoveInHand[i];
            int index = cardInHand.IndexOf(card);
            if(MoveCardToTarget(card, initPos - move * index)){
                cardToMoveInHand.Remove(card);
                i--;
            }
        }
    }

    private void AddCardToHand(CardObject card)
    {
        cardInHand.Add(card);
        cardToMoveInHand = new List<CardObject>(cardInHand);
    }

    private bool MoveCardToTarget(CardObject card, Vector3 target)
    {
        float step = cardSpeed * Time.deltaTime;
        card.transform.position = Vector3.MoveTowards(card.transform.position, target, step);

        if (Vector3.Distance(card.transform.position, target) < 0.001f)
        {
            card.transform.position = target;
            return true;
        } 
        return false;
    }
}
