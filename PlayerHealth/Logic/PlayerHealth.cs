using System.Collections;
using UnityEngine;

namespace PlayerHealth
{
    internal class PlayerHealth : MonoBehaviour
    {
        public static float health;
        public static bool isSick;
        internal static float sickTimer;
        private static bool isDead;

        private void Start()
        {
            health = 95f;
            isSick = false;
            sickTimer = 100f;
            isDead = false;
            Plugin.logger.LogDebug("PlayerHealth initialized.");
        }

        private void LateUpdate()
        {
            if (isDead || PlayerNeeds.instance.godMode || !GameState.playing || GameState.recovering || (bool)GameState.currentShipyard || (EconomyUI.instance.uiActive && !Debugger.buildDebugModeOn))
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
                TakeDamage(40f * Time.deltaTime * Sun.sun.timescale);
            if (PlayerNeeds.food == 0f && PlayerNeeds.foodDebt == 0)
                TakeDamage(80f * Time.deltaTime * Sun.sun.timescale);

            if (PlayerNeeds.water >= 20f && PlayerNeeds.water < 30f)
                TakeDamage(10f * Time.deltaTime * Sun.sun.timescale);
            if (PlayerNeeds.water >= 10f && PlayerNeeds.water < 20f)
                TakeDamage(20f * Time.deltaTime * Sun.sun.timescale);
            if (PlayerNeeds.water > 0f && PlayerNeeds.water < 10f)
                TakeDamage(40f * Time.deltaTime * Sun.sun.timescale);
            if (PlayerNeeds.water == 0f)
                TakeDamage(80f * Time.deltaTime * Sun.sun.timescale);

            if (PlayerNeeds.sleep >= 10f && PlayerNeeds.sleep > 0f)
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
                sickTimer -= Time.deltaTime * Sun.sun.timescale;
                PlayerNeeds.water -= 3f * Time.deltaTime * Sun.sun.timescale;
                PlayerNeeds.food -= Time.deltaTime * Sun.sun.timescale;
                PlayerNeeds.sleep -= Time.deltaTime * Sun.sun.timescale;
                PlayerNeeds.vitamins -= 2f * Time.deltaTime * Sun.sun.timescale;
                PlayerNeeds.protein -= Time.deltaTime * Sun.sun.timescale;

                if (sickTimer <= 0)
                {
                    isSick = false;
                    sickTimer = 300;
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
            
            health += recovery;

            if (health > 100)
            {
                health = 100;
            }
        }

        private void TakeDamage(float damage)
        {
            if (isSick) damage *= 2f;

            health -= damage;

            if (health <= 0)
            {
                health = 0;
                Die();
            }
        }

        private void Die()
        {
            isDead = true;
            Plugin.logger.LogDebug("Player died.");
            StartCoroutine(DieRoutine());
        }

        private IEnumerator DieRoutine()
        {            
            PlayerNeedsUI.instance.CloseNeedsUI();
            MouseLook.ToggleMouseLook(false);
            Refs.SetPlayerControl(false);
            StartCoroutine(Blackout.FadeTo(1f, 3f));
            yield return new WaitForSeconds(3f);
            ContinueUI.continueUI.SetActive(true);
            MouseLook.ToggleMouseLookAndCursor(false);
        }
    }
}
