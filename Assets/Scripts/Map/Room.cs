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
    public List<EnemyBase> EnemyList = new List<EnemyBase>();
    public List<Vector3> EnemySpawn = new List<Vector3>();

    private void Awake()
    {
        init();
    }

    // Check if viable room
    // if this room overlaps
    // if the entrances of the room would lead to another room which doesnt have an entrance there
    public List<RoomPart> GetRoomPart(Direction direction)
    {
        List<RoomPart> rtn = new List<RoomPart>();

        for (int i = 0; i < RoomParts.Count; i++)
        {
            for (int j = 0; j < RoomParts[i].Entrances.Count; i++)
            {
                if (RoomParts[i].Entrances[j].GetDirection() == direction)
                {
                    rtn.Add(RoomParts[i]);
                    break;
                }
            }
        }

        return rtn;
    }

    public List<RoomPart> GetRoomPart()
    {
        return RoomParts;
    }

    public void Enter()
    {
        // reset alive enemies
        for (int i = 0; i < EnemyList.Count; i++)
        {
            if (EnemyList[i] == null)
            {
                EnemyList.RemoveAt(i);
                EnemySpawn.RemoveAt(i);
                i--;
            }
            else
            {
                EnemyList[i].Reset();
                EnemyList[i].transform.position = EnemySpawn[i];
            }
        }
        gameObject.SetActive(true);
    }

    public void Exit()
    {
        gameObject.SetActive(false);
    }

    public void init()
    {
        for (int i = 0; i < EnemyList.Count; i++)
        {
            EnemySpawn.Add(EnemyList[i].transform.position);
        }
    }
}
