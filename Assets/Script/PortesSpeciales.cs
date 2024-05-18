using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortesSpeciales : MonoBehaviour
{
//Avoir la prochaine porte
    public GameObject GetLevelDestination(string tag){
        GameObject[] door = PortesManager.instance.doorsDict[tag];
        return door[PortesManager.instance.levelsDict[tag]];
    }

//Refresh le niveau d'exp apres le passage 
    public void RefreshLevel(string tag)
    {
        PortesManager.instance.levelsDict[tag]++;
        Debug.Log("Level " + tag + " après passage: " + PortesManager.instance.levelsDict[tag]);
    }
}
