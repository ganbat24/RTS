using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static event System.Action onGamePause;
    public static event System.Action onGameUnPause;

    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(this);
        }
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(gamePaused) UnPauseGame();
            else PauseGame();
        }
    }

    public static bool gamePaused = false;
    public static void UnPauseGame(){
        gamePaused = false;
        onGameUnPause?.Invoke();
    }

    public static void PauseGame(){
        gamePaused = true;
        onGamePause?.Invoke();
    }
}