// Override unity's vector2 so we can enforce the use of INT
[System.Serializable]
public class Vector2 {
    public int x;
    public int y;

    public Vector2(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public Vector2(float x, float y) {
        this.x = (int)x;
        this.y = (int)y;
    }
}