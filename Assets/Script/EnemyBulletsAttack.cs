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

    [SerializeField] private PatternData[] _pattern1Data;

    [FormerlySerializedAs("_patternData")]
    [SerializeField] private PatternData[] _pattern2Data;


    [Header("Bullet Hell")]

    [Header("Points d'origine")]
//Points d'origine
    [SerializeField] private Transform firePoint1; //pattern2
    [SerializeField] private Transform firePoint2;  //pattern2

    [Header("Positions des points d'origine")]
//Positions des points d'origine
    [SerializeField] private float cooldownChangePos = 10f;

    [FormerlySerializedAs("_firePoint01Pos")]
    [SerializeField] private Vector3[] _firePointPos;

//Attack
    private float cooldownTimer = Mathf.Infinity;
//Random positions de tir
    private float changePosTimer = 0;
    private System.Random rdm;

//Autres
        //private bool inTrigger;

    //Anim
    private Animator anim;
        //private Enemy enemyMovement;



    private void Awake(){
        anim= GetComponent<Animator>();
        //enemyMovement= GetComponent<Enemy>();
    }

    void Update(){
        if(GetComponent<ScreenVisibility>().OnScreen == true){
            if(_pattern2Data.Length != 0){
                FindNewPosition(firePoint1,firePoint2); 
            }
            if(cooldownTimer>attackCooldown){
                Attack();
            }
        }
        cooldownTimer+= Time.deltaTime;
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
                fireball.GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
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
    private int firePointRandomPosition(){
        rdm = new System.Random();
        return rdm.Next(_firePointPos.Length);  // index rdm dans le tableau des positions
    }

    private void FindNewPosition(Transform firePointA, Transform firePointB = null){
        changePosTimer += Time.deltaTime;
        if(changePosTimer>=cooldownChangePos){
            firePointA.localPosition =  _firePointPos[firePointRandomPosition()];
            if(firePointB != null){
                firePointB.localPosition =  _firePointPos[firePointRandomPosition()];
                if(firePointA.localPosition == firePointB.localPosition){
                    FindNewPosition(firePointA);
                } else{
                    changePosTimer = 0;
                }
            } else{
                changePosTimer = 0;
            }
        }
    }


//inTrigger
    //  void OnTriggerEnter2D(Collider2D truc)
    // {
    //     if (truc.tag == "Player") {
    //         inTrigger=true;
    //     }
    // }

    // void OnTriggerExit2D(Collider2D truc)
    // {
    //     if (truc.tag == "Player") {
    //         inTrigger=false;
    //     }
    // }
}