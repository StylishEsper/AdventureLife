using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();

    private Vector3 moveTo;
    [SerializeField] private OpenCondition condition;
    [SerializeField] private OpenMethod method;

    [SerializeField] private int maxToSlay;
    private int slainTotal;

    private bool doorOpened;

    private void Update()
    {
        if (condition == OpenCondition.ClearEnemies)
        {
            if (enemies.Count == 0 && slainTotal == maxToSlay && !doorOpened)
            {
                OpenDoor();
            }

            foreach (GameObject go in enemies)
            {
                if (go.GetComponent<EnemyVerification>().isDead)
                {
                    slainTotal++;
                    enemies.Remove(go);
                    break;
                }
            }
        }

        if (doorOpened)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTo, Time.deltaTime);

            if (transform.position == moveTo)
            {
                enabled = false;
            }
        }
    }

    public void OpenDoor()
    {
        if (method == OpenMethod.OpenUpwards)
        {
            moveTo = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        }
        else if (method == OpenMethod.OpenDownwards)
        {
            
        }
        else if (method == OpenMethod.MoveRight)
        {

        }
        else if (method == OpenMethod.MoveLeft)
        {

        }
        else if (method == OpenMethod.Instant)
        {

        }
        else if (method == OpenMethod.CustomAnimation)
        {

        }

        doorOpened = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (condition == OpenCondition.Key)
            {
                
            }
        }
    }

    public enum OpenCondition
    {
        None,
        Key,
        ClearEnemies
    }

    public enum OpenMethod
    {
        None,
        OpenUpwards,
        OpenDownwards,
        MoveRight,
        MoveLeft,
        Instant,
        CustomAnimation
    }
}
