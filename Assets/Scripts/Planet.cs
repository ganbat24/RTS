using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ResourceManager))]
public class Planet : MonoBehaviour {
    public static List<Planet> planets = new List<Planet>();

    [HideInInspector] public ResourceManager resourceManager;
    [HideInInspector] public Commander commander;

    [SerializeField] SpriteRenderer sprite = default;
    [SerializeField] Ship exampleShip = default;
    [SerializeField] float spawnRadius = 1f;

    private void Awake() {
        planets.Add(this);
        resourceManager = GetComponent<ResourceManager>();
        spawnRadius = GetComponent<CircleCollider2D>().radius + 0.3f;
        if(commander == null) sprite.color = Color.gray;
    }

    private void Start() {
    }

    public void ChangeCommander(Commander _commander){
        commander?.LoosePlanet(this);
        commander = _commander;
        commander.GainPlanet(this);
        sprite.color = commander.color;
    }

    public void SendFleet(Planet other, int amount){
        List<Ship> fleet = CreateFleet(amount);
        FleetDestination(fleet, other);
    }
    
    public void SendFleet(Planet other){
        SendFleet(other, resourceManager.resources);
    }

    public List<Ship> CreateFleet(int amount) {
        if(resourceManager.SpendResources(amount)){
            List<Ship> fleet = new List<Ship>();
            for(int i = 0; i < amount; i++) {
                //only spawns that type
                Ship newShip = Instantiate(exampleShip, SpawnPoint(), Quaternion.identity);
                newShip.transform.parent = transform;
                newShip.commander = commander;
                newShip.planet = this;
                fleet.Add(newShip);
            }
            return fleet;
        }
        return null;
    }

    public void FleetDestination(List<Ship> fleet, Planet destination) {
        foreach (Ship ship in fleet) {
            ship.SetDestination(destination);
        }
    }

    Vector3 SpawnPoint(){
        //to be implemented
        //returns a random point on the boundary of the circle
        float angle = Random.Range(0f, 2f * Mathf.PI);
        Vector2 randomPosition = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * spawnRadius;
        return (Vector2)transform.position + randomPosition;
    }

    bool selected = false;

    private void OnMouseDown() {
        Commander.PlanetPressed(this);
    }

    private void OnMouseEnter() {
        selected = true;
        sprite.color = Color.white;
    }

    private void OnMouseExit() {
        selected = false;
        if(commander != null) sprite.color = commander.color;
        else sprite.color = Color.gray;
    }
}