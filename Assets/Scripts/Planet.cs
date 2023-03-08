using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ResourceManager))]
public class Planet : MonoBehaviour {
    [HideInInspector] public Commander commander;
    [HideInInspector] public ResourceManager resourceManager;

    [SerializeField] SpriteRenderer sprite = default;
    public static event System.Action<Planet> OnPlanetSpawned;

    List<Ship> fleets = new List<Ship>();

    float spawnRadius = 1f;

    private void Awake() {
        resourceManager = GetComponent<ResourceManager>();
        spawnRadius = GetComponent<CircleCollider2D>().radius + 0.3f;
    }

    private void Start() {
        commander = transform.parent.GetComponent<Commander>();
        sprite.color = commander.color;
    }

    public void ChangeCommander(Commander _commander){
        commander?.LoosePlanet(this);
        commander = _commander;
        commander.GainPlanet(this);
        sprite.color = commander.color;
    }

    public void SendFleet(Planet other, int amount){
        List<Ship> fleet = SendFleet(amount, other);
        fleets.AddRange(fleet);
    }
    
    public void SendFleet(Planet other){
        SendFleet(other, resourceManager.resources);
    }

    public List<Ship> SendFleet(int amount, Planet planet) {
        if(resourceManager.SpendResources(amount)){
            List<Ship> fleet = new List<Ship>();
            for(int i = 0; i < amount; i++) {
                Ship tmp = SaveManager.SpawnShip(this);
                tmp.SetDestination(planet);
                fleet.Add(tmp);
            }
            return fleet;
        }
        return null;
    }

    public Vector3 SpawnPoint(){
        float angle = Random.Range(0f, 2f * Mathf.PI);
        Vector2 randomPosition = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * spawnRadius;
        return (Vector2)transform.position + randomPosition;
    }

    private void OnMouseDown() {
        Commander.PlanetPressed(this);
    }
    private void OnMouseEnter() {
        sprite.color = Color.white;
    }
    private void OnMouseExit() {
        sprite.color = commander.color;
    }
}