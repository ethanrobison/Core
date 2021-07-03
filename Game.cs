using UnityEngine;
using Core;

public class Game : MonoBehaviour
{
    public static GameObject Go { get; private set; }
    public static Services Serv { get; private set; }
    public static SessionContext Ctx { get; private set; }

    private void Awake() {
        Go = gameObject;

        Serv = new Services();
        Serv.Initialize();

        Ctx = new SessionContext();
        Ctx.Initialize();
    }

    private void Update() => Serv.Update();
}

// TODO
public class SessionContext
{
    public void Initialize() { }
}