using System.Diagnostics;
using System.Text;

namespace Utils
{
    public static class Logging
    {
        private static string Stringify (params object[] objects) {
            var sb = new StringBuilder();
            foreach (var item in objects) {
                sb.Append(item.ToString());
                sb.Append(" ");
            }

            var res = sb.ToString();
            sb.Clear();
            return res;
        }

        [Conditional("UNITY_EDITOR")]
        public static void Log (object o) => UnityEngine.Debug.Log(o.ToString());
        [Conditional("UNITY_EDITOR")]
        public static void Log (object o1, object o2) => UnityEngine.Debug.Log(Stringify(o1, o2));
        [Conditional("UNITY_EDITOR")]
        public static void Log (object o1, object o2, object o3)
            => UnityEngine.Debug.Log(Stringify(o1, o2, o3));
        [Conditional("UNITY_EDITOR")]
        public static void Log (object o1, object o2, object o3, object o4)
            => UnityEngine.Debug.Log(Stringify(o1, o2, o3, o4));
        [Conditional("UNITY_EDITOR")]
        public static void Log (object o1, object o2, object o3, object o4, object o5)
            => UnityEngine.Debug.Log(Stringify(o1, o2, o3, o4, o5));


        [Conditional("UNITY_EDITOR")]
        public static void Assert (bool condition, object o1) {
            if (!condition) { Error(Stringify(o1)); }
        }
        [Conditional("UNITY_EDITOR")]
        public static void Assert (bool condition, object o1, object o2) {
            if (!condition) { Error(Stringify(o1, o2)); }
        }
        [Conditional("UNITY_EDITOR")]
        public static void Assert (bool condition, object o1, object o2, object o3) {
            if (!condition) { Error(Stringify(o1, o2, o3)); }
        }
        [Conditional("UNITY_EDITOR")]
        public static void Assert (bool condition, object o1, object o2, object o3, object o4) {
            if (!condition) { Error(Stringify(o1, o2, o3, o4)); }
        }
        [Conditional("UNITY_EDITOR")]
        public static void Assert (bool condition, object o1, object o2, object o3, object o4, object o5) {
            if (!condition) { Error(Stringify(o1, o2, o3, o4, o5)); }
        }

        [Conditional("UNITY_EDITOR")]
        public static void Warn (object o) => UnityEngine.Debug.LogWarning(o.ToString());
        [Conditional("UNITY_EDITOR")]
        public static void Warn (object o1, object o2) => UnityEngine.Debug.LogWarning(Stringify(o1, o2));
        [Conditional("UNITY_EDITOR")]
        public static void Warn (object o1, object o2, object o3)
            => UnityEngine.Debug.LogWarning(Stringify(o1, o2, o3));
        [Conditional("UNITY_EDITOR")]
        public static void Warn (object o1, object o2, object o3, object o4)
            => UnityEngine.Debug.LogWarning(Stringify(o1, o2, o3, o4));
        [Conditional("UNITY_EDITOR")]
        public static void Warn (object o1, object o2, object o3, object o4, object o5)
            => UnityEngine.Debug.LogWarning(Stringify(o1, o2, o3, o4, o5));

        [Conditional("UNITY_EDITOR")]
        public static void Error (object o) => UnityEngine.Debug.LogError(o.ToString());
        [Conditional("UNITY_EDITOR")]
        public static void Error (object o1, object o2) => UnityEngine.Debug.LogError(Stringify(o1, o2));
        [Conditional("UNITY_EDITOR")]
        public static void Error (object o1, object o2, object o3)
            => UnityEngine.Debug.LogError(Stringify(o1, o2, o3));
        [Conditional("UNITY_EDITOR")]
        public static void Error (object o1, object o2, object o3, object o4)
            => UnityEngine.Debug.LogError(Stringify(o1, o2, o3, o4));
        [Conditional("UNITY_EDITOR")]
        public static void Error (object o1, object o2, object o3, object o4, object o5)
            => UnityEngine.Debug.LogError(Stringify(o1, o2, o3, o4, o5));
    }
}