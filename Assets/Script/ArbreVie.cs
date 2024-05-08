using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ArbreVie : MonoBehaviour
{
    private bool inTrigger=false;

    // Start is called before the first frame update
    void Start()
    { 
    }

    // Update is called once per frame
    void Update()
    {
        if(inTrigger){
            StartCoroutine(Heal());
        }
    }

    IEnumerator Heal(){
        if (Life.ActualHealth<Life.InitialHealth){
            Life.ActualHealth +=1;

            yield return new WaitForSeconds(0.15f);
        }
    }

    

    void OnTriggerEnter2D(Collider2D truc)
    {
        if (truc.tag == "Player") {
            inTrigger=true;
        }
    }

    void OnTriggerExit2D(Collider2D truc)
    {
        if (truc.tag == "Player") {
            inTrigger=false;
        }
    }
}
