using System.Collections;
using System.Collections.Generic;
using Platformer.Core;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player has died.
    /// </summary>
    /// <typeparam name="PlayerDeath"></typeparam>
    public class PlayerDeath : Simulation.Event<PlayerDeath>
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            var player = model.player;
            if (player.health.IsAlive || !player.health.IsAlive)
            {
                DissolveEffect dissolveEffect;

                if (player.GetComponent<DissolveEffect>() == null)
                {
                    dissolveEffect = player.gameObject.AddComponent<DissolveEffect>();
                    dissolveEffect.SetDefaultForPlayer(player.spriteRenderer.material);
                }
                else
                {
                    dissolveEffect = player.GetComponent<DissolveEffect>();
                    dissolveEffect.SetDefaultForPlayer(dissolveEffect.defaultMaterial);
                }

                player.health.Die();
                player.isDead = true;
                player.attacking = false;
                model.virtualCamera.m_Follow = null;
                model.virtualCamera.m_LookAt = null;
                player.controlEnabled = false;
                player.animator.SetTrigger("hurt");
                player.animator.SetBool("dead", true);
                Simulation.Schedule<PlayerSpawn>(1.5f);
            }
        }
    }
}