using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
public enum  HeapType {
    MinHeap,
    MaxHeap
}
/// <summary>
/// 二叉堆
/// </summary>
public class BinaryHeap<T> where T : IComparable<T>{
    
    private List<T> _datas = new List<T>();

    public HeapType HType { get; private set; }

    public T Root => _datas[0];
    public int Count => _datas.Count;

    public BinaryHeap(HeapType type) {
        HType = type;
    }

    public T Peek() {
        return Count<=0 ? default : _datas[0];
    }

    public void Push(T item) {
        _datas.Add(item);
        Rise(Count);
    }

    public T Pop() {
        if(Count<=0){return default;}
        var result = _datas[0];
        Swap(1,Count);
        _datas.RemoveAt(Count-1);
        Dive(1);
        return result;
    }

    /// <summary>
    /// 不安全，仅用作测试
    /// </summary>
    /// <returns></returns>
    public IEnumerable<T> Preview() {
        return _datas;
    }
    /// <summary>
    /// 查询堆中特定节点，如果没有找到logicIndex小于等于0并返回default
    /// </summary>
    /// <param name="logicIndex">逻辑坐标</param>
    /// <param name="match"></param>
    /// <returns>节点</returns>
    public T Find(out int logicIndex,[NotNull, InstantHandle] Predicate<T> match) {
        logicIndex = _datas.FindIndex(match)+1;
        if (logicIndex>0) {
            return _datas[logicIndex-1];
        }

        return default;
    }
    /// <summary>
    /// 替换指定节点，会重新计算堆
    /// </summary>
    /// <param name="logicIndex"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Replace(int logicIndex,T item) {
        if (logicIndex<1 || logicIndex>Count) {
            return false;
        }
        Swap(logicIndex,Count);
        _datas.RemoveAt(Count-1);
        Push(item);
        return true;
    }

    public void Clear() {
        _datas.Clear();
    }

    private void Rise(int index) {
        for (int i = index; i > 1 && Compare(i,i/2) ; i/=2) {
            Swap(i, i / 2);
        }
    }

    private void Dive(int index) {
        for (int p = index,s = Son(p); s<=Count && Compare(s,p); p=s,s=Son(p)) {
            Swap(p,s);
        }
    }

    /// <summary>
    /// 寻找较大或较小的那个子节点
    /// MaxHeap时，返回较大的节点；MinHeap时，返回较小的节点
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private int Son(int index) {
        if (index * 2 + 1 <= Count && Compare(index*2+1,index*2)) {
            return index * 2 + 1;
        }
        else {
            return index * 2;
        }
    }
    /// <summary>
    /// 比较两个节点的大小，根据设定的最大堆最小堆返回相应的结果
    /// MaxHeap时，A 大于 B返回true；MinHeap时，A 小于 B返回true
    /// </summary>
    /// <param name="indexA">逻辑下标</param>
    /// <param name="indexB">逻辑下标</param>
    /// <returns>是否满足交换条件</returns>
    private bool Compare(int indexA,int indexB) {
        var result = _datas[indexA - 1].CompareTo(_datas[indexB - 1]);
        return HType == HeapType.MaxHeap ? result > 0 : result < 0;
    }

    private void Swap(int indexA,int  indexB) {
        (_datas[indexA-1], _datas[indexB-1]) = (_datas[indexB-1], _datas[indexA-1]);
    }
}
