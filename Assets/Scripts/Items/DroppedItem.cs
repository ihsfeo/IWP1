using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroppedItem : MonoBehaviour
{
    public ItemBase Item;
    List<GameObject> NearbyObjects = new List<GameObject>();
    Vector3 oldPosition;

    float Up, Right, FallSpeed;
    float TerminalVelocity = 0.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Untagged")
            NearbyObjects.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Untagged")
            NearbyObjects.Remove(collision.gameObject);
    }

    public void Init(ItemBase.ItemID itemID, int count, ItemManager itemManager)
    {
        Item = Instantiate(itemManager.GetItem(itemID), transform).GetComponent<ItemBase>();
        if (count > Item.MaxCount)
        {
            count -= Item.MaxCount;
            DroppedItem temp = Instantiate(itemManager.droppedItem).GetComponent<DroppedItem>();
            temp.Init(itemID, count, itemManager);
            Item.Count = Item.MaxCount;
        }
        else
            Item.Count = count;

        Item.gameObject.SetActive(false);
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Item.GetComponent<SpriteRenderer>().sprite;
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = Item.GetComponent<SpriteRenderer>().color;
        transform.GetChild(0).rotation = Item.transform.rotation;
        transform.GetChild(0).localScale = Item.transform.localScale;
    }

    private void Update()
    {
        oldPosition = transform.position;
        FallSpeed = 1;

        // Check if Player there
        CheckPlayer();

        // Gravity
        {
            Up -= Time.deltaTime / 2 * FallSpeed;
            if (Up < -TerminalVelocity / 2 * FallSpeed)
                Up = -TerminalVelocity / 2 * FallSpeed;
            else if (Up > TerminalVelocity / 2 * FallSpeed)
                Up = TerminalVelocity / 2 * FallSpeed;
            transform.position += new Vector3(0, Up, 0);
        }

        // Vertical Collisions
        CheckCollision("Vertical");

        // Horizontal Friction
        {
            transform.position += new Vector3(Right * 0.7f, 0, 0);
            if (Mathf.Abs(Right) <= 0.055f)
                Right /= 1.04f + Mathf.Abs(Right * 1.2f * 1.3f) * 30 * Time.deltaTime;
            else
                Right /= 1f + Mathf.Abs(Right * 0.69f * 1.3f) * 30 * Time.deltaTime;
        }

        // Horizontal Collisions
        CheckCollision("Horizontal");
    }

    void CheckPlayer()
    {
        for (int i = 0; i < NearbyObjects.Count; i++)
        {
            if (NearbyObjects[i].tag == "Player")
            {
                if (NearbyObjects[i].GetComponent<PlayerInfo>().CanPickup(Item))
                {
                    Vector3 direction = NearbyObjects[i].transform.position - transform.position;
                    if (direction.magnitude < 1)
                    {
                        // Pickup
                        int Count = NearbyObjects[i].GetComponent<PlayerInfo>().AddToInventory(Item);
                        if (Count == 0)
                        {
                            Destroy(gameObject);
                        }
                        else
                        {
                            Item.Count = Count;
                        }
                    }
                    Up = direction.y / (15 - direction.y * direction.y);
                    Right = direction.x / (15 - direction.x * direction.x);
                }
                return;
            }
        }
    }

    void CheckCollision(string direction)
    {
        if (direction == "Horizontal")
        {
            for (int i = 0; i < NearbyObjects.Count; i++)
            {
                GameObject obj = NearbyObjects[i];
                switch (obj.tag)
                {
                    case "Destructible":
                    case "Solid":
                        if (transform.position.y >= obj.transform.position.y + obj.transform.localScale.y / 2 + 0.5f || transform.position.y <= obj.transform.position.y - obj.transform.localScale.y / 2 - 0.5f)
                            continue;

                        // Left of block
                        if (oldPosition.x <= obj.transform.position.x - obj.transform.localScale.x / 2 - 0.5f)
                        {
                            if (transform.position.x > obj.transform.position.x - obj.transform.localScale.x / 2 - 0.5f)
                            {
                                transform.position = new Vector3(obj.transform.position.x - obj.transform.localScale.x / 2 - 0.5f, transform.position.y, 0);
                                Right = 0.0f;
                            }
                        }

                        // Right of block
                        else if (oldPosition.x >= obj.transform.position.x + obj.transform.localScale.x / 2 + 0.5f)
                        {
                            if (transform.position.x < obj.transform.position.x + obj.transform.localScale.x / 2 + 0.5f)
                            {
                                transform.position = new Vector3(obj.transform.position.x + obj.transform.localScale.x / 2 + 0.5f, transform.position.y, 0);
                                Right = 0.0f;
                            }
                        }
                        break;
                    case "Platform":
                        break;
                }
            }
        }
        else if (direction == "Vertical")
        {
            for (int i = 0; i < NearbyObjects.Count; i++)
            {
                GameObject obj = NearbyObjects[i];
                if (obj == null)
                {
                    NearbyObjects.Remove(obj);
                    i--;
                    continue;
                }
                switch (obj.tag)
                {
                    case "Destructible":
                    case "Solid":
                        // Check if raycast into top/bottom lines

                        if (transform.position.x <= obj.transform.position.x - obj.transform.localScale.x / 2 - 0.5f || transform.position.x >= obj.transform.position.x + obj.transform.localScale.x / 2 + 0.5f)
                            continue;
                        // Above the block
                        if (oldPosition.y >= obj.transform.position.y + obj.transform.localScale.y / 2 + 0.5f)
                        {
                            if (transform.position.y < obj.transform.position.y + obj.transform.localScale.y / 2 + 0.5f)
                            {
                                transform.position = new Vector3(transform.position.x, obj.transform.position.y + obj.transform.localScale.y / 2 + 0.5f, 0);
                                Up = 0.0f;
                            }
                        }

                        // Under the block
                        else if (oldPosition.y <= obj.transform.position.y - obj.transform.localScale.y / 2 - 0.5f)
                        {
                            if (transform.position.y > obj.transform.position.y - obj.transform.localScale.y / 2 - 0.5f)
                            {
                                transform.position = new Vector3(transform.position.x, obj.transform.position.y - obj.transform.localScale.y / 2 - 0.5f, 0);
                                Up = 0.0f;
                            }
                        }
                        break;
                    //case "Platform":
                    //    // if going down platforms
                    //    if (PlatformPhasing)
                    //        continue;

                    //    // Stand on top
                    //    if (obj.transform.position.y + 0.5 > transform.position.y - 0.5)
                    //    {
                    //        if (Mathf.Abs(oldPosition.x - obj.transform.position.x) <= obj.transform.localScale.x / 2 + 0.5 && oldPosition.y >= obj.transform.position.y + 1)
                    //        {
                    //            transform.position = new Vector3(transform.position.x, obj.transform.position.y + 1, 0);
                    //            Up = 0.0f;
                    //            JumpCount = 0;
                    //            OnPlatform = true;
                    //        }
                    //    }
                    //    break;
                }
            }
        }
    }
}
