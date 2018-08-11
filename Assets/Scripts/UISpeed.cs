using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpeed : UI {
    [SerializeField] private Button speed1;
    [SerializeField] private Button speed2;
    [SerializeField] private Button speed3;

    public new void Start () {
        speed1.onClick.AddListener(delegate { setSpeed(1); });
        speed2.onClick.AddListener(delegate { setSpeed(2); });
        speed3.onClick.AddListener(delegate { setSpeed(3); });
    }
    
    private void setSpeed(int speed) {
        Time.timeScale = speed;
        Debug.Log("speed = " + speed);
    }
}