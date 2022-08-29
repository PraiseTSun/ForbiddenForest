using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class DungeonGenerator {
    private Random _random;
    private LayerScriptableObject _layer;
    private List<Row> _rows;

    public DungeonGenerator(int seed, LayerScriptableObject layer) {
        _random = new Random(seed);
        _layer = layer;
        _rows = new List<Row>();

        Generate();
    }

    private void Generate() {
        StartingRoom();
    }

    private void StartingRoom() {
        Row row = new Row();
        _rows.Add(row);

        row.rooms.Add(new Room());
    }

    public Room FirstRoom => _rows[0].rooms[0];
}
