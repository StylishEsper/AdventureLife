using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkEnergy : MonoBehaviour
{
    [SerializeField] private GameObject affectedTarget;
    private PlayerController player;

    [SerializeField] private DarkEnergyType energyType;
    [SerializeField] private MovementType movementType;

    private bool isMoving;

    private void Start()
    {
        if (affectedTarget == null)
        {
            affectedTarget = GameObject.Find("Player");
        }

        if (affectedTarget.GetComponent<PlayerController>() != null)
        {
            player = affectedTarget.GetComponent<PlayerController>();
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (energyType == DarkEnergyType.JumpBooster)
            {
                if (player != null)
                {
                    player.darkJump = true;
                    player.IsGrounded = true;
                    player.jumpState = PlayerController.JumpState.PrepareToJump;
                    player.move.y += 20f;
                }
            }
        }
        else if (collision.tag == "PlayerAttack")
        {
            isMoving = true;
        }
    }

    public enum MovementType
    {
        Stationary,
        MoveableX,
        MoveableY,
        MoveableXAndY,
        AlwaysMoving
    }

    public enum DarkEnergyType
    {
        None,
        JumpBooster,
        Teleporter
    }
}
