using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour{
    public List<Commander> commanders = new List<Commander>();
    
    public void UnLoad(){
        gameObject.SetActive(false);
    }

    public void Load(){
        gameObject.SetActive(true);
    }
}