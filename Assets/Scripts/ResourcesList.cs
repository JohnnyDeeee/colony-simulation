using UnityEngine;

public static class ResourcesList {
    public static readonly GameObject WORLD = Resources.Load<GameObject>("world");
    public static readonly GameObject GRID = Resources.Load<GameObject>("grid");
    public static readonly GameObject UI = Resources.Load<GameObject>("ui");
    public static readonly GameObject UI_WORLDINFO = Resources.Load<GameObject>("ui_world_info");
    public static readonly GameObject UI_AIINFO = Resources.Load<GameObject>("ui_ai_info");

    public static readonly TextAsset LEVEL_1 = Resources.Load<TextAsset>("Levels/level1");

    public static readonly GameObject TILE_GROUND = Resources.Load<GameObject>("tile_ground");
    public static readonly GameObject TILE_WALL = Resources.Load<GameObject>("tile_wall");
    public static readonly GameObject TILE_FOOD = Resources.Load<GameObject>("tile_food");

    public static readonly GameObject AI_ANT = Resources.Load<GameObject>("ai_ant");

    public static readonly Font FONT_ARIAL = Resources.GetBuiltinResource<Font>("Arial.ttf");
}