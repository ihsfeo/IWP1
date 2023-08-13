using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPart : MonoBehaviour
{
    public List<Room.Direction> SameRoom = new List<Room.Direction>();
    public List<Entrance> Entrances = new List<Entrance>();
    public Room room;

    public void init()
    {
        room = transform.parent.GetComponent<Room>();
    }
}
