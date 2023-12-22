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
            if (People[i].GetComponent<Person>().Humano != Humano && !Humano.SiblingsID.Contains(People[i].GetComponent<Person>().Humano.ID)
                && (People[i].GetComponent<Person>().Humano.FatherID != 0 && People[i].GetComponent<Person>().Humano.FatherID == Humano.FatherID
                || People[i].GetComponent<Person>().Humano.MotherID != 0 && People[i].GetComponent<Person>().Humano.MotherID == Humano.MotherID))
            {
                Humano.SiblingsID.Add(People[i].GetComponent<Person>().Humano.ID);
            }
        }
    }
    private void GetChildren(GameObject[] People)
    {
        for (int i = 0; i < People.Length; i++)
        {
            if (People[i].GetComponent<Person>().Humano.ID != Humano.ID && !Humano.ChildrenID.Contains(People[i].GetComponent<Person>().Humano.ID)
                && (People[i].GetComponent<Person>().Humano.FatherID == Humano.ID || People[i].GetComponent<Person>().Humano.MotherID == Humano.ID))
            {
                Humano.ChildrenID.Add(People[i].GetComponent<Person>().Humano.ID);
            }
        }
    }
}
