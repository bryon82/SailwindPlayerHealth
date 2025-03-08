using UnityEngine;

namespace PlayerHealth
{
    internal class GPLoadLastSave : GoPointerButton
    {        
        public GameObject continueUI;

        public override void OnActivate()
        {
            continueUI.gameObject.SetActive(false);            
            GameObject.Find("start menu UI").GetComponent<StartMenu>().InvokePrivateMethod("LoadGame", 0);
        }
    }
}
