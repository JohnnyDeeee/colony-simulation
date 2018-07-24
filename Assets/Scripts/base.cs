using UnityEngine;

public abstract class Base {
    // Class name, used in logging
    protected string _name;

    public string GetName() { return this._name; }

    public Base(string name){
        this._name = name;
    }

    // Log to console
    protected void Log(string message) {
        Debug.Log(string.Format("[{0}] {1}", this._name, message));
    }
}