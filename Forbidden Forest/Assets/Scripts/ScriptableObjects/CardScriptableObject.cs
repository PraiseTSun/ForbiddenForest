using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/Card")]
public class CardScriptableObject : ScriptableObject {
    public int energy;
    public CardType type;
    public CardEffect effect;
    public int value;
    public Sprite icon;
    public CardScriptableObject upgrate;
}
