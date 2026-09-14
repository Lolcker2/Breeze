## `SparseRef` class
Acts as an index for `SparsePool<T>`.

### Constructors
|`SparseRef() `| Initializes a new instance of the `SparseRef` class with its default configurations. |
| ------------- |:-------------:|
|`SparseRef(int _index)`| Initializes a new instance of the `SparseRef` class with the given `_index`, generation is set to its default value.|
|`SparseRef(int _index, int _gen)`| Initializes a new instance of the `SparseRef` class with both the given`_index` and `_gen` set.|


### Properties
|`int index`|the ordinal number that refers to the cell at which the referred element sits.|
| ------------- |:-------------:|
|`int generation`|the number of time the cell at place index has been reset|

<br><br><br>

## `SparsePool<T>` class
A list-like iterable data-structure that uses `SparseRef` as an index.
the data-structure itself reuses any empty spaces that sit in the mDiddle of the list allowing for a `SparseRef` to stay stationaty all while allowing for size changes.

### Constructors
|`SparsePool() `| Initializes a new instance of the `SparsePool<T>` class that is empty and has the default initial capacity. |
| ------------- |:-------------:|



### Properties
|`List<T> values`|A list storing all the actual values the SparsePool stores.|
| ------------- |:-------------:|
|`List<int> generations`|A list initialized to all zeros storing the generation of each cell within `values`|
|`Stack<int> freeSlots`|A stack containing the indecies of all deleted cells within `values`|


### Methods
|`int Length()`|Returns the length of the `values` list.|
| ------------- |:-------------:|
|`SparseRef Add(T _item)`|Adds `_item` into the the `values` list, returning a `SparseRef` that refers to the newly added cell.|
|`void Remove(SparseRef _ref)`|Removes the item refered by `_ref`, defaulting it and incrementing the generation of this cell rendering any previous `SparseRef` that refers to this cell invalid.|
|`void Update(SparseRef _ref, T _item)`|Sets the value refered by `_ref` to `_item`|
|`T? GetValue(SparseRef _ref)`|Returns the value refered by `_ref`.|
|`T? SparsePool[SparseRef _ref]`|Returns the value refered by `_ref`.|


<br><br><br>

## `Component` class
A base class that all components inherit from.


### Constructors
|`Component()`|Initializes an empty component with all of its properties defaulted.|
| ------------- |:-------------:|


### Properties
|`SparseRef selfRef`|A `SparseRef` that refers to itself for easy update and lookyp.|
| ------------- |:-------------:|


### Methods
|`void Fetch()`|Looks up and updates all `Component` subclass properties.|
| ------------- |:-------------:|
|`virtual void Start()`|A Method thats called at the begining of the execution of the program.|
|`virtual void Update()`|A Method thats called each frame within the execution of the program.|
|`virtual void Render()`|A Method thats called right before `Update`, works like unity's `FixedUpdate`.|



<br><br><br>

## `GameObject: Component` class
A base class representing game objects, inehirts from the `Component` class.


### Constructors
|||
| ------------- |:-------------:|
|||

### Properties
|` List<SparseRef> components`|A `List<SparseRef>` of referenced to all components of this `GameObject`.|
| ------------- |:-------------:|
|`SparseRef? parent`|A `SparseRef` refering to the parent of this `GameObject`|
|`List<SparseRef> children`|A `List<SparseRef>` of referenced to all children of this `GameObject`. |

        
### Methods
|`T? GetComponent<T>()`| A method returning the `Component` which its type is specified as `<T>`.|
| ------------- |:-------------:|
|`bool IsChild()`|Returns whether this `GameObject` has a parent `GameObject`.|
|`static GameObject ToGameObject(Entity _entity)`|A method that converts `_entity` into a `GameObject` and returns it.|
|`Entity ToEntity()`|converts this `GameObject` into an `Entity` class and returns it.|
|`override void Start()`|Overrides the `Start` method of the `Component` class, thats called at the begining of the execution of the program, and invokes `Start` for each of the components this `GameObject` has.|
|`override void Update()`|Overrides the `Update` method of the `Component` class, thats called each frame within the execution of the program., and invokes `Update` for each of the components this `GameObject` has.|
|`override void Render()`|Overrides the `Render` method of the `Component` class, thats called right before `Update`, works like unity's `FixedUpdate`.Also invokes `Render` for each of the components this `GameObject` has.|



<br><br><br>

## `HashMap<TKey, TValue>` class
A class for implementing `Hashmaps` without using a `Hash` function.

### Constructors
|`HashMap()`| Initializes a new instance of the `HashMap<TKey, TValue>` class that is empty and has the default initial capacity.|
| ------------- |:-------------:|


### Properties
|`Dictionary<TKey, List<TValue>> Contents`|A `dictionary` storing `<TKey, List<TValue>>` as key-value pairs.|
| ------------- |:-------------:|

### Methods
|`void Add(TKey _key, TValue _value)`|Adds `_value` under the key `_key` as a `List<TValue>` element.|
| ------------- |:-------------:|
|`List<TValue> Hashmap[TKey _key]`|Returns the `List<TValue>` paired with the given `_key`.|



<br><br><br>

## `Entity` class
...

### Constructors
|||
| ------------- |:-------------:|
|||

### Properties
|||
| ------------- |:-------------:|
|||

### Methods
|||
| ------------- |:-------------:|
|||



<br><br><br>

## `EntityManager` class
A class for managing all components, entities, and runtime memory for the program.


### Constructors
|`public EntityManager()`|An empty constructor that initialized the `EntityManager` class to its default values and sizes.|
| ------------- |:-------------:|


### Properties
|`SparsePool<Component> componentCache`| A `SparsePool` for storing all `components` within the runtime of the program.|
| ------------- |:-------------:|
|`SparsePool<Entity> entityCache`| A `SparsePool` for storing all `entities` within the runtime of the program.|
|`HashMap<string, Action<object>> Anchors`|A `HashMap` to store all `"anchors"`, those are reference assigning callbacks that allow `components` to store another `component` as immediate access memory.|



### Methods
|`Component? GetComponent(SparseRef _comp)`|Given a reference to a `component`, returns the referenced `component`.|
| ------------- |:-------------:|
|`GameObject? GetGameObject(SparseRef _gameObject)`|Given a reference to a `gameobject`, returns the referenced `gameobject`.|
|`SparseRef Insert(Component _comp)`|Inserts a `Component Subclass` into the EntityManager, and returns a reference to it. if the given `component` is a `GameObject`, the method inserts it into the `entityCache`, and if it is not, the method inserts it into the `componentCache`.|
|`void Update(SparseRef _ref, Component _comp)`|Updates a `Component Subclass` from the EntityManager, given a reference to it. if the given `component` is a `GameObject`, the method updates it from the `entityCache`, and if it is not, the method updates it from the `componentCache`.|
|remove?|remove?|




<br><br><br>

## `Component` class
A base class that all components inherit from.


### Constructors
|||
| ------------- |:-------------:|
|||

### Properties
|||
| ------------- |:-------------:|
|||

### Methods
|||
| ------------- |:-------------:|
|||


