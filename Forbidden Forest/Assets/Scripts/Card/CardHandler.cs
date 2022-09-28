using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHandler : MonoBehaviour
{
    [Header("Transform")]
    [SerializeField] private Transform deckPosition;

    [Header("Prefabs")]
    [SerializeField] private CardObject cardPrefabs;

    void Start()
    {
        EventManager.Instance.OnDrawCard += OnDrawCard;        
    }

    private void OnDrawCard(object sender, EventArgs e)
    {
        Instantiate(cardPrefabs, deckPosition.position, Quaternion.identity);
    }
}
