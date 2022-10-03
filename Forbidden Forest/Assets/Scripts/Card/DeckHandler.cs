using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class DeckHandler : MonoBehaviour
{
    [SerializeField] private List<CardScriptableObject> originalDeck;
    [SerializeField] private int seed;
    [SerializeField] private int nbDraw = 5;
    [SerializeField] private List<CardScriptableObject> drawPile;
    [SerializeField] private List<CardScriptableObject> discardPile;
    [SerializeField] private List<CardScriptableObject> hand;
    private Random random;

    private void Start()
    {
        random = new Random(seed);
        drawPile = new List<CardScriptableObject>(originalDeck);
        discardPile = new List<CardScriptableObject>();
        hand = new List<CardScriptableObject>();
        CombatSystem.Instance.OnPlayerTurn += OnPlayerTurn;
        CombatSystem.Instance.OnEnemyTurn += OnEnemyTurn;
    }

    private void OnPlayerTurn(object sender, EventArgs empty)
    {
        DrawHand();
    }

    private void OnEnemyTurn(object sender, EventArgs empty)
    {
        DiscardAllCard();
    }

    private void DiscardAllCard()
    {
        discardPile.AddRange(hand);
        hand = new List<CardScriptableObject>();
    }

    private void DrawHand()
    {
        for (int i = 0; i < nbDraw; i++)
        {
            DrawCard();
        }
    }

    private void DrawCard()
    {
        if (drawPile.Count == 0)
        {
            drawPile.AddRange(discardPile);
            discardPile = new List<CardScriptableObject>();

            if (drawPile.Count == 0)
                return;
        }

        int pos = random.Next(0, drawPile.Count);
        CardScriptableObject card = drawPile[pos]; 
        hand.Add(card);
        CombatSystem.Instance.OnDrawCardTrigger(card);
        drawPile.RemoveAt(pos);
    }

    public CardScriptableObject CardDataAtIndex(int index){
        return hand[index];
    }
}
