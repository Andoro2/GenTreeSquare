using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OrganizePople : MonoBehaviour
{
    public void OrganizePeople()
    {
        GameObject[] PersonasArray = GameObject.FindGameObjectsWithTag("Person");
        Vector3 GridPlace = new Vector3(-4f, 0f, 0.2f);
        int i = 0;
        Array.Sort(PersonasArray, (persona1, persona2) => string.Compare(persona1.GetComponent<Person>().Humano.Surname1, persona2.GetComponent<Person>().Humano.Surname1, StringComparison.Ordinal));
        
        foreach (GameObject Persona in PersonasArray)
        {
            float x = (i % 5 - 2) * 3f;
            float z = Mathf.Floor(i / 5) * 3f;

            Persona.transform.position = new Vector3(x, 0.2f, z);
            i++;

            Persona.transform.rotation = new Quaternion(0f, 180f, 0f, 1f);
            Persona.GetComponent<RandomMovement>().Stop();
        }
    }
}
