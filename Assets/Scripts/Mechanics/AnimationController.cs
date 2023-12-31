﻿using System.Collections;
using System.Collections.Generic;
using Platformer.Core;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// AnimationController integrates physics and animation. It is generally used for simple enemy animation.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
    public class AnimationController : KinematicObject
    {
        /// <summary>
        /// Max horizontal speed.
        /// </summary>
        public float maxSpeed = 7;
        /// <summary>
        /// Max jump velocity
        /// </summary>
        public float jumpTakeOffSpeed = 7;

        /// <summary>
        /// Used to indicated desired direction of travel.
        /// </summary>
        public Vector2 move;

        /// <summary>
        /// Set to true to initiate a jump.
        /// </summary>
        public bool jump;

        /// <summary>
        /// Set to true to set the current jump velocity to zero.
        /// </summary>
        public bool stopJump;
        public bool isKnockedBack;
        public bool reverseFlip;
        public bool isDead;

        SpriteRenderer spriteRenderer;
        internal Animator animator;
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        protected virtual void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        protected override void ComputeVelocity()
        {
            if (!isDead)
            {
                if (jump && IsGrounded)
                {
                    velocity.y = jumpTakeOffSpeed * model.jumpModifier - 4;
                    jump = false;
                }
                else if (stopJump)
                {
                    stopJump = false;
                    if (velocity.y > 0)
                    {
                        velocity.y = velocity.y * model.jumpDeceleration;
                    }
                }

                if (move.x > 0.01f && !isKnockedBack)
                {
                    if (!reverseFlip)
                    {
                        spriteRenderer.flipX = false;
                    }
                    else
                    {
                        spriteRenderer.flipX = true;
                    }
                }
                else if (move.x < -0.01f && !isKnockedBack)
                {
                    if (!reverseFlip)
                    {
                        spriteRenderer.flipX = true;
                    }
                    else
                    {
                        spriteRenderer.flipX = false;
                    }
                }

                animator.SetBool("grounded", IsGrounded);
                animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

                targetVelocity = move * maxSpeed;
            }
        }
    }
}