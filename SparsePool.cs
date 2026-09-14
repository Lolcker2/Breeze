namespace Breeze
{
    /*
        SparseRef is a data-class that acts as a refrence for
        sparsePools kind of like how ints act as references or indecies for arrays.
    */
    public class SparseRef
    {
        public int index;
        public int generation;

        public SparseRef() { }

        public SparseRef(int _index)
        {
            index = _index;
            generation = 0;
        }

        public SparseRef(int _index, int _gen)
        {
            index = _index;
            generation = _gen;
        }

        public override string ToString()
        {
            return $"SparseRef: Index-{index}, Generation-{generation}";
        }
    }

    public class SparsePool<T>
    {
        private List<T> values;
        private List<int> generations;
        private Stack<int> freeSlots;

        public SparsePool()
        {
            values = new List<T>();
            generations = new List<int>();
            freeSlots = new Stack<int>();
        }

        public int Length()
        {
            return values.Count;
        }

        public void Update(SparseRef _ref, T _item)
        {
            values[_ref.index] = _item;
        }

        /*
            given an item of type T, inserts it into the sparsePool,
            and returing a reference to it.
        */
        public SparseRef Add(T _item)
        {
            if (freeSlots.Count > 0)
            {
                int index = freeSlots.Pop();
                values[index] = _item;
                return new SparseRef(index, generations[index]);
            }
            else
            {
                int index = values.Count;
                values.Add(_item);
                generations.Add(0);
                return new SparseRef(index, 0);
            }
        }

        /*
            given a reference to some item within the sparsePool,
            this method removes it.
        */
        public void Remove(SparseRef _ref)
        {
            values[_ref.index] = default!;
            generations[_ref.index]++;
            freeSlots.Push(_ref.index);
        }

        /*
            given a reference to some item within the sparsePool,
            this method returns it.
        */
        private T? GetValue(SparseRef _ref)
        {
            // the specific cell got reborn
            if (generations[_ref.index] != _ref.generation)
            {
                return default;
            }
            return values[_ref.index];
        }

        public T? this[SparseRef _ref]
        {
            get => GetValue(_ref);
        }
    }
}
