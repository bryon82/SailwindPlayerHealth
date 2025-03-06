using System.Collections;
using UnityEngine;

namespace PlayerHealth
{
    internal class GPLoadLastSave : GoPointerButton
    {
        
        private int animsPlaying;
        public ContinueUI continueUI;
        public StartMenu startMenu;
        private Transform playerObserver;
        private GameObject playerController;

        public override void OnActivate()
        {
            continueUI.gameObject.SetActive(false);
            startMenu.InvokePrivateMethod("LoadGame", 0);
            //if (animsPlaying > 0)
            //{
            //    return;
            //}
            //animsPlaying++;
            //continueUI.SetActive(false);
            //StartCoroutine(LoadGameAnimation());
        }

        //private void Awake()
        //{
        //    playerController = startMenu.GetPrivateField<GameObject>("playerController");
        //    playerObserver = startMenu.GetPrivateField<Transform>("playerObserver");
        //}

        //private IEnumerator LoadGameAnimation()
        //{
            
        //    GameState.currentlyLoading = true;
        //    playerObserver.transform.parent = base.transform.parent;
        //    StartCoroutine(Blackout.FadeTo(1f, 0.5f));
        //    yield return new WaitForSeconds(0.5f);
        //    Sleep.instance.recoveryText.text = "\n\n\nloading...";
        //    while (EditorDebugger.instance.interruptLoadingSequence)
        //    {
        //        yield return new WaitForEndOfFrame();
        //    }

        //    Debug.Log("StartMenu: calling LoadGame()");
        //    SaveLoadManager.instance.LoadGame(0);
        //    if (!PlayerPrefs.HasKey("tutorialHints"))
        //    {
        //        PlayerPrefs.SetInt("tutorialHints", 0);
        //    }

        //    yield return new WaitForSeconds(1.5f);
        //    yield return new WaitForEndOfFrame();
        //    SaveLoadManager.readyToSave = true;
        //    GameState.playing = true;
        //    GameState.justStarted = true;
        //    GameState.currentlyLoading = false;
        //    MouseLook.ToggleMouseLook(newState: true);
        //    yield return new WaitForEndOfFrame();
        //    playerController.GetComponent<CharacterController>().enabled = true;
        //    playerController.GetComponent<OVRPlayerController>().enabled = true;
        //    playerObserver.gameObject.GetComponent<PlayerControllerMirror>().enabled = true;
        //    MouseLook.ToggleMouseLookAndCursor(newState: true);
        //    Sleep.instance.recoveryText.text = "";
        //    yield return new WaitForEndOfFrame();
        //    yield return new WaitForEndOfFrame();            
        //    StartCoroutine(Blackout.FadeTo(0f, 2f));
        //    animsPlaying--;
        //    yield return new WaitForSeconds(2f);
        //    GameState.justStarted = false;
        //    Debug.Log("continue ui load animation finished.");
        //}
    }
}
