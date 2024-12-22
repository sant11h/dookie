using System.Collections;

namespace Dookie.Core.Network;

public class RingBuffer<T>(int capacity) : IEnumerable<T>
{
    private readonly T[] _elements = new T[capacity];
    private int _start;
    private int _end;
    private int _count;

    public T this[int i] => _elements[(_start + i) % capacity];

    public void Add(T element)
    {
        if(_count == capacity) throw new InvalidOperationException();
        _elements[_end] = element;
        _end = (_end + 1) % capacity;
        _count++;
    }

    public void FastClear()
    {
        _start = 0;
        _end = 0;
        _count = 0;
    }

    public int Count => _count;
    public T First => _elements[_start];
    public T Last => _elements[(_start+_count-1)%capacity];
    public bool IsFull => _count == capacity;

    public void RemoveFromStart(int count)
    {
        if(count > capacity || count > _count) throw new InvalidOperationException();
        _start = (_start + count) % capacity;
        _count -= count;
    }

    public IEnumerator<T> GetEnumerator()
    {
        int counter = _start;
        while (counter != _end)
        {
            yield return _elements[counter];
            counter = (counter + 1) % capacity;
        }           
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}