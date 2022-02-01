using System.Collections;
using UnityEngine;

public class BufferedInput
{
    private Coroutine m_bufferTimer;
    private float m_timer = 0;

    public bool IsBuffered
    {
        get
        {
            return m_timer <= 0;
        }
    }

    public void BufferInput( float time, MonoBehaviour bufferOwner )
    {
        bool needsCoroutine = IsBuffered;
        m_timer = Mathf.Max(m_timer, time);

        if ( needsCoroutine )
        {
            m_bufferTimer = bufferOwner.StartCoroutine(StartBuffer());
        }
    }

    private IEnumerator StartBuffer()
    {
        while ( m_timer > 0 )
        {
            m_timer -= Time.deltaTime;
            yield return null;
        }
    }
}

