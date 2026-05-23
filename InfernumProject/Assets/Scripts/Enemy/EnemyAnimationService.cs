using System;
using System.Collections.Generic;
using Infernum.FPS.Core.Animation;
using Infernum.FPS.Enemy.Config;
using UnityEngine;

namespace Infernum.FPS.Enemy
{
    /// <summary>
    /// Покадровая анимация врага на SpriteRenderer в мире.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class EnemyAnimationService : SpriteSequenceAnimationServiceBase, IEnemyAnimationService
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        private SpriteRendererSpriteTarget _spriteTarget;
        private EnemyConfig _config;

        private void Awake()
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }

            EnsureTarget();
        }

        public void BindConfig(EnemyConfig config)
        {
            _config = config;
            EnsureTarget();

            if (config == null)
            {
                Hide();
                return;
            }

            PlayIdle();
        }

        public void Hide()
        {
            StopAndHide();
        }

        public void PlayIdle()
        {
            if (_config == null)
            {
                return;
            }

            PlaySequence(
                _config.GetIdleSprites(),
                GetInterval(_config.AnimationConfig.IdleFrameInterval, 0.35f),
                loop: true,
                onComplete: null);
        }

        public void PlayMove()
        {
            if (_config == null)
            {
                return;
            }

            PlaySequence(
                _config.GetMoveSprites(),
                GetInterval(_config.AnimationConfig.MoveFrameInterval, 0.2f),
                loop: true,
                onComplete: null);
        }

        public void PlayAttack(Action onComplete = null)
        {
            if (_config == null)
            {
                onComplete?.Invoke();
                return;
            }

            PlaySequence(
                _config.GetAttackSprites(),
                GetInterval(_config.AnimationConfig.AttackFrameInterval, 0.25f),
                loop: true,
                onComplete);
        }

        public void PlayDead(Action onComplete = null)
        {
            if (_config == null)
            {
                onComplete?.Invoke();
                return;
            }

            PlaySequence(
                _config.GetDeadSprites(),
                GetInterval(_config.AnimationConfig.DeadFrameInterval, 0.4f),
                loop: false,
                onComplete);
        }

        protected override void EnsureTarget()
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }

            if (spriteRenderer == null)
            {
                return;
            }

            if (_spriteTarget == null)
            {
                _spriteTarget = new SpriteRendererSpriteTarget(spriteRenderer);
                Animator.Bind(_spriteTarget);
            }
        }

        protected override void SetVisible(bool visible)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = visible;
                if (!visible)
                {
                    spriteRenderer.sprite = null;
                }
            }
        }

        private static float GetInterval(float value, float fallback)
        {
            return value > 0f ? value : fallback;
        }
    }
}
