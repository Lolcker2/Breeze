#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace Breeze
{
    public class HashMap<TKey, TValue>
    {
        public Dictionary<TKey, List<TValue>> Contents;

        public HashMap()
        {
            Contents = new();
        }

        public List<TValue> this[TKey _key]
        {
            get { return Contents[_key]; }
        }

        public void Add(TKey _key, TValue _value)
        {
            Console.WriteLine($"Key: {_key}, Value: {_value}");
            if (Contents.ContainsKey(_key))
            {
                Contents[_key].Add(_value);
                return;
            }
            Contents.Add(_key, new List<TValue> { _value });
        }
    }

    /*
        Entity is a data-class, including no logic storing references to
        the components within their respective cache.
    */
    public class Entity
    {
        public string name;
        public List<SparseRef> components;
        public SparseRef? parent;
        public List<SparseRef> children;

        public Entity()
        {
            components = new List<SparseRef>();
            children = new List<SparseRef>();
        }

        public Entity(string _name, List<SparseRef> _components, List<SparseRef> _childs)
        {
            name = _name;
            components = _components;
            children = _childs;
        }

        public Entity(
            string _name,
            List<SparseRef> _components,
            List<SparseRef> _childs,
            SparseRef _parent
        )
        {
            name = _name;
            components = _components;
            children = _childs;
            parent = _parent;
        }
    }

    public class EntityManager
    {
        private SparsePool<Component> componentCache;
        public SparsePool<Entity> entityCache;
        public HashMap<string, Action<object>> Anchors;

        public EntityManager()
        {
            componentCache = new SparsePool<Component>();
            entityCache = new SparsePool<Entity>();
            Anchors = new HashMap<string, Action<object>>();
        }

        /*
            given a component subclass, returns whether is a GameObject
        */
        private bool IsGameObject(Component _comp)
        {
            object obj = _comp;
            if (obj.GetType() == typeof(GameObject))
            {
                return true;
            }
            return false;
        }

        /*
            given a reference to a component, returns the component itself.
        */
        public Component? GetComponent(SparseRef _comp)
        {
            return componentCache[_comp];
        }

        /*
            given a reference to a GameObject, returns the GameObject itself.
        */
        public GameObject? GetGameObject(SparseRef _gameObject)
        {
            return GameObject.ToGameObject(entityCache[_gameObject]);
        }

        /*
            a private and local method for inserting components into
            their respective cache, returning a reference to it.
        */
        private SparseRef _Insert(Component _comp)
        {
            return componentCache.Add(_comp);
        }

        /*
            a private and local method for inserting GameObjects into
            their respective cache, returning a reference to it.
        */
        private SparseRef _Insert(GameObject _gameObj)
        {
            return entityCache.Add(_gameObj.ToEntity());
        }

        /*
            the public method for inserting a component subclass into the EntityManager;
            if the given component is a GameObject, the method inserts it into the entityCache,
            and if it is not, the method inserts it into the componentCache.
        */
        public SparseRef Insert(Component _comp)
        {
            if (IsGameObject(_comp))
            {
                return _Insert((dynamic)_comp);
            }
            return _Insert(_comp);
        }

        // add remove

        /*
            a private and local method for updating components from
            their respective cache.
        */
        private void _Update(SparseRef _ref, Component _comp)
        {
            componentCache.Update(_ref, _comp);
        }

        /*
            a private and local method for updating GameObjects from
            their respective cache.
        */
        private void _Update(SparseRef _ref, GameObject _gameObj)
        {
            entityCache.Update(_ref, _gameObj.ToEntity());
        }

        /*
            the public method for updating a component subclass from the EntityManager;
            if the given component is a GameObject, the method updates it from the entityCache,
            and if it is not, the method updates it from the componentCache.
        */
        public void Update(SparseRef _ref, Component _comp)
        {
            if (IsGameObject(_comp))
            {
                _Update(_ref, (dynamic)_comp);
                return;
            }
            _Update(_ref, _comp);
        }


        public void ResolveAnchors()
        {
            Console.WriteLine("not yet implemented");
            // if (World.Entities.Anchors.Contents.ContainsKey(name))
            // {
            //     Console.WriteLine($"contains: {World.Entities.Anchors.Contents.ContainsKey(name)}");
            //     foreach (Action<object> _callback in World.Entities.Anchors[name])
            //     {
            //         Console.WriteLine($"resolving anchor: {name}");
            //         _callback.Invoke(loaded);
            //     }
            // }
        }
    }
}
