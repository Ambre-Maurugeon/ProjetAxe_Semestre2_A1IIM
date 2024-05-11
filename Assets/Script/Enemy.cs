using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Defense")]
    [SerializeField] private int InitialHealth=50;
    [SerializeField] private int dmgPlayerAttack=10;

    [Header("Attaque")]
    public int damageColl;

    [Header("UI")]
    [SerializeField] private Text _txtVie;

    private Life _lifeScript;   //recup script vie player pr la fonction TakeDamage()

    //Enemy Life 
    private int ActualHealth;
    private bool dead=false;

    //Physic Enemy
    private SpriteRenderer skin;
    private Rigidbody2D rb;
    private Animator anim;

    //Physic Player
    private GameObject player;
    private Rigidbody2D rb_player;
    private Animator animPlayer;
    
    //Mouvement
    [SerializeField]
    private Vector3[] positions;
    private int index;

    //Autres 
    private bool inTrigger=false;


    void Start()
    {
        _lifeScript=FindObjectOfType<Life>();
        InitCurrentLife();
        _txtVie.text="test";

        player = GameObject.FindGameObjectWithTag("Player");
        rb_player= player.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        animPlayer = player.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        skin= GetComponent<SpriteRenderer>();   

        float posX = GetComponent<Transform>().position.x;
        float posY = GetComponent<Transform>().position.y;
        float posZ = GetComponent<Transform>().position.z;
    }

    // Update is called once per frame
    void Update()
    {
        animCheck();
        if(ActualHealth <= 0 && !dead){
                dead=true;
                anim.SetTrigger("mort");
                Invoke("Destroy",1f);
                Invoke("GetStuff",1f);
            }
        if (positions.Length != 0){
            Move();
        }
        if (Input.GetMouseButtonDown(0)){
            StartCoroutine(AttackOpponent(dmgPlayerAttack));
        }
        RefreshUI();
    }

    void InitCurrentLife(){
        ActualHealth = InitialHealth;
    }

    public IEnumerator AttackOpponent(int dmg){
        yield return new WaitForSeconds(0.5f);  // attendre le temps du déclenchement de l'anim
        if(inTrigger){
            StartCoroutine(_lifeScript.Invicibility());          // au moment où le joueur attaque il est safe (check pertinence)
            ActualHealth -= dmg;
        } 
        
    }

    //Position
    private void Move(){
        transform.position = Vector2.MoveTowards(transform.position, positions[index], 3 * Time.deltaTime);
        if (transform.position == positions[index]){
            skin.flipX = !skin.flipX;
            // si l'index atteint la fin de la liste, recommencer
            if(index == positions.Length-1) {
                index = 0;
            }
            else{
                index++;
            }
        }
    }

    //Degats au contact (IEnumerator)
    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("Player")){
            //while (Life.ActualHealth>0){
                if (collision.contacts[0].normal.y<0){       // contact par le haut de l'objet
                    ActualHealth -= 5;
                    rb_player.AddForce(Vector2.up * 3f, ForceMode2D.Impulse);    
                }
                else{
                    _lifeScript.TakeDamage(damageColl);
                    //rb_player.AddForce(Vector2.down * 5f, ForceMode2D.Impulse);
                }
                if(collision.contacts[0].normal.x<0){
                    rb_player.AddForce(Vector2.right * 10f, ForceMode2D.Impulse);
                }
                else{
                    rb_player.AddForce(Vector2.left * 10f, ForceMode2D.Impulse);
                }
                //yield return new WaitForSeconds(0.5f);
            //}
            //for (int i= 0; i<6; i++){ 
            //}
        }
    }

    //Mort de l'ennemi
    void Destroy(){
        Destroy(gameObject);
    }

    //Set Up stuff
    void GetStuff(){
        GameObject stuff = GameObject.Instantiate(GameObject.FindGameObjectWithTag("stuff"));
        stuff.GetComponent<ArbreVie>().lifeTime = 3f; //Arbre de vie temporaire
        stuff.transform.localPosition = transform.position;
        stuff.name += gameObject.name;
    }

    //Visuals
    void animCheck() {
        if(positions.Length != 0){
            anim.SetBool("Move", positions.Length != 0);
        }   
    }

    void RefreshUI(){
        if (ActualHealth<0){
            ActualHealth=0;
        }
        _txtVie.text = ActualHealth + " / " + InitialHealth;
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
