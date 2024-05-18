using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortesManager : MonoBehaviour
{
    [Header("Portes par étape")]
    [Tooltip ("Portes Fight qui mènent au 1er niveau de chaque étape")]
    public Transform[] Fight = new Transform[0];

    [Tooltip ("Portes Enigme qui mènent au 1er niveau de chaque étape")]
    public Transform[] Enigme = new Transform[0];

    [Tooltip ("Portes Survie qui mènent au 1er niveau de chaque étape")]
    public Transform[] Survie = new Transform[0];

    [HideInInspector] public static int levelFight = 0;
    [HideInInspector] public static int levelEnigme = 0;
    [HideInInspector] public static int levelSurvie = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
