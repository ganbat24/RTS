using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commander : MonoBehaviour
{
    [HideInInspector] public Save save = default;

    public List<Planet> ownedPlanets = default;
    public static event System.Action<Planet> OnPlanetPressed;
    public event System.Action OnCommanderAttack;
    public event System.Action OnCommanderDamaged;
    public event System.Action<Planet> OnGainedPlanet;
    public event System.Action<Planet> OnLostPlanet;

    public Sprite spriteSmall = default;
    public Sprite spriteMed = default;
    public Sprite spriteBig = default;
    public Sprite spriteUnit = default;

    private void Start() {
        save = transform.parent.GetComponent<Save>();
    }

    public void RaiseCommanderAttack(Commander commander){
        OnCommanderAttack?.Invoke();
    }

    public void RaiseCommanderDamaged(Commander commander){
        OnCommanderAttack?.Invoke();
    }

    public int shipCount = 0;

    public int Resources(){
        int ans = 0;
        ownedPlanets.ForEach((planet) => ans += planet.resourceManager.resources);
        return ans + shipCount;
    }

    public void LoosePlanet(Planet planet){
        ownedPlanets.Remove(planet);
        OnLostPlanet?.Invoke(planet);
    }

    public void GainPlanet(Planet planet){
        ownedPlanets.Add(planet);
        OnGainedPlanet?.Invoke(planet);
    }


    public static void PlanetPressed(Planet planet){
        OnPlanetPressed?.Invoke(planet);
    }
}
