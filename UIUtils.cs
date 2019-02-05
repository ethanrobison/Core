using UnityEngine;

namespace Utils
{
    public static class UIUtils
    {
        public static Transform GetCanvas () {
            return GameObject.Find("Canvas").transform; // hope there's only ever one canvas
        }


        /// <summary>
        /// Given a transform and the name of a child, get its component of type T.
        /// Complains if child/component nonexistent.
        /// </summary>
        public static T FindUICompOfType<T> (Transform parent, string name) where T : MonoBehaviour {
            var child = parent.Find(name);
            if (child == null) {
                Logging.Error("Missing child: " + name);
                return null;
            }

            var comp = child.gameObject.GetComponent<T>();
            Logging.Assert(comp != null, "Child missing component of type: " + typeof(T));
            return comp;
        }

        /// <summary>
        /// Given a gameobject and the name of a child, get its component of type T.
        /// </summary>
        public static T FindUICompOfType<T> (GameObject go, string name) where T : MonoBehaviour {
            return FindUICompOfType<T>(go.transform, name);
        }
    }
}