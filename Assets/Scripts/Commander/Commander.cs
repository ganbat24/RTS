using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commander : MonoBehaviour
{
    [HideInInspector] public Save save = default;

    public List<Planet> ownedPlanets = default;
    public Color color = new Color(199, 155, 155, 155);
    public static event System.Action<Planet> OnPlanetPressed;
    public event System.Action<Commander> OnCommanderAttack;
    public event System.Action<Planet> OnGainedPlanet;
    public event System.Action<Planet> OnLostPlanet;

    private void Start() {
        save = transform.parent.GetComponent<Save>();
    }

    protected void RaiseCommanderAttack(Commander commander){
        OnCommanderAttack?.Invoke(commander);
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
