using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [Header("Zoom Settings")]
    [SerializeField] private float ZoomSize;
    [SerializeField] private float ZoomSpeed;

    // private CamerasManager camerasManager;
    // private Camera cameraToZoom;
    // private GameObject cameraGameObject;
    private Camera _camera;

    private float initSettings;
    private bool entre=false;
    private bool sors=false;

    void Awake(){
        _camera=GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<Camera>(); 
        initSettings = _camera.orthographicSize;
    }

    // Start is called before the first frame update
    void Start()
    {
        // camerasManager = GetComponent<CamerasManager>();        //script chgmt cam
        // cameraGameObject = camerasManager.ToggleCamera;      // récupere la camera enregistrée
        // cameraToZoom = cameraGameObject.GetComponent<Camera>();  
    }

    void Update(){
        if (entre)
            //cameraToZoom.orthographicSize = Mathf.Lerp(cameraToZoom.orthographicSize,ZoomSize,ZoomSpeed*Time.deltaTime); // Mathf.Lerp(start,end,temps) Time.deltaTime = tps écoulé depuis der frame
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize,ZoomSize,ZoomSpeed*Time.deltaTime);
        
        if (sors){
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize,initSettings,ZoomSpeed*Time.deltaTime);
        }

    }

    void OnTriggerEnter2D(Collider2D truc)
    {
        if (truc.tag == "Player") {
            Debug.Log("initSettings" + initSettings);
            entre=true;
        }
    }
    void OnTriggerExit2D(Collider2D truc){
        if (truc.tag == "Player") {
            entre = false;
            sors=true;
        }
    }
        
}
