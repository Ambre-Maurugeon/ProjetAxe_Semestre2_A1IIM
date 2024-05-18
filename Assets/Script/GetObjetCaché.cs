using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetObjetCache : MonoBehaviour
{
    [HideInInspector]
    public bool objectDiscovered=false;

//inTrigger
    private bool inTrigger;

    // Update is called once per frame
    void Update()
    {
        DiscoverObjetCache();
    }

    private void DiscoverObjetCache(){
        if(Input.GetKey(KeyCode.E) && inTrigger){
            Debug.Log("objet à détruire, découverte de la clé");
            Invoke("Destroy",0.01f);
        }
    }
//Destroy
    private void Destroy(){
        objectDiscovered=true;
        Destroy(gameObject);
    }

//inTrigger
    private void OnTriggerEnter2D(Collider2D other){
        if(other.tag =="Player"){
            inTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.tag =="Player"){
            inTrigger = false;
        }
    }
}
