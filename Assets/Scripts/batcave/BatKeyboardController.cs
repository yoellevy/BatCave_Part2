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
        public override bool WantsToFlyUp()
        {
            return Input.GetKeyDown(KeyCode.Space);
        }
    }
}
