using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ArbreVie : MonoBehaviour
{

    //Gerer la vitesse d'augmentation de la vie
    [SerializeField] private float speed=0.1f;

    [Header("Stuff")]
    public float lifeTime;

    // vitesse d'augmentation de la saturation
    private Life _life;
    private bool wait=false;

    //Détruire l'objet
    private float timer=0f;

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
        if(lifeTime!=0){
            DestructGameObject();
        }
    }

    IEnumerator Heal(){
        if(Life.ActualHealth<Life.InitialHealth){ // 100 dc 3coeurs
            wait = true;
            Life.ActualHealth +=1;
            yield return new WaitForSeconds(speed);
           _life.AugSaturation();
            wait = false;
        }
    }

//Vie temporaire
    private void DestructGameObject(){
        timer += Time.deltaTime;
        if(timer >= lifeTime){
            Destroy(gameObject);
            timer = 0;
        }
    }
    
//InTrigger
    void OnTriggerEnter2D(Collider2D truc)
    {
        if (truc.tag == "Player") {
            inTrigger=true;
            _life.SetSaturation(); // Saturation = vie perso quand il entre 
        }
    }

    void OnTriggerExit2D(Collider2D truc)
    {
        if (truc.tag == "Player") {
            inTrigger=false;
        }
    }

}
