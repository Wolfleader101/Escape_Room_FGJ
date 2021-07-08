using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SLSceneSwitcher : MonoBehaviour {

    [SerializeField] private string nextScene;
    [SerializeField] private Image screenFadeUI;

    private bool switchScenes = false;
    private float timer;

    private void Start() {
        screenFadeUI.color = new Vector4(0f, 0f, 0f, 1f);
    }

    private void Update() {
        if(switchScenes) {
            timer = Mathf.Max(timer - Time.deltaTime, 0f);
            screenFadeUI.color = new Vector4(0f, 0f, 0f, Mathf.SmoothStep(0f, 1f, 1f - timer));

            if(timer <= 0f) {
                SceneManager.LoadScene(nextScene);
            }
        } else {
            if(timer < 1f) {
                timer = Mathf.Min(timer + Time.deltaTime * 0.66f, 1f);
                screenFadeUI.color = new Vector4(0f, 0f, 0f, Mathf.SmoothStep(1f, 0f, timer));
            }
        }
    }

    private void OnTriggerEnter(Collider collider) {
        if(collider.gameObject.TryGetComponent<FirstPersonController>(out FirstPersonController _)) {
            switchScenes = true;
        }
    }
}
