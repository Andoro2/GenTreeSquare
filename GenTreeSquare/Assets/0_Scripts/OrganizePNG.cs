using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OrganizePNG : MonoBehaviour
{
    public int ID;
    public Persona Humano;

    public TreeCreator TC;

    public void AssignHuman()
    {
        foreach (Persona Persona in PeopleList.PeopleRegistry.Registry)
        {
            if (Persona.ID == ID)
            {
                Humano = Persona;
            }
        }
    }
    void Start()
    {
        transform.name = ID + "_" + Humano.FirstName + Humano.Surname1;

        transform.Find("Data").transform
                 .Find("Name").transform
                 .GetComponent<TextMeshProUGUI>().text =
            Humano.FirstName + " " + Humano.SecondName;
        transform.Find("Data").transform
                .Find("Surnames").transform
                .GetComponent<TextMeshProUGUI>().text =
            Humano.Surname1 + " " + Humano.Surname2;
    }
    public void FocusOnPerson()
    {
        CameraFollow.ChangeTarget(ID);
    }
}
