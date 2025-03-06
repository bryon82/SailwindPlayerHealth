using UnityEngine;

namespace PlayerHealth
{
    internal class GPQuitNoSaveButton : GoPointerButton
    {
        public StartMenu startMenu;

        public override void OnActivate()
        {
            Application.Quit();
        }

        public GPQuitNoSaveButton()
        {
        }        
    }
}