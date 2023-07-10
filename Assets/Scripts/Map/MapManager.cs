using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public enum Areas
    {
        NormalArea,
        PoisonArea,
        FireArea,
        IceArea,
        LightningArea,
        FinalArea
    }

    [SerializeField] List<Room> RoomList = new List<Room>();
    [SerializeField] List<Room> ImportantStructures = new List<Room>();
    [SerializeField] Map map;

    int SizeX; // Width of Map
    int SizeY; // Height of Map
    Vector2 StartingPosition; // Starting Room Location

    [SerializeField] List<Vector2> AreaSize = new List<Vector2>();
    [SerializeField] List<int> AreaOffset = new List<int>();

    //Vector2 Area3Size;
    //int Area3Offset;

    bool CanPath()
    {
        return false;
    }

    private void Awake()
    {
        for (int i = 0; i < 6; i++)
        {
            AreaSize.Add(new Vector2(10, 5));
            AreaOffset.Add(0);
        }
    }

    bool GenerateMap()
    {
        // Generate Approximate Locations of each area
        // Occupy each area with important structures
        // Occupy each area with rooms


        int areaCount = 0;
        // Area Generation
        {
            while (areaCount < 6)
            {
                int AreaSizeX = (int)AreaSize[areaCount].x;
                int AreaSizeY = (int)AreaSize[areaCount].y;

                switch (areaCount)
                {
                    case 0:
                        {
                            int StartX = 0;
                            int StartY = Random.Range(0, SizeY - AreaSizeY);

                            // Make Sure Area1 Includes spawning area
                            if (StartingPosition.y - StartY >= AreaSizeY)
                            {
                                continue;
                            }

                            AreaSize[areaCount] = new Vector2(StartX, StartY);

                            break;
                        }
                    case 1:
                        {
                            // Has to be somewhat close to the 1st area taht it can be connected
                            break;
                        }
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    default: continue;
                }

                areaCount++;
            }
        }

        // Important Structures
        {
            // just state a general position for structures in their respective areas
            areaCount = 0;
            while (areaCount < 6)
            {
                switch (areaCount)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    default: continue;
                }

                areaCount++;
            }
        }

        // Rooms
        {
            // from the already existing room, make another room that fits and connect it
            areaCount = 0;
            while (areaCount < 6)
            {
                switch (areaCount)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    default: continue;
                }

                areaCount++;
            }
        }


        return false;
    }

    // Direction of path
    // This Room
    public void MakeRoom(RoomPart roomPart, Room.Direction direction)
    {
        // Make new Room that fits
        Room NRoom = GetRoom(roomPart, direction);

        // Making of the new room
        for (int i = 0; i < NRoom.GetRoomPart().Count; i++)
        {
            for (int j = 0; j < NRoom.GetRoomPart()[i].Entrances.Count; i++)
            {
                MakeRoom(NRoom.GetRoomPart()[i], NRoom.GetRoomPart()[i].Entrances[j].GetDirection());
            }
        }
    }


    public Room GetRoom(RoomPart roomPart, Room.Direction direction)
    {
        // Check the corresponding surrounds
        // Get rid of rooms that cant fit
        // Select a room from the remains

        return new Room();
    }

    public void AStar()
    {
        // do in the library later
    }
}
