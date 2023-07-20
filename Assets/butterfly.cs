using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lifecycle;

namespace Lifecycle
{
    public class butterfly : MonoBehaviour
    {
        public float speed = 5f;
        public Transform startPosition;
        public Transform targetPosition;
        public Transform endPosition;
        public Transform hatchingPosition;
        public float timeToStart = 60f;
        public float yAdjust = 0f;
        public float zAdjust = 0f;
        public GameObject[] eggs;
        public float hatchingDuration = 1f;
        public GameObject controller;

        private float eggActivationDelay = 5f;
        private int currentIndex = 0;
        private float timer = 0f;
        private Animator anim;
        private float animationLength;
        private float currentAnimationTime;
        private Vector3 hatchingScale = new Vector3(0.2f, 0.1f, 0.5f);
        private AudioSource audio;
        private stateController.GameStates state;
        private Vector3 direction;
        private float distanceToTarget;


        void Start()
        {
            transform.position = startPosition.position;
            anim = GetComponent<Animator>();
            audio = GetComponent<AudioSource>();
            AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;

            // Suche die Flug-Animation basierend auf ihrem Namen
            foreach (AnimationClip clip in clips)
            {
                if (clip.name == "take off")  // Anpassen des Animationsnamens
                {
                    animationLength = clip.length;
                    break;
                }
            }
            gameObject.SetActive(false);
            //Invoke("StartFlying", timeToStart);
        }

        void Update()
        {
            state = controller.GetComponent<stateController>().CurrentState;
            switch (state)
            {
                case stateController.GameStates.Approach:
                    // ------------ Flying at the beginning ----------------------------------
                        direction = (targetPosition.position - transform.position).normalized;
                        transform.position += direction * speed * Time.deltaTime;

                        // Drehe das GameObject in Richtung des Ziels
                        transform.LookAt(targetPosition);

                        // Überprüfe, ob das GameObject das Ziel erreicht hat
                        distanceToTarget = Vector3.Distance(transform.position, targetPosition.position);
                        if (distanceToTarget <= 0.1f)
                        {
                            anim.SetBool("shouldLand", true);
                            transform.position += new Vector3(0, yAdjust, zAdjust);
                            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                            controller.GetComponent<stateController>().CurrentState = stateController.GameStates.Pause;
                        }
                    break;
                case stateController.GameStates.Eggs:
                    // ------------ Laying eggs----------------------------------------
                    timer += Time.deltaTime;

                    if (timer >= eggActivationDelay && currentIndex < eggs.Length)
                    {
                        Debug.Log("Laying egg number: " + eggs[currentIndex]);
                        eggActivationDelay = 1f; //first egg should be layed after 10 secs, the rest after 1
                        eggs[currentIndex].SetActive(true);
                        currentIndex++;
                        timer = 0f;
                    }
                    else if (currentIndex == eggs.Length)
                    {
                        anim.SetBool("shouldStart", true);
                        controller.GetComponent<stateController>().CurrentState = stateController.GameStates.Takeoff;
                    }
                    break;
                case stateController.GameStates.Takeoff:
                    // ------------ Take off ------------------------------------------
                    if (anim.GetCurrentAnimatorStateInfo(0).IsName("take off"))
                    {
                        currentAnimationTime += Time.deltaTime;
                    }
                    // Überprüfe, ob die Animation abgeschlossen ist
                    if (currentAnimationTime + 0.26 >= animationLength)
                    {
                        //Animation wechselt automatisch zur flying weil keine Transition angegeben ist, nur Position muss geändert werden
                        transform.position += new Vector3(0, -yAdjust, zAdjust);
                        controller.GetComponent<stateController>().CurrentState = stateController.GameStates.Leaving;
                    }
                    break;
                case stateController.GameStates.Leaving:
                    // ------------ Leaving ------------------------------------------
                    direction = (endPosition.position - transform.position).normalized;
                    transform.position += direction * speed * Time.deltaTime;

                    // Drehe das GameObject in Richtung des Ziels
                    transform.LookAt(endPosition);

                    // Überprüfe, ob das GameObject das Ziel erreicht hat
                    distanceToTarget = Vector3.Distance(transform.position, endPosition.position);
                    if (distanceToTarget <= 0.1f)
                    {
                        gameObject.SetActive(false);
                        controller.GetComponent<stateController>().CurrentState = stateController.GameStates.Pause;
                    }
                    break;
                case stateController.GameStates.Hatching2:
                    // ------------ Hatching ------------------------------------------

                    timer += Time.deltaTime;
                    if (timer < hatchingDuration)
                    {
                        timer += Time.deltaTime;
                        float t = Mathf.Clamp01(timer / hatchingDuration);
                        transform.localScale = Vector3.Lerp(hatchingScale, Vector3.one, t);
                        Debug.Log("timer: " + timer + ", hatchingDuration: " + hatchingDuration);
                    }
                    if (timer >= hatchingDuration + 5f)
                    {
                        Debug.Log("juhu");
                        currentAnimationTime = 0f;
                        anim.SetBool("isHatching", false);
                        controller.GetComponent<stateController>().CurrentState = stateController.GameStates.Pause;
                    }
                    break;
            }

        }

        public void StartFlying()
        {
            gameObject.SetActive(true);
            controller.GetComponent<stateController>().CurrentState = stateController.GameStates.Approach;
        }

        public void StartHatching()
        {
            gameObject.SetActive(true);
            transform.position = hatchingPosition.position;
            transform.localScale = hatchingScale;
            transform.rotation = Quaternion.Euler(0f, -180f, transform.rotation.eulerAngles.z);
            timer = 0f;
            anim.SetBool("isHatching", true);
        }

        public void flyHome()
        {
            transform.LookAt(endPosition);
            anim.SetBool("shouldStart", true);
            controller.GetComponent<stateController>().CurrentState = stateController.GameStates.Takeoff;
        }

        public void startAudio()
        {
                audio.Play();
        }
        public void stopAudio()
        {
                audio.Stop();
        }
    }
}
