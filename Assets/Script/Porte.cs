using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porte : MonoBehaviour
{
    private Vector3 _playerPos;
    [SerializeField] private GameObject destination;

    //Portes spéciales
    private PortesSpeciales _portesSpeciales;
    private string[] Tags = new string[] { "Fight", "Survie","Enigme"};

    //Anim
    private Animator anim;

    //inTrigger
    private bool inTrigger;
    
    void Start()
    {
        _portesSpeciales = FindObjectOfType<PortesSpeciales>();

        _playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(inTrigger && Input.GetKeyDown(KeyCode.E)){
            anim.SetTrigger("open");
            Invoke("MoveTo", 1f);
        }
    }

//Déplacer
    private void MoveTo(){
        if(isSpecialDoor()){
            _UpdateSpecialDoor();
        } else {
            GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(destination.transform.position.x, destination.transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z);
            //Debug.Log("La porte est une porte basique, destination originale");
        }

    }

// Portes Speciales
    private void _UpdateSpecialDoor()
    {
        //Debug.Log("La porte est une porte spéciale de type " + gameObject.tag);
        destination = _portesSpeciales.GetLevelDestination(gameObject.tag);
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(destination.transform.position.x, destination.transform.position.y, GameObject.FindGameObjectWithTag("Player").transform.position.z);
        _portesSpeciales.RefreshLevel(gameObject.tag);
    }

    private bool isSpecialDoor(){
        foreach (string tag in Tags)
        {
            if (gameObject.tag == tag)
            {
                return true;
            }
        }
        return false;
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
