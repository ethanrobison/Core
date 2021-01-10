using Core;
using UnityEngine;

public class PrefabFactory : UiPrefabFactory
{
    // TODO I know Rob had them loaded in a scene... maybe I should do that instead? We'll come back to it... I like his method, but we're hacking it for now, and next game it will get more love
    // TODO can I just move this into UiService? Does it need to be an abstract class?
    public override GameObject MakeUiElement(UiPrefabs.UiPrefabs prefabName, Transform holder = null) {
        if (holder == null) {
            holder = UiUtils.GetCanvas().transform;
        }

        var path = PrefabPathFromEnum(prefabName);
        var prefab = Resources.Load<GameObject>(path);

        if (prefab == null) {
            Logging.Error("No prefab with path", path);
            return null;
        }

        var go = Object.Instantiate(prefab, holder);
        return go;
    }
}


