using System.Collections;
using Platformer.Mechanics;
using System.Collections.Generic;
using UnityEngine;

public class Absorbable : MonoBehaviour
{
    public EnergyBar energy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            Absorb();
        }
    }

    public void Absorb()
    {
        energy.SetEnergy((int)energy.slider.value + 10);
    }
}
