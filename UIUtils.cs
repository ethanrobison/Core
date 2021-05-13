using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Core
{
    public static class UiUtils
    {
        public static Canvas GetCanvas() {
            var canvastr = GameObject.Find("Canvas").transform; // hope there's only ever one canvas
            if (canvastr == null) {
                Logging.Error("Missing GameObject named \"Canvas\".");
                return null;
            }

            var canvas = canvastr.GetComponent<Canvas>();
            if (canvas == null) {
                Logging.Error("Canvas GameObject has no Canvas component.");
                return null;
            }

            return canvas;
        }

        public static TextMeshProUGUI GetTextMesh(Transform tr, string name) => FindUICompOfType<TextMeshProUGUI>(tr, name);
        public static TextMeshProUGUI GetTextMesh(GameObject go, string name) => GetTextMesh(go.transform, name);

        public static Button GetButton(GameObject go, string name) => FindUICompOfType<Button>(go, name);
        public static Button GetAndSetButton(GameObject go, string name, UnityAction onClick) {
            var btn = GetButton(go, name);
            if (btn == null) {
                Logging.Error("Null button in search of", name);
                return null;
            }

            btn.SetListener(onClick);
            return btn;
        }

        public static T FindUICompOfType<T>(Transform parent, string name) where T : MonoBehaviour {
            if (parent == null) {
                Logging.Error("Null Transform in search of", name);
                return null;
            }

            var child = parent.Find(name);
            if (child == null) {
                Logging.Error(parent.name, "missing child:", name);
                return null;
            }

            var comp = child.gameObject.GetComponent<T>();
            Logging.Assert(comp != null, "Child missing component of type:", typeof(T));
            return comp;
        }

        public static T FindUICompOfType<T>(GameObject go, string name) where T : MonoBehaviour {
            if (go == null) {
                Logging.Error("Null GameObject in search of", name);
                return null;
            }
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