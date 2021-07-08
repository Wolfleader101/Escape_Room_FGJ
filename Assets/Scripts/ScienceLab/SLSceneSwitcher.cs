using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SLSceneSwitcher : MonoBehaviour {

    [SerializeField] private Scene nextScene;

    private bool switchScenes = false;

    private void OnTriggerEnter(Collider collider) {
        if(collider.gameObject.TryGetComponent<FirstPersonController>(out FirstPersonController _)) {
            
        }
    }
}
