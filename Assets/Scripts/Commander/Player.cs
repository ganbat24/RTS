using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Commander
{
    public static Player instance;

    private void Awake() {
        instance = this;
        OnPlanetPressed += SomePlanetPressed;
        GameManager.onGamePause += () => {
            initialPlanet.Selected = false;
            initialPlanet = null;
        };
    }

    private void OnEnable() {
        instance = this;
    }

    [SerializeField] float nextBonusTime = 0f;
    private void Update() {
        if(GameManager.gamePaused) return;
        if(Resources() < 40 && Time.time > nextBonusTime){
            ownedPlanets.ForEach(planet => planet.resourceManager.GatherResources(1));
            nextBonusTime = Time.time + 1f / 7f;
        }
    }

    public Planet initialPlanet = null;
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
            }
            initialPlanet.Selected = false;
            initialPlanet = null;
        }
    }
}
