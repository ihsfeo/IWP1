using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    List<Direction> EntranceDirection = new List<Direction>();
    public int PathsCount;

    private void Awake()
    {
        for (int i = 0; i < PathsCount; i++)
            EntranceDirection.Add(0);
    }

}
