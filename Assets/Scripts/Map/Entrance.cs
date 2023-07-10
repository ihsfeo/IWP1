using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : MonoBehaviour
{
    bool Usable = true;
    Vector3 Position;
    [SerializeField] Entrance otherEntrance;
    RoomPart roomPart;
    [SerializeField] Room.Direction direction;

    private void Awake()
    {
        init();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
         if (collision.gameObject.tag == "Player")
        {
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
        Position = transform.GetChild(0).position;
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
        return Position;
    }
}
