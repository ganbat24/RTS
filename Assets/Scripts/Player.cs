using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Commander
{
    public static Player instance;

    [SerializeField] List<Planet> desiredPlanets = default;

    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
        OnPlanetPressed += SomePlanetPressed;
        desiredPlanets.ForEach(planet => planet.ChangeCommander(this));
    }

    private void Update() {
        if(Input.GetMouseButtonDown(0)){
            // Debug.Log(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)));
        }
    }

    Planet initialPlanet = null;
    void SomePlanetPressed(Planet planet)
    {
        if(initialPlanet == null){
            if(planet.commander == this){
                initialPlanet = planet;
            }
        }else{
            if(initialPlanet != planet){
                initialPlanet.SendFleet(planet);
            }
            initialPlanet = null;
        }
    }
}
