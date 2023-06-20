using Platformer.Mechanics;
using UnityEngine;

public class SignalTransmitter : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private GameObject receiver;

    [SerializeField] private Cause cause;
    [SerializeField] private Trigger trigger;
    [SerializeField] private EventSignal eventSignal;

    private bool permaDisableControl;

    private void Start()
    {
        if (cause == Cause.Player)
        {
            player = GameObject.Find("Player").GetComponent<PlayerController>();
        }
    }

    private void Update()
    {
        if (cause == Cause.Player)
        {
            if (trigger == Trigger.Distance)
            {
                if (player.transform.position.x >= transform.position.x)
                {
                    if (eventSignal == EventSignal.BreakBridge)
                    {
                        receiver.GetComponent<BridgeController>().BreakBridge();
                    }
                    else if (eventSignal == EventSignal.DisableControl)
                    {
                        permaDisableControl = true;
                    }
                    else if (eventSignal == EventSignal.EnableObject)
                    {
                        receiver.SetActive(true);
                    }
                    else if (eventSignal == EventSignal.DisableObject)
                    {
                        receiver.SetActive(false);
                    }

                    if (eventSignal != EventSignal.None && !permaDisableControl)
                    {
                        enabled = false;
                    }
                }
            }
        }

        if (permaDisableControl)
        {
            player.controlEnabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (trigger == Trigger.Touch)
        {
            if (cause == Cause.EnemyHit)
            {
                if (collision.tag == "Breakable")
                {
                    if (eventSignal == EventSignal.BreakBridge)
                    {
                        receiver.GetComponent<BridgeController>().BreakBridge();
                        GetComponent<BulletController>().LoadHitEffect();
                    }
                }
            }
        }
    }

    public enum Cause
    {
        None,
        Player,
        EnemyHit,
        PlayerHit
    }

    public enum Trigger
    {
        None,
        Distance,
        Touch
    }

    public enum EventSignal
    {
        None,
        DisableControl,
        BreakBridge,
        EnableObject,
        DisableObject
    }
}
