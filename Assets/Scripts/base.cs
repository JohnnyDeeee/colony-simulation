using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base {
    // Class name, used in logging
    protected string _name { get; set; }

    public Base(string name){
        this._name = name;
    }

    // Log to console
    protected void Log(string message) {
        Debug.Log(string.Format("[{0}] {1}", this._name, message));
    }
}