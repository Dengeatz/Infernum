using System;
using System.Collections.Generic;
using Infernum.FPS.Weapons.Config;
using UnityEngine;
using UnityEngine.UI;

namespace Infernum.FPS.Player
{
    /// <summary>
    /// UI-оружие на экране: позиция, Image и проигрывание idle / fire / reload из конфигов.
    /// </summary>
    public sealed class PlayerWeaponAnimationService : MonoBehaviour, IPlayerWeaponAnimationService
    {
        [SerializeField] private Canvas targetCanvas;
        [SerializeField] private RectTransform weaponSlot;
        [SerializeField] private Image weaponImage;
        [SerializeField] private Vector2 anchoredPosition = new Vector2(48f, 48f);
        [SerializeField] private Vector2 weaponSize = new Vector2(280f, 200f);
        [SerializeField] private bool createCanvasIfMissing = true;

        private readonly SpriteSequenceAnimator _animator = new();
        private WeaponConfig _config;

        public Image WeaponImage => weaponImage;
        public bool IsPlaying => _animator.IsPlaying;

        private void Awake()
        {
            EnsureUI();
            _animator.Bind(weaponImage);
            Hide();
        }

        public void BindWeapon(WeaponConfig config)
        {
            _config = config;
            EnsureUI();

            if (weaponSlot != null)
            {
                weaponSlot.gameObject.SetActive(config != null);
            }

            if (config == null)
            {
                Hide();
                return;
            }

            PlayIdle();
        }

        public void Hide()
        {
            _animator.Stop();

            if (weaponImage != null)
            {
                weaponImage.enabled = false;
                weaponImage.sprite = null;
            }

            if (weaponSlot != null)
            {
                weaponSlot.gameObject.SetActive(false);
            }
        }

        public void PlayIdle()
        {
            if (_config == null)
            {
                return;
            }

            PlaySequence(
                _config.GetIdleSprites(),
                _config.AnimationConfig != null ? _config.AnimationConfig.IdleFrameInterval : 0.35f,
                loop: true,
                onComplete: null);
        }

        public void PlayFire(Action onComplete)
        {
            if (_config == null)
            {
                onComplete?.Invoke();
                return;
            }

            PlaySequence(
                _config.GetFireSprites(),
                _config.AnimationConfig != null ? _config.AnimationConfig.FireFrameInterval : 0.2f,
                loop: false,
                onComplete);
        }

        public void PlayReload(Action onComplete)
        {
            if (_config == null)
            {
                onComplete?.Invoke();
                return;
            }

            PlaySequence(
                _config.GetReloadSprites(),
                _config.AnimationConfig != null ? _config.AnimationConfig.ReloadFrameInterval : 0.45f,
                loop: false,
                onComplete);
        }

        public void Tick(float deltaTime)
        {
            _animator.Tick(deltaTime);
        }

        private void PlaySequence(IReadOnlyList<Sprite> sprites, float frameInterval, bool loop, Action onComplete)
        {
            EnsureUI();

            if (weaponSlot != null)
            {
                weaponSlot.gameObject.SetActive(true);
            }

            _animator.Play(sprites, frameInterval, loop, onComplete);
        }

        private void EnsureUI()
        {
            if (weaponImage != null && weaponSlot != null)
            {
                return;
            }

            if (!createCanvasIfMissing)
            {
                return;
            }

            if (targetCanvas == null)
            {
                var canvasGo = new GameObject("WeaponUI_Canvas");
                canvasGo.transform.SetParent(transform, false);
                targetCanvas = canvasGo.AddComponent<Canvas>();
                targetCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                targetCanvas.sortingOrder = 50;
                var scaler = canvasGo.AddComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920f, 1080f);
                canvasGo.AddComponent<GraphicRaycaster>();
            }

            if (weaponSlot == null)
            {
                var slotGo = new GameObject("WeaponHandSlot", typeof(RectTransform));
                slotGo.transform.SetParent(targetCanvas.transform, false);
                weaponSlot = slotGo.GetComponent<RectTransform>();
                weaponSlot.anchorMin = new Vector2(0f, 0f);
                weaponSlot.anchorMax = new Vector2(0f, 0f);
                weaponSlot.pivot = new Vector2(0f, 0f);
                weaponSlot.anchoredPosition = anchoredPosition;
                weaponSlot.sizeDelta = weaponSize;
            }

            if (weaponImage == null)
            {
                weaponImage = weaponSlot.gameObject.AddComponent<Image>();
                weaponImage.raycastTarget = false;
                weaponImage.preserveAspect = true;
            }

            _animator.Bind(weaponImage);
        }
    }
}
