using UnityEngine;
using System.Collections;

public class wwYieldable : IEnumerator
{

	private bool finished = false;

    public wwYieldable()
    {
        finished = false;
    }
    //-- IEnumerator Interface
    public object Current
    {
        get
        {
            return null;
        }
    }
 
    //-- IEnumerator Interface
    public bool MoveNext()
    {
        return (!finished);
    }
 
    //-- IEnumerator Interface
    public void Reset()
    {
        finished = false;
    }

    /// <summary>
    /// Finish
    /// </summary>
    public void Finish()
    {
        finished = true;
        
    }
}
