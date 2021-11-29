using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Rule{
    
    public char inital;
    public string result;
    public bool Match(char i){

        if(i == inital){

            return true;

        } else {

            return false;

        }

    }

    public string GetResult(){

        return result;

    }

}
public class LSystem : MonoBehaviour{
    public string axiom;
    public float angle;
    public int n;
    [SerializeField]
    public List<Rule> rules = new List<Rule>();

}
