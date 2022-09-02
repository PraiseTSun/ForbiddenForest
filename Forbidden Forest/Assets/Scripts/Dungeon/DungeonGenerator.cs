using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class DungeonGenerator
{
    public Random random { get; private set; }
    private LayerScriptableObject _layer;
    private List<Row> _rows;
    private List<Room> _roomsWithoutType;

    public DungeonGenerator(int seed, LayerScriptableObject layer)
    {
        random = new Random(seed);
        _layer = layer;
        _rows = new List<Row>();
        _roomsWithoutType = new List<Room>();

        Generate();
    }

    private void Generate()
    {
        CreateRows();
        AddRoomsToRows();
        StartingRoom();
        BossRoom();
        RoomPath();
        AssignRoomsTypes();
    }

    private void AssignRoomsTypes()
    {
        int eliteCount = _roomsWithoutType.Count * _layer.eliteEncounter / 100;
        int specialCount = _roomsWithoutType.Count * _layer.specialEncounter / 100;

        AssignRoomsType(eliteCount, Room.Type.Elite);
        AssignRoomsType(specialCount, Room.Type.Special);
        AssignRoomsType(_roomsWithoutType.Count, Room.Type.Combat);
    }

    private void AssignRoomsType(int count, Room.Type type)
    {
        for (int i = 0; i < count; i++)
        {
            Room room = _roomsWithoutType[random.Next(0, _roomsWithoutType.Count)];
            _roomsWithoutType.Remove(room);
            room.type = type;
        }
    }

    private void RoomPath()
    {
        for (int i = 0; i < _rows.Count - 1; i++)
        {
            Row origin = _rows[i];
            Row destination = _rows[i + 1];

            foreach (Room room in origin.rooms)
            {
                Room roomRandom = destination.rooms[random.Next(0, destination.Lenght)];
                room.destinations.Add(roomRandom);
                roomRandom.origins.Add(room);
            }

            foreach (Room room in destination.rooms)
            {
                if (room.HasNoOrigins)
                {
                    Room roomRandom = origin.rooms[random.Next(0, origin.Lenght)];
                    room.origins.Add(roomRandom);
                    roomRandom.destinations.Add(room);
                }
            }
        }
    }

    private void BossRoom()
    {
        Row row = new Row();
        _rows.Add(row);

        row.rooms.Add(new Room());
    }

    private void AddRoomsToRows()
    {
        foreach (Row row in _rows)
        {
            int roomCount = random.Next(_layer.minRoom, _layer.maxRoom);

            for (int i = 0; i < roomCount; i++)
            {
                Room room = new Room();
                row.rooms.Add(room);
                _roomsWithoutType.Add(room);
            }
        }
    }

    private void CreateRows()
    {
        for (int i = 0; i < _layer.rowCount; i++)
        {
            _rows.Add(new Row());
        }
    }

    private void StartingRoom()
    {
        Row row = new Row();
        _rows.Insert(0, row);

        row.rooms.Add(new Room());
    }

    public Room FirstRoom => _rows[0].rooms[0];
    public List<Row> Rows => _rows;
    public int Lenght => _rows.Count;
}
