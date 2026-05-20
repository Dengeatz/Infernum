using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Infernum.FPS.UI
{
    /// <summary>
    /// Создаёт Canvas и виджет кулдауна рывка при старте сцены.
    /// Добавь на пустой объект в GameplayScene или оставь автоустановку.
    /// </summary>
    public sealed class DashHudInstaller : MonoBehaviour
    {
        [SerializeField] private Vector2 anchorPosition = new Vector2(36f, 36f);
        [SerializeField] private float iconSize = 72f;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AutoInstall()
        {
            if (FindFirstObjectByType<DashHudInstaller>() != null)
            {
                return;
            }

            var installerGo = new GameObject("[Dash HUD Installer]");
            installerGo.AddComponent<DashHudInstaller>();
        }

        private void Awake()
        {
            BuildHud();
            Destroy(this);
        }

        private void BuildHud()
        {
            Sprite dashSprite = LoadDashSprite();

            var canvasGo = new GameObject("DashHUD_Canvas");
            var canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;
            canvasGo.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasGo.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920f, 1080f);
            canvasGo.AddComponent<GraphicRaycaster>();

            var root = CreateRect("DashCooldownWidget", canvasGo.transform);
            var rootRect = root.GetComponent<RectTransform>();
            rootRect.anchorMin = new Vector2(0f, 0f);
            rootRect.anchorMax = new Vector2(0f, 0f);
            rootRect.pivot = new Vector2(0f, 0f);
            rootRect.anchoredPosition = anchorPosition;
            rootRect.sizeDelta = new Vector2(iconSize + 48f, iconSize + 24f);

            var iconRoot = CreateRect("Icon", root.transform);
            var iconRootRect = iconRoot.GetComponent<RectTransform>();
            iconRootRect.anchorMin = new Vector2(0f, 0f);
            iconRootRect.anchorMax = new Vector2(0f, 0f);
            iconRootRect.pivot = new Vector2(0f, 0f);
            iconRootRect.sizeDelta = new Vector2(iconSize, iconSize);
            iconRootRect.anchoredPosition = new Vector2(0f, 8f);

            var iconBg = CreateImage("IconBackground", iconRoot.transform, dashSprite);
            iconBg.color = new Color(0.42f, 0.42f, 0.42f, 1f);
            Stretch(iconBg.rectTransform, Vector2.zero, Vector2.one);

            var fill = CreateImage("IconFill", iconRoot.transform, dashSprite);
            fill.color = Color.white;
            fill.type = Image.Type.Filled;
            fill.fillMethod = Image.FillMethod.Vertical;
            fill.fillOrigin = (int)Image.OriginVertical.Bottom;
            fill.fillAmount = 1f;
            Stretch(fill.rectTransform, Vector2.zero, Vector2.one);

            var labelGo = CreateRect("CooldownLabel", root.transform);
            var labelRect = labelGo.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0f, 0f);
            labelRect.anchorMax = new Vector2(0f, 0f);
            labelRect.pivot = new Vector2(0f, 0.5f);
            labelRect.anchoredPosition = new Vector2(iconSize + 10f, iconSize * 0.5f + 8f);
            labelRect.sizeDelta = new Vector2(56f, 28f);

            var text = labelGo.AddComponent<TextMeshProUGUI>();
            text.font = LoadDefaultTmpFont();
            text.fontSize = 20f;
            text.fontStyle = FontStyles.Bold;
            text.alignment = TextAlignmentOptions.Center;
            text.color = Color.white;
            text.overflowMode = TextOverflowModes.Overflow;
            text.raycastTarget = false;

            var hud = root.AddComponent<DashCooldownHUD>();
            hud.Configure(
                FindFirstObjectByType<Infernum.FPS.Player.PlayerMovement>(),
                iconBg,
                fill,
                text);
        }

        private static Sprite LoadDashSprite()
        {
            return Resources.Load<Sprite>("UI/dash_icon");
        }

        private static TMP_FontAsset LoadDefaultTmpFont()
        {
            var font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
            if (font != null)
            {
                return font;
            }

            if (TMP_Settings.defaultFontAsset != null)
            {
                return TMP_Settings.defaultFontAsset;
            }

            return null;
        }

        private static GameObject CreateRect(string name, Transform parent)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            return go;
        }

        private static Image CreateImage(string name, Transform parent, Sprite sprite)
        {
            var go = CreateRect(name, parent);
            var image = go.AddComponent<Image>();
            image.sprite = sprite;
            image.raycastTarget = false;
            return image;
        }

        private static void Stretch(RectTransform rect, Vector2 min, Vector2 max)
        {
            rect.anchorMin = min;
            rect.anchorMax = max;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            rect.pivot = new Vector2(0.5f, 0.5f);
        }

    }
}
