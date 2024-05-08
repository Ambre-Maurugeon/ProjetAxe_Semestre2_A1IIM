using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    private static ScenesManager instance;

    private void Awake(){
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else if (instance != this) 
        {
            Destroy(gameObject);
            return;
        }
    }

    void Update(){
        if(Input.GetKey(KeyCode.Escape)){
            GameObject.FindGameObjectWithTag("Pause").SetActive(true);
        }
    }

    public void Play(){
        SceneManager.LoadScene("SampleScene");
    }

    public void Quit(){
        Application.Quit();
    }
}
