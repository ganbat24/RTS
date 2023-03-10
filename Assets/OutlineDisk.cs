using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Planet))]
public class OutlineDisk : MonoBehaviour
{
    [SerializeField] Planet planet = default;
    private void Awake() {
        planet = GetComponent<Planet>();
        SaveManager.OnLoaded += DisableOutline;
    }

    private void OnDestroy() {
        SaveManager.OnLoaded -= DisableOutline;
    }

    public bool outlEnabled = false;
    public SpriteRenderer outline = default;
    public void EnableOutline(){
        outlEnabled = true;
        RefreshOutline();
    }
    public void DisableOutline(){
        outlEnabled = false;
        RefreshOutline();
    }
    public void RefreshOutline(){
        if(outline == null) {
            outline = gameObject.AddComponent<SpriteRenderer>();
            outline.size = Vector2.up * 2f;
            outline.color = Color.white;
            outline.material.shader = Shader.Find("GUI/Text Shader");
        }

        outline.sprite = planet.sprite.sprite;
        outline.enabled = outlEnabled;
    }
}
