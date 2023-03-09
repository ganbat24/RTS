using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {
    public int maxResources = 30;
    //units per second
    public float resourcesGrowthRate = 0.5f;
    public int resources = 0;

    public event System.Action OnResourceChange;

    float nextGrowthTime = 0f;
    private void Update() {
        if(GameManager.gamePaused){
            nextGrowthTime += Time.deltaTime;
        }
        if(Time.time > nextGrowthTime && resources < maxResources){
            resources ++;
            OnResourceChange?.Invoke();
            nextGrowthTime = Time.time + 1f/resourcesGrowthRate;
        }
    }

    public void GatherResources(int amount) {
        resources += amount;
        OnResourceChange?.Invoke();
    }

    public bool SpendResources(int amount) {
        if (resources >= amount) {
            resources -= amount;
            OnResourceChange?.Invoke();
            return true;
        } else {
            return false;
        }
    }

    public bool TryConquer(int amount){
        resources -= amount;
        if(resources < 0){
            resources *= -1;
            OnResourceChange?.Invoke();
            return true;
        }else{
            OnResourceChange?.Invoke();
            return false;
        }
    }
}

