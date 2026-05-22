using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Infernum.FPS.Player
{
    /// <summary>
    /// Поочерёдно показывает спрайты на UI Image (1 кадр — пауза, N кадров — цикл или один проход).
    /// </summary>
    public sealed class SpriteSequenceAnimator
    {
        private Image _target;
        private IReadOnlyList<Sprite> _sprites = Array.Empty<Sprite>();
        private float _frameInterval = 0.25f;
        private bool _loop;
        private Action _onComplete;
        private int _frameIndex;
        private float _timer;
        private bool _isPlaying;

        public bool IsPlaying => _isPlaying;

        public void Bind(Image target)
        {
            _target = target;
        }

        public void Play(IReadOnlyList<Sprite> sprites, float frameInterval, bool loop, Action onComplete)
        {
            Stop();

            _sprites = sprites ?? Array.Empty<Sprite>();
            _frameInterval = Mathf.Clamp(frameInterval, 0.05f, 0.5f);
            _loop = loop;
            _onComplete = onComplete;

            if (_target == null || _sprites.Count == 0)
            {
                Complete();
                return;
            }

            _frameIndex = 0;
            _target.sprite = _sprites[0];
            _target.enabled = true;
            _timer = _frameInterval;
            _isPlaying = !loop || _sprites.Count > 1;
        }

        public void Tick(float deltaTime)
        {
            if (!_isPlaying || _target == null)
            {
                return;
            }

            _timer -= deltaTime;
            if (_timer > 0f)
            {
                return;
            }

            _timer = _frameInterval;
            _frameIndex++;

            if (_frameIndex >= _sprites.Count)
            {
                if (_loop)
                {
                    _frameIndex = 0;
                }
                else
                {
                    Complete();
                    return;
                }
            }

            _target.sprite = _sprites[_frameIndex];
        }

        public void Stop()
        {
            _isPlaying = false;
            _onComplete = null;
        }

        private void Complete()
        {
            _isPlaying = false;
            Action callback = _onComplete;
            _onComplete = null;
            callback?.Invoke();
        }
    }
}
