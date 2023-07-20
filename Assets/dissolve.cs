using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lifecycle
{  
    public class dissolve : MonoBehaviour
    {
        public float dissolveDuration = 1f;
        public AnimationCurve dissolveCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        private bool isDissolving = false;
        private Material dissolveMaterial;
        private float dissolveAmount = 0f;
        private float dissolveTimer = 0f;

        void Start()
        {
                dissolveMaterial = GetComponent<Renderer>().material;

        }

        // Update is called once per frame
        void Update()
        {
            if (isDissolving)
            {
                dissolveTimer += Time.deltaTime;
                dissolveAmount = dissolveCurve.Evaluate(dissolveTimer / dissolveDuration);
                dissolveMaterial.SetFloat("_DissolveAmount", dissolveAmount);

                if (dissolveTimer >= dissolveDuration)
                {
                    // Optional: Do something after the object is fully dissolved

                    // Disable or destroy the object
                    Destroy(gameObject);
                }
            }
        }

        public void StartDissolving()
        {
            isDissolving = true;
        }
    }
}
