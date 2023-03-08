using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] GameObject defaultResourceUI = default;
    
    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(this);
        }
        Planet.OnPlanetSpawned += instance.CreateResourceUI;
        Planet.OnPlanetDestroyed += instance.DestroyResourceUI;
        Planet.OnPlanetSpawned += ResourceUIVisibility;
        
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
        tmp.transform.SetParent(transform);
        tmp.SetActive(planet.commander == Player.instance || planet.commander == null);
        TextMeshProUGUI tmptext = tmp.GetComponent<TextMeshProUGUI>();
        tmptext.text = "" + planet.resourceManager.resources;
        planet.resourceManager.OnResourceChange += () => tmptext.text = "" + planet.resourceManager.resources;
        resourceUI.Add(planet, tmp);
    }
}
