using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectScene : MonoBehaviour{
    public Loader.Scene scene;
    public void SceneLoad(){
        Loader.Load(scene);
    }
}
