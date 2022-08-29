using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class DungeonGenerator {
    private Random _random;
    private LayerScriptableObject _layer;
    private List<Row> _rows;
    private int _roomCount;

    public DungeonGenerator(int seed, LayerScriptableObject layer) {
        _random = new Random(seed);
        _layer = layer;
        _rows = new List<Row>();

        Generate();
    }

    private void Generate() {
        CreateRows();
        AddRoomsToRows();
        StartingRoom();
        BossRoom();
        RoomPath();
    }

    private void RoomPath() {
        for(int i = 0; i < _rows.Count - 2; i++){
            Row origin = _rows[i];
            Row destination = _rows[i + 1];

            foreach(Room room in origin.rooms){
                Room roomRandom = destination.rooms[_random.Next(0, destination.Lenght)];
                room.destinations.Add(roomRandom);
                roomRandom.origins.Add(room);
            }

            foreach(Room room in destination.rooms){
                if(room.HasNoOrigins){
                    Room roomRandom = origin.rooms[_random.Next(0, origin.Lenght)];
                    room.origins.Add(roomRandom);
                    roomRandom.destinations.Add(room);
                }
            }
        }
    }

    private void BossRoom() {
        Row row = new Row();
        _rows.Add(row);

        row.rooms.Add(new Room());
    }

    private void AddRoomsToRows() {
        foreach(Row row in _rows){
            int roomCount = _random.Next(_layer.minRoom, _layer.maxRoom);

            for(int i = 0; i < roomCount; i++){
                row.rooms.Add(new Room());
                _roomCount++;
            }
        }
    }

    private void CreateRows() {
        for(int i = 0; i < _layer.rowCount; i++){
            _rows.Add(new Row());
        }
    }

    private void StartingRoom() {
        Row row = new Row();
        _rows.Insert(0, row);

        row.rooms.Add(new Room());
    }

    public Room FirstRoom => _rows[0].rooms[0];
}
