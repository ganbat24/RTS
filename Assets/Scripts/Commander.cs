using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commander : MonoBehaviour
{
    [HideInInspector] public Save save = default;

    public List<Planet> ownedPlanets = default;
    public Color color = default;
    public static event System.Action<Planet> OnPlanetPressed;
    public event System.Action<Planet> OnGainedPlanet;
    public event System.Action<Planet> OnLostPlanet;

    private void Start() {
        save = transform.parent.GetComponent<Save>();
    }

    public virtual Commander Copy(Save _save){
        Commander newCommander = Instantiate(this, Vector3.zero, Quaternion.identity);
        newCommander.transform.parent = _save.transform;
        newCommander.save = _save;
        newCommander.name = name;
        return newCommander;
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
