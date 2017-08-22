/* oOo * 11/19/2007 : 8:00 AM */
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace System
{
	/// <summary>
	/// an overridden Dictionary just in case I'd like additional custom methods, etc...
	/// </summary>
	public class DICT<TKey,TValue> : Dictionary<TKey,TValue>, IDisposable
	{
		public TKey[] KeyArray { get { return ToKeyArray(); } }
		public object GetKey(TValue value)
		{
			if (!ContainsValue(value)) return null;
			foreach (TKey key in KeyArray)
			{
				if (this[key].Equals(value)) return key;
			}
			return null;
		}
		public struct DictNode
		{
			public TKey		Key;
			public TValue	Value;
			public DictNode(TKey key, TValue val) { Key = key; Value=val; }
		}

		public DICT() : base() {}
		public DICT(IDictionary<TKey,TValue> d) : base(d) {}
		public DICT(params DictNode[] nodes) : base() { if (nodes==null) return; Create(nodes); }

		virtual public DICT<TKey,TValue>.DictNode[] ToDictNodeArray() { return GetDictNodes<TKey,TValue>(this); }
		
		virtual public TKey[] ToKeyArray()
		{
			TKey[] array = new TKey[Count]; int i=0;
			foreach(TKey key in Keys) { array[i]=key; i++; }
			return array;
		}
		virtual public TValue[] ToValueArray()
		{
			TKey[] keys = ToKeyArray();
			TValue[] array = new TValue[Count];
			for (int i=0; i< Count; i++) array[i]=this[keys[i]]; keys = null;
			return array;
		}

		virtual public void Add(DICT<TKey,TValue>.DictNode item) { this.Add(item.Key,item.Value); }
		virtual public void AddRange(params DICT<TKey,TValue>.DictNode[] item) { foreach (DICT<TKey,TValue>.DictNode node in item) this.Add(node); }

		static public DICT<TKey,TValue>.DictNode CreateNode(TKey key,TValue val) { return new DICT<TKey, TValue>.DictNode(key,val); }
		static public DICT<K,V>.DictNode[] GetDictNodes<K,V>(DICT<K,V> dict)
		{
			K[] keys = dict.ToKeyArray(); DICT<K,V>.DictNode[] array = new DICT<K,V>.DictNode[dict.Count];
			for (int i=0; i<dict.Count; i++) array[i] = new DICT<K,V>.DictNode(keys[i],dict[keys[i]]); keys = null;
			return array;
		}

		/// <summary>pertinant perhaps to an overriding DICT class defining TKey & TValue.</summary>
		static public DICT<TKey,TValue> Empty { get { return new DICT<TKey,TValue>(); } }
		static public DICT<TKey,TValue> Create(params DictNode[] nodes) { return Create<TKey,TValue>(nodes); }
		static public DICT<T,V> Create<T,V>(params DICT<T,V>.DictNode[] nodes)
		{
			DICT<T,V> dict = new DICT<T, V>();
			if (nodes==null) return dict;
			foreach (DICT<T,V>.DictNode node in nodes)
			{
				if (!dict.ContainsKey(node.Key)) dict.Add(node.Key,node.Value);
				else throw new ArgumentException("Dictonaries do not accept duplicate 'Keys'!");
			}
			return dict;
		}
		public void Dispose() { Clear(); GC.SuppressFinalize(this); }
	}
	/// <summary>
	/// This is a dictionary class where each item contains a List of values.
	/// </summary>
	[SerializableAttribute]
	public sealed class DICT_List<TKey,TValue> : DICT<TKey,List<TValue>>
	{
		const string
			xception   = "DICT Usage Error",
		key_exists = "Argument:\n'{0}'\n... Has allready been added to the DICT";
		void ErrorMsg(object parameter) { MessageBox.Show(string.Format(key_exists,parameter),xception); }
		public void AddV(TKey key, params TValue[] value)
		{
			if (!ContainsKey(key)) CreateKey(key);
			this[key].AddRange(value);
		}
		public bool CreateKey(TKey key)
		{
			if (ContainsKey(key)) { ErrorMsg(key); return false; }
			Add(key,new List<TValue>());
			return true;
		}
		public void TryAddKeys(params TKey[] keys)
		{
			foreach (TKey val in keys)
			{
				if (!CreateKey(val)) ErrorMsg(val);
			}
		}
		public DICT_List() : base() {}
		public DICT_List(TKey id,TValue[] value) : base() {}
		public DICT_List(IDictionary<TKey,List<TValue>> d) : base(d) {}
	}
}
