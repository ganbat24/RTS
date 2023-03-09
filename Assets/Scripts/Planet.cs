using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ResourceManager))]
public class Planet : MonoBehaviour {
    [HideInInspector] public Commander commander;
    [HideInInspector] public ResourceManager resourceManager;

    [SerializeField] SpriteRenderer sprite = default;
    public static event System.Action<Planet> OnPlanetSpawned;
    public static event System.Action<Planet> OnPlanetDestroyed;

    List<Ship> fleets = new List<Ship>();

    float spawnRadius = 1f;

    private void Awake() {
        resourceManager = GetComponent<ResourceManager>();
        spawnRadius = GetComponent<CircleCollider2D>().radius + 0.3f;
    }

    private void Start() {
        commander = transform.parent.GetComponent<Commander>();
        sprite.color = commander.color;
        OnPlanetSpawned?.Invoke(this);
        GameManager.onGamePause += OnMouseExit;
    }

    private void OnDestroy() {
        OnPlanetDestroyed?.Invoke(this);
    }

    private void Update() {
        
    }

    public void ChangeCommander(Commander _commander){
        commander?.LoosePlanet(this);
        commander = _commander;
        transform.parent = commander.transform;
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

    bool selected = false;
    public bool Selected {
        get { return selected; }
        set {
            selected = value;
            if(!selected) sprite.color = commander.color;
            else sprite.color = Color.white;
        }
    }
    private void OnMouseDown() {
        if(GameManager.gamePaused) return;
        Commander.PlanetPressed(this);
    }
    private void OnMouseEnter() {
        if(GameManager.gamePaused) return;
        sprite.color = Color.white;
    }
    private void OnMouseExit() {
        if(!Selected) sprite.color = commander.color;
    }
}