using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomObject : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image sprite;
    private Room _room;

    private void OnEnable()
    {
        if (DungeonProgression.Instance.IsDetinationRoom(_room))
        {
            button.enabled = true;
            sprite.color = Color.green;
        }
        else
        {
            button.enabled = false;
            sprite.color = Color.white;
        }
    }

    public void SetRoom(Room room)
    {
        _room = room;
    }

    public void SelectRoom()
    {
        DungeonProgression.Instance.SelectDestinationRoom(_room);
    }
}
