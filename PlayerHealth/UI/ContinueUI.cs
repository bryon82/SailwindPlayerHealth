using UnityEngine;

namespace PlayerHealth
{
    internal class ContinueUI: MonoBehaviour
    {
        public static ContinueUI instance;
        internal static GameObject continueUI;

        private void Awake()
        {
            instance = this;            
        }

        internal static void SetupContinueUI(StartMenu startMenu)
        {
            GameObject confirmQuitUI = startMenu.GetPrivateField<GameObject>("confirmQuitUI");
            continueUI = Instantiate(confirmQuitUI, confirmQuitUI.transform.parent);
            continueUI.name = "continue UI";
            continueUI.SetActive(false);

            GameObject text = continueUI.transform.GetChild(4).gameObject;
            text.GetComponent<TextMesh>().text = "You died\n\n\nContinue from last save?";
            Destroy(continueUI.transform.GetChild(2).gameObject);

            GameObject quitButton = continueUI.transform.GetChild(1).gameObject;
            GameObject continueButton = Instantiate(quitButton, quitButton.transform.parent);

            quitButton.name = "button quit no save";
            quitButton.transform.localPosition = new Vector3(0.5f, -0.35f, 0f);
            GameObject smButton = quitButton.GetComponentInChildren<StartMenuButton>().gameObject;
            Destroy(smButton.GetComponent<StartMenuButton>());
            smButton.AddComponent<GPQuitNoSaveButton>().startMenu = startMenu;
            
            continueButton.name = "button continue";
            continueButton.transform.localPosition = new Vector3(-0.5f, -0.35f, 0f);
            continueButton.GetComponentInChildren<TextMesh>().text = "Continue";
            smButton = continueButton.GetComponentInChildren<StartMenuButton>().gameObject;
            Destroy(smButton.GetComponent<StartMenuButton>());
            smButton.AddComponent<GPLoadLastSave>().continueUI = continueUI;
        }
    }
}
