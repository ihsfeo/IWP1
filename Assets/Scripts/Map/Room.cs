using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    public int PathsCount;
    public bool BiggerRoom = false;
    public List<RoomPart> RoomParts = new List<RoomPart>();
    public MapManager.Areas Area;

    // Check if viable room
    // if this room overlaps
    // if the entrances of the room would lead to another room which doesnt have an entrance there
    public List<RoomPart> GetRoom(Direction direction)
    {
        List<RoomPart> rtn = new List<RoomPart>();

        for (int i = 0; i < RoomParts.Count; i++)
        {
            for (int j = 0; j < RoomParts[i].EntranceDirection.Count; i++)
            {
                if (RoomParts[i].EntranceDirection[j] == direction)
                {
                    rtn.Add(RoomParts[i]);
                    break;
                }
            }
        }

        return rtn;
    }
}
