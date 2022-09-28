using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    public event EventHandler OnWin;
    public event EventHandler OnLose;
    public event EventHandler OnEncouterDone;
    public event EventHandler OnNextRoom;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("The Instance EventHandler already exist → " + gameObject);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void OnWinTrigger()
    {
        OnWin?.Invoke(this, EventArgs.Empty);
    }

    public void OnLoseTrigger()
    {
        OnLose?.Invoke(this, EventArgs.Empty);
    }

    public void OnEncounterEndTrigger()
    {
        OnEncouterDone?.Invoke(this, EventArgs.Empty);
    }

    public void OnNextRoomTrigger()
    {
        OnNextRoom?.Invoke(this, EventArgs.Empty);
    }
}
