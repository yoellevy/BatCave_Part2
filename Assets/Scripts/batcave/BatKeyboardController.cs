using BatCave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.batcave
{
    [CreateAssetMenu(menuName = "Bat Controller/keyboard")]
    class BatKeyboardController : BatController
    {
        bool flyUp;
        public override bool WantsToFlyUp()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                flyUp = true;
            } else if(Input.GetKeyUp(KeyCode.Space))
            {
                flyUp = false;
            }
            return flyUp;
        }
    }
}
