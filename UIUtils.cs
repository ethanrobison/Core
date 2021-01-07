using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Core
{
    public static class UIUtils
    {
        public static Transform GetCanvas() {
            return GameObject.Find("Canvas").transform; // hope there's only ever one canvas
        }

        public static TextMeshProUGUI GetTextMesh(Transform tr, string name) => FindUICompOfType<TextMeshProUGUI>(tr, name);
        public static TextMeshProUGUI GetTextMesh(GameObject go, string name) => GetTextMesh(go.transform, name);

        public static Button GetButton(GameObject go, string name) => FindUICompOfType<Button>(go, name);
        public static Button GetAndSetButton(GameObject go, string name, UnityAction onClick) {
            var btn = GetButton(go, name);
            btn.SetListener(onClick);
            return btn;
        }

        public static T FindUICompOfType<T>(Transform parent, string name) where T : MonoBehaviour {
            var child = parent.Find(name);
            if (child == null) {
                Logging.Error(parent.name, " missing child: ", name);
                return null;
            }

            var comp = child.gameObject.GetComponent<T>();
            Logging.Assert(comp != null, "Child missing component of type: " + typeof(T));
            return comp;
        }

        public static T FindUICompOfType<T>(GameObject go, string name) where T : MonoBehaviour {
            return FindUICompOfType<T>(go.transform, name);
        }
    }

    public static class ButtonExtensions
    {
        public static void SetListener(this Button btn, UnityAction onclick) {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(onclick);
        }
    }
}