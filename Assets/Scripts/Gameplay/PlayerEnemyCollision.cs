using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when a Player collides with an Enemy.
    /// </summary>
    /// <typeparam name="EnemyCollision"></typeparam>
    public class PlayerEnemyCollision : Simulation.Event<PlayerEnemyCollision>
    {
        public EnemyVerification enemy;
        public PlayerController player;

        public bool stuck;

        private EntityElement.Element attackElement;

        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            if (!player.invincible && !player.isDead)
            {
                attackElement = enemy.element;

                if (!player.isFrozen && !player.isParalyzed)
                {
                    player.animator.SetTrigger("hurt");
                }

                player.Bounce(3);

                if (enemy.transform.position.x > player.transform.position.x)
                {
                    player.Knockback("left");                  
                }
                else
                {
                    player.Knockback("right");
                }


                player.Invincible();

                player.health.Decrement(10, false, false, attackElement);
            }
        }
    }
}