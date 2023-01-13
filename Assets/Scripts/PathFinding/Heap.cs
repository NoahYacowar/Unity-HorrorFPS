using System.Collections;
using System;
using UnityEngine;

public class Heap<T> where T : IHeapItem<T>
{
    //Heap array and counter
    T[] items;
    int currentItemCount;

    //Pre:
    //Post:
    //Desc: Heap constructor
    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    //Pre:
    //Post:
    //Desc: Adding value to Heap array
    public void Add(T item)
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    //Pre:
    //Post:
    //Desc: Removing the first item within the heap, replacing with item at end of heap
    public T RemoveFirst()
    {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    //Pre:
    //Post:
    //Desc: Used to adjust priorities (pathfinding priorities only increase)
    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    //Pre:
    //Post:
    //Desc: Returns item count within heap
    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }

    //Pre:
    //Post:
    //Desc: Determines whether item equal to item in heap with same HeapIndex value
    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    //Pre:
    //Post:
    //Desc: Determining <T> place in heap
    void SortDown(T item)
    {
        while (true)
        {
            //Calculating child index values
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            //Determining whether left child exists
            if (childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;

                //Determining whether right child exists
                if (childIndexRight < currentItemCount)
                {
                    //Determining which child has higher priority
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0) swapIndex = childIndexRight;
                }

                //Determining whether child has higher priority than parent
                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }
                else return;
            }
            else return;
        }
    }

    //Pre:
    //Post:
    //Desc: Sorts
    void SortUp(T item)
    {
        //Calculating parent index
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            //Determines whether the child item has a higher priority (based on f-cost)
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                //Swap the two items
                Swap(item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    //Pre: 
    //Post:
    //Desc: Swapping values of two <Generic> items within heap
    void Swap(T itemA, T itemB)
    {
        //Swapping two values within heap
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }

}

//Interface which holds getter and setter for a index within the heap
public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}
