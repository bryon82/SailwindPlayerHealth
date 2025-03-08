using System.Collections;
using UnityEngine.PostProcessing;
using UnityEngine;
using System;

namespace PlayerHealth
{
    internal class PlayerHealthWarningEffects : MonoBehaviour
    {
        internal static PlayerHealthWarningEffects instance;
        internal PostProcessingProfile postProcessing;
        private float healthWarning;
        private float sickWarning;

        readonly static Color healthy = new Color32(0xE9, 0x24, 0x42, 0x90);
        readonly static Color sick = new Color32(0x8B, 0xBF, 0x2C, 0x80);

        private void Awake()
        {            
            instance = this;
        }

        private void Update()
        {
            float num = Mathf.InverseLerp(12f, 0f, PlayerHealth.health);
            if (healthWarning > num)
            {
                num = healthWarning;
            }

            VignetteModel.Settings settings = postProcessing.vignette.settings;
            settings.intensity = Mathf.Lerp(0.26f, 0.5f, num);
            settings.color = Color.Lerp(Color.clear, healthy, num);
            postProcessing.vignette.settings = settings;
            
            float num2 = Mathf.InverseLerp(12f, 0f, PlayerHealth.isSick ? 200f - PlayerHealth.sickTimer : 200f);
            if (sickWarning > num2)
            {
                num2 = sickWarning;
            }
            VignetteModel.Settings settings2 = postProcessing.vignette.settings;
            settings.intensity = Mathf.Lerp(0.26f, 0.5f, num2);
            settings.color = Color.Lerp(Color.clear, sick, num2);
            postProcessing.vignette.settings = settings;
        }

        public void PlayHealthWarning()
        {
            StartCoroutine(HealthWarningSequence());
        }

        private IEnumerator HealthWarningSequence()
        {
            Plugin.logger.LogDebug("Playing health warning sequence...");
            for (float t2 = 0f; t2 < 1f; t2 += Time.deltaTime * 0.33f)
            {
                healthWarning = t2;
                yield return new WaitForEndOfFrame();
            }

            for (float t2 = 1f; t2 > 0f; t2 -= Time.deltaTime * 0.33f)
            {
                healthWarning = t2;
                yield return new WaitForEndOfFrame();
            }

            Plugin.logger.LogDebug("Finished health warning sequence.");
            healthWarning = 0f;
        }

        public void PlaySickWarning()
        {
            StartCoroutine(SickWarningSequence());
        }

        private IEnumerator SickWarningSequence()
        {
            Plugin.logger.LogDebug("Playing sick warning sequence...");           
            for (float t2 = 0f; t2 < 1f; t2 += Time.deltaTime * 0.66f)
            {
                sickWarning = t2;
                yield return new WaitForEndOfFrame();
            }

            for (float t2 = 1f; t2 > 0f; t2 -= Time.deltaTime * 0.66f)
            {
                sickWarning = t2;
                yield return new WaitForEndOfFrame();
            }

            Plugin.logger.LogDebug("Finished sick warning sequence.");
            sickWarning = 0f;
        }
    }
}