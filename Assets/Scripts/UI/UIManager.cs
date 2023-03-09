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
    [SerializeField] TextMeshProUGUI timerText = default;
    [SerializeField] List<Dialogue> OnPlayerAttackDialogue = new List<Dialogue>();
    [SerializeField] Dictionary<Planet, GameObject> resourceUI = new Dictionary<Planet, GameObject>();
    
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
        SaveManager.OnLoaded += () => {
            Player.instance.OnGainedPlanet += ResourceUIVisibility;
            Player.instance.OnLostPlanet += ResourceUIVisibility;
            Player.instance.OnCommanderAttack += OnPlayerAttack;
        };
    }

    [SerializeField] int timeInc = 15;
    private void Update() {
        float p = SaveManager.instance.timePercent;
        p *= 24 * 3600;
        timerText.text = "" + Mathf.Floor(p / 3600) + " : " + Mathf.Floor((p%3600)/(60 * timeInc))*timeInc;
    }

    [SerializeField] Coroutine currentDialogue = null;
    [SerializeField] bool dialogueFinished = true;
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
