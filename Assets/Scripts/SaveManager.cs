using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] Transform checkMarkHolder = default;
    [SerializeField] Ship shipStruct = default;
    [SerializeField] CheckMark checkMarkOb = default;
    
    public List<Save> saves = new List<Save>();
    public List<CheckMark> checks = new List<CheckMark>();
    public Save currentSave = default;

    public static event System.Action OnLoaded;
    public static event System.Action<CheckMark> OnMarked;

    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(this);
        }
    }

    private void Start() {

    }

    public float timeInDay = 15f;
    public float timePercent = 0f;

    public event System.Action OnUnMark;
    private void Update() {
        if(GameManager.gamePaused){
            return;
        }
        if(saves.Count == 0) NewSave();
        timePercent += Time.deltaTime / timeInDay;
        if(timePercent >= 1f){
            timePercent = 0f;
            NewSave();
        }
    }

    //idk
    public void NewSave(){
        Vector3 pos = new Vector3(-7 + 0.37f * (saves.Count % 7), 3 - 0.27825f * Mathf.Floor(saves.Count/7), 0);
        Save newSave = Save(currentSave, "" + saves.Count);
        saves.Add(newSave);
        CheckMark newcheck = Instantiate(checkMarkOb, pos, Quaternion.identity); 
        checks.Add(newcheck);
        newcheck.transform.parent = checkMarkHolder;

        newcheck.save = newSave;
        newcheck.OnMarked += (check) => {
            OnUnMark?.Invoke();
            Load(check.save);
        };
        newcheck.OnMarked += OnMarked;
        OnUnMark += newcheck.UnMark;
        newcheck.Mark();
    }

    public static Ship SpawnShip(Planet planet){
        Ship newShip = Instantiate(instance.shipStruct, planet.SpawnPoint(), Quaternion.identity);
        newShip.transform.parent = planet.transform;
        newShip.planet = planet;
        newShip.commander = planet.commander;

        return newShip;
    }

    public static Save Save(Save save, string _name){
        return instance.HiddenSave(save, _name);
    }

    Save HiddenSave(Save save, string _name){
        currentSave.gameObject.SetActive(false);
        Save tmp = Instantiate(save, transform);
        tmp.name = "[Save] : " + _name;
        tmp.gameObject.SetActive(false);
        currentSave.gameObject.SetActive(true);
        return tmp;
    }

    public void Load(int index){
        Load(saves[index%saves.Count]);
    }

    public void Load(Save save){
        Save newSave = Save(save, "[Loaded] : " + save.name); 

        save.gameObject.SetActive(false);
        newSave.gameObject.SetActive(true);
        Destroy(currentSave.gameObject);
        currentSave = newSave;

        OnLoaded?.Invoke();
    }
}
