using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

namespace Lexicons
{
    public interface ILexicon<K,V> : ICollection<KeyValuePair<K,V>>, IEnumerable<KeyValuePair<K, V>>, IEnumerable
    {
        // Properties

        int KeyCount { get; }
        ICollection<K> Keys { get; }
        ICollection<List<V>> ValueLists { get; }
        IEnumerable<V> Values { get; } 
        List<V> this[K key] { get; set; }

        // Methods
        
        void Add(K key, V value);
        void AddList(K key, List<V> valueList);
        bool ChangeValue(K key, V oldvalue, V newValue);
        bool Contains (K key, V value); 
        bool ContainsKey(K key);       
        int GetValueCount(K key);
        bool Remove(K key, V value);
        bool RemoveKey(K key);
        bool TryGetValueList(K key, out List<V> valueList);        
    }

    [Serializable]
    [ComVisible(false)]
    public class Lexicon<K, V> : IEnumerable<KeyValuePair<K, V>>, ICollection<KeyValuePair<K, V>>, ILexicon<K,V>, IEnumerable, ICollection, ISerializable, IDeserializationCallback
    {
        // Fields

        private Dictionary<K,List<V>> dict;

        // Properties

        public IEqualityComparer<K> Comparer 
        { 
            get { return dict.Comparer; }                
        }

        public int Count
        {
            get
            {
                int count = 0;
                foreach (List<V> list in dict.Values)
                {
                    count += list.Count;
                }
                return count;
            }
        }

        public int KeyCount
        {
           
            get { return dict.Keys.Count; }

        }
           
        public ICollection<K> Keys
        {
            get { return dict.Keys; }
        }

        public ICollection<List<V>> ValueLists
        {
            get { return dict.Values; }
        }

        public IEnumerable<V> Values
        {
            get
            {
                foreach (K key in dict.Keys)
                {
                    foreach (V value in dict[key])
                    {
                        yield return value;
                    }
                }
            }
        } 

        public List<V> this[K key]
        {
            get { return dict[key]; }
            set { dict[key] = new List<V>(value); }
        }
             

        public V this[K key, int index]
        {
            get
            {
                List<V> list = dict[key];
                if (index < 0 || index >= list.Count)
                {
                    throw new ArgumentException("Index out of range for key");
                }
                return list[index];
            }

            set
            {
                if (dict.ContainsKey(key))
                {                                    
                    List<V> list = dict[key];
                    
                    if (index < 0 || index > list.Count)
                    {
                        throw new ArgumentException("Index out of range for key");
                    }                    
                    else if (index == list.Count)
                    {
                        list.Add(value);
                    }
                    else
                    {
                        list[index] = value;
                    }
                }
                else if (index == 0)
                {
                    List<V> list = new List<V>();
                    list.Add(value);
                    dict.Add(key, list);
                }
                else
                {
                    throw new ArgumentException("Index out of range for key");
                }
            }
        }
       
        // Constructors

        public Lexicon()
        {
            dict = new Dictionary<K, List<V>>();
        }

        public Lexicon(IDictionary<K, V> dictionary)
        {
            dict = new Dictionary<K, List<V>>();
            foreach (K key in dictionary.Keys)
            {
                List<V> list = new List<V>();
                list.Add(dictionary[key]);
                dict.Add(key, list);
            }
        }

        public Lexicon(ILexicon<K, V> lexicon)
        {
            dict = new Dictionary<K, List<V>>();
            foreach (K key in lexicon.Keys)
            {
                dict.Add(key, new List<V>(lexicon[key]));
            }
        }

        public Lexicon(IEqualityComparer<K> comparer)
        {
            dict = new Dictionary<K, List<V>>(comparer);
        }

        public Lexicon(int capacity)
        {
            dict = new Dictionary<K, List<V>>(capacity);
        }

        public Lexicon(IDictionary<K, V> dictionary, IEqualityComparer<K> comparer)
        {
            dict = new Dictionary<K, List<V>>(comparer);
            foreach (K key in dictionary.Keys)
            {
                List<V> list = new List<V>();
                list.Add(dictionary[key]);
                dict.Add(key, list);
            }
        }

        public Lexicon(ILexicon<K, V> lexicon, IEqualityComparer<K> comparer)
        {
            dict = new Dictionary<K, List<V>>(comparer);
            foreach (K key in lexicon.Keys)
            {
                dict.Add(key, new List<V>(lexicon[key]));
            }
        }

        public Lexicon(int capacity, IEqualityComparer<K> comparer)
        {
            dict = new Dictionary<K, List<V>>(capacity, comparer);
        }

        protected Lexicon(SerializationInfo info, StreamingContext context)
        {
            if (info == null) return;
            dict = (Dictionary<K, List<V>>)info.GetValue("InternalDictionary", typeof(Dictionary<K, List<V>>));
        }

        // Methods

        public void Add(K key, V value)
        {
            if (dict.ContainsKey(key))
            {
                List<V> list = dict[key];
                list.Add(value);
            }
            else
            {
                List<V> list = new List<V>();
                list.Add(value);
                dict.Add(key, list);
            }
        }

        public void Add(KeyValuePair<K, V> keyValuePair)
        {
            this.Add(keyValuePair.Key, keyValuePair.Value);
        }

        public void AddList(K key, List<V> valueList)
        {
            if(dict.ContainsKey(key))
            {
               List<V> list = dict[key];
               foreach(V val in valueList) list.Add(val);       
            }
            else
            { 
               dict.Add(key, new List<V>(valueList));
            }
        }

        public void AddRange(IEnumerable<KeyValuePair<K, V>> keyValuePairs)
        {
            foreach (KeyValuePair<K, V> kvp in keyValuePairs) this.Add(kvp.Key, kvp.Value);
        }

        public bool ChangeValue(K key, V oldValue, V newValue)
        {
            if (dict.ContainsKey(key))
            {
               List<V> list = dict[key];
            
               for (int i = 0; i < list.Count; i++)
               {
                  if (Object.Equals(list[i], oldValue))
                  {
                     list[i] = newValue;
                     return true;
                  }
               }
            }
            return false;
        }

        public bool ChangeValueAt(K key, int index, V newValue)
        {
           if (dict.ContainsKey(key))
           {                                    
               List<V> list = dict[key];
                    
               if (index < 0 || index >= list.Count)
               {
                  return false;
               }                    
               else
               {
                  list[index] = newValue;
                  return true;
               }
           }
           return false;
        }  

        public void Clear()
        {
            dict.Clear();
        }

        public bool Contains(K key, V value)
        {
            if (dict.ContainsKey(key))
            {
                List<V> list = dict[key];
                foreach (V val in list)
                {
                    if (Object.Equals(val, value)) return true;
                }
            }
            return false;
        }

        public bool Contains(KeyValuePair<K, V> keyValuePair)
        {
            return this.Contains(keyValuePair.Key, keyValuePair.Value);
        }

        public bool ContainsKey(K key)
        {
            return dict.ContainsKey(key);
        }

        public bool ContainsValue(V value)
        {
            K firstKey;
            return ContainsValue(value, out firstKey);
        }

        public bool ContainsValue(V value, out K firstKey)
        {
            foreach (K key in dict.Keys)
            {
                foreach (V val in dict[key])
                {
                    if (Object.Equals(val, value))
                    {
                        firstKey = key;       
                        return true;
                    }  
                }
            }
            firstKey = default(K); 
            return false;
        }
      
        public void CopyTo(KeyValuePair<K, V>[] array, int index)
        {
            if (array == null) throw new ArgumentNullException();
            if (index < 0) throw new ArgumentOutOfRangeException();
            if (array.Length < index + this.Count) throw new ArgumentException();
            int i = index;
            foreach (KeyValuePair<K, V> kvp in this)
            {
                array[i++] = kvp;
            }
        }

        public IEnumerable<KeyValuePair<K, int>> FindKeyIndexPairs(V value)
        {
            foreach (K key in dict.Keys)
            {
                List<V> list = dict[key];
                for (int i = 0; i < list.Count; i++)
                {
                    if (Object.Equals(list[i], value))
                    {
                       yield return new KeyValuePair<K, int>(key, i);
                    }
                }
            }
        }
               
        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            foreach (K key in dict.Keys)
            {
                List<V> list = dict[key];
                foreach (V value in list)
                {
                    yield return new KeyValuePair<K, V>(key, value);
                }
            }
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) return;  
            info.AddValue("InternalDictionary", dict);
        }

        public int GetValueCount(K key)
        {
            if (dict.ContainsKey(key)) return dict[key].Count;
            return 0;
        }

        public int IndexOfValue(K key, V value)
        {
            if (dict.ContainsKey(key))
            {
               List<V> list = dict[key];
            
               for (int i = 0; i < list.Count; i++)
               {
                  if (Object.Equals(list[i], value)) return i;
               }
            }
            return -1;
        }
      
        public virtual void OnDeserialization(object sender)
        {
            // nothing to do
        }

        public bool Remove(K key, V value)
        {
            int count = this.GetValueCount(key);
            if (count == 0) return false;
            for (int i = 0; i < count; i++)
            {
                V val = dict[key][i];
                if (Object.Equals(val, value))
                {
                    if (count == 1)
                    {
                       dict.Remove(key);
                    }
                    else  
                    { 
                       dict[key].RemoveAt(i);
                    }
                    return true;
                }
            }
            return false;
        }

        public bool Remove(KeyValuePair<K, V> keyValuePair)
        {
            return this.Remove(keyValuePair.Key, keyValuePair.Value);
        }

        public bool RemoveAt(K key, int index)
        {
            int count = this.GetValueCount(key);
            if (count == 0 || index < 0 || index >= count) return false;
            if (count == 1)
            {
                dict.Remove(key);
            }
            else
            {
                List<V> list = dict[key];
                list.RemoveAt(index);               
            }
            return true;
        }

        public bool RemoveKey(K key)
        {
            int count = this.GetValueCount(key);
            if (count > 0)
            {
                dict.Remove(key);
                return true;
            }
            return false;
        }

        public bool TryGetValueList(K key, out List<V> valueList)
        {
            return dict.TryGetValue(key, out valueList);
        }
               
        public bool TryGetValueAt(K key, int index, out V value)
        {
            if (dict.ContainsKey(key) && index >= 0 && index < dict[key].Count)
            {
                value = dict[key][index];
                return true;
            }
            else
            {
                value = default(V);
                return false;
            }
        }

        // Explicit Property Implementations

        bool ICollection<KeyValuePair<K, V>>.IsReadOnly 
        { 
            get { return false; } 
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get { return ((ICollection)dict).SyncRoot; }
        }

        // Explicit Method Implementations

        void ICollection.CopyTo(Array array, int index)
        {
            this.CopyTo((KeyValuePair<K, V>[]) array, index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            // create and populate a Lexicon object

            Lexicon<string, int> lex = new Lexicon<string, int>();
            lex.Add("Dave", 1);
            lex.Add("John", 2);
            lex.Add("Dave", 3);
            lex.Add("Stan", 4);
            lex.Add("Dave", 5);
            lex.Add(new KeyValuePair<string, int>("Fred", 6));

            // iterate through key/value pairs 

            Console.WriteLine("The lexicon initially contains the following key/value pairs\n");

            foreach (KeyValuePair<string, int> kvp in lex)
            {
                Console.WriteLine("{0} : {1}", kvp.Key, kvp.Value);
            }

            // add a new entry to the Lexicon
            lex["Alan"] = new List<int>{7};

            // add some more values for the new key
            lex.AddList("Alan", new List<int>{8, 9}); 

            // add another new entry
            lex.Add("Mary", 10);
            
            // iterate the Lexicon again, this time using the Keys collection
            
            Console.WriteLine("\nFollowing the addition of new entries, the lexicon now contains\n");

            foreach(string key in lex.Keys)
            {
               foreach(int value in lex[key])
               {
                  Console.WriteLine("{0} : {1}", key, value);
               }
            }
           
            Console.WriteLine("\nDave has {0} values", lex.GetValueCount("Dave"));
 
            lex.RemoveKey("Dave"); // remove key and all its values            
            lex.Remove("Alan", 8); // remove a single value
            lex.ChangeValue("Fred", 6, 5); // change a value

            // iterate the Lexicon again 

            Console.WriteLine("\nFollowing some removals and a change, the lexicon now contains\n"); 
            foreach (KeyValuePair<string, int> kvp in lex)
            {
                Console.WriteLine("{0} : {1}", kvp.Key, kvp.Value);
            }

            if (lex.Contains("Stan", 4))
            {
                Console.WriteLine("\nStan has a value of 4");
            }
             
            // create an array of key/value pairs and copy the Lexicon's contents to it

            KeyValuePair<string, int>[] kvpa  = new KeyValuePair<string, int>[lex.Count];  
            lex.CopyTo(kvpa, 0);

            Console.WriteLine("There are currently {0} key value pairs in the Lexicon", kvpa.Length);
            
            // try and get the value at index 1 for Alan 
 
            int val;
            bool b = lex.TryGetValueAt("Alan", 1, out val);
            if (b) Console.WriteLine("Alan has a value of {0} at index 1", val);

            // create a new dictionary

            Dictionary<string, int> dict = new Dictionary<string, int>();
            dict["Nora"] = 3; 
            dict["John"] = 4; // uses a key already in the Lexicon

            // create a new Lexicon from the Dictionary

            Lexicon<string, int> lex2 = new Lexicon<string, int>(dict);

            // add some more members to it

            lex2["Zena"] = new List<int>{11};
            lex2["Myra", 0] = 12;

            // merge with existing Lexicon

            lex.AddRange(lex2);
            
            lex.Remove(new KeyValuePair<string, int>("Stan", 4)); // effectively remove Stan
            lex.RemoveAt("Mary", 0); // effectively remove Mary

            // iterate the Lexicon again
            Console.WriteLine("\nFollowing a number of changes, the lexicon now contains\n");
            foreach (KeyValuePair<string, int> kvp in lex)
            {
                Console.WriteLine("{0} : {1}", kvp.Key, kvp.Value);
            }

            Console.WriteLine("\nNora has a value of 3 at index {0}", lex.IndexOfValue("Nora", 3));
            
            lex["Zena", 1] = 1; // add a new value for Zena

            if(lex.ContainsValue(12))
            {
               Console.WriteLine("The lexicon contains a value of 12");
            }
 
            string k;
            if(lex.ContainsValue(5, out k)) Console.Write("{0} had a value of 5 ", k);
            lex.ChangeValue(k, 5, 2);
            if(lex[k, 0] == 2) Console.WriteLine("but now has a value of 2", k); 
 
            Console.WriteLine("\nThe following key/index pairs have a value of 2\n"); 
            foreach(KeyValuePair<string, int> kip in lex.FindKeyIndexPairs(2))
            { 
                Console.WriteLine("Key : {0}  Value : 2  Index : {1}", kip.Key, kip.Value);
            }
            
            Console.ReadKey();
        }
    }
}
