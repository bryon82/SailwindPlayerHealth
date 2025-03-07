using UnityEngine;

namespace PlayerHealth
{
    internal class PlayerHealthUI : MonoBehaviour
    {
        public static PlayerHealthUI instance;
        private static Transform healthBar;
        private static Material healthBarMat;
        readonly static Color healthy = new Color32(0xE9, 0x24, 0x42, 0x90);
        readonly static Color sick = new Color32(0x8B, 0xBF, 0x2C, 0x80);
        private static PlayerNeedsUI playerNeedsUI;

        private void Awake()
        {
            instance = this;
            Plugin.logger.LogDebug("PlayerHealthUI initialized.");
        }

        private void Update()
        {
            UpdateBars();

            var warningCooldown = playerNeedsUI.GetPrivateField<float>("warningCooldown");
            if (warningCooldown > 0f)
            {
                playerNeedsUI.SetPrivateField("warningCooldown", warningCooldown - Time.deltaTime);
            }
            else if (!GameState.recovering && !GameState.currentlyLoading && !GameState.justStarted && GameState.playing)
            {
                if (PlayerHealth.health < 20f)
                {
                    playerNeedsUI.PlayWarning(healthBar, true);
                }
            }
        }

        internal static void SetupPlayerHealthUI(PlayerNeedsUI playerNeedsUI)
        {
            PlayerHealthUI.playerNeedsUI = playerNeedsUI;

            var staticBg = Instantiate(playerNeedsUI.transform.GetChild(20).gameObject, playerNeedsUI.transform);
            staticBg.name = "health static";
            staticBg.transform.localPosition = new Vector3(staticBg.transform.localPosition.x, staticBg.transform.localPosition.y - 0.003f, 0f);

            var healthBarGO = Instantiate(playerNeedsUI.transform.GetChild(3).gameObject, playerNeedsUI.transform);
            healthBar = healthBarGO.transform;
            healthBarGO.name = "bar_health";
            healthBar.localPosition = new Vector3(healthBar.localPosition.x, healthBar.localPosition.y, 0.045f);
            healthBar.localScale = new Vector3(healthBar.localScale.x, healthBar.localScale.y, 0.9f);
            healthBarMat = healthBarGO.GetComponent<Renderer>().material;
            healthBarMat.name = "health_mat";
            healthBarMat.color = healthy;            
            var healthBarMeshFilter = healthBarGO.GetComponent<MeshFilter>();
            healthBarMeshFilter.mesh.name = "bar_health instance";

            GameObject healthIcon = GameObject.CreatePrimitive(PrimitiveType.Quad);
            healthIcon.layer = 5;
            Destroy(healthIcon.GetComponent<MeshCollider>());
            healthIcon.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            healthIcon.transform.SetParent(playerNeedsUI.transform, false);
            healthIcon.transform.localScale = new Vector3(0.015f, 0.015f, 0.25f);
            healthIcon.transform.localEulerAngles = new Vector3(90f, 180f, 0f);
            healthIcon.transform.localPosition = new Vector3(0.15f, 0.038f, 0.05f);
            healthIcon.name = "icon_health";
            healthIcon.GetComponent<MeshRenderer>().material = AssetsLoader.redCrossMaterial;
            healthIcon.SetActive(true);
        }

        internal void UpdateBars()
        {
            float t = Time.timeScale == 0f ? 1f : Time.deltaTime * 2f;
            Vector3 localScale = healthBar.localScale;
            localScale.x = Mathf.Lerp(localScale.x, PlayerHealth.health * 0.01f + 0.01f, t);
            healthBar.localScale = localScale;
            healthBarMat.SetColor("_EmissionColor", healthy);
            if (PlayerHealth.isSick) healthBarMat.SetColor("_EmissionColor", sick);
        }
    }
}
