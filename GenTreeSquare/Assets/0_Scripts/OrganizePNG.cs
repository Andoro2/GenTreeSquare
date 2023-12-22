using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganizePNG : MonoBehaviour
{
    public int ID;
    public Persona Humano;

    public TreeCreator TC;
    // Start is called before the first frame update
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
        






        //TC = GameObject.FindWithTag("Tree").GetComponent<TreeCreator>();


        /*if (RootPerson != Humano)
        {
            GetAncestors();
            //GetDescendents();
            //currentYPosition = GetAncestors();
            transform.localPosition = new Vector3(transform.localPosition.x, GetDescendents(Humano), 0f);

            //transform.position = new Vector3(GetAncestors(), GetDescendents(Humano) * PeopleList.VerticalS, 0f);
            //transform.localPosition = new Vector3(GetAncestors(), GetDescendents(Humano) * PeopleList.VerticalS, 0f);
        }
        else
        {
            transform.localPosition = new Vector3(0f, 0f, 0f);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        //if (RootPerson != TC.RootPerson.GetComponent<Person>().Humano) RootPerson = TC.RootPerson.GetComponent<Person>().Humano;
    }
    /*
    public void GetAncestors()
    {
        //int AncestorCounter = 0;
        //Persona PersonData = Person.GetComponent<Person>().Humano;
        Persona PersonAnalized = Humano;

        if (PersonAnalized.Gender == Persona.Genders.Male)
        {
            transform.localPosition = new Vector3(transform.localPosition.x - 75, transform.localPosition.y, transform.localPosition.z);
            while (PersonAnalized.Mother != null)
            {
                if (PersonAnalized.Mother != null && PersonAnalized.Mother.transform.GetComponent<Person>().Humano != PersonAnalized)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x - 75, transform.localPosition.y, transform.localPosition.z);
                    //AncestorCounter++;
                }
                PersonAnalized = PersonAnalized.Mother.transform.GetComponent<Person>().Humano;
            }
            //return AncestorCounter * (-1) * PeopleList.VerticalS ;
        }
        if (PersonAnalized.Gender == Persona.Genders.Female)
        {
            transform.localPosition = new Vector3(transform.localPosition.x + 75, transform.localPosition.y, transform.localPosition.z);
            while (PersonAnalized.Father != null)
            {
                if (PersonAnalized.Father != null && PersonAnalized.Father.transform.GetComponent<Person>().Humano != PersonAnalized)
                {
                    transform.localPosition = new Vector3(transform.localPosition.x + 75, transform.localPosition.y, transform.localPosition.z);
                    //AncestorCounter++;
                }
                PersonAnalized = PersonAnalized.Father.transform.GetComponent<Person>().Humano;
            }
            //return AncestorCounter * PeopleList.VerticalS;
        }

        //return AncestorCounter;
    }*/
    /*
    public int GetDescendents(Persona PersonAnalized)
    {
        int DescendentsCounter = 0;*/

        /*if (Humano.Siblings.Count != 0)
        {
            int maxSiblingY = int.MinValue;

            foreach (GameObject Sibling in PersonAnalized.Siblings)
            {
                int siblingPositionY = (int)Sibling.transform.position.y;
                maxSiblingY = Mathf.Max(maxSiblingY, siblingPositionY);
            }

            // DescendentsCounter = Mathf.Max((int)transform.position.y, maxSiblingY) / PeopleList.VerticalS;
            return maxSiblingY / PeopleList.VerticalS;
        }
        else
        {*/
            /*foreach (GameObject Child in PersonAnalized.Children) 
            {
                int ChildSDescendent = 1 + GetDescendents(Child.transform.GetComponent<Person>().Humano);
                DescendentsCounter = Mathf.Max(DescendentsCounter, ChildSDescendent);
            }

            return DescendentsCounter;*/
        //}
    //}
}
