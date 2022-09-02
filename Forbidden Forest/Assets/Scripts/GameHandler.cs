using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance { get; private set; }

    public DungeonGenerator dungeonGenerator { get; private set; }
    [SerializeField] private int seed;
    [SerializeField] private LayerScriptableObject layer;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("The Instance GameHandler already exist! " + gameObject);
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // NewSeed();
        dungeonGenerator = new DungeonGenerator(seed, layer);
    }

    private void NewSeed()
    {
        seed = UnityEngine.Random.Range(0, int.MaxValue);
    }
}
