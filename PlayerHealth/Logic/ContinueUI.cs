using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PlayerHealth
{
    internal class ContinueUI: MonoBehaviour
    {
        public static ContinueUI instance;
        internal static GameObject continueUI;
        internal static StartMenu startMenu;
        private const StartMenuButtonType loadLastSave = (StartMenuButtonType)12;
        private const StartMenuButtonType quitNoSave = (StartMenuButtonType)13;

        private void Awake()
        {
            instance = this;
            SetupContinueUI();
        }

        private void SetupContinueUI()
        {
            startMenu = FindObjectOfType<StartMenu>();        

            GameObject confirmQuitUI = startMenu.GetPrivateField<GameObject>("confirmQuitUI");
            continueUI = Instantiate(confirmQuitUI, confirmQuitUI.transform.parent);
            continueUI.name = "continue UI";

            GameObject text = continueUI.transform.GetChild(4).gameObject;
            text.GetComponent<TextMesh>().text = "You died\nContinue from last save?";
            Destroy(continueUI.transform.GetChild(2).gameObject);

            GameObject quitButton = continueUI.transform.GetChild(1).gameObject;
            GameObject continueButton = Instantiate(quitButton, quitButton.transform.parent);

            quitButton.name = "button quit no save";
            quitButton.transform.localPosition = new Vector3(0.5f, quitButton.transform.position.y, quitButton.transform.position.z);
            GameObject smButton = quitButton.GetComponentInChildren<StartMenuButton>().gameObject;
            Destroy(smButton.GetComponent<StartMenuButton>());
            smButton.AddComponent<GPQuitNoSaveButton>().startMenu = startMenu;
            
            continueButton.name = "button continue";
            continueButton.transform.localPosition = new Vector3(-0.5f, continueButton.transform.position.y, continueButton.transform.position.z);
            smButton = continueButton.GetComponentInChildren<StartMenuButton>().gameObject;
            Destroy(smButton.GetComponent<StartMenuButton>());
            smButton.AddComponent<GPLoadLastSave>().continueUI = instance;
            smButton.AddComponent<GPLoadLastSave>().startMenu = startMenu;

        }
    }
}
