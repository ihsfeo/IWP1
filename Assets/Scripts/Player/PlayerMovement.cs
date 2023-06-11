using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private new Camera camera;
    [SerializeField] Status status;

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    [SerializeField] PlayerInfo playerInfo;

    Direction FacingDirection;
    Direction DashDirection;

    bool PlatformPhasing;
    bool OnPlatform;
    bool IsGrounded;

    int JumpCount;
    int JumpCountMax;

    float TerminalVelocity;

    bool CanDash;
    float DashInterval;
    float DashCD;
    float DashImmunity;

    float Up;
    float Right;
    float FallSpeed = 1;

    float OxygenLevel = 10;
    float DrowningTime = 0;
    bool CantBreathe = false;
    float LavaTime = 0;

    List<GameObject> CollisionObjects = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Untagged")
            CollisionObjects.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Untagged")
            CollisionObjects.Remove(collision.gameObject);
    }

    private void Awake()
    {
        JumpCountMax = 3;
        JumpCount = 0;
        TerminalVelocity = 4;

        DashDirection = Direction.Left;
        DashCD = 0;
        DashInterval = 0;
        DashImmunity = 0;
        CanDash = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FallSpeed = 1;
        if (transform.position.y < -30) // Test
            transform.position = new Vector3(2, -1, 0);

        if (!playerInfo.SwingingSword)
        {
            if (FacingDirection == Direction.Left)
                transform.LookAt(transform.position + new Vector3(0, 0, -1));
            else if (FacingDirection == Direction.Right)
                transform.LookAt(transform.position + new Vector3(0, 0, 1));
        }

        // Original Position
        Vector3 oldPosition = transform.position;
        PlatformPhasing = false;

        // Jump
        if (!status.InWater)
        {
            if (Input.GetKeyDown(KeyCode.Space) && JumpCount < JumpCountMax)
            {
                Up = 12 * Time.deltaTime;
                JumpCount++;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Up += 0.2f * Time.deltaTime;
            }
            FallSpeed *= 0.06f;
        }

        if (OxygenLevel < 10 && !CantBreathe)
        {
            DrowningTime = 0;
            OxygenLevel += Time.deltaTime * 2;
            if (OxygenLevel > 10)
                OxygenLevel = 10;
        }
        else if (OxygenLevel > 0 && CantBreathe)
        {
            DrowningTime = 0;
            OxygenLevel -= Time.deltaTime;
            if (OxygenLevel < 0)
                OxygenLevel = 0;
        }
        else if (OxygenLevel <= 0)
        {
            // Lose Health
            DrowningTime += Time.deltaTime;
            if (DrowningTime >= 1)
            {
                DrowningTime--;
                status.Health--;
                if (status.Health < 0)
                    status.Health = 0;
            }
        }

        // Going Down Platforms
        if (Input.GetKeyDown(KeyCode.S) && OnPlatform) // && Currently on a platform
        {
            PlatformPhasing = true;
        }

        OnPlatform = false;

        // Gravity
        Up -= Time.deltaTime / 2 * FallSpeed;
        if (Up < -TerminalVelocity / 2 * FallSpeed)
            Up = -TerminalVelocity / 2 * FallSpeed;
        else if (Up > TerminalVelocity / 2 * FallSpeed)
            Up = TerminalVelocity / 2 * FallSpeed;
        transform.position += new Vector3(0, Up, 0);

        // Vertical Collisions
        {
            for (int i = 0; i < CollisionObjects.Count; i++)
            {
                GameObject obj = CollisionObjects[i];
                if (obj == null)
                {
                    CollisionObjects.Remove(obj);
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
                                JumpCount = 0;
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
                    case "Platform":
                        // if going down platforms
                        if (PlatformPhasing)
                            continue;

                        // Stand on top
                        if (obj.transform.position.y + 0.5 > transform.position.y - 0.5)
                        {
                            if (Mathf.Abs(oldPosition.x - obj.transform.position.x) <= obj.transform.localScale.x / 2 + 0.5 && oldPosition.y >= obj.transform.position.y + 1)
                            {
                                transform.position = new Vector3(transform.position.x, obj.transform.position.y + 1, 0);
                                Up = 0.0f;
                                JumpCount = 0;
                                OnPlatform = true;
                            }
                        }
                        break;
                }
            }
        }

        oldPosition = transform.position;

        // Horizontal Movement
        if (Input.GetKey(KeyCode.A))
        {
            if (!Input.GetKey(KeyCode.D))
            {
                Right -= 0.5f * Time.deltaTime;
                FacingDirection = Direction.Left;
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Right += 0.5f * Time.deltaTime;
            FacingDirection = Direction.Right;
        }

        // Dash
        {
            if (DashCD > 0)
                DashCD -= Time.deltaTime;
            if (DashInterval > 0)
                DashInterval -= Time.deltaTime;
            if (CanDash) // Unlocked Dashing
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    if (DashCD <= 0) // Dashing Cooldown is over
                    {
                        if (DashInterval > 0) // if > 0 means time between taps is ok
                        {
                            // Checking if same Direction
                            if (DashDirection == Direction.Left) // Successful Dash
                            {
                                Right = -35f * Time.deltaTime;
                                DashCD = 0.4f;
                                DashInterval = 0;
                                DashImmunity = 0.1f;
                            }
                            else
                            {
                                DashInterval = 0.2f;
                                DashDirection = Direction.Left;
                            }
                        }
                        else // First Tap in the direction
                        {
                            DashInterval = 35f * Time.deltaTime;
                            DashDirection = Direction.Left;
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    if (DashCD <= 0) // Dashing Cooldown is over
                    {
                        if (DashInterval > 0) // if > 0 means time between taps is ok
                        {
                            // Checking if same Direction
                            if (DashDirection == Direction.Right) // Successful Dash
                            {
                                Right = 35f * Time.deltaTime;
                                DashCD = 0.4f;
                                DashInterval = 0;
                                DashImmunity = 0.1f;
                            }
                            else
                            {
                                DashInterval = 0.2f;
                                DashDirection = Direction.Right;
                            }
                        }
                        else // First Tap in the direction
                        {
                            DashInterval = 0.2f;
                            DashDirection = Direction.Right;
                        }
                    }
                }
            }
        }

        // Horizontal Movement Update
        if (DashImmunity > 0)
        {
            DashImmunity -= Time.deltaTime;
            if (status.InWater)
                transform.position += new Vector3(Right * 0.7f * status.MovementSpeed, 0, 0);
            else
                transform.position += new Vector3(Right * status.MovementSpeed, 0, 0);
            Right /= 1 + 6f * Time.deltaTime;
        }
        else if (!status.InWater)
        {
            transform.position += new Vector3(Right * status.MovementSpeed, 0, 0);
            Right /= 1 + 8 * Time.deltaTime;
        }
        else
        {
            transform.position += new Vector3(Right * 0.7f * status.MovementSpeed, 0, 0);
            if (Mathf.Abs(Right) <= 0.055f)
                Right /= 1.04f + Mathf.Abs(Right * 1.2f * 1.3f) * 30 * Time.deltaTime;
            else
                Right /= 1f + Mathf.Abs(Right * 0.69f * 1.3f) * 30 * Time.deltaTime;
        }

        status.InWater = false;
        CantBreathe = false;
        status.InLava = false;

        // Horizonal Collisions
        {
            for (int i = 0; i < CollisionObjects.Count; i++)
            {
                GameObject obj = CollisionObjects[i];
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
                    case "Water":
                         if (transform.position.x - transform.localScale.x / 2 < obj.transform.position.x + obj.transform.localScale.x / 2 &&
                            transform.position.x + transform.localScale.x / 2 > obj.transform.position.x - obj.transform.localScale.x / 2 &&
                            transform.position.y - transform.localScale.y / 2 < obj.transform.position.y + obj.transform.localScale.y / 2 &&
                            transform.position.y > obj.transform.position.y - obj.transform.localScale.y / 2 &&
                            obj.transform.localScale.y > 0.5)
                        {
                            status.InWater = true;
                            if (transform.position.y + transform.localScale.y / 4 > obj.transform.position.y - obj.transform.localScale.y / 2 &&
                                transform.position.y + transform.localScale.y / 4 < obj.transform.position.y + obj.transform.localScale.y / 2 &&
                                transform.position.x < obj.transform.position.x + obj.transform.localScale.x / 2 &&
                                transform.position.x > obj.transform.position.x - obj.transform.localScale.x / 2)
                            {
                                CantBreathe = true;
                            }
                        }
                        break;
                    case "Lava":
                        if (transform.position.x - transform.localScale.x / 2 < obj.transform.position.x + obj.transform.localScale.x / 2 &&
                            transform.position.x + transform.localScale.x / 2 > obj.transform.position.x - obj.transform.localScale.x / 2 &&
                            transform.position.y - transform.localScale.y / 2 < obj.transform.position.y + obj.transform.localScale.y / 2 &&
                            transform.position.y > obj.transform.position.y - obj.transform.localScale.y / 2 &&
                            obj.transform.localScale.y > 0.5)
                        {
                            status.InLava = true;
                        }
                        break;
                }
            }
        }

        // Camera
        {
            Vector3 cPosition = camera.transform.position;
            cPosition.z = 0;
            if ((cPosition - transform.position).magnitude > 10 ||
                Mathf.Abs(cPosition.y - transform.position.y) > 5.7)
            {
                cPosition = cPosition + (-cPosition + transform.position) / 40;
            }
            else
            {
                cPosition = cPosition + (-cPosition + transform.position) / 80;
            }

            // Clamp
            if (cPosition.y - transform.position.y > 6)
            {
                cPosition += new Vector3(0, 6 + transform.position.y - cPosition.y, 0);
            }
            else if (cPosition.y - transform.position.y < -6)
            {
                cPosition += new Vector3(0, -6 + transform.position.y - cPosition.y, 0);
                
            }
            cPosition.z = -3;

            camera.transform.position = cPosition;
        }
    }
}
