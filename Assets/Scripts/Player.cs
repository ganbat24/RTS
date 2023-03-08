using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Commander
{
    public static Player instance;

    private void Awake() {
        instance = this;
        OnPlanetPressed += SomePlanetPressed;
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
                RaiseCommanderAttack(planet.commander);
            }
            initialPlanet = null;
        }
    }
}
