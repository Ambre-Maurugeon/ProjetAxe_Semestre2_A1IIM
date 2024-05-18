using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortesManager : MonoBehaviour
{
    
//Utilisation d'un singleton pr avoir les données uniques au jeu en public
    public static PortesManager instance;

//Portes Origines qui dirigient vers la prochaine étape
    [HideInInspector]
    public Dictionary<string, GameObject[]> doorsDict;

    [Header("Portes par étape")]
    [Tooltip ("Portes Fight du 1er niveau de chaque étape")]
    public GameObject[] Fight = new GameObject[0];

    [Tooltip ("Portes Survie du 1er niveau de chaque étape")]
    public GameObject[] Survie = new GameObject[0];

    [Tooltip ("Portes Enigme du 1er niveau de chaque étape")]
    public GameObject[] Enigme = new GameObject[0];

    
// Index
    [HideInInspector] public Dictionary<string, int> levelsDict;

    [HideInInspector] public int levelFight = 0;
    [HideInInspector] public int levelEnigme = 0;
    [HideInInspector] public int levelSurvie = 0;



    void Awake()
    {
        if (instance != null && instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            instance = this; 
        } 

        //Définir les dico
        doorsDict = new Dictionary<string, GameObject[]>
        {
            { "Fight", Fight },
            { "Survie", Survie },
            { "Enigme", Enigme }
        };

        levelsDict = new Dictionary<string, int>
        {
            { "Fight", levelFight },
            { "Survie", levelSurvie },
            { "Enigme", levelEnigme }
        };


    }
}
