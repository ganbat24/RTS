using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commander : MonoBehaviour
{
    public List<Planet> ownedPlanets = default;
    public Color color = default;
    public static event System.Action<Planet> OnPlanetPressed;
    public event System.Action<Planet> OnGainedPlanet;
    public event System.Action<Planet> OnLostPlanet;

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
