using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room {
    public enum Type{
        Combat,
        Elite,
        Special,
        Boss,
        Starting,
    }

    public List<Room> origins {get; private set;}
    public List<Room> destinations {get; private set;}
    public Type type;

    public Room(){
        origins = new List<Room>();
        destinations = new List<Room>();
    }


    public bool HasNoOrigins => origins.Count == 0;
}
