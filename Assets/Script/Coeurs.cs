using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coeurs : MonoBehaviour
{
//UI
    [SerializeField] private GameObject _panelCoeurs;
    private Image _spriteVie;

//Recup les Sprites 
    [Header("Sprites")]
    public Sprite _unCoeur;
    public Sprite _deuxCoeurs;
    public Sprite _troisCoeurs;
    public Sprite _quatreCoeurs;
    public Sprite _cinqCoeurs;


    
    void Start()
    {
        _spriteVie = _panelCoeurs.GetComponent<Image>();
        _spriteVie.sprite = _troisCoeurs;
    }

    // Update is called once per frame
    void Update()
    {
       ManageCoeurs();
    }

    private void ManageCoeurs(){
        if(Life.ActualHealth <= 33)
        {
            //Debug.Log("1 coeur");
            _spriteVie.sprite = _unCoeur;
        } else if(Life.ActualHealth > 33 && Life.ActualHealth <= 66)
        {
            //Debug.Log("2 coeurs");
            _spriteVie.sprite = _deuxCoeurs;
        } else if(Life.ActualHealth > 66 && Life.ActualHealth <= 100)
        {
            //Debug.Log("3 coeurs");
            _spriteVie.sprite = _troisCoeurs;
        } else if(Life.ActualHealth > 100 && Life.ActualHealth <= 133)
        {
            //Debug.Log("4 coeurs");
            _spriteVie.sprite = _quatreCoeurs;
        }
        else if(Life.ActualHealth > 133) //&& Life.ActualHealth <= 166
        {
            //Debug.Log("5 coeurs");
            _spriteVie.sprite = _cinqCoeurs;
        }
    }
}




