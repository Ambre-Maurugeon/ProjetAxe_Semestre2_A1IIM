using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlockDoor : MonoBehaviour
{
    private Porte _porte;
    [SerializeField] private GameObject key;


    // Start is called before the first frame update
    void Start()
    {
        _porte = GetComponent<Porte>();
        _porte.canOpen=false;
    }


    // Update is called once per frame
    void Update()
    {   
        if(key !=null){
            if(key.GetComponent<GetKey>().kept==true){
                Debug.Log("Porte débloquée");
                _porte.canOpen=true;
            }
        }
    }
}



