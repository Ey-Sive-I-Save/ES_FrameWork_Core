using System.Collections.Generic;
public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	public static readonly IReadOnlyList<string> PatchedAOTAssemblyList = new List<string>
	{
		"DOTween.dll",
		"ESStand.dll",
		"FishNet.Runtime.dll",
		"Sirenix.Utilities.dll",
		"System.Core.dll",
		"System.dll",
		"Unity.InputSystem.dll",
		"Unity.RenderPipelines.Core.Runtime.dll",
		"UnityEngine.CoreModule.dll",
		"mscorlib.dll",
	};
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// DG.Tweening.Core.DOGetter<UnityEngine.Color>
	// DG.Tweening.Core.DOGetter<UnityEngine.Quaternion>
	// DG.Tweening.Core.DOGetter<UnityEngine.Vector2>
	// DG.Tweening.Core.DOGetter<UnityEngine.Vector3>
	// DG.Tweening.Core.DOGetter<float>
	// DG.Tweening.Core.DOGetter<int>
	// DG.Tweening.Core.DOGetter<object>
	// DG.Tweening.Core.DOSetter<UnityEngine.Color>
	// DG.Tweening.Core.DOSetter<UnityEngine.Quaternion>
	// DG.Tweening.Core.DOSetter<UnityEngine.Vector2>
	// DG.Tweening.Core.DOSetter<UnityEngine.Vector3>
	// DG.Tweening.Core.DOSetter<float>
	// DG.Tweening.Core.DOSetter<int>
	// DG.Tweening.Core.DOSetter<object>
	// ES.BaseKey<object>
	// ES.ESFactory_CustomFunction<object>
	// ES.ESReferAbstract<object>
	// ES.ESSimpleObjectPool<object>
	// ES.GlobalDataStand.GlobalDataSupportStand.<>c<object>
	// ES.GlobalDataStand.GlobalDataSupportStand<object>
	// ES.IFactory<object>
	// ES.IKey<object>
	// ES.IKeyGroup<object,object>
	// ES.IPointer<ES.BuffStatusTest,object,object,object>
	// ES.IPointer<UnityEngine.AnimatorStateInfo,object,object,object>
	// ES.IPointer<UnityEngine.Color,object,object,object>
	// ES.IPointer<UnityEngine.Quaternion,object,object,object>
	// ES.IPointer<UnityEngine.Vector2,object,object,object>
	// ES.IPointer<UnityEngine.Vector3,object,object,object>
	// ES.IPointer<byte,object,object,object>
	// ES.IPointer<float,object,object,object>
	// ES.IPointer<int,object,object,object>
	// ES.IPointer<object,object,object,object>
	// ES.IPointerChain<object>
	// ES.IPointerChainAny<object,object,object,object>
	// ES.IPointerForIPointer<object,object,object>
	// ES.IPointerOnlyBack<ES.BuffStatusTest>
	// ES.IPointerOnlyBack<UnityEngine.AnimatorStateInfo>
	// ES.IPointerOnlyBack<UnityEngine.Color>
	// ES.IPointerOnlyBack<UnityEngine.Quaternion>
	// ES.IPointerOnlyBack<UnityEngine.Vector2>
	// ES.IPointerOnlyBack<UnityEngine.Vector3>
	// ES.IPointerOnlyBack<byte>
	// ES.IPointerOnlyBack<float>
	// ES.IPointerOnlyBack<int>
	// ES.IPointerOnlyBack<object>
	// ES.IPointerOnlyBackIEnumerable<object>
	// ES.IPointerOnlyBackList<UnityEngine.Quaternion>
	// ES.IPointerOnlyBackList<UnityEngine.Vector2>
	// ES.IPointerOnlyBackList<UnityEngine.Vector3>
	// ES.IPointerOnlyBackList<byte>
	// ES.IPointerOnlyBackList<float>
	// ES.IPointerOnlyBackList<int>
	// ES.IPointerOnlyBackList<object>
	// ES.IPointerOnlyBackSingle<ES.BuffStatusTest>
	// ES.IPointerOnlyBackSingle<UnityEngine.Vector2>
	// ES.IPointerOnlyBackSingle<UnityEngine.Vector3>
	// ES.IPointerOnlyBackSingle<float>
	// ES.IPointerOnlyBackSingle<int>
	// ES.IPointerOnlyBackSingle<object>
	// ES.ISafeList<object>
	// ES.ISelectDic<object,object,object>
	// ES.IWithKey<object>
	// ES.Pointer.IPointerForString<object,object,object>
	// ES.Pool<object>
	// ES.SingletonAsMono<object>
	// ES.SingletonAsNormalClass<object>
	// ES.SingletonAsSeriMono<object>
	// ES.SoDataGroup<object>
	// ES.SoDataPack<object>
	// FishNet.Serializing.GenericReader<ES.IESTESET>
	// FishNet.Serializing.GenericReader<ES.aa>
	// FishNet.Serializing.GenericReader<object>
	// FishNet.Serializing.GenericWriter<ES.IESTESET>
	// FishNet.Serializing.GenericWriter<ES.aa>
	// FishNet.Serializing.GenericWriter<object>
	// System.Action<ES.ESTag>
	// System.Action<ES.es666.aaa>
	// System.Action<FishNet.Transporting.ClientConnectionStateArgs>
	// System.Action<FishNet.Transporting.ServerConnectionStateArgs>
	// System.Action<UnityEngine.Quaternion>
	// System.Action<UnityEngine.Vector2>
	// System.Action<UnityEngine.Vector3>
	// System.Action<byte,object>
	// System.Action<byte>
	// System.Action<float>
	// System.Action<int>
	// System.Action<object,ES.IESTESET>
	// System.Action<object,ES.aa>
	// System.Action<object,object,object>
	// System.Action<object,object>
	// System.Action<object>
	// System.ArraySegment.Enumerator<byte>
	// System.ArraySegment<byte>
	// System.Collections.Generic.ArraySortHelper<ES.ESTag>
	// System.Collections.Generic.ArraySortHelper<ES.es666.aaa>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.Quaternion>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.Vector2>
	// System.Collections.Generic.ArraySortHelper<UnityEngine.Vector3>
	// System.Collections.Generic.ArraySortHelper<byte>
	// System.Collections.Generic.ArraySortHelper<float>
	// System.Collections.Generic.ArraySortHelper<int>
	// System.Collections.Generic.ArraySortHelper<object>
	// System.Collections.Generic.Comparer<ES.ESTag>
	// System.Collections.Generic.Comparer<ES.es666.aaa>
	// System.Collections.Generic.Comparer<UnityEngine.Quaternion>
	// System.Collections.Generic.Comparer<UnityEngine.Vector2>
	// System.Collections.Generic.Comparer<UnityEngine.Vector3>
	// System.Collections.Generic.Comparer<byte>
	// System.Collections.Generic.Comparer<float>
	// System.Collections.Generic.Comparer<int>
	// System.Collections.Generic.Comparer<object>
	// System.Collections.Generic.ComparisonComparer<ES.ESTag>
	// System.Collections.Generic.ComparisonComparer<ES.es666.aaa>
	// System.Collections.Generic.ComparisonComparer<UnityEngine.Quaternion>
	// System.Collections.Generic.ComparisonComparer<UnityEngine.Vector2>
	// System.Collections.Generic.ComparisonComparer<UnityEngine.Vector3>
	// System.Collections.Generic.ComparisonComparer<byte>
	// System.Collections.Generic.ComparisonComparer<float>
	// System.Collections.Generic.ComparisonComparer<int>
	// System.Collections.Generic.ComparisonComparer<object>
	// System.Collections.Generic.Dictionary.Enumerator<ES.es666.aaa,int>
	// System.Collections.Generic.Dictionary.Enumerator<int,int>
	// System.Collections.Generic.Dictionary.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.Enumerator<object,UnityEngine.Color>
	// System.Collections.Generic.Dictionary.Enumerator<object,byte>
	// System.Collections.Generic.Dictionary.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<ES.es666.aaa,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,UnityEngine.Color>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,byte>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection<ES.es666.aaa,int>
	// System.Collections.Generic.Dictionary.KeyCollection<int,int>
	// System.Collections.Generic.Dictionary.KeyCollection<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection<object,UnityEngine.Color>
	// System.Collections.Generic.Dictionary.KeyCollection<object,byte>
	// System.Collections.Generic.Dictionary.KeyCollection<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<ES.es666.aaa,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,UnityEngine.Color>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,byte>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection<ES.es666.aaa,int>
	// System.Collections.Generic.Dictionary.ValueCollection<int,int>
	// System.Collections.Generic.Dictionary.ValueCollection<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection<object,UnityEngine.Color>
	// System.Collections.Generic.Dictionary.ValueCollection<object,byte>
	// System.Collections.Generic.Dictionary.ValueCollection<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection<object,object>
	// System.Collections.Generic.Dictionary<ES.es666.aaa,int>
	// System.Collections.Generic.Dictionary<int,int>
	// System.Collections.Generic.Dictionary<int,object>
	// System.Collections.Generic.Dictionary<object,UnityEngine.Color>
	// System.Collections.Generic.Dictionary<object,byte>
	// System.Collections.Generic.Dictionary<object,int>
	// System.Collections.Generic.Dictionary<object,object>
	// System.Collections.Generic.EqualityComparer<ES.es666.aaa>
	// System.Collections.Generic.EqualityComparer<UnityEngine.Color>
	// System.Collections.Generic.EqualityComparer<byte>
	// System.Collections.Generic.EqualityComparer<int>
	// System.Collections.Generic.EqualityComparer<object>
	// System.Collections.Generic.HashSet.Enumerator<object>
	// System.Collections.Generic.HashSet<object>
	// System.Collections.Generic.HashSetEqualityComparer<object>
	// System.Collections.Generic.ICollection<ES.ESTag>
	// System.Collections.Generic.ICollection<ES.es666.aaa>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<ES.es666.aaa,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,UnityEngine.Color>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,byte>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ICollection<UnityEngine.Quaternion>
	// System.Collections.Generic.ICollection<UnityEngine.Vector2>
	// System.Collections.Generic.ICollection<UnityEngine.Vector3>
	// System.Collections.Generic.ICollection<byte>
	// System.Collections.Generic.ICollection<float>
	// System.Collections.Generic.ICollection<int>
	// System.Collections.Generic.ICollection<object>
	// System.Collections.Generic.IComparer<ES.ESTag>
	// System.Collections.Generic.IComparer<ES.es666.aaa>
	// System.Collections.Generic.IComparer<UnityEngine.Quaternion>
	// System.Collections.Generic.IComparer<UnityEngine.Vector2>
	// System.Collections.Generic.IComparer<UnityEngine.Vector3>
	// System.Collections.Generic.IComparer<byte>
	// System.Collections.Generic.IComparer<float>
	// System.Collections.Generic.IComparer<int>
	// System.Collections.Generic.IComparer<object>
	// System.Collections.Generic.IDictionary<int,object>
	// System.Collections.Generic.IEnumerable<ES.ESTag>
	// System.Collections.Generic.IEnumerable<ES.es666.aaa>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<ES.es666.aaa,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,UnityEngine.Color>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,byte>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerable<UnityEngine.Quaternion>
	// System.Collections.Generic.IEnumerable<UnityEngine.Vector2>
	// System.Collections.Generic.IEnumerable<UnityEngine.Vector3>
	// System.Collections.Generic.IEnumerable<byte>
	// System.Collections.Generic.IEnumerable<float>
	// System.Collections.Generic.IEnumerable<int>
	// System.Collections.Generic.IEnumerable<object>
	// System.Collections.Generic.IEnumerable<ushort>
	// System.Collections.Generic.IEnumerator<ES.ESTag>
	// System.Collections.Generic.IEnumerator<ES.es666.aaa>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<ES.es666.aaa,int>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,int>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,UnityEngine.Color>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,byte>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,int>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerator<UnityEngine.Quaternion>
	// System.Collections.Generic.IEnumerator<UnityEngine.Vector2>
	// System.Collections.Generic.IEnumerator<UnityEngine.Vector3>
	// System.Collections.Generic.IEnumerator<byte>
	// System.Collections.Generic.IEnumerator<float>
	// System.Collections.Generic.IEnumerator<int>
	// System.Collections.Generic.IEnumerator<object>
	// System.Collections.Generic.IEnumerator<ushort>
	// System.Collections.Generic.IEqualityComparer<ES.es666.aaa>
	// System.Collections.Generic.IEqualityComparer<int>
	// System.Collections.Generic.IEqualityComparer<object>
	// System.Collections.Generic.IList<ES.ESTag>
	// System.Collections.Generic.IList<ES.es666.aaa>
	// System.Collections.Generic.IList<UnityEngine.Quaternion>
	// System.Collections.Generic.IList<UnityEngine.Vector2>
	// System.Collections.Generic.IList<UnityEngine.Vector3>
	// System.Collections.Generic.IList<byte>
	// System.Collections.Generic.IList<float>
	// System.Collections.Generic.IList<int>
	// System.Collections.Generic.IList<object>
	// System.Collections.Generic.KeyValuePair<ES.es666.aaa,int>
	// System.Collections.Generic.KeyValuePair<int,int>
	// System.Collections.Generic.KeyValuePair<int,object>
	// System.Collections.Generic.KeyValuePair<object,UnityEngine.Color>
	// System.Collections.Generic.KeyValuePair<object,byte>
	// System.Collections.Generic.KeyValuePair<object,int>
	// System.Collections.Generic.KeyValuePair<object,object>
	// System.Collections.Generic.List.Enumerator<ES.ESTag>
	// System.Collections.Generic.List.Enumerator<ES.es666.aaa>
	// System.Collections.Generic.List.Enumerator<UnityEngine.Quaternion>
	// System.Collections.Generic.List.Enumerator<UnityEngine.Vector2>
	// System.Collections.Generic.List.Enumerator<UnityEngine.Vector3>
	// System.Collections.Generic.List.Enumerator<byte>
	// System.Collections.Generic.List.Enumerator<float>
	// System.Collections.Generic.List.Enumerator<int>
	// System.Collections.Generic.List.Enumerator<object>
	// System.Collections.Generic.List<ES.ESTag>
	// System.Collections.Generic.List<ES.es666.aaa>
	// System.Collections.Generic.List<UnityEngine.Quaternion>
	// System.Collections.Generic.List<UnityEngine.Vector2>
	// System.Collections.Generic.List<UnityEngine.Vector3>
	// System.Collections.Generic.List<byte>
	// System.Collections.Generic.List<float>
	// System.Collections.Generic.List<int>
	// System.Collections.Generic.List<object>
	// System.Collections.Generic.ObjectComparer<ES.ESTag>
	// System.Collections.Generic.ObjectComparer<ES.es666.aaa>
	// System.Collections.Generic.ObjectComparer<UnityEngine.Quaternion>
	// System.Collections.Generic.ObjectComparer<UnityEngine.Vector2>
	// System.Collections.Generic.ObjectComparer<UnityEngine.Vector3>
	// System.Collections.Generic.ObjectComparer<byte>
	// System.Collections.Generic.ObjectComparer<float>
	// System.Collections.Generic.ObjectComparer<int>
	// System.Collections.Generic.ObjectComparer<object>
	// System.Collections.Generic.ObjectEqualityComparer<ES.es666.aaa>
	// System.Collections.Generic.ObjectEqualityComparer<UnityEngine.Color>
	// System.Collections.Generic.ObjectEqualityComparer<byte>
	// System.Collections.Generic.ObjectEqualityComparer<int>
	// System.Collections.Generic.ObjectEqualityComparer<object>
	// System.Collections.Generic.Queue.Enumerator<ES.ModuleStateMachine_CrashDodge.Applyable_CrashDodge>
	// System.Collections.Generic.Queue.Enumerator<UnityEngine.Vector3>
	// System.Collections.Generic.Queue.Enumerator<object>
	// System.Collections.Generic.Queue<ES.ModuleStateMachine_CrashDodge.Applyable_CrashDodge>
	// System.Collections.Generic.Queue<UnityEngine.Vector3>
	// System.Collections.Generic.Queue<object>
	// System.Collections.Generic.SortedList.Enumerator<int,object>
	// System.Collections.Generic.SortedList.KeyList<int,object>
	// System.Collections.Generic.SortedList.SortedListKeyEnumerator<int,object>
	// System.Collections.Generic.SortedList.SortedListValueEnumerator<int,object>
	// System.Collections.Generic.SortedList.ValueList<int,object>
	// System.Collections.Generic.SortedList<int,object>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass52_0<object>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass53_0<object>
	// System.Collections.Generic.SortedSet.<>c__DisplayClass85_0<object>
	// System.Collections.Generic.SortedSet.<Reverse>d__94<object>
	// System.Collections.Generic.SortedSet.Enumerator<object>
	// System.Collections.Generic.SortedSet.Node<object>
	// System.Collections.Generic.SortedSet.TreeSubSet.<>c__DisplayClass9_0<object>
	// System.Collections.Generic.SortedSet.TreeSubSet<object>
	// System.Collections.Generic.SortedSet<object>
	// System.Collections.Generic.SortedSetEqualityComparer<object>
	// System.Collections.Generic.Stack.Enumerator<object>
	// System.Collections.Generic.Stack<object>
	// System.Collections.Generic.TreeWalkPredicate<object>
	// System.Collections.ObjectModel.ReadOnlyCollection<ES.ESTag>
	// System.Collections.ObjectModel.ReadOnlyCollection<ES.es666.aaa>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.Quaternion>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.Vector2>
	// System.Collections.ObjectModel.ReadOnlyCollection<UnityEngine.Vector3>
	// System.Collections.ObjectModel.ReadOnlyCollection<byte>
	// System.Collections.ObjectModel.ReadOnlyCollection<float>
	// System.Collections.ObjectModel.ReadOnlyCollection<int>
	// System.Collections.ObjectModel.ReadOnlyCollection<object>
	// System.Comparison<ES.ESTag>
	// System.Comparison<ES.es666.aaa>
	// System.Comparison<UnityEngine.Quaternion>
	// System.Comparison<UnityEngine.Vector2>
	// System.Comparison<UnityEngine.Vector3>
	// System.Comparison<byte>
	// System.Comparison<float>
	// System.Comparison<int>
	// System.Comparison<object>
	// System.Func<System.Collections.Generic.KeyValuePair<object,object>,object>
	// System.Func<UnityEngine.Quaternion,byte>
	// System.Func<UnityEngine.Quaternion,float>
	// System.Func<UnityEngine.Vector2,byte>
	// System.Func<UnityEngine.Vector2,float>
	// System.Func<UnityEngine.Vector3,UnityEngine.Vector3,UnityEngine.Vector3,float>
	// System.Func<UnityEngine.Vector3,UnityEngine.Vector3,float>
	// System.Func<UnityEngine.Vector3,byte>
	// System.Func<UnityEngine.Vector3,float>
	// System.Func<byte,byte>
	// System.Func<byte,float>
	// System.Func<byte>
	// System.Func<float,byte>
	// System.Func<float,float>
	// System.Func<int,byte>
	// System.Func<int,float>
	// System.Func<object,ES.IESTESET>
	// System.Func<object,ES.aa>
	// System.Func<object,UnityEngine.Vector3>
	// System.Func<object,byte>
	// System.Func<object,float>
	// System.Func<object,object,float>
	// System.Func<object,object,object,float>
	// System.Func<object,object,object>
	// System.Func<object,object>
	// System.Func<object>
	// System.Func<ushort,byte>
	// System.IComparable<object>
	// System.Linq.Buffer<UnityEngine.Quaternion>
	// System.Linq.Buffer<UnityEngine.Vector2>
	// System.Linq.Buffer<UnityEngine.Vector3>
	// System.Linq.Buffer<byte>
	// System.Linq.Buffer<float>
	// System.Linq.Buffer<int>
	// System.Linq.Buffer<object>
	// System.Linq.Enumerable.<CastIterator>d__99<object>
	// System.Linq.Enumerable.<TakeIterator>d__25<UnityEngine.Quaternion>
	// System.Linq.Enumerable.<TakeIterator>d__25<UnityEngine.Vector2>
	// System.Linq.Enumerable.<TakeIterator>d__25<UnityEngine.Vector3>
	// System.Linq.Enumerable.<TakeIterator>d__25<byte>
	// System.Linq.Enumerable.<TakeIterator>d__25<float>
	// System.Linq.Enumerable.<TakeIterator>d__25<int>
	// System.Linq.Enumerable.<TakeIterator>d__25<object>
	// System.Linq.Enumerable.Iterator<UnityEngine.Quaternion>
	// System.Linq.Enumerable.Iterator<UnityEngine.Vector2>
	// System.Linq.Enumerable.Iterator<UnityEngine.Vector3>
	// System.Linq.Enumerable.Iterator<byte>
	// System.Linq.Enumerable.Iterator<float>
	// System.Linq.Enumerable.Iterator<int>
	// System.Linq.Enumerable.Iterator<object>
	// System.Linq.Enumerable.WhereArrayIterator<UnityEngine.Quaternion>
	// System.Linq.Enumerable.WhereArrayIterator<UnityEngine.Vector2>
	// System.Linq.Enumerable.WhereArrayIterator<UnityEngine.Vector3>
	// System.Linq.Enumerable.WhereArrayIterator<byte>
	// System.Linq.Enumerable.WhereArrayIterator<float>
	// System.Linq.Enumerable.WhereArrayIterator<int>
	// System.Linq.Enumerable.WhereArrayIterator<object>
	// System.Linq.Enumerable.WhereEnumerableIterator<UnityEngine.Quaternion>
	// System.Linq.Enumerable.WhereEnumerableIterator<UnityEngine.Vector2>
	// System.Linq.Enumerable.WhereEnumerableIterator<UnityEngine.Vector3>
	// System.Linq.Enumerable.WhereEnumerableIterator<byte>
	// System.Linq.Enumerable.WhereEnumerableIterator<float>
	// System.Linq.Enumerable.WhereEnumerableIterator<int>
	// System.Linq.Enumerable.WhereEnumerableIterator<object>
	// System.Linq.Enumerable.WhereListIterator<UnityEngine.Quaternion>
	// System.Linq.Enumerable.WhereListIterator<UnityEngine.Vector2>
	// System.Linq.Enumerable.WhereListIterator<UnityEngine.Vector3>
	// System.Linq.Enumerable.WhereListIterator<byte>
	// System.Linq.Enumerable.WhereListIterator<float>
	// System.Linq.Enumerable.WhereListIterator<int>
	// System.Linq.Enumerable.WhereListIterator<object>
	// System.Linq.EnumerableSorter<UnityEngine.Quaternion,float>
	// System.Linq.EnumerableSorter<UnityEngine.Quaternion>
	// System.Linq.EnumerableSorter<UnityEngine.Vector2,float>
	// System.Linq.EnumerableSorter<UnityEngine.Vector2>
	// System.Linq.EnumerableSorter<UnityEngine.Vector3,float>
	// System.Linq.EnumerableSorter<UnityEngine.Vector3>
	// System.Linq.EnumerableSorter<byte,float>
	// System.Linq.EnumerableSorter<byte>
	// System.Linq.EnumerableSorter<float,float>
	// System.Linq.EnumerableSorter<float>
	// System.Linq.EnumerableSorter<int,float>
	// System.Linq.EnumerableSorter<int>
	// System.Linq.EnumerableSorter<object,float>
	// System.Linq.EnumerableSorter<object>
	// System.Linq.OrderedEnumerable.<GetEnumerator>d__1<UnityEngine.Quaternion>
	// System.Linq.OrderedEnumerable.<GetEnumerator>d__1<UnityEngine.Vector2>
	// System.Linq.OrderedEnumerable.<GetEnumerator>d__1<UnityEngine.Vector3>
	// System.Linq.OrderedEnumerable.<GetEnumerator>d__1<byte>
	// System.Linq.OrderedEnumerable.<GetEnumerator>d__1<float>
	// System.Linq.OrderedEnumerable.<GetEnumerator>d__1<int>
	// System.Linq.OrderedEnumerable.<GetEnumerator>d__1<object>
	// System.Linq.OrderedEnumerable<UnityEngine.Quaternion,float>
	// System.Linq.OrderedEnumerable<UnityEngine.Quaternion>
	// System.Linq.OrderedEnumerable<UnityEngine.Vector2,float>
	// System.Linq.OrderedEnumerable<UnityEngine.Vector2>
	// System.Linq.OrderedEnumerable<UnityEngine.Vector3,float>
	// System.Linq.OrderedEnumerable<UnityEngine.Vector3>
	// System.Linq.OrderedEnumerable<byte,float>
	// System.Linq.OrderedEnumerable<byte>
	// System.Linq.OrderedEnumerable<float,float>
	// System.Linq.OrderedEnumerable<float>
	// System.Linq.OrderedEnumerable<int,float>
	// System.Linq.OrderedEnumerable<int>
	// System.Linq.OrderedEnumerable<object,float>
	// System.Linq.OrderedEnumerable<object>
	// System.Nullable<ES.BuffStatusTest>
	// System.Nullable<UnityEngine.Color>
	// System.Nullable<UnityEngine.Quaternion>
	// System.Nullable<UnityEngine.Vector3>
	// System.Nullable<byte>
	// System.Nullable<float>
	// System.Nullable<int>
	// System.Predicate<ES.ESTag>
	// System.Predicate<ES.es666.aaa>
	// System.Predicate<UnityEngine.Quaternion>
	// System.Predicate<UnityEngine.Vector2>
	// System.Predicate<UnityEngine.Vector3>
	// System.Predicate<byte>
	// System.Predicate<float>
	// System.Predicate<int>
	// System.Predicate<object>
	// System.ValueTuple<int,object>
	// UnityEngine.Events.UnityAction<UnityEngine.SceneManagement.Scene,int>
	// UnityEngine.Events.UnityAction<UnityEngine.SceneManagement.Scene>
	// UnityEngine.InputSystem.InputBindingComposite<UnityEngine.Vector2>
	// UnityEngine.InputSystem.InputBindingComposite<UnityEngine.Vector3>
	// UnityEngine.InputSystem.InputBindingComposite<float>
	// UnityEngine.InputSystem.InputControl<UnityEngine.Vector2>
	// UnityEngine.InputSystem.InputControl<UnityEngine.Vector3>
	// UnityEngine.InputSystem.InputControl<float>
	// UnityEngine.InputSystem.InputProcessor<UnityEngine.Vector2>
	// UnityEngine.InputSystem.InputProcessor<UnityEngine.Vector3>
	// UnityEngine.InputSystem.InputProcessor<float>
	// UnityEngine.InputSystem.Utilities.InlinedArray<object>
	// UnityEngine.Rendering.SerializedDictionary<object,object,object,object>
	// UnityEngine.Rendering.SerializedDictionary<object,object>
	// }}

	public void RefMethods()
	{
		// object DG.Tweening.TweenSettingsExtensions.From<object>(object)
		// object DG.Tweening.TweenSettingsExtensions.From<object>(object,bool,bool)
		// object DG.Tweening.TweenSettingsExtensions.OnComplete<object>(object,DG.Tweening.TweenCallback)
		// object DG.Tweening.TweenSettingsExtensions.OnKill<object>(object,DG.Tweening.TweenCallback)
		// object DG.Tweening.TweenSettingsExtensions.OnPause<object>(object,DG.Tweening.TweenCallback)
		// object DG.Tweening.TweenSettingsExtensions.OnPlay<object>(object,DG.Tweening.TweenCallback)
		// object DG.Tweening.TweenSettingsExtensions.OnRewind<object>(object,DG.Tweening.TweenCallback)
		// object DG.Tweening.TweenSettingsExtensions.OnStepComplete<object>(object,DG.Tweening.TweenCallback)
		// object DG.Tweening.TweenSettingsExtensions.OnUpdate<object>(object,DG.Tweening.TweenCallback)
		// object DG.Tweening.TweenSettingsExtensions.SetAutoKill<object>(object,bool)
		// object DG.Tweening.TweenSettingsExtensions.SetDelay<object>(object,float)
		// object DG.Tweening.TweenSettingsExtensions.SetEase<object>(object,DG.Tweening.Ease)
		// object DG.Tweening.TweenSettingsExtensions.SetEase<object>(object,UnityEngine.AnimationCurve)
		// object DG.Tweening.TweenSettingsExtensions.SetId<object>(object,object)
		// object DG.Tweening.TweenSettingsExtensions.SetLoops<object>(object,int,DG.Tweening.LoopType)
		// object DG.Tweening.TweenSettingsExtensions.SetRecyclable<object>(object,bool)
		// object DG.Tweening.TweenSettingsExtensions.SetRelative<object>(object)
		// object DG.Tweening.TweenSettingsExtensions.SetRelative<object>(object,bool)
		// object DG.Tweening.TweenSettingsExtensions.SetTarget<object>(object,object)
		// object DG.Tweening.TweenSettingsExtensions.SetUpdate<object>(object,bool)
		// FishNet.Serializing.PooledWriter FishNet.Broadcast.Helping.BroadcastsSerializers.WriteBroadcast<object>(FishNet.Managing.NetworkManager,FishNet.Serializing.PooledWriter,object,FishNet.Transporting.Channel&)
		// System.Void FishNet.Managing.Client.ClientManager.Broadcast<object>(object,FishNet.Transporting.Channel)
		// System.Void FishNet.Managing.Server.ServerManager.Broadcast<object>(object,bool,FishNet.Transporting.Channel)
		// System.Void FishNet.Serializing.Writer.Write<object>(object)
		// object Sirenix.Utilities.TypeExtensions.GetCustomAttribute<object>(System.Type)
		// object Sirenix.Utilities.TypeExtensions.GetCustomAttribute<object>(System.Type,bool)
		// object System.Activator.CreateInstance<object>()
		// object[] System.Array.Empty<object>()
		// bool System.Array.Exists<object>(object[],System.Predicate<object>)
		// int System.Array.FindIndex<object>(object[],System.Predicate<object>)
		// int System.Array.FindIndex<object>(object[],int,int,System.Predicate<object>)
		// int System.Array.IndexOf<object>(object[],object)
		// int System.Array.IndexOfImpl<object>(object[],object,int,int)
		// object System.Linq.Enumerable.Aggregate<object,object>(System.Collections.Generic.IEnumerable<object>,object,System.Func<object,object,object>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Cast<object>(System.Collections.IEnumerable)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.CastIterator<object>(System.Collections.IEnumerable)
		// bool System.Linq.Enumerable.Contains<object>(System.Collections.Generic.IEnumerable<object>,object)
		// bool System.Linq.Enumerable.Contains<object>(System.Collections.Generic.IEnumerable<object>,object,System.Collections.Generic.IEqualityComparer<object>)
		// int System.Linq.Enumerable.Count<object>(System.Collections.Generic.IEnumerable<object>)
		// int System.Linq.Enumerable.Count<ushort>(System.Collections.Generic.IEnumerable<ushort>,System.Func<ushort,bool>)
		// object System.Linq.Enumerable.First<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Linq.IOrderedEnumerable<UnityEngine.Quaternion> System.Linq.Enumerable.OrderBy<UnityEngine.Quaternion,float>(System.Collections.Generic.IEnumerable<UnityEngine.Quaternion>,System.Func<UnityEngine.Quaternion,float>)
		// System.Linq.IOrderedEnumerable<UnityEngine.Vector2> System.Linq.Enumerable.OrderBy<UnityEngine.Vector2,float>(System.Collections.Generic.IEnumerable<UnityEngine.Vector2>,System.Func<UnityEngine.Vector2,float>)
		// System.Linq.IOrderedEnumerable<UnityEngine.Vector3> System.Linq.Enumerable.OrderBy<UnityEngine.Vector3,float>(System.Collections.Generic.IEnumerable<UnityEngine.Vector3>,System.Func<UnityEngine.Vector3,float>)
		// System.Linq.IOrderedEnumerable<byte> System.Linq.Enumerable.OrderBy<byte,float>(System.Collections.Generic.IEnumerable<byte>,System.Func<byte,float>)
		// System.Linq.IOrderedEnumerable<float> System.Linq.Enumerable.OrderBy<float,float>(System.Collections.Generic.IEnumerable<float>,System.Func<float,float>)
		// System.Linq.IOrderedEnumerable<int> System.Linq.Enumerable.OrderBy<int,float>(System.Collections.Generic.IEnumerable<int>,System.Func<int,float>)
		// System.Linq.IOrderedEnumerable<object> System.Linq.Enumerable.OrderBy<object,float>(System.Collections.Generic.IEnumerable<object>,System.Func<object,float>)
		// System.Linq.IOrderedEnumerable<UnityEngine.Vector3> System.Linq.Enumerable.OrderByDescending<UnityEngine.Vector3,float>(System.Collections.Generic.IEnumerable<UnityEngine.Vector3>,System.Func<UnityEngine.Vector3,float>)
		// System.Linq.IOrderedEnumerable<object> System.Linq.Enumerable.OrderByDescending<object,float>(System.Collections.Generic.IEnumerable<object>,System.Func<object,float>)
		// System.Collections.Generic.IEnumerable<UnityEngine.Quaternion> System.Linq.Enumerable.Take<UnityEngine.Quaternion>(System.Collections.Generic.IEnumerable<UnityEngine.Quaternion>,int)
		// System.Collections.Generic.IEnumerable<UnityEngine.Vector2> System.Linq.Enumerable.Take<UnityEngine.Vector2>(System.Collections.Generic.IEnumerable<UnityEngine.Vector2>,int)
		// System.Collections.Generic.IEnumerable<UnityEngine.Vector3> System.Linq.Enumerable.Take<UnityEngine.Vector3>(System.Collections.Generic.IEnumerable<UnityEngine.Vector3>,int)
		// System.Collections.Generic.IEnumerable<byte> System.Linq.Enumerable.Take<byte>(System.Collections.Generic.IEnumerable<byte>,int)
		// System.Collections.Generic.IEnumerable<float> System.Linq.Enumerable.Take<float>(System.Collections.Generic.IEnumerable<float>,int)
		// System.Collections.Generic.IEnumerable<int> System.Linq.Enumerable.Take<int>(System.Collections.Generic.IEnumerable<int>,int)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Take<object>(System.Collections.Generic.IEnumerable<object>,int)
		// System.Collections.Generic.IEnumerable<UnityEngine.Quaternion> System.Linq.Enumerable.TakeIterator<UnityEngine.Quaternion>(System.Collections.Generic.IEnumerable<UnityEngine.Quaternion>,int)
		// System.Collections.Generic.IEnumerable<UnityEngine.Vector2> System.Linq.Enumerable.TakeIterator<UnityEngine.Vector2>(System.Collections.Generic.IEnumerable<UnityEngine.Vector2>,int)
		// System.Collections.Generic.IEnumerable<UnityEngine.Vector3> System.Linq.Enumerable.TakeIterator<UnityEngine.Vector3>(System.Collections.Generic.IEnumerable<UnityEngine.Vector3>,int)
		// System.Collections.Generic.IEnumerable<byte> System.Linq.Enumerable.TakeIterator<byte>(System.Collections.Generic.IEnumerable<byte>,int)
		// System.Collections.Generic.IEnumerable<float> System.Linq.Enumerable.TakeIterator<float>(System.Collections.Generic.IEnumerable<float>,int)
		// System.Collections.Generic.IEnumerable<int> System.Linq.Enumerable.TakeIterator<int>(System.Collections.Generic.IEnumerable<int>,int)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.TakeIterator<object>(System.Collections.Generic.IEnumerable<object>,int)
		// object[] System.Linq.Enumerable.ToArray<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Collections.Generic.Dictionary<object,object> System.Linq.Enumerable.ToDictionary<System.Collections.Generic.KeyValuePair<object,object>,object,object>(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>,System.Func<System.Collections.Generic.KeyValuePair<object,object>,object>,System.Func<System.Collections.Generic.KeyValuePair<object,object>,object>)
		// System.Collections.Generic.Dictionary<object,object> System.Linq.Enumerable.ToDictionary<System.Collections.Generic.KeyValuePair<object,object>,object,object>(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>,System.Func<System.Collections.Generic.KeyValuePair<object,object>,object>,System.Func<System.Collections.Generic.KeyValuePair<object,object>,object>,System.Collections.Generic.IEqualityComparer<object>)
		// System.Collections.Generic.List<UnityEngine.Quaternion> System.Linq.Enumerable.ToList<UnityEngine.Quaternion>(System.Collections.Generic.IEnumerable<UnityEngine.Quaternion>)
		// System.Collections.Generic.List<UnityEngine.Vector2> System.Linq.Enumerable.ToList<UnityEngine.Vector2>(System.Collections.Generic.IEnumerable<UnityEngine.Vector2>)
		// System.Collections.Generic.List<UnityEngine.Vector3> System.Linq.Enumerable.ToList<UnityEngine.Vector3>(System.Collections.Generic.IEnumerable<UnityEngine.Vector3>)
		// System.Collections.Generic.List<byte> System.Linq.Enumerable.ToList<byte>(System.Collections.Generic.IEnumerable<byte>)
		// System.Collections.Generic.List<float> System.Linq.Enumerable.ToList<float>(System.Collections.Generic.IEnumerable<float>)
		// System.Collections.Generic.List<int> System.Linq.Enumerable.ToList<int>(System.Collections.Generic.IEnumerable<int>)
		// System.Collections.Generic.List<object> System.Linq.Enumerable.ToList<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Collections.Generic.IEnumerable<UnityEngine.Quaternion> System.Linq.Enumerable.Where<UnityEngine.Quaternion>(System.Collections.Generic.IEnumerable<UnityEngine.Quaternion>,System.Func<UnityEngine.Quaternion,bool>)
		// System.Collections.Generic.IEnumerable<UnityEngine.Vector2> System.Linq.Enumerable.Where<UnityEngine.Vector2>(System.Collections.Generic.IEnumerable<UnityEngine.Vector2>,System.Func<UnityEngine.Vector2,bool>)
		// System.Collections.Generic.IEnumerable<UnityEngine.Vector3> System.Linq.Enumerable.Where<UnityEngine.Vector3>(System.Collections.Generic.IEnumerable<UnityEngine.Vector3>,System.Func<UnityEngine.Vector3,bool>)
		// System.Collections.Generic.IEnumerable<byte> System.Linq.Enumerable.Where<byte>(System.Collections.Generic.IEnumerable<byte>,System.Func<byte,bool>)
		// System.Collections.Generic.IEnumerable<float> System.Linq.Enumerable.Where<float>(System.Collections.Generic.IEnumerable<float>,System.Func<float,bool>)
		// System.Collections.Generic.IEnumerable<int> System.Linq.Enumerable.Where<int>(System.Collections.Generic.IEnumerable<int>,System.Func<int,bool>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Where<object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,bool>)
		// object System.Reflection.CustomAttributeExtensions.GetCustomAttribute<object>(System.Reflection.MemberInfo)
		// System.Void* Unity.Collections.LowLevel.Unsafe.UnsafeUtility.AddressOf<UnityEngine.Vector2>(UnityEngine.Vector2&)
		// System.Void* Unity.Collections.LowLevel.Unsafe.UnsafeUtility.AddressOf<UnityEngine.Vector3>(UnityEngine.Vector3&)
		// System.Void* Unity.Collections.LowLevel.Unsafe.UnsafeUtility.AddressOf<float>(float&)
		// int Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<UnityEngine.Vector2>()
		// int Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<UnityEngine.Vector3>()
		// int Unity.Collections.LowLevel.Unsafe.UnsafeUtility.SizeOf<float>()
		// object UnityEngine.Component.GetComponent<object>()
		// object UnityEngine.Component.GetComponentInChildren<object>()
		// object UnityEngine.Component.GetComponentInParent<object>()
		// object UnityEngine.Component.GetComponentInParent<object>(bool)
		// object[] UnityEngine.Component.GetComponentsInChildren<object>()
		// object[] UnityEngine.Component.GetComponentsInChildren<object>(bool)
		// object UnityEngine.GameObject.AddComponent<object>()
		// object UnityEngine.GameObject.GetComponent<object>()
		// object UnityEngine.GameObject.GetComponentInChildren<object>()
		// object UnityEngine.GameObject.GetComponentInChildren<object>(bool)
		// object UnityEngine.GameObject.GetComponentInParent<object>()
		// object UnityEngine.GameObject.GetComponentInParent<object>(bool)
		// object[] UnityEngine.GameObject.GetComponents<object>()
		// object[] UnityEngine.GameObject.GetComponentsInChildren<object>()
		// object[] UnityEngine.GameObject.GetComponentsInChildren<object>(bool)
		// UnityEngine.Vector2 UnityEngine.InputSystem.InputAction.ReadValue<UnityEngine.Vector2>()
		// UnityEngine.Vector3 UnityEngine.InputSystem.InputAction.ReadValue<UnityEngine.Vector3>()
		// float UnityEngine.InputSystem.InputAction.ReadValue<float>()
		// UnityEngine.Vector2 UnityEngine.InputSystem.InputActionState.ApplyProcessors<UnityEngine.Vector2>(int,UnityEngine.Vector2,UnityEngine.InputSystem.InputControl<UnityEngine.Vector2>)
		// UnityEngine.Vector3 UnityEngine.InputSystem.InputActionState.ApplyProcessors<UnityEngine.Vector3>(int,UnityEngine.Vector3,UnityEngine.InputSystem.InputControl<UnityEngine.Vector3>)
		// float UnityEngine.InputSystem.InputActionState.ApplyProcessors<float>(int,float,UnityEngine.InputSystem.InputControl<float>)
		// UnityEngine.Vector2 UnityEngine.InputSystem.InputActionState.ReadValue<UnityEngine.Vector2>(int,int,bool)
		// UnityEngine.Vector3 UnityEngine.InputSystem.InputActionState.ReadValue<UnityEngine.Vector3>(int,int,bool)
		// float UnityEngine.InputSystem.InputActionState.ReadValue<float>(int,int,bool)
		// object UnityEngine.Object.FindAnyObjectByType<object>()
		// object UnityEngine.Object.FindObjectOfType<object>()
		// object UnityEngine.Object.Instantiate<object>(object)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform,bool)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Vector3,UnityEngine.Quaternion)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Vector3,UnityEngine.Quaternion,UnityEngine.Transform)
		// object UnityEngine.ScriptableObject.CreateInstance<object>()
	}
}