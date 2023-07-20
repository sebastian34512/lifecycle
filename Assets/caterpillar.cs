using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lifecycle;

namespace Lifecycle { 
    public class caterpillar : MonoBehaviour
    {
        public float speed = 5f;
        public float waitTime = 2f;
        public Transform[] movementPoints;
        public AnimationCurve movementCurve;
        public Animator animationController;
        public bool allowMovement = false;
        public Transform player;
        public float detectionRange = 10f;
        public GameObject cocoon;
        public GameObject caterpillarObject;
        public GameObject caterpillarTextureObject;
        public GameObject controller;

        private int currentPointIndex;
        private bool isMoving = true;
        private float waitTimer;
        private Vector3 startPosition;
        private Vector3 targetPosition;
        private float journeyLength;
        private float journeyTime;
        private float currentLerpTime;
        private bool isNearPlayer = false;
        private bool hasReachedCocoon = false;
        private stateController.GameStates state;
        private bool awaitPupating = false;


        void Start()
        {
            currentPointIndex = 0;
            waitTimer = waitTime;
            isMoving = true;

            startPosition = transform.position;
            targetPosition = movementPoints[currentPointIndex].position;
            journeyLength = Vector3.Distance(startPosition, targetPosition);
            journeyTime = journeyLength / speed;
            currentLerpTime = 0f;
        }

        void Update()
        {
            state = controller.GetComponent<stateController>().CurrentState;

            switch(state)
            {
                case (stateController.GameStates.Growing):
                    // ------------ Moving from feeding point to feeding point ------------------------------
                    if (!isNearPlayer)
                    {
                        if (isMoving)
                        {
                            if (currentLerpTime <= journeyTime)
                            {
                                currentLerpTime += Time.deltaTime;
                                float t = currentLerpTime / journeyTime;
                                t = movementCurve.Evaluate(t);

                                transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                            }
                            else
                            {
                                if (waitTimer <= 0f)
                                {
                                    currentPointIndex++;
                                    if (currentPointIndex >= movementPoints.Length)
                                    {
                                        currentPointIndex = 0;
                                    }
                                    waitTimer = waitTime;
                                    isMoving = false; // Stop moving

                                    startPosition = transform.position;
                                    targetPosition = movementPoints[currentPointIndex].position;
                                    journeyLength = Vector3.Distance(startPosition, targetPosition);
                                    journeyTime = journeyLength / speed;
                                    currentLerpTime = 0f;
                                }
                                else
                                {
                                    waitTimer -= Time.deltaTime;
                                }
                            }

                            Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
                            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
                        }
                        else
                        {
                            // Start moving again after waiting
                            waitTimer -= Time.deltaTime;
                            if (waitTimer <= 0f)
                            {
                                isMoving = true;

                                startPosition = transform.position;
                                targetPosition = movementPoints[currentPointIndex].position;
                                journeyLength = Vector3.Distance(startPosition, targetPosition);
                                journeyTime = journeyLength / speed;
                                currentLerpTime = 0f;
                            }
                        }
                    }

                    //// ------------ Stopping and looking in direction of player ----------------------------------
                    //Vector3 directionToPlayer = player.position - transform.position;
                    //Debug.Log("Distanz " + directionToPlayer.magnitude);

                    //// Überprüfung, ob der Spieler in der Reichweite ist
                    //if (directionToPlayer.magnitude <= detectionRange)
                    //{
                    //    isNearPlayer = true;
                    //    // Berechnung der Rotation, um den Spieler anzuschauen
                    //    Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

                    //    // Anwendung der Rotation auf das Gameobjekt
                    //    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime);
                    //}
                    //else
                    //{
                    //    isNearPlayer = false;
                    //}

                    // ------------ Check if growth complete ----------------------------------
                    if (caterpillarObject != null && caterpillarObject.GetComponent<Growing>().growthProgress > 0.99 && awaitPupating)
                    {
                        controller.GetComponent<stateController>().CurrentState = stateController.GameStates.Pupating;
                    }

                    break;
                case (stateController.GameStates.Pupating):
                    // ------------ Moving to cocoon if growth is done -------------------------------------------
                    if (!hasReachedCocoon)
                    {
                        isMoving = true;
                        // Bewege das GameObject in Richtung des Ziels
                        Vector3 direction = (cocoon.transform.position - transform.position).normalized;
                        transform.position += direction * speed * Time.deltaTime;

                        // Drehe das GameObject in Richtung des Ziels
                        transform.LookAt(cocoon.transform);

                        // Überprüfe, ob das GameObject das Ziel erreicht hat
                        float distanceToTarget = Vector3.Distance(transform.position, cocoon.transform.position);
                        if (distanceToTarget <= 0.1f)
                        {
                            hasReachedCocoon = true;
                            cocoon.GetComponent<cocoon>().StartPupating();
                            caterpillarTextureObject.GetComponent<dissolve>().StartDissolving();
                        }
                    }
                    break;
            }
            animationController.SetBool("isNearPlayer", isNearPlayer);
            animationController.SetBool("isEating", !isMoving);
        }

        public void requestPupating()
        {
            awaitPupating = true;
        }
    }
}
