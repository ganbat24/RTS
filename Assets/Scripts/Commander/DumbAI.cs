using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbAI : Commander
{
    //full attacks per second
    public float attackSpeed = 5f;
    [SerializeField] float nextAttackTime = 0f;

    private void Update() {
        if(GameManager.gamePaused){
            nextAttackTime += Time.deltaTime;
            return;
        }
        if(Time.time >= nextAttackTime){
            if(Player.instance.Resources() * 0.75f > Resources()){
                ownedPlanets.ForEach((planet) => planet.resourceManager.GatherResources(1));
            }
            SortPlanets();
            ownedPlanets.ForEach((planet) => {
                if(planet.resourceManager.resources >= 60){
                    for(int i = 0; i < Mathf.Min(save.planets.Count, 3); i++){
                        if(!save.planets[i].commander.Equals(this)){
                            planet.SendFleet(save.planets[i], 20);
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
