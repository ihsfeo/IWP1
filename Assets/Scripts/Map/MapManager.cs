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
    [SerializeField] Room StartingRoom;
    [SerializeField] public GameObject BaseArea;

    int SizeX; // Width of Map
    int SizeY; // Height of Map
    Vector2 StartingPosition; // Starting Room Location
    int RoomCount = 0;

    [SerializeField] List<Vector2> AreaSize = new List<Vector2>();
    [SerializeField] List<int> AreaOffset = new List<int>();

    public List<List<Room>> RMap = new List<List<Room>>();

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

    public void Init()
    {
        for (int i = 0; i < 11; i++)
        {
            List<Room> temp = new List<Room>();
            for (int j = 0; j < 11; j++)
            {
                temp.Add(null);
            }
            RMap.Add(temp);
        }
    }

    public void Reset()
    {
        for (int i = 0; i < RMap.Count; i++)
        {
            for (int j = 0; j < RMap[i].Count; j++)
            {
                if (RMap[i][j])
                    Destroy(RMap[i][j].gameObject);
            }
        }
    }

    public bool GenerateMap()
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
                            //int StartX = 0;
                            //int StartY = Random.Range(0, SizeY - AreaSizeY);

                            //// Make Sure Area1 Includes spawning area
                            //if (StartingPosition.y - StartY >= AreaSizeY)
                            //{
                            //    continue;
                            //}

                            //AreaSize[areaCount] = new Vector2(StartX, StartY);

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
                    default: break;
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
                        // Room - > next path
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    default: break;
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
                        Room FirstRoom = Instantiate(StartingRoom, transform.GetChild(0));
                        FirstRoom.MapPosition = new Vector2(5, 5);
                        FirstRoom.init();
                        RMap[5][5] = FirstRoom;
                        MakeRoom(FirstRoom);
                        RoomCount = 1;
                        break;
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    default: break;
                }

                areaCount++;
            }
        }


        return false;
    }

    void MakeRoom(Room Current)
    {
        for (int i = 0; i < Current.RoomParts.Count; i++)
        {
            for (int j = 0; j < Current.RoomParts[i].Entrances.Count; j++)
            {
                // Check current no other entrance
                // also need to check if there is another room on the other side
                if (!Current.RoomParts[i].Entrances[j].otherEntrance)
                {
                    Vector2 NextPosition = Current.MapPosition;
                    switch (Current.RoomParts[i].Entrances[j].direction)
                    {
                        case Room.Direction.Up: NextPosition += new Vector2(0, 1); break;
                        case Room.Direction.Down: NextPosition += new Vector2(0, -1); break;
                        case Room.Direction.Left: NextPosition += new Vector2(-1, 0); break;
                        case Room.Direction.Right: NextPosition += new Vector2(1, 0); break;
                        default: break;
                    }
                    // if theres an existing room
                    if (RMap[(int)NextPosition.x][(int)NextPosition.y])
                    {
                        LinkRoomEntrance(Current, RMap[(int)NextPosition.x][(int)NextPosition.y], i, j);
                        return;
                    }
                    // Pick a room
                    Room ToMake = GetRoom(Current, Current.RoomParts[i].Entrances[j].direction);
                    if (!ToMake) break;
                    // Make new Room in that direction
                    Room newRoom = Instantiate(ToMake, transform.GetChild(0));
                    newRoom.transform.localPosition = new Vector3(250 - 50 * (int)NextPosition.x, 250 - 50 * (int)NextPosition.y, 0);
                    newRoom.gameObject.SetActive(false);
                    newRoom.init();
                    // Link Entrances
                    LinkRoomEntrance(Current, newRoom, i, j);
                    newRoom.MapPosition = NextPosition;
                    RMap[(int)NextPosition.x][(int)NextPosition.y] = newRoom;
                    RoomCount++;
                    // Make Room
                    MakeRoom(newRoom);
                }
            }
        }
    }

    void LinkRoomEntrance(Room Current, Room newRoom, int i, int j)
    {
        for (int Rooms = 0; Rooms < newRoom.RoomParts.Count; Rooms++)
        {
            for (int Entrances = 0; Entrances < newRoom.RoomParts[Rooms].Entrances.Count; Entrances++)
            {
                if ((Current.RoomParts[i].Entrances[j].direction == Room.Direction.Up && newRoom.RoomParts[Rooms].Entrances[Entrances].direction == Room.Direction.Down) ||
                    (Current.RoomParts[i].Entrances[j].direction == Room.Direction.Down && newRoom.RoomParts[Rooms].Entrances[Entrances].direction == Room.Direction.Up) ||
                    (Current.RoomParts[i].Entrances[j].direction == Room.Direction.Left && newRoom.RoomParts[Rooms].Entrances[Entrances].direction == Room.Direction.Right) ||
                    (Current.RoomParts[i].Entrances[j].direction == Room.Direction.Right && newRoom.RoomParts[Rooms].Entrances[Entrances].direction == Room.Direction.Left))
                {
                    Current.RoomParts[i].Entrances[j].otherEntrance = newRoom.RoomParts[Rooms].Entrances[Entrances];
                    newRoom.RoomParts[Rooms].Entrances[Entrances].otherEntrance = Current.RoomParts[i].Entrances[j];
                    return;
                }
            }
        }
    }

    public Room GetRoom(Room room, Room.Direction direction)
    {
        List<Room.Direction> directionNeeded = new List<Room.Direction>();
        List<Room> AcceptableRooms = new List<Room>();
        Vector2 MapPosition = room.MapPosition;

        switch (direction)
        {
            case Room.Direction.Up:
                directionNeeded.Add(Room.Direction.Down);
                MapPosition += new Vector2(0, 1);
                break;
            case Room.Direction.Down:
                directionNeeded.Add(Room.Direction.Up);
                MapPosition += new Vector2(0, -1);
                break;
            case Room.Direction.Left:
                directionNeeded.Add(Room.Direction.Right);
                MapPosition += new Vector2(-1, 0);
                break;
            case Room.Direction.Right:
                directionNeeded.Add(Room.Direction.Left);
                MapPosition += new Vector2(1, 0);
                break;
            default: break;
        }

        for (int i = 0; i < 4; i++)
        {
            Room.Direction Direction;
            if ((int)directionNeeded[0] != i)
            {
                switch (i)
                {
                    case 0:
                        if (HasDirection(RMap[(int)MapPosition.x][(int)MapPosition.y + 1], Room.Direction.Down))
                        {
                            directionNeeded.Add(Room.Direction.Up);
                        }
                        break;
                    case 1:
                        if (HasDirection(RMap[(int)MapPosition.x][(int)MapPosition.y - 1], Room.Direction.Up))
                        {
                            directionNeeded.Add(Room.Direction.Down);
                        }
                        break;
                    case 2:
                        if (HasDirection(RMap[(int)MapPosition.x - 1][(int)MapPosition.y], Room.Direction.Right))
                        {
                            directionNeeded.Add(Room.Direction.Left);
                        }
                        break;
                    case 3:
                        if (HasDirection(RMap[(int)MapPosition.x + 1][(int)MapPosition.y], Room.Direction.Left))
                        {
                            directionNeeded.Add(Room.Direction.Right);
                        }
                        break;
                }
            }
        }
        // Check the corresponding surrounds
        // Get rid of rooms that cant fit
        for (int i = 0; i < RoomList.Count; i++)
        {
            bool Can = false;
            for (int j = 0; j < RoomList[i].RoomParts.Count; j++)
            {
                int PathsWork = 0;
                for (int l = 0; l < directionNeeded.Count; l++)
                {
                    bool DirectionOk = false;
                    for (int k = 0; k < RoomList[i].RoomParts[j].Entrances.Count; k++)
                    {
                        if (RoomList[i].RoomParts[j].Entrances[k].direction == directionNeeded[l])
                        {
                            DirectionOk = true;
                            break;
                        }
                    }
                    if (DirectionOk)
                    {
                        PathsWork++;
                        if (PathsWork == directionNeeded.Count)
                            Can = true;
                    }
                }
                if (Can) break;
            }
            if (Can)
            {
                AcceptableRooms.Add(RoomList[i]);
            }
        }
        // Select a room from the remains
        if (AcceptableRooms.Count != 0)
        {
            int num = Random.Range(0, AcceptableRooms.Count);
            if (num == 1)
                num = 1;
            if (RoomCount == 3) return AcceptableRooms[0];
            return AcceptableRooms[num];
        }
        else return null;
    }

    bool HasDirection(Room room, Room.Direction direction)
    {
        if (room)
        {
            for (int i = 0; i < room.RoomParts[0].Entrances.Count; i++)
            {
                if (room.RoomParts[0].Entrances[i].direction == direction)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void AStar()
    {
        // do in the library later
    }

    public int RandomDirection(int Chance1, int Chance2, int Chance3, int Chance4)
    {
        int Result = Random.Range(1, Chance1 + Chance2 + Chance3 + Chance4);
        if (Result <= Chance1) return 1;
        else if (Result <= Chance1 + Chance2) return 2;
        else if (Result <= Chance1 + Chance2 + Chance3) return 3;
        else return 4;
    }
}
