using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform t1;
    [SerializeField] private Transform t2;
    private Rigidbody2D rb;
    private PlayerController player;

    [SerializeField] private Mode mode;
    [SerializeField] private Positions startPosition;

    private Vector3 p1;
    private Vector3 p2;
    private Vector3 currMoveTowards;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float travelDistance;

    [SerializeField] private bool upgoing;
    [SerializeField] private bool useSpecifiedTravel;

    private void Start()
    {
        if (mode == Mode.Transporter)
        {
            if (t1 == null || t2 == null)
            {
                this.enabled = false;
                return;
            }
            else
            {
                p1 = t1.position;
                p2 = t2.position;
            }

            if (startPosition == Positions.First)
            {
                transform.position = p1;
            }
            else
            {
                transform.position = p2;
            }

            rb = GetComponent<Rigidbody2D>();
            player = GameObject.Find("Player").GetComponent<PlayerController>();
        }
    }

    private void FixedUpdate()
    {
        if (mode == Mode.Transporter)
        {
            if (upgoing)
            {
                if (useSpecifiedTravel)
                {
                    t2.transform.position = new Vector3(0, travelDistance, 0);
                }

                if (rb.interpolation != RigidbodyInterpolation2D.Interpolate)
                {
                    rb.interpolation = RigidbodyInterpolation2D.Interpolate;
                }
            }
            else
            {
                if (useSpecifiedTravel)
                {
                    t2.transform.position = new Vector3(travelDistance, 0, 0);
                }

                if (rb.interpolation != RigidbodyInterpolation2D.None)
                {
                   rb.interpolation = RigidbodyInterpolation2D.None;
                }
            }

            if (startPosition == Positions.First)
            {
                Vector3 newP = Vector3.MoveTowards(transform.position, p2,
                    Time.deltaTime * movementSpeed);

                rb.MovePosition(newP);

                if (transform.position.x >= p2.x && transform.position.y >= p2.y)
                {
                    startPosition = Positions.Second;
                }
                else
                {
                    currMoveTowards = p2;
                }
            }
            else
            {
                Vector3 newP = Vector3.MoveTowards(transform.position, p1,
                    Time.deltaTime * movementSpeed);

                rb.MovePosition(newP);

                if (transform.position.x <= p1.x && transform.position.y <= p1.y)
                {
                    startPosition = Positions.First;
                }
                else
                {
                    currMoveTowards = p1;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (mode == Mode.Transporter)
        {
            if (collision.gameObject.tag == "Player" && collision.otherCollider.GetType() == typeof(EdgeCollider2D))
            {
                if (!upgoing)
                {
                    int add;

                    if (startPosition == Positions.Second)
                    {
                        add = -1;
                    }
                    else
                    {
                        add = 1;
                    }

                    Vector3 newP = Vector3.MoveTowards(collision.transform.position,
                    new Vector3(currMoveTowards.x + add, collision.transform.position.y, collision.transform.position.z),
                    Time.deltaTime * movementSpeed);

                    player.transform.position = newP;
                }
                else
                {
                    if (startPosition == Positions.Second && currMoveTowards.y < player.transform.position.y)
                    {
                        if (!Input.GetButtonDown("Jump"))
                        {
                            player.rb.gravityScale = 500;
                        }
                        else
                        {
                            player.rb.gravityScale = 0;
                        }
                    }
                    else
                    {
                        player.rb.gravityScale = 0;
                    }
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (mode == Mode.Transporter)
        {
            if (collision.gameObject.tag == "Player" && collision.otherCollider.GetType() == typeof(EdgeCollider2D))
            {
                if (!upgoing)
                {
                    int add;

                    if (startPosition == Positions.Second)
                    {
                        add = -1;
                    }
                    else
                    {
                        add = 1;
                    }

                    Vector3 newP = Vector3.MoveTowards(collision.transform.position,
                    new Vector3(currMoveTowards.x + add, collision.transform.position.y, collision.transform.position.z),
                    Time.deltaTime * movementSpeed);

                    player.transform.position = newP;
                }
                else
                {
                    if (startPosition == Positions.Second && currMoveTowards.y < player.transform.position.y)
                    {
                        if (!Input.GetButton("Jump"))
                        {
                            player.rb.gravityScale = 500;
                        }
                        else
                        {
                            player.rb.gravityScale = 0;
                        }
                    }
                    else
                    {
                        player.rb.gravityScale = 0;
                    }
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (mode == Mode.Transporter)
        {
            if (collision.gameObject.tag == "Player" && collision.otherCollider.GetType() == typeof(EdgeCollider2D))
            {
                player.rb.gravityScale = 0;
            }
        }
    }

    public enum Mode
    {
        None,
        Transporter,
        Obstacle
    }

    public enum Positions
    {
        None,
        First,
        Second
    }
}
