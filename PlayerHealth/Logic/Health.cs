using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PlayerHealth
{
    internal class Health : MonoBehaviour
    {
        public float health;
        public bool isSick;
        private PlayerNeeds playerNeeds;

        private void Awake()
        {
            health = 100;
            isSick = false;
            playerNeeds = PlayerNeeds.instance;
        }
    }
}
