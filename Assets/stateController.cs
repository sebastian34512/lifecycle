using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*States Ablauf
 * 1. Initial
 * 2. Approach: Nach Starttime kommt Schmetterling und setzt auf Approach
 * 3. Pause: Schmetterling Landet und setzt auf Pause
 * 4. Eggs: Spieler klickt bei Info Monarchfalter auf weiter und setzt auf Eggs
 * 5. Takeoff: Schmetterling legt Eier, wenn fertig setzt er auf takeoff
 * 6. Leaving: Nach takeoff setzt Schmetterling auf leaving
 * 7. Pause: Schmetterling fliegt weg und setzt auf Pause
 * 8. Hatching: Spieler drückt bei Info Ei Ablage auf weiter und setzt auf Hatching
 * 9. Growing: Ei schlüpft, löst sich auf und setzt auf Growing
 * 10. Pupating: Spieler drückt bei Info Wachstum auf weiter, damit wird requestPupating bei caterpillar aufgerufen, und wenn die Raupe auch ausgewachsen ist setzt sie state auf Pupating
 * 11. Pause: Wenn die Raupe ausgewachsen ist und den Kokon erreicht hat setzt sie auf Pause
 * 12. Hatching2: Spieler drückt bei Info Kokon auf weiter und setzt auf Hatching2, wodurch der Kokon aufgeht und der Schmetterling schlüpft
 * 13. Pause: Wenn der Schmetterling schlüpft setzt er auf Pause
 * 14. Takeoff: Spieler drückt bei Info Junges auf weiter und ruft methode fly home bei butterfly auf, die state auf takeoff setzt
 * 15. Leaving: Nach takeoff setzt Schmetterling auf leaving
 */


namespace Lifecycle
{
    public class stateController : MonoBehaviour
    {
        public enum GameStates
        {
            Initial,  //0
            Approach, //1
            Eggs,     //2
            Takeoff,  //3
            Hatching, //4
            Growing,  //5
            Pupating, //6
            Hatching2,//7
            Leaving,  //8
            Pause,    //9
            AwaitPupating //10
        }

        public GameStates state;

        public GameObject[] infoPanels;
        private int panelIndex = 0;

        public GameObject[] tutorialPanels;
        private int tutorialIndex = 0;

        public GameStates CurrentState
        {
            get { return state; }
            set
            {
                if (value == GameStates.Pause || value == GameStates.Growing)
                {
                    // Überprüfen, ob das infoPanels-Array Elemente enthält
                    if (infoPanels.Length > 0)
                    {
                        // Aktiviere das nächste (oder erste) Panel
                        infoPanels[panelIndex].SetActive(true);
                        panelIndex++;
                    }
                }

                state = value;
            }
        }

        void Start()
        {
            state = GameStates.Initial;
            Invoke("ActivateNextTutorialPanel", 10f);
            //ActivateNextTutorialPanel();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ActivateNextTutorialPanel()
        {
            foreach (GameObject panel in tutorialPanels)
            {
                panel.SetActive(false);
            }
            if (tutorialIndex < tutorialPanels.Length)
            {
                tutorialPanels[tutorialIndex].SetActive(true);
            } 
            tutorialIndex++;
        }


        public void SwitchStateTo(int nextState)
        {
            CurrentState = (GameStates)nextState;
            Debug.Log("aktueller state: " + CurrentState);
        }
    }
}
