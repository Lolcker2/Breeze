using System.Reflection;
using YamlDotNet.Serialization;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.

namespace Breeze
{
    static class Utils
    {
        static IDeserializer deserializer = new DeserializerBuilder()
            .IncludeNonPublicProperties()
            .IgnoreUnmatchedProperties()
            .Build();
        static ISerializer serializer = new SerializerBuilder().Build();

        // a relative path distance to the 'Breeze' folder
        public static string basedir = "../../../";

        static void Anchor(object _instance, FieldInfo _field, object _value)
        {
            _field.SetValue(_instance, _value);
        }

        public static void CreateAnchor(object _instance, KeyValuePair<object, FieldInfo> _field)
        {
            // checking prefix assuming correct standards
            string[] segments = ((string)_field.Key).Split('.');
            string prefix = segments[0];
            //... checking the prefix ...

            //removing the prefix
            segments[0] = "";
            string name = String.Join(".", segments).Substring(1);

            Action<object> anchor = (_value) => Anchor(_instance, _field.Value, _value);
            World.Entities.Anchors.Add(name, anchor);
            Console.WriteLine($"anchor: {name}");
        }

        /*
            local method using a genetic typing that takes a dictionary and returns
            an object of the given generic type with its properties initialized based
            on the dictionary.
        */
        private static T? _CreateFromDict<T>(IDictionary<object, object> _dict)
        {
            // looking for fields that require a naming reference (components)
            List<KeyValuePair<object, FieldInfo>> AnchorFields = new();
            foreach (FieldInfo _field in typeof(T).GetFields())
            {
                // make sure to not treat selfRef as an anchored field
                if (_field.FieldType.IsSubclassOf(typeof(Component)) && _field.Name != "selfRef")
                {
                    // revoming them from the dictionary and queueing
                    // the field for anchoring
                    AnchorFields.Add(
                        new KeyValuePair<object, FieldInfo>(_dict[_field.Name], _field)
                    );
                    _dict.Remove(_field.Name);
                }
            }

            // translating the rest of the object
            string? _yml = ToYaml(_dict);
            T result = FromYaml<T>(_yml);

            // creating anchors for each field within AnchorFields
            foreach (KeyValuePair<object, FieldInfo> _field in AnchorFields)
            {
                CreateAnchor(result, _field);
            }

            return result;
        }

        /*
            the global version of the method '_CreateFromDict', using reflections to make the
            generic typing needed by _CreateFromDict to work with runtime types.
        */
        public static object? CreateFromDict(Type _type, IDictionary<object, object> _dict)
        {
            // fetching the MethodInfo
            MethodInfo method = typeof(Utils).GetMethod(
                nameof(_CreateFromDict),
                BindingFlags.NonPublic | BindingFlags.Static
            );

            // translating and invoking the method with the given type
            MethodInfo generic = method.MakeGenericMethod(_type);
            return generic.Invoke(null, [_dict]);
        }

        /*
            a method for loading a GameObject from the path of the YAML file.
            returns a tuple containing both the GameObject itself, and a refernce to it.
        */

        // need to add a name
        public static GameObject? LoadGameObject(string _path)
        {
            // put the name later on
            string[] filename = _path.Split('\\');
            filename[0] = "";
            filename[filename.Length - 1] = "";
            string name = String.Join(".", filename);
            name = name.Substring(1, name.Length - 2);

            Console.WriteLine($"name: {name}");

            // load the yaml into a dictionary
            IDictionary<string, object>? _yaml = fFromYaml<IDictionary<string, object>>(_path);

            // get the GameObject component
            IDictionary<object, object> game_object =
                (IDictionary<object, object>)_yaml["GameObject"];
            _yaml.Remove("GameObject");
            List<Component> components = new List<Component>();

            // loops through each component, loading and initializing it
            foreach (KeyValuePair<string, object> _pair in _yaml)
            {
                Type? obj_type = GetType(_pair.Key);
                IDictionary<object, object> dict = (IDictionary<object, object>)_pair.Value;
                Component comp = (Component)CreateFromDict(obj_type, dict);
                components.Add(comp);
            }

            //finally creating the GameObject

            GameObject loaded = _CreateFromDict<GameObject>(game_object);
            if(loaded == null)
            {
                Console.WriteLine("nullllllllllllllll");
            }
            List<SparseRef> componentRefs = components.Select(World.Entities.Insert).ToList();
            loaded.components = componentRefs;
            loaded.selfRef = World.Entities.Insert(loaded); // set the self reference
            World.Entities.Update(loaded.selfRef, loaded); // update the self reference

            // updating the EntityManager and returning it
            // add a param to inseret containing the name

            // handle anchors, check if the recently loaded object is the end of an achor,
            // if so, use the action

            // what if the object was loaded beforehand?
            // if (World.Entities.Anchors.Contents.ContainsKey(name))
            // {
            //     Console.WriteLine($"contains: {World.Entities.Anchors.Contents.ContainsKey(name)}");
            //     foreach (Action<object> _callback in World.Entities.Anchors[name])
            //     {
            //         Console.WriteLine($"resolving anchor: {name}");
            //         _callback.Invoke(loaded);
            //     }
            // }

            return loaded;
        }

        /*
            returns the type present in a given string
            for example, the string "System.int" would return the type int.
        */
        public static Type? GetType(string _type)
        {
            Type foundType = AppDomain
                .CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.Name == _type);

            return foundType;
        }

        /*
            serializes a given object into a yaml formatted string.
        */
        public static string? ToYaml(object _obj)
        {
            try
            {
                return serializer.Serialize(_obj);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception while serializing {0}: {1}", _obj.GetType(), e);
                return null;
            }
        }

        /*
            serializes a given object into a yaml formatted string
            written into the file at the specified path.
        */
        public static int fToYaml(object _obj, string _path)
        {
            try
            {
                File.WriteAllText(_path, ToYaml(_obj));
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception while writing to file {0}: {1}", _path, e);
                return 1;
            }
        }

        /*
            deserializes a given  yaml formatted string into an object
            of the given generic type.
        */
        public static T? FromYaml<T>(string _yml)
        {
            try
            {
                return deserializer.Deserialize<T>(_yml);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception while deserializing object {0}, {1}", _yml, e);
                return default;
            }
        }

        /*
            deserializes a given yaml formatted string from the file at the specified
            path into an object of the given generic type.
        */
        public static T? fFromYaml<T>(string _path)
        {
            try
            {
                return FromYaml<T>(File.ReadAllText(_path));
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception while reading from file {0}: {1}", _path, e);
                return default;
            }
        }
    }
}
