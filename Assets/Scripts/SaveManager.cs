using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] Ship shipStruct = default;
    
    public List<Save> saves = new List<Save>();
    public Save currentSave = default;

    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(this);
        }
    }

    private void Start() {
        saves.Add(Save(currentSave, "Start"));
    }

    int index = 0;
    private void Update() {
        if(Input.GetKeyDown(KeyCode.DownArrow)){
            index++;
            if(index >= saves.Count) index -= saves.Count;
            Load(index);
        }else if(Input.GetKeyDown(KeyCode.UpArrow)){
            index--;
            if(index < 0) index += saves.Count;
            Load(index);
        }else if(Input.GetMouseButtonDown(1)){
            saves.Add(Save(currentSave, "" + saves.Count));
        }
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
        Destroy(currentSave.gameObject);
        currentSave = Save(save, "[Loaded] : " + save.name); 
        save.gameObject.SetActive(false);
        currentSave.gameObject.SetActive(true);
    }
}
