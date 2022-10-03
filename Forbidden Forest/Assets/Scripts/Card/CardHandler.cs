using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-20)]
public class CardHandler : MonoBehaviour
{
    [Header("Card")]
    [SerializeField] private LayerMask cardLayer;
    [SerializeField] private float cardSpeed = 35f;

    [Header("Transform")]
    [SerializeField] private Transform deckPosition;
    [SerializeField] private Transform handParent;
    [SerializeField] private Transform transitionParent;
    [SerializeField] private Transform graveyardParent;
    [SerializeField] private float distanceBetweenCard = 1.25f;


    [Header("Prefabs")]
    [SerializeField] private CardObject cardPrefabs;


    private CardInput input;
    [SerializeField] private CardObject selectedCard;
    private Camera mainCam;

    private List<CardObject> cardsObj;
    private List<CardObject> movingToHand;
    private List<CardObject> movingToGraveyard;
    private List<CardObject> cardInHand;
    private List<CardObject> cardToMoveInHand;
    private DeckHandler deck;

    void Start()
    {
        cardsObj = new List<CardObject>();
        movingToHand = new List<CardObject>();
        movingToGraveyard = new List<CardObject>();
        cardInHand = new List<CardObject>();
        cardToMoveInHand = new List<CardObject>();

        input = CardInput.Instance;
        mainCam = Camera.main;
        deck = GetComponent<DeckHandler>();

        CombatSystem.Instance.OnDrawCard += OnDrawCard;
        CombatSystem.Instance.OnEnemyTurn += OnEnemyTurn;
    }

    private void Update()
    {
        MovingCardFromDeckToHand();
        MovingCardFromHandToGraveyard();
        MovingCardInHand();
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (input.onClick)
        {
            OnSelectCard();
        }
        else if (input.onRealize)
        {
            OnRealizeCard();
        }
        else if (input.onHeld)
        {
            OnCardHeld();
        }
    }

    private void OnCardHeld()
    {
        if (selectedCard == null)
            return;

        MoveCardToTarget(selectedCard, MouseInWorld);
    }

    private void OnRealizeCard()
    {
        if (selectedCard == null)
            return;

        cardToMoveInHand.Add(selectedCard);
        selectedCard = null;
    }

    private void OnSelectCard()
    {
        CardObject card = GetCardOnMousePosition();

        if (card == null) return;

        selectedCard = card;
        if (cardToMoveInHand.Contains(card)) cardToMoveInHand.Remove(card);
    }

    private CardObject GetCardOnMousePosition()
    {
        Vector2 target = MouseInWorld;
        RaycastHit2D[] hits = Physics2D.RaycastAll(target, Vector2.down, 1, cardLayer);

        if (hits.Length != 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                CardObject card = hit.collider.GetComponent<CardObject>();
                if (card != null && cardInHand.Contains(card))
                {
                    return card;
                }
            }
        }

        return null;
    }

    private Vector2 MouseInWorld => mainCam.ScreenToWorldPoint(input.mousePosition);

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
        card.SetUpCard(deck.CardDataAtIndex(cardsObj.Count));
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

    private void MovingCardInHand()
    {
        if (cardToMoveInHand.Count == 0)
            return;

        Vector3 move = Vector3.left * -distanceBetweenCard;
        Vector3 initPos = new Vector3((int)(cardInHand.Count / 2) * distanceBetweenCard, 0f);
        if (cardInHand.Count % 2 == 0) initPos.x -= distanceBetweenCard / 2;
        initPos += handParent.transform.position;

        for (int i = 0; i < cardToMoveInHand.Count; i++)
        {
            CardObject card = cardToMoveInHand[i];
            int index = cardInHand.IndexOf(card);
            if (MoveCardToTarget(card, initPos - move * index))
            {
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
        if (Vector3.Distance(card.transform.position, target) < 0.001f)
        {
            card.transform.position = target;
            return true;
        }

        float step = cardSpeed * Time.deltaTime;
        card.transform.position = Vector3.MoveTowards(card.transform.position, target, step);

        return false;
    }
}
