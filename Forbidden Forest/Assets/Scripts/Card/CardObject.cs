using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardObject : MonoBehaviour
{
    [SerializeField] private SpriteRenderer icon;
    [SerializeField] private TextMeshPro title;
    [SerializeField] private TextMeshPro description;
    [SerializeField] private TextMeshPro energy;

    private CardScriptableObject data;

    public void SetUpCard(CardScriptableObject data){
        this.data = data;
        icon.sprite = data.icon;
        title.text = data.name;
        energy.text = data.energy.ToString();
    }

    public int getEnergy => data.energy;
}
