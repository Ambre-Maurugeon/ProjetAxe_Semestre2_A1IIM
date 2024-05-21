using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victoire : MonoBehaviour
{
    [SerializeField] private GameObject finalDoor;

    private GameObject boss;
    private GameObject player;
    private bool alreadyTeleported=false;


    // Start is called before the first frame update
    void Start()
    {
        boss= GameObject.FindGameObjectWithTag("Boss");
        player= GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(boss==null && ! alreadyTeleported){
            alreadyTeleported = true;
            Debug.Log("partie finie");

            finalDoor.GetComponent<Porte>().canOpen=true;
            //player.transform.position = new Vector3(603.92f, -74.69f,player.transform.position.z);
        }
    }
}
