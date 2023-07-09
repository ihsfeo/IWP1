using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPart : Room
{
    public List<Direction> SameRoom = new List<Direction>();
    public List<Entrance> Entrances = new List<Entrance>();
    Room room;

    public void init()
    {
        room = transform.parent.GetComponent<Room>();
    }
}
