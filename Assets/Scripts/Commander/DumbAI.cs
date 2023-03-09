using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbAI : Commander
{
    //full attacks per second
    public float attackSpeed = 2f;
    [SerializeField] float nextAttackTime = 0f;

    private void Update() {
        if(GameManager.gamePaused){
            nextAttackTime += Time.deltaTime;
        }
        if(Time.time >= nextAttackTime){
            SortPlanets();
            ownedPlanets.ForEach((planet) => {
                if(planet.resourceManager.resources > 10){
                    for(int i = 0; i < save.planets.Count; i++){
                        if(!save.planets[i].commander.Equals(this)){
                            planet.SendFleet(save.planets[i]);
                            break;
                        }
                    }
                }
            });
            nextAttackTime = Time.time + 1/attackSpeed;
        }
    }

    void SortPlanets(){
        save.planets.Sort((p1, p2) => {
            return p1.resourceManager.resources.CompareTo(p2.resourceManager.resources);
        });
    }
}
