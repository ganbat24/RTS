using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ResourceManager))]
public class Planet : MonoBehaviour {
    [HideInInspector] public Commander commander;
    [HideInInspector] public ResourceManager resourceManager;

    public SpriteRenderer sprite = default;
    public static event System.Action<Planet> OnPlanetSpawned;
    public static event System.Action<Planet> OnPlanetDestroyed;

    List<Ship> fleets = new List<Ship>();

    float spawnRadius = 1f;

    public OutlineDisk outline = default;

    private void Awake() {
        resourceManager = GetComponent<ResourceManager>();
        outline = GetComponent<OutlineDisk>();
        spawnRadius = GetComponent<CircleCollider2D>().radius + 0.3f;
        resourceManager.OnResourceChange += CheckSprite;
    }

    private void Start() {
        commander = transform.parent.GetComponent<Commander>();
        OnPlanetSpawned?.Invoke(this);
        GameManager.onGamePause += () => Selected = false;
    }

    private void OnDestroy() {
        OnPlanetDestroyed?.Invoke(this);
    }

    void CheckSprite(){
        if(resourceManager.resources < 1f/3 * resourceManager.maxResources){
            sprite.sprite = commander.spriteSmall;
        }else if(resourceManager.resources < 2f/3 * resourceManager.maxResources){
            sprite.sprite = commander.spriteMed;
        }else{
            sprite.sprite = commander.spriteBig;
        }
    }

    private void Update() {
        
    }

    public void ChangeCommander(Commander _commander){
        Commander oldCommander = commander;
        commander = _commander;
        transform.parent = commander.transform;
        oldCommander.LoosePlanet(this);
        commander.GainPlanet(this);
        Player.instance.initialPlanet = null;
        Selected = false;
    }

    public void SendFleet(Planet other, int amount){
        if(amount <= 0) return;
        List<Ship> fleet = SendFleet(amount, other);
        if(fleet != null) fleets.AddRange(fleet);
    }
    
    public void SendFleet(Planet other){
        SendFleet(other, resourceManager.resources);
    }

    public List<Ship> SendFleet(int amount, Planet planet) {
        if(amount <= 0) return null;
        commander.RaiseCommanderAttack(commander);
        if(!planet.commander.Equals(commander)) planet.commander.RaiseCommanderAttack(planet.commander);
        if(resourceManager.SpendResources(amount)){
            List<Ship> fleet = new List<Ship>();
            for(int i = 0; i < amount; i++) {
                Ship tmp = SaveManager.SpawnShip(this);
                tmp.commander = commander;
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
            outline.outlEnabled = selected;
            outline.RefreshOutline();
        }
    }
    private void OnMouseDown() {
        if(GameManager.gamePaused) return;
        Commander.PlanetPressed(this);
        outline.RefreshOutline();
    }
    private void OnMouseEnter() {
        if(GameManager.gamePaused) return;
        outline.EnableOutline();
        sprite.color = Color.white;
    }
    private void OnMouseExit() {
        if(!Selected) outline.DisableOutline();
        outline.RefreshOutline();
    }
}