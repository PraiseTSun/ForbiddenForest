using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonProgression : MonoBehaviour {
    public static DungeonProgression Instance {get; private set;}

    private Room currentRoom;

    private void Awake() {
        if (Instance != null){
            Debug.LogError("The Instance DungeonProgression already exist → " + gameObject);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start() {
        currentRoom = GameHandler.Instance.dungeonGenerator.FirstRoom;
    }

    public void SelectDestinationRoom (Room room){
        currentRoom = room;
        EventManager.Instance.OnNextRoomTrigger();
    }

    public bool IsDetinationRoom(Room room){
        return currentRoom == null ? false : currentRoom.destinations.Contains(room);
    }
}
