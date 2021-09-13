using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Core.SaveLoad
{
    public static class Serializer
    {
        public static (Type, object) TrySerialize(object o) {

            Type type = o.GetType();
            Logging.Log("Serializing object of type", type.FullName);

            bool primitive = TrySerializePrimitive(type, o);
            if (primitive)
                return (null, null);

            bool array = TrySerializeArray(type, o);
            if (array)
                return (null, null);

            bool valueType = TrySerializeValueType(type, o);
            if (valueType)
                return (null, null);



            // Access default ctor
            ConstructorInfo ctor = type.GetConstructor(new Type[0]);
            if (ctor == null) {
                Logging.Error("No default ctor for object of type", type.FullName);
                return (null, null);
            }

            bool hasSaveLoad = Attribute.IsDefined(type, typeof(SaveLoadClassAttribute));
            if (!hasSaveLoad) {
                Logging.Log("No SaveLoadClass attribute for", type.FullName); // TODO can get rid of this eventually?
                return (null, null);
            }

            var fields = new HashSet<FieldInfo>();
            Bar(type, fields);
            // TODO worry about infinite recursion...
            foreach (var item in fields) {
                Logging.Log(item.Name);
                object value = item.GetValue(o);
                Logging.Log(value);
                TrySerialize(value);
            }

            return (type, null);
        }

        private static bool TrySerializePrimitive(Type type, object o) {
            if (!type.IsPrimitive)
                return false;

            Logging.Assert(o != null, "Null primitive");

            Logging.Log("Type is primitive", type.FullName);
            return true;
        }

        private static bool TrySerializeArray(Type type, object o) {
            if (!type.IsArray)
                return false;

            if (o == null) {
                Logging.Log("Serializing null array");
                return true;
            }

            Logging.Log("Type is array", type.FullName, type.GetArrayRank());
            int rank = type.GetArrayRank();
            int[] lengths = new int[rank];

            Array arr = (Array)o;
            for (int r = 0; r < rank; r++) {
                int length = arr.GetLength(r);
                lengths[r] = length;
            }
            Logging.Log(lengths);
            SerializeArrayElement(arr, lengths, 0, new int[rank]);

            return true;
        }

        private static readonly StringBuilder _sb = new StringBuilder();
        private static void SerializeArrayElement(Array array, int[] lengths, int offset, int[] indices) {
            if (indices == null) {
                Logging.Error("Null indices array");
                return;
            }

            int rank = array.Rank;
            int length = lengths[offset];

            for (int i = 0; i < length; i++) {
                indices[offset] = i;
                if (offset < rank - 1) {
                    SerializeArrayElement(array, lengths, offset + 1, indices);
                    continue;
                }

                _sb.Clear();
                for (int j = 0; j < rank; j++) {
                    _sb.Append(indices[j].ToString());
                    _sb.Append(",");
                }
                var value = array.GetValue(indices);
                TrySerialize(value);
                //Logging.Log(value, _sb.ToStringAndClear());
            }
        }

        private static bool TrySerializeValueType(Type type, object o) {
            return false;
        }
        

        private static void Bar(Type type, HashSet<FieldInfo> fields) {
            var saveLoadClass = type.GetCustomAttribute<SaveLoadClassAttribute>();
            if (saveLoadClass == null) {
                Logging.Error("No SaveLoadClassAttribute defined for", type.FullName);
                return;
            }

            if (!saveLoadClass.Any()) {
                Logging.Log("No fields to serialize for", type.FullName);
                return; // TODO we should still serialize the thing itself? just return an empty list
            }

            //bool shouldPublic = saveLoadClass.AllPublic();
            //if (shouldPublic)
            //    GetFields(type, BindingFlags.Public);

            //bool shouldPrivate = saveLoadClass.AllPrivate();
            //if (shouldPrivate)
            //    GetFields(type, BindingFlags.NonPublic);

            GetExplicitFields(type, saveLoadClass, fields);
        }

        private static void GetFields(Type type, BindingFlags flags) {
        }

        private static void GetExplicitFields(Type type, SaveLoadClassAttribute saveLoadClass, HashSet<FieldInfo> fieldsSet) {
            bool shouldExplicit = saveLoadClass.AnyExplicit();
            if (!shouldExplicit)
                return;

            bool shouldPublic = saveLoadClass.AllPublic();
            bool shouldPrivate = saveLoadClass.AllPrivate();
            // TODO protected?

            BindingFlags flags = BindingFlags.Instance;

            if (!shouldPublic)
                flags |= BindingFlags.Public;

            if (!shouldPrivate)
                flags |= BindingFlags.NonPublic;

            var fields = type.GetFields(flags);
            foreach (var field in fields) {
                var saveLoad = field.GetCustomAttribute<SaveLoadAttribute>();
                bool skip = saveLoad == null || (shouldPublic && field.IsPublic) || (shouldPrivate && field.IsPrivate);
                if (skip)
                    continue;

                Logging.Log("Should serialize field", field.Name);
                fieldsSet.Add(field);
            }
        }
    }
}
