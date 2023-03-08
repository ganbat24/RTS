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
            Save("" + saves.Count);
        }
    }

    public static Ship SpawnShip(Planet planet){
        Ship newShip = Instantiate(instance.shipStruct, planet.SpawnPoint(), Quaternion.identity);
        newShip.transform.parent = planet.transform;
        newShip.planet = planet;
        newShip.commander = planet.commander;

        return newShip;
    }

    public static Save Save(string _name){
        return instance.HiddenSave(_name);
    }

    Save HiddenSave(string _name){
        Save tmp = Instantiate(currentSave, transform);
        tmp.name = "Save : " + _name;
        saves.Add(tmp);
        tmp.gameObject.SetActive(false);
        return tmp;
    }

    public void Load(int index){
        Load(saves[index%saves.Count]);
    }

    public void Load(Save save){
        Debug.Log("Loaded from " + save.name);
        saves.Remove(currentSave);
        Destroy(currentSave.gameObject);
        currentSave = save;
        currentSave = Save("Current"); 
        save.gameObject.SetActive(false);
        currentSave.gameObject.SetActive(true);
    }
}
