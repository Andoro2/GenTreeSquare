using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public float m_Speed = 2.5f,
        m_RotRange = 90f,
        m_StoppedTimerMin = 2.5f,
        m_StoppedTimerMax = 5f,
        m_DirChangeTimer = 2f;

    private enum m_States { Walking, Stopped };
    [SerializeField] private m_States CharState = m_States.Walking;

    public Animator m_Anim;

    private float actionTimer;

    void Start()
    {
        actionTimer = Random.Range(5f, 15f);

        switch (GetComponent<Person>().Humano.Gender)
        {
            case Genders.Male:
                {
                    GameObject Body = transform.Find("Y Bot").gameObject;
                    Body.SetActive(true);
                    m_Anim = Body.GetComponent<Animator>();

                    if (transform.Find("X Bot").gameObject) transform.Find("X Bot").gameObject.SetActive(false);
                    if (transform.Find("Body").gameObject) transform.Find("Body").gameObject.SetActive(false);
                    break;
                }
            case Genders.Female:
                {
                    GameObject Body = transform.Find("X Bot").gameObject;
                    Body.SetActive(true);
                    m_Anim = Body.GetComponent<Animator>();

                    if (transform.Find("Y Bot").gameObject) transform.Find("Y Bot").gameObject.SetActive(false);
                    if (transform.Find("Body").gameObject) transform.Find("Body").gameObject.SetActive(false);
                    break;
                }
            case Genders.Other:
                {
                    GameObject Body = transform.Find("Body").gameObject;
                    Body.SetActive(true);

                    if (transform.Find("Y Bot").gameObject) transform.Find("Y Bot").gameObject.SetActive(false);
                    if (transform.Find("X Bot").gameObject) transform.Find("X Bot").gameObject.SetActive(false);
                    break;
                }
        }
    }
    void Update()
    {
        if (actionTimer > 0f)
        {
            actionTimer -= Time.deltaTime;
            if(m_DirChangeTimer > 0)
            {
                m_DirChangeTimer -= Time.deltaTime;
            }
            else
            {
                m_DirChangeTimer = Random.Range(5f, 10f);
                ChangeDirection();
            }
        }
        else
        {
            if (CharState == m_States.Walking)
            {
                Stop();
            }
            else if (CharState == m_States.Stopped)
            {
                StartWalking();
            }
        }

        if (CharState == m_States.Walking)
        {
            Walk();
        }
        else if (CharState == m_States.Stopped)
        {
            if (m_Anim != null) m_Anim.SetBool("Walking", false);
        }
    }
    void Walk()
    {
        if (m_Anim != null) m_Anim.SetBool("Walking", true);

        Vector3 movement = transform.forward * m_Speed * Time.deltaTime;
        transform.Translate(movement, Space.World);
    }
    void ChangeDirection()
    {
        float randomRotation = Random.Range(-m_RotRange, m_RotRange);
        transform.Rotate(Vector3.up, randomRotation);
    }
    void StartWalking()
    {
        CharState = m_States.Walking;
        actionTimer = Random.Range(5f,15f);
    }
    public void Stop()
    {
        CharState = m_States.Stopped;
        actionTimer = Random.Range(5f, 15f);
    }
}