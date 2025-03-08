using System.Collections;
using UnityEngine;
using UnityEngine.PostProcessing;

namespace PlayerHealth
{
    internal class PlayerHealth : MonoBehaviour
    {
        public static float health;
        public static bool isSick;
        public static float sickTimer;
        internal static bool isDead;
        private bool isEnableAutoSave;

        private void Start()
        {
            health = 95f;
            isSick = false;
            sickTimer = 100f;
            isDead = false;
            isEnableAutoSave = SaveLoadManager.instance.enableAutosave;
            Plugin.logger.LogDebug("PlayerHealth initialized.");
        }

        private void LateUpdate()
        {
            if (GameState.justStarted == true) 
            {
                SaveLoadManager.instance.enableAutosave = isEnableAutoSave;
                isDead = false;
            }
            if (isDead || PlayerNeeds.instance.godMode || GameState.wasInSettingsMenu || !GameState.playing || GameState.recovering || (bool)GameState.currentShipyard || (EconomyUI.instance.uiActive && !Debugger.buildDebugModeOn))
            {
                return;
            }

            if (PlayerNeeds.food >= 90f)
                RecoverHealth(10f * Time.deltaTime * Sun.sun.timescale);
            if (PlayerNeeds.food >= 75f && PlayerNeeds.food < 90f)
                RecoverHealth(5f * Time.deltaTime * Sun.sun.timescale);
            if (PlayerNeeds.food >= 50f && PlayerNeeds.food < 75f)
                RecoverHealth(2.5f * Time.deltaTime * Sun.sun.timescale);
            if (PlayerNeeds.food >= 15f && PlayerNeeds.food < 30f)
                TakeDamage(5f * Time.deltaTime * Sun.sun.timescale);
            if (PlayerNeeds.food > 0f && PlayerNeeds.food < 15f)
                TakeDamage(20f * Time.deltaTime * Sun.sun.timescale);
            if (PlayerNeeds.food == 0f)
                TakeDamage(40f * Time.deltaTime * Sun.sun.timescale * PlayerNeeds.foodDebt == 0 ? 2 : 1);

            if (PlayerNeeds.water >= 20f && PlayerNeeds.water < 30f)
                TakeDamage(10f * Time.deltaTime * Sun.sun.timescale);
            if (PlayerNeeds.water >= 10f && PlayerNeeds.water < 20f)
                TakeDamage(20f * Time.deltaTime * Sun.sun.timescale);
            if (PlayerNeeds.water > 0f && PlayerNeeds.water < 10f)
                TakeDamage(40f * Time.deltaTime * Sun.sun.timescale);
            if (PlayerNeeds.water == 0f)
                TakeDamage(80f * Time.deltaTime * Sun.sun.timescale);

            if (PlayerNeeds.sleep >= 0f && PlayerNeeds.sleep < 20f)
                TakeDamage(10f * Time.deltaTime * Sun.sun.timescale);
            if (PlayerNeeds.sleep == 0f)
                TakeDamage(20f * Time.deltaTime * Sun.sun.timescale);

            if (PlayerNeeds.vitamins >= 0f && PlayerNeeds.vitamins < 33f)
                TakeDamage(5f * Time.deltaTime * Sun.sun.timescale);
            if (PlayerNeeds.vitamins == 0f)
                TakeDamage(20f * Time.deltaTime * Sun.sun.timescale);

            if (PlayerNeeds.protein >= 0f && PlayerNeeds.protein < 33f)
                TakeDamage(1f * Time.deltaTime * Sun.sun.timescale);
            if (PlayerNeeds.protein == 0f)
                TakeDamage(5f * Time.deltaTime * Sun.sun.timescale);

            if (isSick)
            {
                sickTimer -= GameState.sleeping ? 2 * Time.deltaTime * Sun.sun.timescale : Time.deltaTime * Sun.sun.timescale;
                PlayerNeeds.water -= 3f * Time.deltaTime * Sun.sun.timescale;
                PlayerNeeds.food -= Time.deltaTime * Sun.sun.timescale;
                PlayerNeeds.sleep -= Time.deltaTime * Sun.sun.timescale;
                PlayerNeeds.vitamins -= 2f * Time.deltaTime * Sun.sun.timescale;
                PlayerNeeds.protein -= Time.deltaTime * Sun.sun.timescale;

                if (sickTimer <= 0)
                {
                    isSick = false;
                    sickTimer = 100f;
                }
            }

            if (GameState.sleepingInTavern)
            {
                health = 100f;
                isSick = false;
                sickTimer = 100f;
            }
        }

        private void RecoverHealth(float recovery)
        {
            if (isSick) recovery *= 0.5f;
            if (GameState.sleeping) recovery *= 2f;

            //PlayerHealthUI.instance.HealthEmit(recovery);

            health += recovery;

            if (health > 100)
            {
                health = 100;
            }
        }

        private void TakeDamage(float damage)
        {
            if (isSick) damage *= 2f;

            //PlayerHealthUI.instance.HealthEmit(-damage);

            health -= damage;

            if (health <= 0)
            {
                health = 0;
                Die();
            }
        }

        private void Die()
        {           
            StartCoroutine(DieRoutine());
        }

        private IEnumerator DieRoutine()
        {
            Plugin.logger.LogDebug("Player died.");
            PlayerNeedsUI.instance.CloseNeedsUI();
            isDead = true;
            isEnableAutoSave = SaveLoadManager.instance.enableAutosave;
            SaveLoadManager.instance.enableAutosave = false;
            MouseLook.ToggleMouseLook(false);
            Refs.SetPlayerControl(false);
            StartCoroutine(Blackout.FadeTo(1f, 3f));
            yield return new WaitForSeconds(3f);           
            ContinueUI.instance.MoveMenuToPlayer();
            ContinueUI.continueUI.SetActive(true);
            MouseLook.ToggleMouseLookAndCursor(false);
        }
    }
}
