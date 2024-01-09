using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TreeCreator : MonoBehaviour
{
    public int RootPersonID;
    public GameObject PersonaPNG, ParejaPNG;
    public List<GameObject> Personas = new List<GameObject>();
    public float VOffset = 150f;
    private int CounterID = 1;

    // Start is called before the first frame update
    void Start()
    {
        //PeopleList PeopleManager = FindObjectOfType<PeopleList>();
        //PeopleManager.OnPeopleCreated += CreatePersonPNG;
    }

    // Update is called once per frame
    void Update()
    {
        //ExistingPeople = PeopleList.ExistingPeople;
        Personas = PeopleList.ExistingPeople;
    }
    private void OnEnable()
    {
        OrganizeUp(FindPerson(RootPersonID));
    }
    public void OrganizeUp(Persona FirstPersonData, Persona SecondPersonData = null)
    {
        float VOffset = 125f;
        // Crear a el PNG de la persona individual en caso de no tener asignada una pareja,
        // mientras que si tiene pareja asignada, creara el PNG combinado
        if (SecondPersonData == null)
        {
            GameObject PersonPNG = Instantiate(PersonaPNG, new Vector3(0f, 0f, 0f), Quaternion.identity);

            OrganizePNG SingleData = PersonPNG.GetComponent<OrganizePNG>();
            SingleData.Humano = FirstPersonData;
            SingleData.ID = FirstPersonData.ID;

            GameObject[] PNGs = GameObject.FindGameObjectsWithTag("PersonPNG");
            foreach (GameObject PNG in PNGs)
            {
                OrganizePNG PNGData = PNG.GetComponent<OrganizePNG>();
                if (SingleData.Humano.ChildrenID.Contains(PNGData.ID))
                {
                    PersonPNG.transform.parent = PNG.transform;
                }
            }

            PersonPNG.transform.SetParent(transform);
            if(FirstPersonData.ID != RootPersonID) PersonPNG.transform.localPosition = new Vector3(60f, VOffset, 0f);
            else PersonPNG.transform.localPosition = new Vector3(0f, 0f, 0f);
        }
        else
        {
            Vector3 PairPosition = new Vector3(0f, 0f, 0f);
            if (FirstPersonData.ID != FindPerson(RootPersonID).FatherID && FirstPersonData.ID != FindPerson(RootPersonID).MotherID &&
                SecondPersonData.ID != FindPerson(RootPersonID).FatherID && SecondPersonData.ID != FindPerson(RootPersonID).MotherID)
            { // Si no eres el pare o mare de la RootPerson
                int HOffset = 0;
                if (FindPerson(FirstPersonData.ChildrenID[0]).Gender == Genders.Male)
                {
                    HOffset = -60 - 120 * (CountAncestors(FindPerson(SecondPersonData.ID)));
                }
                else if (FindPerson(FirstPersonData.ChildrenID[0]).Gender == Genders.Female)
                {
                    HOffset = 60 + 120 * (CountAncestors(FindPerson(FirstPersonData.ID)));
                }


                PairPosition = new Vector3(HOffset, VOffset, 0f);
            }
            else
            {
                PairPosition = new Vector3(0f, VOffset, 0f);
            }
            GameObject PairPNG = Instantiate(ParejaPNG, PairPosition, Quaternion.identity);
            PairPNG.transform.localPosition = PairPosition;
            
            PairPNG.transform.name = FirstPersonData.FirstName + "&" + SecondPersonData.FirstName;

            OrganizePNG MaleData = PairPNG.transform.Find("MalePNG").GetComponent<OrganizePNG>();
            OrganizePNG FemaleData = PairPNG.transform.Find("FemalePNG").GetComponent<OrganizePNG>();
            if (FirstPersonData.Gender == Genders.Male)
            {
                MaleData.Humano = FirstPersonData;
                MaleData.ID = FirstPersonData.ID;

                FemaleData.Humano = SecondPersonData;
                FemaleData.ID = SecondPersonData.ID;
            }
            else if (FirstPersonData.Gender == Genders.Female)
            {
                FemaleData.Humano = FirstPersonData;
                FemaleData.ID = FirstPersonData.ID;

                MaleData.Humano = SecondPersonData;
                MaleData.ID = SecondPersonData.ID;
            }

            GameObject[] PNGs = GameObject.FindGameObjectsWithTag("PersonPNG");
            foreach (GameObject PNG in PNGs)
            {
                OrganizePNG PNGData = PNG.GetComponent<OrganizePNG>();
                if (MaleData.Humano.ChildrenID.Contains(PNGData.ID) && FemaleData.Humano.ChildrenID.Contains(PNGData.ID))
                {
                    PairPNG.transform.SetParent(PNG.transform);
                    PairPNG.transform.localPosition = PairPosition;
                }
            }
        }
        //----------------------------------------//

        CreateParents(FirstPersonData);
        /*Debug.Log(FirstPersonData.ID + "_" + FirstPersonData.FirstName + FirstPersonData.Surname1 +
            " - Total: " + CountAncestors(FirstPersonData));*/
        
        CreateParents(SecondPersonData);
        /*Debug.Log(SecondPersonData.ID + "_" + SecondPersonData.FirstName + SecondPersonData.Surname1 +
                " - Total: " + CountAncestors(SecondPersonData));*/
    }
    void CreateSiblings(Persona Person)
    {
        if(Person.SiblingsID.Count > 0)
        {

        }
    }
    int CountDescendants(Persona Person)
    {
        int TotalDescendants = 0;

        if (Person.ChildrenID.Count > 0)
        {
            TotalDescendants += Person.ChildrenID.Count;
            foreach (int ChildID in Person.ChildrenID)
            {
                TotalDescendants += CountDescendants(FindPerson(ChildID));
            }
        }

        return TotalDescendants;
    }
    void CreateParents(Persona Child)
    {
        if (Child.FatherID != 0 && Child.MotherID != 0) // Si té pare i mare
        {
            OrganizeUp(FindPerson(Child.FatherID), FindPerson(Child.MotherID));
        }
        else if (Child.FatherID != 0 && Child.MotherID == 0) // Sols té pare
        {
            OrganizeUp(FindPerson(Child.FatherID), null);
        }
        else if (Child.FatherID == 0 && Child.MotherID != 0) // Sols té mare
        {
            OrganizeUp(FindPerson(Child.MotherID), null);
        }
    }
    int CountAncestors(Persona Person)
    {
        int TotalAncestors = 0;

        if (Person.FatherID != 0)
        {
            TotalAncestors += CountAncestors(FindPerson(Person.FatherID));
        }
        if (Person.MotherID != 0)
        {
            TotalAncestors += CountAncestors(FindPerson(Person.MotherID));
        }

        if (Person.FatherID != 0 && Person.MotherID != 0) TotalAncestors++;

        return TotalAncestors;
    }
    Persona FindPerson(int ID)
    {
        foreach (Persona Person in PeopleList.PeopleRegistry.Registry)
        {
            if (Person.ID == ID)
            {
                return Person;
            }
        }
        return null;
    }
}
