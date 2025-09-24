using UnityEngine;

public interface IFactory<T>
{
 
    T Create();
    T CreateAt(Vector3 position, Quaternion rotation, Transform parent = null);
}