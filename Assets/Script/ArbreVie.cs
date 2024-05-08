using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ArbreVie : MonoBehaviour
{

    //Gerer la vitesse d'augmentation de la vie
    [SerializeField] private float speed=0.1f;
    private bool wait=false;

    // vitesse d'augmentation de la saturation
    private Life _life;

    //Autres
    private bool inTrigger=false;


    void Start(){
        _life = FindObjectOfType<Life>();
    }

    void Update()
    {
        if(inTrigger && !wait){
           StartCoroutine(Heal());
        }
    }

    IEnumerator Heal(){
        if(Life.ActualHealth<Life.InitialHealth){
            wait = true;
            Life.ActualHealth +=1;
            yield return new WaitForSeconds(speed);
           _life.AugSaturation();
            wait = false;
        }
    }

    

    void OnTriggerEnter2D(Collider2D truc)
    {
        if (truc.tag == "Player") {
            inTrigger=true;
            _life.SetSaturation();
        }
    }

    void OnTriggerExit2D(Collider2D truc)
    {
        if (truc.tag == "Player") {
            inTrigger=false;
        }
    }

}
