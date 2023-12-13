using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Person : MonoBehaviour
{
    public Persona Humano;
    private GameObject[] People;
    private int PeopleCount;

    void Start()
    {
        GameObject[] People = GameObject.FindGameObjectsWithTag("Person");
        PeopleCount = People.Length;
        GetSiblings(People);
        GetChildren(People);
    }
    void Update()
    {
        People = GameObject.FindGameObjectsWithTag("Person");
        GetSiblings(People);
        GetChildren(People);
        PeopleCount = People.Length;
        /*if (PeopleCount != People.Length)
        {
            GetSiblings(People);
            GetChildren(People);
            PeopleCount = People.Length;
        }*/
    }
    private void GetSiblings(GameObject[] People)
    {
        for (int i = 0; i < People.Length; i++)
        {
            if (People[i] != gameObject && !Humano.Siblings.Contains(People[i]) && People[i].GetComponent<Person>().Humano.Father.gameObject != null
                && People[i].GetComponent<Person>().Humano.Father.gameObject == Humano.Father.gameObject && People[i].GetComponent<Person>().Humano.Mother.gameObject == Humano.Mother.gameObject)
            {
                Humano.Siblings.Add(People[i].gameObject);
            }
        }
    }
    private void GetChildren(GameObject[] People)
    {
        for (int i = 0; i < People.Length; i++)
        {
            if (People[i] != gameObject && !Humano.Children.Contains(People[i]) 
                && (People[i].GetComponent<Person>().Humano.Father.gameObject == gameObject || People[i].GetComponent<Person>().Humano.Mother.gameObject == gameObject))
            {
                Humano.Children.Add(People[i].gameObject);
            }
        }
    }
}
