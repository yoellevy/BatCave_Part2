using BatCave;
using Infra.Gameplay.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.batcave
{
    [CreateAssetMenu(menuName = "Bat Controller/Mouse")]
    class BatMouseController : BatController
    {
        private bool isMouseDown = false;
        protected void OnEnable()
        {
            GameInputCapture.OnTouchDown += OnTouchDown;
            GameInputCapture.OnTouchUp += OnTouchUp;
        }

        protected void OnDisable()
        {
            GameInputCapture.OnTouchDown -= OnTouchDown;
            GameInputCapture.OnTouchUp -= OnTouchUp;
        }

        public override bool WantsToFlyUp()
        {
            return isMouseDown;
        }

        private void OnTouchDown(PointerEventData e)
        {            
            isMouseDown = true;
        }

        private void OnTouchUp(PointerEventData e)
        {
            isMouseDown = false;
        }
        
    }
}
