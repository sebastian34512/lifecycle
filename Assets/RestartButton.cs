//======= Copyright (c) Valve Corporation, All rights reserved. ===============

using UnityEngine;
using System.Collections;

namespace Valve.VR.InteractionSystem.Sample
{
    public class RestartButton : UIElement
    {
        public AudioSource infoSound;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnButtonClick()
        {
            base.OnButtonClick();
            infoSound.Stop();
            infoSound.Play();
            Debug.Log("Continue Button pressed");
        }
    }
}