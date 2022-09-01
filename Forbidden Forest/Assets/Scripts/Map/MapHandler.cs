using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MapHandler : MonoBehaviour {
    public static MapHandler Instance {get; private set;}

    [SerializeField] private Vector2Int borderOffset;
    [SerializeField] private float rowHeight = 100f;
    [SerializeField] private float roomRadius = 250f;

    [Header("Prefabs")]
    [SerializeField] private RoomObject roomPrefab;
    [SerializeField] private RectTransform pathPrefab;

    [Header("Parent")]
    [SerializeField] private RectTransform roomParent;
    [SerializeField] private RectTransform pathParent;
    [SerializeField] private RectTransform scrollObject;

    private Vector3 _mapOffset;
    private Vector3Int _size;
    private Dictionary<Room, Vector2> _roomPosition;
    private List<Vector3> _roomRowPos;
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

    private void Start() {
        GenerateMap();
        gameObject.SetActive(false);
    }

    private void SetUpOffsets(){
        RectTransform size = GetComponent<RectTransform>();
        _mapOffset = transform.position - new Vector3(size.sizeDelta.x / 2, size.sizeDelta.y / 2, 0f);
        
        _size = new Vector3Int (
            (int) size.sizeDelta.x,
            (int) size.sizeDelta.y,
            0
        );
    }
    

    public void GenerateMap(){
        _random = GameHandler.Instance.dungeonGenerator.random;
        _roomPosition = new Dictionary<Room, Vector2>();
        AdjustScrollHeight();
        GenerateRooms();
        GeneratePath();
    }

    private void GeneratePath() {
        List<Row> rows = GameHandler.Instance.dungeonGenerator.Rows;

        foreach(Row row in rows){
            foreach(Room room in row.rooms){
                foreach (Room target in room.destinations) {
                    float angle = VectorsAngle(_roomPosition[room], _roomPosition[target]);
                    float width = (_roomPosition[room] - _roomPosition[target]).magnitude;

                    RectTransform rect = Instantiate(pathPrefab, _roomPosition[room], 
                        Quaternion.Euler(new Vector3(0f, 0f, angle)), 
                        pathParent);
                    rect.sizeDelta = new Vector2(width, 32f);
                }
            }
        }
    }

    private float VectorsAngle(Vector3 origin, Vector3 target){
        Vector3 targetDir = target - origin;
        return Vector3.Angle(targetDir, transform.right);
    }

    private void AdjustScrollHeight() {
        int rowCount = GameHandler.Instance.dungeonGenerator.Lenght;

        Vector2 size = new Vector2(
            scrollObject.sizeDelta.x,
            rowCount * rowHeight + borderOffset.y * 2
        );

        scrollObject.sizeDelta = size;
    }

    private void GenerateRooms() {
        List<Row> rows = GameHandler.Instance.dungeonGenerator.Rows;
        
        Vector3 center = _mapOffset + new Vector3(0.5f * _size.x, borderOffset.y, 0f);
        RoomObject roomObj = Instantiate(roomPrefab, center, Quaternion.identity, roomParent);
        _roomPosition.Add(rows[0].rooms[0], center);
        roomObj.gameObject.name = "Start";

        for(int i = 1; i < rows.Count - 1; i++){
            float height = i * rowHeight;
            _roomRowPos = new List<Vector3>();

            foreach(Room room in rows[i].rooms){
                Vector3 position = GetRoomPosition(height);
                _roomPosition.Add(room, position);
                _roomRowPos.Add(position);
                Instantiate(roomPrefab, position, Quaternion.identity, roomParent);
            }
        }

        center += Vector3.up * (rows.Count - 1 ) * rowHeight;
        roomObj = Instantiate(roomPrefab, center, Quaternion.identity, roomParent);
        _roomPosition.Add(rows[rows.Count - 1].rooms[0], center);
        roomObj.gameObject.name = "Boss";
    }

    private Vector3 GetRoomPosition(float height){
        int posX = 0;
        
        do{
            posX = _random.Next(borderOffset.x, _size.x - borderOffset.x);
        } while(RoomOverlap(posX, height));

        return _mapOffset + new Vector3(
            posX,
            height + borderOffset.y,
            0f
        );
    }

    private bool RoomOverlap(int posX, float height) {
        Vector3 target = new Vector3(posX, height, 0);

        foreach(Vector3 pos in _roomRowPos){
            if((pos - target).magnitude <= roomRadius){
                Debug.Log((pos - target).magnitude);
                return true;
            }
        }

        return false; 
    }
}
