using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class infoPanel : MonoBehaviour
    {
        public AudioSource startClip;
        public AudioSource infoClip;

        void Start()
        {
            startClip.Play();
            Invoke("PlayInfoClip", startClip.clip.length);
    }

        void Update()
        {
        }

        void PlayInfoClip()
        {
            infoClip.Play();
        }
}
