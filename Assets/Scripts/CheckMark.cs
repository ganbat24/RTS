using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMark : MonoBehaviour
{
    public Save save;
    public bool marked = false;
    public event System.Action<CheckMark> OnMarked;
    [SerializeField] SpriteRenderer sprite;
    private void Awake() {
        sprite = GetComponent<SpriteRenderer>();
    }
    public void UnMark() {
        marked = false;
        sprite.color =  Color.white;
    }
    public void Mark(){
        OnMarked?.Invoke(this);
        marked = true;
        sprite.color =  Color.black;
    }
    private void OnMouseDown() {
        if(GameManager.gamePaused) return;
        Mark();
    }
}
