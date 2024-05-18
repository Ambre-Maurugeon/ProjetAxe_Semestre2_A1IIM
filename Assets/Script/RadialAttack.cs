using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialAttack : MonoBehaviour
{
    //Set up le radius
    [Header("Radius Settings")]
    [SerializeField] private float finalSize;
    [SerializeField] private float radialSpeed;

    private float InitSize;

    //CapsuleCollider
    private CapsuleCollider2D _coll;
    private float _newCollSize;

    //Ref aux autres Script
    private Enemy _enemy;
    private EnemyBulletsAttack _enemyBulletsAttack;

    

    private void Start(){
        _coll = GetComponent<CapsuleCollider2D>();
        InitSize =  _coll.size.x;

        _enemy = GetComponent<Enemy>();
        _enemyBulletsAttack = GetComponent<EnemyBulletsAttack>();
    }

    private void Update(){
        if(_enemy.justAttacked==true){
            //Anim
            GetComponent<Animator>().SetTrigger("radial");

            //Continue à repousser le joueur
            if(Mathf.Abs(_coll.size.x-finalSize)>0.015f)
            {
                //Stop les balles pdt l'attaque radiale
                if(_enemyBulletsAttack!=null){
                    _enemyBulletsAttack.canAttack = false;
                }

                _newCollSize = Mathf.Lerp(_coll.size.x, finalSize ,radialSpeed*Time.deltaTime);
                _coll.size = new Vector2(_newCollSize, _coll.size.y);
            
            } 
            //Fini de repousser le joueur = reset
            else{
                _coll.size = new Vector2(InitSize, _coll.size.y);

                _enemy.justAttacked=false;
                _enemyBulletsAttack.canAttack = true;
            } 
        }
    }
}