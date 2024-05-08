using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IlludiverneAttack : MonoBehaviour
{
    [Header("Data Attaque")]
    [SerializeField] private float attackCooldown;

    [Header("Bullet hell")]
//Points d'origine
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform firePoint2;

//Balles
    [SerializeField] private GameObject[] fireballs; //Pattern1
    [SerializeField] private GameObject[] fireballs2; //Pattern2

//Positions des points d'origine
    [SerializeField] private float cooldownChangePos = 10f;
    [SerializeField] private Vector3[] _firePoint01Pos;
    [SerializeField] private Vector3[] _firePoint02Pos;

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
            FindNewPosition();
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
        foreach (var fireball in fireballs) {
            if (!fireball.activeInHierarchy) {
                fireball.transform.position = firePoint.position;
                fireball.GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
                break;
            }
        }

        //2e pattern de tirs
        foreach (var fireball2 in fireballs2) {
            if (!fireball2.activeInHierarchy) {
                fireball2.transform.position = firePoint2.position;
                //fireball2.GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
                fireball2.GetComponent<Projectile>().CalculateDirection(transform.position, firePoint2.position);
                break;
            }
        }

        //fireballs[FindFireball()].transform.position = firePoint.position;
        //fireballs[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private int FindFireball(){
        for(int i=0;i<fireballs.Length;i++){
            if(!fireballs[i].activeInHierarchy){
                return i;
            }
        }
        return 0;
    }


//Random Position
    private int firePointPosition(){
        rdm = new System.Random();
        return rdm.Next(_firePoint01Pos.Length);
    }

    private void FindNewPosition(){
        changePosTimer += Time.deltaTime;
        if(changePosTimer>=cooldownChangePos){
            firePoint2.localPosition =  _firePoint01Pos[firePointPosition()];
            changePosTimer = 0;
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