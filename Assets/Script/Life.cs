using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Life : MonoBehaviour
{
//Life
    [Header("Life")]
    public static int InitialHealth=100;
    public static int ActualHealth;
    [SerializeField] private float invicibilityDuration=1.5f;
    
    [HideInInspector]
    public bool invincible = false;
    private bool dead=false;
    
//UI
    [Header("UI")]
    [SerializeField] private Text _txtVie;

//Saturation
    private VolumeProfile _volProfile;
    private ColorAdjustments _colorAdjustments;

//Physic
    private Collider2D _monColl;

//Anim
    private Animator anim;


    void Awake(){
        _monColl = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        
        GameObject _globVolume = GameObject.FindGameObjectWithTag("GlobalVolume");
        _volProfile = _globVolume.GetComponent<Volume>().profile;
    
        if (_volProfile.TryGet(out _colorAdjustments))
        {
            _colorAdjustments.saturation.value = 0f; 
        }
        else
        {
            Debug.Log("Pas de color adjustement sur Life");
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        InitCurrentLife();
    }

    // Update is called once per frame
    void Update()
    {
        isALive();
        RefreshUI();
    }

    public void TakeDamage(int damage){
        if (!invincible){
            StartCoroutine(Invicibility());
            anim.SetTrigger("hit");
            ActualHealth -= damage;
            DimSaturation();
            if(ActualHealth<=0){
                Debug.Log("Le joueur est mort");
            }
        }
    }

    public IEnumerator Invicibility(){
        //Debug.Log("debut d'invicibilité");
        invincible = true;
        yield return new WaitForSeconds(invicibilityDuration); // tps d'invincibilité
        invincible = false;
        //Debug.Log("fin d'invicibilité");
    }

//Saturation

    public void DimSaturation(){
        if(ActualHealth <=100 && ActualHealth > 66){
            _colorAdjustments.saturation.value = 0;
            } else if(ActualHealth <= 66 && ActualHealth >33){
                _colorAdjustments.saturation.value = -50;
            } else {
                _colorAdjustments.saturation.value = -100;
            }
    }

    public void AugSaturation(){
        _colorAdjustments.saturation.value += 1;
    }

    public void SetSaturation(){
        _colorAdjustments.saturation.value = -100+ActualHealth; // qd il entre ds l'arbre de vie : sat = vie
    }

//Mort
    void isALive(){
        if (ActualHealth<=0! && !dead){ 
            dead=true;
            anim.SetTrigger("mort");
            Invoke("Respawn",1.5f);
        }
    }

    void Respawn(){
        //changer posittion en le dernier de la liste c'est à dire .Count - 1
        transform.position = CheckPoint.checkpoint[CheckPoint.checkpoint.Count-1];
        InitCurrentLife();
        InitSaturation();
        dead=false;
    }

// Réinitialisation
    void InitCurrentLife(){
        ActualHealth = InitialHealth;
    }

    void InitSaturation(){
        _colorAdjustments.saturation.value = 0f;
    }


 //UI   
    void RefreshUI(){
        if (ActualHealth<0){
            ActualHealth=0;
        }
        _txtVie.text = ActualHealth + " / " + InitialHealth;
    }
    
}
