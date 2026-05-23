using System;
using System.Collections.Generic;
using UnityEngine;

namespace Infernum.FPS.Core.Animation
{
    /// <summary>
    /// Базовый сервис покадровой анимации спрайтов (оружие UI, враги в мире и т.д.).
    /// </summary>
    public abstract class SpriteSequenceAnimationServiceBase : MonoBehaviour, ISpriteAnimationService
    {
        protected readonly SpriteSequenceAnimator Animator = new();

        public bool IsPlaying => Animator.IsPlaying;

        protected abstract void EnsureTarget();
        protected abstract void SetVisible(bool visible);

        public virtual void Tick(float deltaTime)
        {
            Animator.Tick(deltaTime);
        }

        protected void PlaySequence(IReadOnlyList<Sprite> sprites, float frameInterval, bool loop, Action onComplete)
        {
            EnsureTarget();
            SetVisible(true);
            Animator.Play(sprites, frameInterval, loop, onComplete);
        }

        protected void StopAndHide()
        {
            Animator.Stop();
            SetVisible(false);
        }
    }
}
