using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lifecycle;

public class eggScript : MonoBehaviour
{
    public float hatchDuration = 5f;
    public float dissolveDuration = 2f;
    public float shakeAmount = 0.001f;
    public float shakeSpeed = 20f;
    public float timeToStart = 60f;
    public AnimationCurve dissolveCurve;
    public GameObject child;
    public GameObject lockpoint;
    public GameObject controller;
    private bool isHatching = false;
    private float startTime;

    private void Start()
    {
        child.SetActive(false);
    }

    private void Update()
    {
        if (controller.GetComponent<stateController>().CurrentState == stateController.GameStates.Hatching)
        {
            startTime += Time.deltaTime;

            if (startTime <= hatchDuration)
            {
                //Wackeln des Eis
                float shakeX = Mathf.Sin(startTime * shakeSpeed) * shakeAmount;
                transform.position += new Vector3(shakeX, 0f, shakeX);
            }
            else if (startTime <= hatchDuration + dissolveDuration)
            {
                Debug.Log("bin in der if");
                // Auflösen des Eis
                float dissolveProgress = (startTime - hatchDuration) / dissolveDuration;
                float dissolveAmount = dissolveCurve.Evaluate(dissolveProgress);
                Material material = GetComponent<Renderer>().material;
                material.SetFloat("_DissolveAmount", dissolveAmount);
            }
            else
            {
                child.SetActive(true);
                //lockpoint.GetComponent<caterpillar>().allowMovement = true;
                controller.GetComponent<stateController>().CurrentState = stateController.GameStates.Growing;
                // Auflösen abgeschlossen
                Destroy(gameObject);
            }
        }
    }
}
