//======= Copyright (c) Valve Corporation, All rights reserved. ===============

using UnityEngine;
using System.Collections;

namespace Valve.VR.InteractionSystem.Sample
{
    public class ContinueButton : UIElement
    {
        public GameObject infoPanel;
        public AudioSource endSound;

        protected override void Awake()
        {
            base.Awake();

           // ui = this.GetComponentInParent<SkeletonUIOptions>();
        }

        protected override void OnButtonClick()
        {
            base.OnButtonClick();
            endSound.Play();
            Destroy(infoPanel, endSound.clip.length);
            //infoPanel.SetActive(false);

            //if (ui != null)
            //{
            //    ui.SetRenderModel(this);
            //}
            Debug.Log("Continue Button pressed");
        }
    }
}