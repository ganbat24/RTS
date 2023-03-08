using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] Canvas UI = default;
    [SerializeField] Canvas InGameUI = default;
    [SerializeField] GameObject defaultResourceUI = default;
    [SerializeField] TextMeshProUGUI dialogueText = default;
    [SerializeField] List<Dialogue> OnPlayerAttackDialogue = new List<Dialogue>();
    
    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(this);
        }
        Planet.OnPlanetSpawned += instance.CreateResourceUI;
        Planet.OnPlanetDestroyed += instance.DestroyResourceUI;
        Planet.OnPlanetSpawned += ResourceUIVisibility;
        ((RectTransform)InGameUI.transform).sizeDelta = new Vector2(Screen.width, Screen.height);

        Player.instance.OnGainedPlanet += ResourceUIVisibility;
        Player.instance.OnCommanderAttack += OnPlayerAttack;
    }

    Coroutine currentDialogue = null;
    bool dialogueFinished = true;
    void OnPlayerAttack(Commander commander){
        if(!dialogueFinished) return;
        if(Random.Range(0, 100) < 40){
            if(currentDialogue != null) StopCoroutine(currentDialogue);
            dialogueFinished = false;
            Dialogue rand = OnPlayerAttackDialogue[Random.Range(0, OnPlayerAttackDialogue.Count)];
            currentDialogue = StartCoroutine(rand.Display(dialogueText, () => dialogueFinished = true));
        }
    }

    void ResourceUIVisibility(Planet planet){
        resourceUI[planet].SetActive(planet.isActiveAndEnabled && planet.commander.Equals(Player.instance));
    }

    Dictionary<Planet, GameObject> resourceUI = new Dictionary<Planet, GameObject>();
    void DestroyResourceUI(Planet planet){
        if(resourceUI.ContainsKey(planet)){
            Destroy(resourceUI[planet].gameObject);
            resourceUI.Remove(planet);
        }
    }

    void CreateResourceUI(Planet planet){
        GameObject tmp = Instantiate(defaultResourceUI, planet.transform.position, Quaternion.identity);
        tmp.transform.SetParent(InGameUI.transform);
        tmp.SetActive(planet.commander == Player.instance || planet.commander == null);
        TextMeshProUGUI tmptext = tmp.GetComponent<TextMeshProUGUI>();
        tmptext.text = "" + planet.resourceManager.resources;
        planet.resourceManager.OnResourceChange += () => tmptext.text = "" + planet.resourceManager.resources;
        resourceUI.Add(planet, tmp);
    }
}
