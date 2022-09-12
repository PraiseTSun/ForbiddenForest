using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObject/Card")]
public class CardScriptableObject : ScriptableObject {
    public CardType type;
    public CardEffect effect;
    public int value;
    public CardScriptableObject upgrate;
}
