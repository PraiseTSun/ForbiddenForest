using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MapHandler : MonoBehaviour {
    public static MapHandler Instance {get; private set;}

    [SerializeField] private Vector2 borderOffset;

    [Header("Prefabs")]
    [SerializeField] private RoomObject roomPrefab;
    [SerializeField] private RectTransform pathPrefab;

    [Header("Parent")]
    [SerializeField] private RectTransform roomParent;
    [SerializeField] private RectTransform pathParent;

    private Vector3 _mapOffset;
    private Vector3 _size;
    private Dictionary<Room, Vector2> _roomPosition;
    private Random _random;
    
    private void Awake() {
        if(Instance != null){
            Debug.LogError("The Instance MapHandler already exist! " + gameObject);
            Destroy(gameObject);
            return;
        }

        Instance = this;
        SetUpOffsets();
    }

    private void SetUpOffsets(){
        RectTransform size = GetComponent<RectTransform>();
        _mapOffset = transform.position - new Vector3(size.sizeDelta.x / 2, size.sizeDelta.y / 2, 0f);
        _size = size.sizeDelta;
    }
    

    public void GenerateMap(Random random){
        _random = random;
        GenerateRooms();
    }

    private void GenerateRooms() {
        List<Row> rows = GameHandler.Instance.dungeonGenerator.Rows;

        Instantiate(roomPrefab, _mapOffset + new Vector3(0.5f * _size.x, 100f, 0f), Quaternion.identity, roomParent);
    }

    private Vector3 GetRoomPosition(float height){


        return new Vector3();
    }
}
