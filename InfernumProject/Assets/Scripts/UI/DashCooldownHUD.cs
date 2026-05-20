using Infernum.FPS.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Infernum.FPS.UI
{
    /// <summary>
    /// HUD рывка: серая иконка на кулдауне, белая заливка по мере восстановления, таймер в секундах.
    /// </summary>
    public sealed class DashCooldownHUD : MonoBehaviour
    {
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private Image iconImage;
        [SerializeField] private Image fillImage;
        [SerializeField] private TMP_Text cooldownText;
        [SerializeField] private Color readyIconColor = Color.white;
        [SerializeField] private Color cooldownIconColor = new Color(0.42f, 0.42f, 0.42f, 1f);
        [SerializeField] private Color fillColor = Color.white;

        public void Configure(PlayerMovement movement, Image icon, Image fill, TMP_Text label)
        {
            playerMovement = movement;
            iconImage = icon;
            fillImage = fill;
            cooldownText = label;
        }

        private void Awake()
        {
            if (playerMovement == null)
            {
                playerMovement = FindFirstObjectByType<PlayerMovement>();
            }
        }

        private void Update()
        {
            if (playerMovement == null || iconImage == null || fillImage == null || cooldownText == null)
            {
                return;
            }

            float remaining = playerMovement.DashCooldownRemaining;
            float progress = playerMovement.DashCooldownNormalized;
            bool ready = playerMovement.IsDashReady;

            iconImage.color = ready ? readyIconColor : cooldownIconColor;
            fillImage.color = fillColor;
            fillImage.fillAmount = Mathf.Clamp01(progress);

            if (ready)
            {
                cooldownText.text = string.Empty;
            }
            else
            {
                cooldownText.text = $"{remaining:0.0}s";
            }
        }
    }
}
