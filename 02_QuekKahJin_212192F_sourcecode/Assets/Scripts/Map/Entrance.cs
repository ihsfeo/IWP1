using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : MonoBehaviour
{
    bool Usable = true;
    [SerializeField] public Entrance otherEntrance;
    RoomPart roomPart;
    [SerializeField] public Room.Direction direction;
    [SerializeField] bool StartRoom = false;
    [SerializeField] GameObject MapPrefab;
    [SerializeField] GameObject ManagerThing;

    private void Awake()
    {
        init();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
         if (collision.gameObject.tag == "Player")
        {
            if (StartRoom)
            {
                ManagerThing.transform.GetChild(1).GetComponent<MapManager>().Init();
                ManagerThing.transform.GetChild(1).GetComponent<MapManager>().GenerateMap();




                //GameObject temp = Instantiate(MapPrefab, ManagerThing.transform);
                //temp.transform.position = new Vector3(-50, -2.7f, 0);
                collision.gameObject.transform.position = new Vector3(-10, 0, 0);

                roomPart.room.gameObject.SetActive(false);

                return;
            }
            // Check if other entrance is Usable
            if (otherEntrance.IsUsable() && Usable)
            {
                // Make other room appear
                {
                    roomPart.room.Exit();
                    otherEntrance.roomPart.room.Enter();
                }
                // teleport player to other entrance
                {
                    collision.transform.position = otherEntrance.GetPosition();
                    collision.GetComponent<PlayerMovement>().SpeedZero();
                }

            }    
        }
    }

    public bool IsUsable()
    {
        return Usable;
    }

    public void init()
    {
        roomPart = transform.parent.GetComponent<RoomPart>();
    }

    public Room.Direction GetDirection()
    {
        return direction;
    }

    public void ConnectEntrance(Entrance entrance)
    {
        otherEntrance = entrance;
    }

    public Vector3 GetPosition()
    {
        return transform.GetChild(0).position;
    }
}
