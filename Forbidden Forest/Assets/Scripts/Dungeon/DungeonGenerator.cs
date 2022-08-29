using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class DungeonGenerator {
    private Random _random;

    public DungeonGenerator(int seed){
        _random = new Random(seed);
    }
}
