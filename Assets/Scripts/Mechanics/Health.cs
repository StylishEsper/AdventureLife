using System;
using Platformer.Gameplay;
using UnityEngine;
using TMPro;
using static EntityElement;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    /// <summary>
    /// Represebts the current vital statistics of some game entity.
    /// </summary>
    public class Health : MonoBehaviour
    {
        /// <summary>
        /// The maximum hit points for the entity.
        /// </summary>
        public HealthBar healthBar;

        public Effectiveness effectiveness;

        public Entity entity;
        public Element entityElement;

        /// <summary>
        /// Indicates if the entity should be considered 'alive'.
        /// </summary>
        public bool IsAlive => currentHP > 0;

        private int maxHP = 1000;
        private int maxPlayerHP = 100;
        private int maxBlueSlimeHP = 50;
        private int maxBarrelDudeHP = 200;
        private int maxBattyBatHP = 100;
        private int maxSkullimeHP = 150;
        private int maxNetcasterHP = 100;
        private int maxArachnaHP = 500;
        private int currentHP;

        /// <summary>
        /// Increment the HP of the entity.
        /// </summary>
        public void Increment(int heal, bool causedByAttack, bool isOvertime)
        {
            if (entity == Entity.Player)
            {
                maxHP = maxPlayerHP;
            }
            else if (entity == Entity.BlueSlime)
            {
                maxHP = maxBlueSlimeHP;
            }
            else if (entity == Entity.BarrelDude)
            {
                maxHP = maxBarrelDudeHP;
            }
            else if (entity == Entity.BattyBat)
            {
                maxHP = maxBattyBatHP;
            }
            else if (entity == Entity.Skullime)
            {
                maxHP = maxSkullimeHP;
            }
            else if (entity == Entity.Netcaster)
            {
                maxHP = maxNetcasterHP;
            }
            else if (entity == Entity.Arachna)
            {
                maxHP = maxArachnaHP;
            }
            else
            {
                maxHP = 2;
            }

            LoadPopupText("+" + heal, 0, 1, 0, false, "None", false);
            currentHP = Mathf.Clamp(currentHP + heal, 0, maxHP);
            healthBar.SetHealth(currentHP);

            if (causedByAttack && entity != Entity.Player)
            {
                GetComponent<EnemyVerification>().Hurt(isOvertime);
            }
        }

        public void FullHeal()
        {
            if (entity == Entity.Player)
            {
                currentHP = maxPlayerHP;
            }
            else if (entity == Entity.BlueSlime)
            {
                currentHP = maxBlueSlimeHP;
            }
            else if (entity == Entity.BarrelDude)
            {
                currentHP = maxBarrelDudeHP;
            }
            else if (entity == Entity.BattyBat)
            {
                currentHP = maxBattyBatHP;
            }
            else if (entity == Entity.Skullime)
            {
                currentHP = maxSkullimeHP;
            }
            else if (entity == Entity.Netcaster)
            {
                currentHP = maxNetcasterHP;
            }
            else if (entity == Entity.Arachna)
            {
                currentHP = maxArachnaHP;
            }
            else
            {
                currentHP = 2;
            }

            LoadPopupText("+" + currentHP, 0, 1, 0, false, "None", false);
            healthBar.SetHealth(currentHP);
        }

        /// <summary>
        /// Decrement the HP of the entity. Will trigger a HealthIsZero event when
        /// current HP reaches 0.
        /// </summary>
        public void Decrement(int damage, bool isOvertime, bool critical, 
            Element attackElement)
        {
            damage = CalculateDamage(damage, attackElement);

            if (damage <= 0)
            {
                Increment(Mathf.Abs(damage), true, isOvertime);
            }
            else
            {
                if (effectiveness == Effectiveness.Supereffective)
                {
                    LoadPopupText("-" + damage + " x2", 1, 0, 0, critical, "None", false);
                }
                else if (effectiveness == Effectiveness.Ineffective)
                {
                    LoadPopupText("-" + damage, 1, 0, 0, critical, "None", true);
                }
                else
                {
                    LoadPopupText("-" + damage, 1, 0, 0, critical, "None", false);
                }

                currentHP = Mathf.Clamp(currentHP - damage, 0, maxHP);

                if (currentHP <= 0 && entity == Entity.Player)
                {
                    var ev = Schedule<HealthIsZero>();
                    ev.health = this;
                }
                else if (currentHP <= 0 && entity != Entity.Player)
                {
                    Schedule<EnemyDeath>().enemy = GetComponent<EnemyVerification>();
                }
                else if (currentHP > 0 && entity != Entity.Player)
                {
                    GetComponent<EnemyVerification>().Hurt(isOvertime);
                }

                healthBar.SetHealth(currentHP);
            }
        }

        /// <summary>
        /// Decrement the HP of the entitiy until HP reaches 0.
        /// </summary>
        public void Die()
        {
            while (currentHP > 0) Decrement(currentHP, false, false, Element.Elementless);
            healthBar.SetHealth(currentHP);
        }

        private void Awake()
        {
            if (entity == Entity.Player)
            {
                maxPlayerHP += GetComponent<PlayerController>().bonusHP;
                currentHP = maxPlayerHP;
            }
            else if (entity == Entity.BlueSlime)
            {
                currentHP = maxBlueSlimeHP;
            }
            else if (entity == Entity.BarrelDude)
            {
                currentHP = maxBarrelDudeHP;
            }
            else if (entity == Entity.BattyBat)
            {
                currentHP = maxBattyBatHP;
            }
            else if (entity == Entity.Skullime)
            {
                currentHP = maxSkullimeHP;
            }
            else if (entity == Entity.Netcaster)
            {
                currentHP = maxNetcasterHP;
            }
            else if (entity == Entity.Arachna)
            {
                currentHP = maxArachnaHP;
            }
            else
            {
                currentHP = 2;
            }

            healthBar.SetMaxHealth(currentHP);
        }

        public int CalculateDamage(int damage, Element attackElement)
        {
            effectiveness = Effectiveness.Effective;

            if (attackElement == Element.Water)
            {
                if (entityElement == Element.Water)
                {
                    damage *= -1;
                    effectiveness = Effectiveness.Superineffective;
                }
                else if (entityElement == Element.Electric || entityElement == Element.Poison)
                {
                    damage /= 2;
                    effectiveness = Effectiveness.Ineffective;
                }
                else if (entityElement == Element.Fire || entityElement == Element.Wind)
                {
                    damage *= 2;
                    effectiveness = Effectiveness.Supereffective;
                }
            }
            else if (attackElement == Element.Fire)
            {
                if (entityElement == Element.Fire)
                {
                    damage *= -1;
                    effectiveness = Effectiveness.Superineffective;
                }
                else if (entityElement == Element.Water || entityElement == Element.Wind)
                {
                    damage /= 2;
                    effectiveness = Effectiveness.Ineffective;
                }
                else if (entityElement == Element.Ice || entityElement == Element.Wood)
                {
                    damage *= 2;
                    effectiveness = Effectiveness.Supereffective;
                }
            }
            else if (attackElement == Element.Electric)
            {
                if (entityElement == Element.Electric)
                {
                    damage *= -1;
                    effectiveness = Effectiveness.Superineffective;
                }
                else if (entityElement == Element.Wood || entityElement == Element.Ice)
                {
                    damage /= 2;
                    effectiveness = Effectiveness.Ineffective;
                }
                else if (entityElement == Element.Water || entityElement == Element.Wind)
                {
                    damage *= 2;
                    effectiveness = Effectiveness.Supereffective;
                }
            }
            else if (attackElement == Element.Ice)
            {
                if (entityElement == Element.Ice)
                {
                    damage *= -1;
                    effectiveness = Effectiveness.Superineffective;
                }
                else if (entityElement == Element.Fire || entityElement == Element.Wood)
                {
                    damage /= 2;
                    effectiveness = Effectiveness.Ineffective;
                }
                else if (entityElement == Element.Electric || entityElement == Element.Poison)
                {
                    damage *= 2;
                    effectiveness = Effectiveness.Supereffective;
                }
            }
            else if (attackElement == Element.Wood)
            {
                if (entityElement == Element.Wood)
                {
                    damage *= -1;
                    effectiveness = Effectiveness.Superineffective;
                }
                else if (entityElement == Element.Poison || entityElement == Element.Fire)
                {
                    damage /= 2;
                    effectiveness = Effectiveness.Ineffective;
                }
                else if (entityElement == Element.Electric || entityElement == Element.Ice)
                {
                    damage *= 2;
                    effectiveness = Effectiveness.Supereffective;
                }
            }
            else if (attackElement == Element.Wind)
            {
                if (entityElement == Element.Wind)
                {
                    damage *= -1;
                    effectiveness = Effectiveness.Superineffective;
                }
                else if (entityElement == Element.Water || entityElement == Element.Electric)
                {
                    damage /= 2;
                    effectiveness = Effectiveness.Ineffective;
                }
                else if (entityElement == Element.Fire || entityElement == Element.Poison)
                {
                    damage *= 2;
                    effectiveness = Effectiveness.Supereffective;
                }
            }
            else if (attackElement == Element.Poison)
            {
                if (entityElement == Element.Poison)
                {
                    damage *= -1;
                    effectiveness = Effectiveness.Superineffective;
                }
                else if (entityElement == Element.Ice || entityElement == Element.Wind)
                {
                    damage /= 2;
                    effectiveness = Effectiveness.Ineffective;
                }
                else if (entityElement == Element.Water || entityElement == Element.Wood)
                {
                    damage *= 2;
                    effectiveness = Effectiveness.Supereffective;
                }
            }

            return damage;
        }

        public void UpdateMaxHealth()
        {
            Awake();
        }

        public void SetHealth(int value)
        {
            currentHP = value;
            healthBar.SetHealth(currentHP);
        }

        public int GetHealth()
        {
            return (int)healthBar.slider.value;
        }

        public void LoadPopupText(string text, float r, float g, float b, bool critical, string element, bool resistance)
        {
            GameObject popupText = (GameObject)Instantiate(Resources.Load("Prefabs/Effects/PopupText"));
            popupText.transform.position = new Vector3(transform.position.x,
                transform.position.y, -1);
            TextPopUp textPopUp = popupText.GetComponent<TextPopUp>();
            textPopUp.SetText(text);
            textPopUp.SetColor(r, g, b);

            if (element != null && element != "None")
            {
                textPopUp.SetElement(element, false);
            }

            if (resistance)
            {
                textPopUp.SetElement(element, true);
            }

            if (critical)
            {
                textPopUp.Critical();
            }
        }

        public enum Entity
        {
            Player,
            BlueSlime,
            BarrelDude,
            BattyBat,
            Skullime,
            Netcaster,
            Arachna
        }

        public enum Effectiveness
        {
            Superineffective,
            Ineffective,
            Effective,
            Supereffective
        }
    }
}
