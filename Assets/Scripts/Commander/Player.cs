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
        
    }

    Planet initialPlanet = null;
    void SomePlanetPressed(Planet planet)
    {
        if(initialPlanet == null){
            if(planet.commander.Equals(this)){
                initialPlanet = planet;
                planet.Selected = true;
            }
        }else{
            if(initialPlanet != planet){
                planet.Selected = false;
                initialPlanet.SendFleet(planet);
                RaiseCommanderAttack(planet.commander);
            }
            initialPlanet.Selected = false;
            initialPlanet = null;
        }
    }
}
