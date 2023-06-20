using Platformer.Core;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This class exposes the game model in the inspector, and ticks the
    /// simulation.
    /// </summary> 
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        //This model field is public and can be therefore be modified in the 
        //inspector.
        //The reference actually comes from the InstanceRegister, and is shared
        //through the simulation and events. Unity will deserialize over this
        //shared reference when the scene loads, allowing the model to be
        //conveniently configured inside the inspector.
        public PlatformerModel model = Simulation.GetModel<PlatformerModel>();
        public Texture2D cursorSword;
        public ParticleSystem cursorParticle;

        public bool alwaysVisible;

        private void Start()
        {
            if (cursorSword != null)
            {
                Cursor.SetCursor(cursorSword, Vector2.zero, CursorMode.ForceSoftware);
            }

            if (!alwaysVisible)
            {
                SetCursorVisibility(false);
            }
            else
            {
                SetCursorVisibility(true);
            }
        }

        private void OnEnable()
        {
            Instance = this;
        }

        private void OnDisable()
        {
            if (Instance == this) Instance = null;
        }

        private void Update()
        {
            if (Instance == this) Simulation.Tick();
        }

        public void SetCursorVisibility(bool visible)
        {
            Cursor.visible = visible;

            if (cursorParticle != null)
            {
                if (visible)
                {
                    cursorParticle.Play();
                }
                else
                {
                    cursorParticle.Stop();
                }
            }
        }
    }
}