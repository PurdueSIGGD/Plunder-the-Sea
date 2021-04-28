using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialAimingProgression : MonoBehaviour
{

    [SerializeField]
    private int counterMax = 1;
    private int counter = 0;

    void Start()
    {

    }

    void Update()
    {
        
    }

    public UnityEvent counterComplete;

    public void Increment() {
        counter++;

        if (counter >= counterMax) {
            this.enabled = false;
            counterComplete.Invoke();
        }
    }
}
