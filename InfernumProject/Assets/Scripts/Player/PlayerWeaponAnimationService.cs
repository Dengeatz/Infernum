using System;
using System.Collections.Generic;
using Infernum.FPS.Core.Animation;
using Infernum.FPS.Weapons.Config;
using UnityEngine;
using UnityEngine.UI;

namespace Infernum.FPS.Player
{
    /// <summary>
    /// UI-оружие на экране: idle / fire / reload из конфигов.
    /// </summary>
    public sealed class PlayerWeaponAnimationService : SpriteSequenceAnimationServiceBase, IPlayerWeaponAnimationService
    {
        [SerializeField] private Canvas targetCanvas;
        [SerializeField] private RectTransform weaponSlot;
        [SerializeField] private Image weaponImage;
        [SerializeField] private Vector2 anchoredPosition = new Vector2(48f, 48f);
        [SerializeField] private Vector2 weaponSize = new Vector2(280f, 200f);
        [SerializeField] private bool createCanvasIfMissing = true;

        private ImageSpriteTarget _spriteTarget;
        private WeaponConfig _config;

        public Image WeaponImage => weaponImage;

        private void Awake()
        {
            EnsureTarget();
            StopAndHide();
        }

        public void BindWeapon(WeaponConfig config)
        {
            _config = config;

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

        public void PlayFire(Action onComplete)
        {
            if (_config == null)
            {
                onComplete?.Invoke();
                return;
            }

            PlaySequence(
                _config.GetFireSprites(),
                GetInterval(_config.AnimationConfig.FireFrameInterval, 0.2f),
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
                GetInterval(_config.AnimationConfig.ReloadFrameInterval, 0.45f),
                loop: false,
                onComplete);
        }

        protected override void EnsureTarget()
        {
            if (weaponImage != null && weaponSlot != null)
            {
                if (_spriteTarget == null)
                {
                    _spriteTarget = new ImageSpriteTarget(weaponImage);
                    Animator.Bind(_spriteTarget);
                }

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

            _spriteTarget = new ImageSpriteTarget(weaponImage);
            Animator.Bind(_spriteTarget);
        }

        protected override void SetVisible(bool visible)
        {
            if (weaponSlot != null)
            {
                weaponSlot.gameObject.SetActive(visible);
            }

            if (weaponImage != null)
            {
                weaponImage.enabled = visible;
                if (!visible)
                {
                    weaponImage.sprite = null;
                }
            }
        }

        private static float GetInterval(float value, float fallback)
        {
            return value > 0f ? value : fallback;
        }
    }
}
