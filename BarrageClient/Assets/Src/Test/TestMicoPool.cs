using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using Microsoft.Extensions.ObjectPool;
public class TestMicoPool : MonoBehaviour
{
   
    void Start()
    {
       
        // Arrange
        var pool = new DefaultObjectPool<object>(new DefaultPooledObjectPolicy<object>(),4);

        var obj1 = pool.Get();
        var obj2 = pool.Get();
        pool.Return(obj1);
        pool.Return(obj2);
        
        for(int i=0;i<4;++i)
        {
            pool.Return(new object());
        }
        // Act
        obj2 = pool.Get();

        if(obj1 == obj2)
        {
            Log.Debug("True");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
