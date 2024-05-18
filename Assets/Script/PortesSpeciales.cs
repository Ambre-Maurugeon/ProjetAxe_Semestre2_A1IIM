using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortesSpeciales : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshLevel(GameObject destination){
        if(destination.tag=="Fight"){
            PortesManager.levelFight+=1;
            Debug.Log("Level Fight : " + PortesManager.levelFight);
        }
        else if(destination.tag=="Survie"){
            PortesManager.levelSurvie+=1;
            Debug.Log("Level Survie : " + PortesManager.levelSurvie);
        }
        else if(destination.tag=="Enigme"){  
            PortesManager.levelEnigme+=1;
            Debug.Log("Level Enigme : " + PortesManager.levelSurvie);
        }
    }


    // public void GetLevelDestination(){
    //     GameObjet destination = PortesManager.//tag du 
    // }
}
