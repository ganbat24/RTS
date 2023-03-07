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
    }

    private void Start() {
        Player.instance.OnGainedPlanet += (planet) => resourceUI[planet].SetActive(true);
        Player.instance.OnLostPlanet += (planet) => resourceUI[planet].SetActive(false);

        Planet.planets.ForEach((planet) => {
            instance.CreateResourceUI(planet);
        });
    }

    Dictionary<Planet, GameObject> resourceUI = new Dictionary<Planet, GameObject>();

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
