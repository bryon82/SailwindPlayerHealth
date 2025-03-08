using System.CodeDom;
using UnityEngine;

namespace PlayerHealth
{
    internal class PlayerHealthUI : MonoBehaviour
    {
        public static PlayerHealthUI instance;
        private static Transform healthBar;
        private static Material healthBarMat;
        private static GameObject waterBar;
        private static PlayerNeedsUI playerNeedsUI;
        private static TextMesh waterHealthText;
        private static TextMesh foodHealthText;
        private static TextMesh sleepHealthText;
        private static TextMesh healthText;
        private static float lastHealth;

        readonly static Color healthy = new Color32(0xE9, 0x24, 0x42, 0x90);
        readonly static Color sick = new Color32(0x8B, 0xBF, 0x2C, 0x80);
        //readonly static Color recoveryColor = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
        //readonly static Color damageColor = new Color32(0x00, 0x05, 0x05, 0xFF);
        readonly static string red = "#4D0000";
        readonly static string green = "#003300";

        private void Awake()
        {
            instance = this;
            Plugin.logger.LogDebug("PlayerHealthUI initialized.");
            lastHealth = 0f;
        }

        private void Update()
        {
            UpdateBars();
            UpdateHealthTexts();

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
                    PlayerHealthWarningEffects.instance.PlayHealthWarning();
                }
            }
        }

        internal static void SetupPlayerHealthUI(PlayerNeedsUI playerNeedsUI)
        {
            PlayerHealthUI.playerNeedsUI = playerNeedsUI;            
            
            var staticBg = Instantiate(playerNeedsUI.transform.GetChild(20).gameObject, playerNeedsUI.transform);
            staticBg.name = "health static";
            staticBg.transform.localPosition = new Vector3(staticBg.transform.localPosition.x, staticBg.transform.localPosition.y - 0.003f, 0f);

            waterBar = playerNeedsUI.transform.GetChild(3).gameObject;
            var healthBarGO = Instantiate(waterBar, playerNeedsUI.transform);
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
            healthIcon.transform.localScale = new Vector3(0.02f, 0.015f, 0.25f);
            healthIcon.transform.localEulerAngles = new Vector3(90f, 180f, 0f);
            healthIcon.transform.localPosition = new Vector3(0.149f, 0.038f, 0.05f);
            healthIcon.name = "icon_health";
            healthIcon.GetComponent<MeshRenderer>().material = AssetsLoader.healthIconMaterial;
            healthIcon.SetActive(true);

            var waterHealthGO = Instantiate(playerNeedsUI.transform.GetChild(23).GetChild(0).gameObject, playerNeedsUI.transform);
            waterHealthGO.name = "text_water_health";
            waterHealthText = waterHealthGO.GetComponent<TextMesh>();
            waterHealthText.transform.localPosition = new Vector3(-0.175f, 0.0038f, 0f);
            waterHealthText.transform.localEulerAngles = new Vector3(90f, 180f, 0f);
            waterHealthText.fontSize = 60;
            waterHealthText.text = "";
            var foodHealthGO = Instantiate(playerNeedsUI.transform.GetChild(23).GetChild(0).gameObject, playerNeedsUI.transform);
            foodHealthGO.name = "text_food_health";
            foodHealthText = foodHealthGO.GetComponent<TextMesh>();
            foodHealthText.transform.localPosition = new Vector3(-0.175f, 0.0038f, 0.0225f);
            foodHealthText.transform.localEulerAngles = new Vector3(90f, 180f, 0f);
            foodHealthText.fontSize = 60;
            foodHealthText.text = "";
            var sleepHealthGO = Instantiate(playerNeedsUI.transform.GetChild(23).GetChild(0).gameObject, playerNeedsUI.transform);
            sleepHealthGO.name = "text_sleep_health";
            sleepHealthText = sleepHealthGO.GetComponent<TextMesh>();
            sleepHealthText.transform.localPosition = new Vector3(-0.175f, 0.0038f, -0.0225f);
            sleepHealthText.transform.localEulerAngles = new Vector3(90f, 180f, 0f);
            sleepHealthText.fontSize = 60;
            sleepHealthText.text = "";
            var healthGO = Instantiate(playerNeedsUI.transform.GetChild(23).GetChild(0).gameObject, playerNeedsUI.transform);
            healthGO.name = "text_health";
            healthText = healthGO.GetComponent<TextMesh>();
            healthText.transform.localPosition = new Vector3(-0.175f, 0.0038f, 0.045f);
            healthText.transform.localEulerAngles = new Vector3(90f, 180f, 0f);
            healthText.fontSize = 65;
            healthText.text = "";
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

        internal void UpdateHealthTexts()
        {
            if (PlayerNeeds.water < 30f)
                waterHealthText.text = $"<color={red}>❤︎↓</color>";
            else
                waterHealthText.text = "";
            
            if (PlayerNeeds.food < 30f || PlayerNeeds.vitamins < 33f || PlayerNeeds.protein < 33f)
                foodHealthText.text = $"<color={red}>❤︎↓</color>";
            else if (PlayerNeeds.food >= 50)
                foodHealthText.text = $"<color={red}>❤︎</color><color={green}>↑</color>";
            else
                foodHealthText.text = "";

            if (PlayerNeeds.sleep < 20f)
                sleepHealthText.text = $"<color={red}>❤︎↓</color>";
            else
                sleepHealthText.text = "";
            
            lastHealth = lastHealth != 0f ? lastHealth : PlayerHealth.health;
            var healthChange = PlayerHealth.health - lastHealth;
            if (healthChange > 0f)
                healthText.text = $"<color={green}>↑</color>";
            else if (healthChange < 0f)
                healthText.text = $"<color={red}>↓</color>";
            else
                healthText.text = "";
            lastHealth = PlayerHealth.health;
        }

        //internal void HealthEmit(float amt)
        //{
        //    healthChange = 0f;
        //    healthEmitCooldown = 0f;
        //    while (healthEmitCooldown < 3f)
        //    {
        //        healthEmitCooldown += Time.deltaTime;
        //        healthChange += amt;
        //    }
        //    float num = Mathf.InverseLerp(0f, 0.5f, Mathf.Abs(healthChange));
        //    float t = Mathf.InverseLerp(0f, 5f, Mathf.Abs(healthEmitCooldown)) * num;
        //    var color = healthChange >= 0 ? recoveryColor : damageColor;
        //    healthBarMat.SetColor("_EmissionColor", Color.Lerp(PlayerHealth.isSick ? sick : healthy, color, t));
            
        //}
    }
}
