using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    private UIWorldInfo worldInfo;
    private UIAIInfo aiInfo;
    protected string newLine = "\n";

    public void Start() {
        // Add world info to ui
        GameObject worldInfoInstance = GameObject.Instantiate(ResourcesList.UI_WORLDINFO, Vector2.zero, Quaternion.identity, this.transform);
        worldInfoInstance.name = "world_info";
        this.worldInfo = worldInfoInstance.GetComponent<UIWorldInfo>();

        // Add ai info to ui
        GameObject aiInfoInstance = GameObject.Instantiate(ResourcesList.UI_AIINFO, Vector2.zero, Quaternion.identity, this.transform);
        aiInfoInstance.name = "ai_info";
        this.aiInfo = aiInfoInstance.GetComponent<UIAIInfo>();
    }
}