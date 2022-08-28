using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Layer", menuName = "ScriptableObjects/Layer")]
public class LayerScriptableObject : ScriptableObject {
    public string naming;
    public int rowCount;
}