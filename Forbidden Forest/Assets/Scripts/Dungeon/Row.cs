using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row
{
    public List<Room> rooms { get; private set; }

    public Row()
    {
        rooms = new List<Room>();
    }

    public int Lenght => rooms.Count;
}
