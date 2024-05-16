using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetStuff : MonoBehaviour
{
    [SerializeField] private Image _imageStuff;
    private bool inTrigger;
    public bool kept=false;

    // Start is called before the first frame update
    void Start()
    {
        _imageStuff = _imageStuff.GetComponent<Image>();
        _imageStuff.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(inTrigger){
            if(Input.GetKeyDown(KeyCode.E)){
                Debug.Log(gameObject.name + " récupéré");
                kept = true;
                _imageStuff.enabled = true;

                Destroy(gameObject);
            }
        }   
    }

    //inTrigger
    void OnTriggerEnter2D(Collider2D truc)
    {
        if (truc.tag == "Player") {
            inTrigger=true;
        }
    }

    void OnTriggerExit2D(Collider2D truc)
    {
        if (truc.tag == "Player") {
            inTrigger=false;
        }
    }
}
