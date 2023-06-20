using UnityEngine;
using Platformer.Core;
using Platformer.Mechanics;

namespace Platformer.Gameplay
{
    public class EnemyRespawner : Simulation.Event<EnemyRespawner>
    {
        public SimpleEnemyController enemy;

        public override void Execute()
        {
            enemy.GetComponent<AnimationController>().animator.SetTrigger("respawn");
            enemy.Harmful();
            enemy.control.enabled = true;
        }
    }
}
