using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lifecycle;

namespace Lifecycle
{
    public class cocoon : MonoBehaviour
    {
        public float dissolveDuration = 1f;
        public float incubationDuration = 5f;
        public AnimationCurve dissolveCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        public GameObject butterfly;
        public GameObject controller;

        private Material dissolveMaterial;
        private float dissolveAmount = 0f;
        private float incubationTimer = 0f;
        private float dissolveTimer = 0f;
        private stateController.GameStates state;


        private void Start()
        {
            dissolveMaterial = GetComponent<Renderer>().material;
            gameObject.SetActive(false);

        }

        private void Update()
        {
            if (controller.GetComponent<stateController>().CurrentState == stateController.GameStates.Pupating)
            {
                dissolveTimer += Time.deltaTime;
                dissolveAmount = dissolveCurve.Evaluate(dissolveTimer / dissolveDuration);
                dissolveMaterial.SetFloat("_DissolveAmount", ((dissolveAmount - 1) * -1));

                if (dissolveTimer >= dissolveDuration)
                {
                    incubationTimer += Time.deltaTime;
                    if(incubationTimer >= incubationDuration)
                    {
                        controller.GetComponent<stateController>().CurrentState = stateController.GameStates.Pause;
                        dissolveTimer = 0f;
                    }
                }
            }

            if (controller.GetComponent<stateController>().CurrentState == stateController.GameStates.Hatching2)
            {
                dissolveTimer += Time.deltaTime;
                dissolveAmount = dissolveCurve.Evaluate(dissolveTimer / dissolveDuration);
                dissolveMaterial.SetFloat("_DissolveAmount", dissolveAmount);

                if (dissolveTimer >= dissolveDuration)
                {
                    butterfly.GetComponent<butterfly>().StartHatching();
                    Destroy(gameObject);
                }
            }
        }

        public void StartPupating()
        {
            gameObject.SetActive(true);
        }
    }
}
