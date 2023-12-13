using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public float m_Speed = 2.5f,
        m_RotRange = 180f,
        m_DirChangeTimer = 2f;
    private enum m_States { Walking, Stopped };
    [SerializeField] private m_States CharState = m_States.Walking;

    private float tiempoPasado,
        m_TemporalStop;

    void Start()
    {
        tiempoPasado = 0f;
    }
    void Update()
    {
        if(CharState == m_States.Walking && m_TemporalStop <= 0f)
        {
            tiempoPasado += Time.deltaTime;

            if (tiempoPasado >= m_DirChangeTimer)
            {
                CambiarDireccion();
                tiempoPasado = 0f;
            }

            Vector3 movimiento = transform.forward * m_Speed * Time.deltaTime;
            transform.Translate(movimiento, Space.World);
        }

        if(m_TemporalStop > 0f) m_TemporalStop -= Time.deltaTime;
        
        if (CharState == m_States.Stopped)
        {
            m_TemporalStop = Random.Range(5f, 15f);
            CharState = m_States.Walking;
        }
    }
    void CambiarDireccion()
    {
        float rotacionAleatoria = Random.Range(-m_RotRange, m_RotRange);
        transform.Rotate(Vector3.up, rotacionAleatoria);

        m_DirChangeTimer = Random.Range(5f, 10f);
    }
    public void Stop()
    {
        CharState = m_States.Stopped;
    }
}
