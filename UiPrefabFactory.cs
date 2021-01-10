using System.Text;
using UnityEngine;

namespace Core
{
    public class UiPrefabFactory
    {
        public GameObject MakeUiElement(UiPrefabs.UiPrefabs prefab, Canvas canvas) => MakeUiElement(prefab, canvas.transform);

        // TODO I know Rob had them loaded in a scene... maybe I should do that instead? We'll come back to it... I like his method, but we're hacking it for now, and next game it will get more love
        // TODO can I just move this into UiService? Does it need to be an abstract class?
        public GameObject MakeUiElement(UiPrefabs.UiPrefabs prefabName, Transform holder = null) {
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

        private static readonly StringBuilder _sb = new StringBuilder();
        protected string PrefabPathFromEnum(UiPrefabs.UiPrefabs prefabName) {
            var name = prefabName.ToString();
            var length = name.Length;

            _sb.Append("Ui/");

            _sb.Append(name[0]);
            for (int i = 1; i < length; i++) {
                char c = name[i];
                if (char.IsUpper(c)) {
                    _sb.Append(" ");
                }

                // TODO possibly include parsing rules for inserting /'s

                _sb.Append(c);
            }

            return _sb.ToStringAndClear();
        }
    }
}


