using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Serialization;


public class EnemyBulletsAttack : MonoBehaviour
{
    [Header("Data Attaque")]
    [SerializeField] private float attackCooldown;


//firePoints par Patterns
    [Header("Pattern")]
    [Tooltip("Pattern horizontal")]
    [SerializeField] private PatternData[] _pattern1Data;


    [Tooltip("Pattern balles qui partent en diagonal")]
    [FormerlySerializedAs("_patternData")]
    [SerializeField] private PatternData[] _pattern2Data;

    [Header("Bullet Hell")]
    [Header("Points d'origine")]
//Points d'origine
    private List<Transform> firePoints02 = new List<Transform>(); // tous mes firePoints pour comparer leur Pos
    private List<Transform> firePoints01 = new List<Transform>();


//Pattern01
    [Header("Pattern 01")]
    [SerializeField] private bool randomPosPattern01=true;
    [SerializeField] private Vector3[] _firePointPos01;
//Pattern01
    [Header("Pattern 02")]
    [SerializeField] private bool randomPosPattern02=true;
   
    //[FormerlySerializedAs("_firePoint01Pos")]
    [FormerlySerializedAs("_firePointPos")]
    [SerializeField] private Vector3[] _firePointPos02;


    [Header("Rythme changement de position")]
//Positions des points d'origine
    [Tooltip("4 = toutes les 4 balles, nouvelles positions")]//description
    [SerializeField] private float cooldownChangePos = 10f;




//Attack
    [HideInInspector] public bool canAttack;
    private float cooldownTimer = Mathf.Infinity;
//Random positions de tir
    private float changePosTimer = 0;
    private System.Random rdm;


    //Anim
    private Animator anim;






    private void Awake(){
        anim= GetComponent<Animator>();
    }


    private void Start(){
        if(_pattern2Data.Length!=0){
            foreach(var data in _pattern2Data){
                firePoints02.Add(data.firePoint);
            }
        }
        if(_pattern1Data.Length!=0){
            foreach(var data in _pattern1Data){
                firePoints01.Add(data.firePoint);
            }
        }
        canAttack = true;
    }


    void Update(){
        if(canAttack){
            if(GetComponent<ScreenVisibility>().OnScreen == true)
            {
                if(_pattern2Data.Length != 0 && randomPosPattern02){
                    FindNewPosition(firePoints02, _firePointPos02);
                }
                if (_pattern1Data.Length != 0 && randomPosPattern01)
                {
                    FindNewPosition(firePoints01, _firePointPos01);
                }


                if(cooldownTimer>attackCooldown){
                    Attack();
                }
            }
            cooldownTimer+= Time.deltaTime;
        }
    }


    private void Attack(){
        anim.SetTrigger("attack");
        cooldownTimer = 0;
       
        //1er pattern de tirs
        if(_pattern1Data.Length != 0){
            foreach(PatternData data in _pattern1Data){
                _GetPattern1(data.fireballs, data.firePoint);
            }
        }


        //2e pattern de tirs
        if(_pattern2Data.Length != 0){
            foreach(PatternData data in _pattern2Data){
                _GetPattern2(data.fireballs,data.firePoint);
            }
        }
    }


//Inutile pour l'instant
    private int FindFireball(PatternData data){
        for(int i=0;i<data.fireballs.Length;i++){
            if(!data.fireballs[i].activeInHierarchy){
                return i;
            }
        }
        return 0;
    }


//Bullets Pattern 1 (tirs horizontaux)
    private void _GetPattern1(GameObject[] fireballs, Transform firePoint){
        foreach (var fireball in fireballs) {
            if (!fireball.activeInHierarchy) {
                fireball.transform.position = firePoint.position;
                fireball.GetComponent<Projectile>().SetDirection(Mathf.Sign(firePoint.position.x - transform.position.x));
                break;
            }
        }
    }


//Bullets Pattern 2 (tirs multidirection)
    private void _GetPattern2(GameObject[] fireballs, Transform firePoint){
        foreach (var fireball in fireballs) {
            if (!fireball.activeInHierarchy) {
                fireball.transform.position = firePoint.position;
                fireball.GetComponent<Projectile>().CalculateDirection(transform.position, firePoint.position);
                break;
            }
        }
    }


//Random Position
    private int firePointRandomPosition(Vector3[] _firePointPos){
        rdm = new System.Random();
        return rdm.Next(_firePointPos.Length);  // index rdm dans le tableau des positions
    }


    private void FindNewPosition(List<Transform> firePoints, Vector3[] _firePointPos){
        changePosTimer += Time.deltaTime;
        if (changePosTimer >= cooldownChangePos)
        {
            foreach (Transform firePoint in firePoints)
            {
                int randomIndex = firePointRandomPosition(_firePointPos);
                Vector3 newPosition = _firePointPos[randomIndex];


                while (!IsUniquePosition(newPosition,firePoints))
                {
                    randomIndex = firePointRandomPosition(_firePointPos);
                    newPosition = _firePointPos[randomIndex];
                }


                firePoint.localPosition = newPosition;
            }
            changePosTimer = 0;
        }
    }


    private bool IsUniquePosition(Vector3 positionToCheck, List<Transform> firePoints){
         foreach (Transform firePoint in firePoints)
        {
            if (firePoint.localPosition == positionToCheck)
            {
                return false; // pos existe deja
            }
        }
        return true; // pos unique
    }




}

