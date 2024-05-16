using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potions : MonoBehaviour
{
// Aug saturation
    [Header("Augmentation Saturation")]
    [SerializeField] private float speed=0.005f;
    private Life _life;
    private bool startingPot = false;

//Autres
    private bool inTrigger=false;

    
    void Start()
    {
        _life = FindObjectOfType<Life>();
    }

   
    void Update()
    {
        if(inTrigger && Input.GetKeyDown(KeyCode.E) && !startingPot){
            startingPot = true;
            //Debug.Log("Boit la potion avec " + Life.ActualHealth);
            //Debug.Log("A bu la potion a " + Life.ActualHealth);
            StartCoroutine(_UpdateSat());
        }
    }

//Boire la potion
    private void DrinkPot(){
        Life.ActualHealth +=33;
    }

//Saturation 

    private IEnumerator _UpdateSat(){
        for (int i=0; i<33;i++) // 33 ou 34?
        {
            yield return new WaitForSeconds(speed);
            _life.AugSaturation();
            //Debug.Log("aug la sat");
        }
        DrinkPot();
        Debug.Log("Pot finie donc saturation finie");
        Destroy(gameObject);
    }
    IEnumerator ProgressiveSaturation(float vitesse){
            yield return new WaitForSeconds(vitesse);
            _life.AugSaturation();
    }

//inTrigger
    private void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player"){
            inTrigger=true;
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.tag == "Player"){
            inTrigger=false;
        }
    }
}
