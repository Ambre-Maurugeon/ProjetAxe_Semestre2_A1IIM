using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    private static ScenesManager instance;

    [SerializeField] private GameObject MenuPause;

    private void Awake(){
        if (instance != null && instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            instance = this; 
        } 
    }

    void Update(){
        
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(!MenuPause.activeSelf){
                    PauseGame();
            }
            else if (MenuPause.activeSelf){   
                ResumeGame();
            }
        }
    }

    public void Play(){
        SceneManager.LoadScene("SampleScene");
    }

    public void Quit(){
        Application.Quit();
    }

//Pause
     void PauseGame()   //Met sur Pause
    {
        Time.timeScale = 0;
        MenuPause.SetActive(true);
    }

    public void ResumeGame(){ //Reprendre la game
        MenuPause.SetActive(false);
        Time.timeScale = 1;
    }

    // public void PlayResumeGame()    
    // {
    //     Invoke("ResumeGame",1f);
    // }

    
}
