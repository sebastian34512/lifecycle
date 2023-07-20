using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lifecycle
{
    public class Growing : MonoBehaviour
    {
        public float growthDuration = 20f;
        public Vector3 targetScale = new Vector3(2f, 2f, 2f); // Zielgröße des Objekts
        public float growthProgress = 0f;

        private Vector3 initialScale;
        private float startTime;
        private Animator anim;
        private float animationTime;
        private bool audioPlayed = false;

        private void Start()
        {
            initialScale = transform.localScale;
            startTime = Time.time;

            anim = GetComponent<Animator>();

        }

        private void Update()
        {
            if (anim && anim.GetCurrentAnimatorStateInfo(0).IsName("bulging"))
            {
                animationTime += Time.deltaTime;
                Debug.Log("Animation spielt. Aktuelle Zeit: " + animationTime);
                if (!audioPlayed)
                {
                    GetComponent<AudioSource>().Play();
                    audioPlayed = true;
                }

                if (animationTime <= growthDuration)
                {
                    float t = animationTime / growthDuration; // Normalisierte Zeit (0 bis 1)
                    growthProgress = t;
                    Debug.Log("Fortschritt: " + growthProgress);

                    transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
                }
                else
                {
                    // Wachstum abgeschlossen
                    transform.localScale = targetScale;

                    //if (audioPlayed)
                    //{
                    //    GetComponent<AudioSource>().Stop();
                    //    audioPlayed = false;
                    //}
                }
            }
            else
            {
                // Animation "bulging" spielt nicht mehr
                if (audioPlayed)
                {
                    GetComponent<AudioSource>().Stop();
                    audioPlayed = false;
                }
            }
        }
    }
}
