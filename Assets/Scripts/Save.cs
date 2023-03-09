using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour{
    public List<Commander> commanders = new List<Commander>();
    public List<Planet> planets = new List<Planet>();

    public static Save instance;

    private void Awake() {
        instance = this;
        // Planet.OnPlanetSpawned += (planet) => planets.Add(planet);
    }
    
    public void UnLoad(){
        gameObject.SetActive(false);
    }

    public void Load(){
        gameObject.SetActive(true);
    }
}