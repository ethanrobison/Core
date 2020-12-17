using UnityEngine;

public class Game : MonoBehaviour
{
    public static GameObject Go { get; private set; }
    public static SessionContext Ctx { get; private set; }

    private void Awake() {
        Go = gameObject;

        Ctx = new SessionContext();
        Ctx.Initialize();
    }
}