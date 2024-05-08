using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeurAttack : MonoBehaviour
{
    //Charge
    [Header("Charge")]
    [SerializeField] private int dmgInCharge=45;
    [SerializeField] private float distance=10;
    [SerializeField] private float speedAttack=1.5f;
    [SerializeField] private float cooldownCharge=10f;

    //Detection du Player
    [Header("Player Detection")]
    [SerializeField] private Transform[] _detectionPoints;
    [SerializeField] private LayerMask _playerMask;
    [SerializeField] private float _detectionLength = 0.05f;
    
    //Charge
    private Vector3 destination;
    private int dmgColl; //degats du collider sans charge

    //Autres
    private bool playingCharge = false;
    private bool waitingCooldown = false;
    
    //Component
    private Rigidbody2D _rb;
    private SpriteRenderer skin;
    private Enemy _enemy;

    //Anim
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        skin = GetComponent<SpriteRenderer>(); 
        _enemy = GetComponent<Enemy>();
        dmgColl = _enemy.damageColl;
    }

    void Update()
    {   
        animCheck();
        _enemy.damageColl =  DamageValue();

        if(GetComponent<ScreenVisibility>().OnScreen == true && !playingCharge && !waitingCooldown){
            anim.SetTrigger("prepCharge");
            //Debug.Log("l'attaque du chargeur commence");
            GetOrientAttack();
        } else if (playingCharge){
            Charge();
        }
    }

    void Charge(){
        playingCharge=true;
        transform.localPosition = Vector2.MoveTowards(transform.localPosition, destination, speedAttack * Time.deltaTime);
        if(transform.localPosition==destination){
            playingCharge=false;
            //Debug.Log("charge finie");
            StartCoroutine(WaitCooldown());
        }
    }

    void animCheck(){
        anim.SetBool("run", playingCharge);
        anim.SetBool("pause",waitingCooldown); // idl ou fin de charge pr les visuels
    }

    void GetOrientAttack(){
        float orientX = Detection();
        destination = new Vector3(transform.localPosition.x + distance * orientX,transform.localPosition.y,transform.localPosition.z);
        Invoke("Charge",3f);
    }

    private int Detection(){
         foreach (Transform detectionPoint in _detectionPoints){
            RaycastHit2D hitRight = Physics2D.Raycast(
                detectionPoint.position, //origine
                Vector2.right,            //direction
                _detectionLength,
                _playerMask             // type d'objets à détecter
            );

            RaycastHit2D hitLeft = Physics2D.Raycast(
                detectionPoint.position, // origine
                Vector2.left,                // direction 
                _detectionLength
                ,_playerMask                 
            );

            if(hitRight.collider != null){
                Debug.Log("joueur detecté sur la droite");
                skin.flipX=false;
                return 1;
            }else if (hitLeft.collider != null){
                Debug.Log("joueur detecté sur la gauche");
                skin.flipX=true;
                return -1;
            }
        }
        Debug.Log("Pas de joueur");
        return 0;
    }

//Cooldown apres une attaque
    private IEnumerator WaitCooldown(){
        //Debug.Log("debut cooldown");
        waitingCooldown = true;
        yield return new WaitForSeconds(cooldownCharge);
        //Debug.Log("fin cooldown");
        waitingCooldown = false;
    }

// Attaque plus forte lors de la charge
    private int DamageValue(){
        if(playingCharge){
           return dmgInCharge;
        } else {
            return dmgColl;
        }
    }
}
