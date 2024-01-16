using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class TreeCreator : MonoBehaviour
{
    public int RootPersonID;
    public GameObject PersonaPNG, ParejaPNG;
    public List<GameObject> Personas = new List<GameObject>();
    public float VOffset = 150f;
    private int CounterID = 1;
    private GameObject RootPersonPNG;

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
        Personas = PeopleList.ExistingPeople;
        if (RootPersonPNG != null) Destroy(RootPersonPNG);
        OrganizeUp(FindPerson(RootPersonID));
    }
    private void OnDisable()
    {
        if (RootPersonPNG != null) Destroy(RootPersonPNG);
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
                    PersonPNG.transform.SetParent(PNG.transform);
                }
            }

            PersonPNG.transform.SetParent(transform);
            if(FirstPersonData.ID != RootPersonID) PersonPNG.transform.localPosition = new Vector3(60f, VOffset, 0f);
            else PersonPNG.transform.localPosition = new Vector3(0f, 0f, 0f);

            if (FirstPersonData == FindPerson(RootPersonID)) RootPersonPNG = PersonPNG;
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
                CreateSiblings(FirstPersonData);
                CreateSiblings(SecondPersonData);
            }
            GameObject PairPNG = Instantiate(ParejaPNG, PairPosition, Quaternion.identity);
            
            PairPNG.transform.name = FirstPersonData.FirstName + "&" + SecondPersonData.FirstName;

            OrganizePNG LeftPData = PairPNG.transform.Find("LeftPPNG").GetComponent<OrganizePNG>();
            OrganizePNG RightPData = PairPNG.transform.Find("RightPPNG").GetComponent<OrganizePNG>();

            if (FirstPersonData.Gender == Genders.Male)
            {
                LeftPData.Humano = FirstPersonData;
                LeftPData.ID = FirstPersonData.ID;

                RightPData.Humano = SecondPersonData;
                RightPData.ID = SecondPersonData.ID;
            }
            else if (FirstPersonData.Gender == Genders.Female)
            {
                RightPData.Humano = FirstPersonData;
                RightPData.ID = FirstPersonData.ID;

                LeftPData.Humano = SecondPersonData;
                LeftPData.ID = SecondPersonData.ID;
            }

            GameObject[] PNGs = GameObject.FindGameObjectsWithTag("PersonPNG");
            foreach (GameObject PNG in PNGs)
            {
                OrganizePNG PNGData = PNG.GetComponent<OrganizePNG>();
                if (LeftPData.Humano.ChildrenID.Contains(PNGData.ID) && RightPData.Humano.ChildrenID.Contains(PNGData.ID))
                {
                    PairPNG.transform.SetParent(PNG.transform);
                    PairPNG.transform.localPosition = PairPosition;
                }
            }

            if (FirstPersonData == FindPerson(RootPersonID)) RootPersonPNG = PairPNG;
        }

        //----------------------------------------//

        CreateParents(FirstPersonData);
        /*Debug.Log(FirstPersonData.ID + "_" + FirstPersonData.FirstName + FirstPersonData.Surname1 +
            " - Total: " + CountAncestors(FirstPersonData));*/

        if (SecondPersonData != null)
        {
            CreateParents(SecondPersonData);
            /*Debug.Log(SecondPersonData.ID + "_" + SecondPersonData.FirstName + SecondPersonData.Surname1 +
                    " - Total: " + CountAncestors(SecondPersonData));*/

            if (FirstPersonData.ID == FindPerson(RootPersonID).FatherID || FirstPersonData.ID == FindPerson(RootPersonID).MotherID &&
                SecondPersonData.ID == FindPerson(RootPersonID).FatherID || SecondPersonData.ID == FindPerson(RootPersonID).MotherID)
            {
                CreateSiblings(FirstPersonData);
                CreateSiblings(SecondPersonData);
            }
        }

        if (FirstPersonData.ID == RootPersonID) CreateSiblings(FirstPersonData);
    }
    void CreateSiblings(Persona Person)
    {
        if (Person.SiblingsID.Count > 0)
        {
            List<Persona> SiblingsList = new List<Persona>();
            foreach (int SiblingID in Person.SiblingsID)
            {
                SiblingsList.Add(FindPerson(SiblingID));
            }

            var SiblingsOrdered = SiblingsList
                .OrderBy(p => p.BirthDate.Year)
                .ThenBy(p => p.BirthDate.Month)
                .ThenBy(p => p.BirthDate.Day)
                .ThenBy(p => p.FirstName)
                .ToList();

            float HOffset = 0f;
            GameObject[] PNGs = GameObject.FindGameObjectsWithTag("PersonPNG");

            bool PrevSiblingPair = false;

            foreach (Persona Sibling in SiblingsOrdered)
            {
                foreach (GameObject PNG in PNGs)
                {
                    OrganizePNG PNGData = PNG.GetComponent<OrganizePNG>();
                    if (PNGData.ID == Person.ID)
                    {
                        int ChildrenIndex = PNG.gameObject.transform.GetSiblingIndex(); // Índice del familiar parent
                        if (Person.ID == RootPersonID)
                        {
                            SiblingsList.Add(FindPerson(RootPersonID));
                            SiblingsOrdered = SiblingsList
                                .OrderBy(p => p.BirthDate.Year)
                                .ThenBy(p => p.BirthDate.Month)
                                .ThenBy(p => p.BirthDate.Day)
                                .ThenBy(p => p.FirstName)
                                .ToList();

                            if (FindPerson(RootPersonID).PartnerID == 0)
                            {
                                HOffset += (SiblingsOrdered.IndexOf(Sibling) < SiblingsOrdered.IndexOf(FindPerson(RootPersonID))) ? -120f : 120f;
                            }
                            else
                            {
                                HOffset += (SiblingsOrdered.IndexOf(Sibling) > SiblingsOrdered.IndexOf(FindPerson(RootPersonID))) ? -240f : 240f;
                            }
                        }
                        else
                        {
                            if (Sibling.PartnerID != 0) // Si tiene pareja
                            {
                                if (SiblingsOrdered.IndexOf(Sibling) == 0) // Primer hermano
                                {
                                    HOffset += (ChildrenIndex == 1) ? -180f :
                                    (ChildrenIndex == 2 && Person.ID != RootPersonID) ? 180f : 0f;
                                }
                                else
                                {
                                    if (ChildrenIndex == 1)
                                    {
                                        HOffset += PrevSiblingPair ? -240f : -180;
                                    }
                                    else if (ChildrenIndex == 2)
                                    {
                                        HOffset += PrevSiblingPair ? 240f : 180;
                                    }
                                }
                                PrevSiblingPair = true;
                            }
                            else if (Sibling.PartnerID == 0) // Si no tiene pareja
                            {
                                if (SiblingsOrdered.IndexOf(Sibling) == 0) // Primer hermano
                                {
                                    HOffset += (ChildrenIndex == 1) ? -120f :
                                    (ChildrenIndex == 2 && Person.ID != RootPersonID) ? 120f : 0f;
                                }
                                else
                                {
                                    if (ChildrenIndex == 1)
                                    {
                                        HOffset += PrevSiblingPair ? -180f : -120;
                                    }
                                    else if (ChildrenIndex == 2)
                                    {
                                        HOffset += PrevSiblingPair ? 180f : 120;
                                    }
                                }
                                PrevSiblingPair = false;
                            }
                        }
                    }
                }

                //----CREAR EL OBJETO PAREJA O PERSONA----//

                if (Sibling.PartnerID == 0) // Hermano/a soltero
                {
                    Vector3 SiblingPosition = new Vector3(HOffset, 0f, 0f);

                    GameObject SiblingSinglePNG = Instantiate(PersonaPNG, SiblingPosition, Quaternion.identity);
                    OrganizePNG SiblingData = SiblingSinglePNG.GetComponent<OrganizePNG>();

                    SiblingData.Humano = Sibling;
                    SiblingData.ID = Sibling.ID;

                    SiblingSinglePNG.transform.name = Sibling.FirstName;

                    foreach (GameObject PNG in PNGs)
                    {
                        OrganizePNG PNGData = PNG.GetComponent<OrganizePNG>();
                        if (PNGData.ID == Person.ID)
                        {
                            SiblingSinglePNG.transform.SetParent(PNG.transform);
                            SiblingSinglePNG.transform.localPosition = SiblingPosition;
                        }
                    }
                }
                else // Hermano en pareja
                {
                    Vector3 SiblingPosition = new Vector3(HOffset, 0f, 0f);

                    GameObject SiblingPairPNG = Instantiate(ParejaPNG, SiblingPosition, Quaternion.identity);

                    SiblingPairPNG.transform.name = Sibling.FirstName + "&" + FindPerson(Sibling.PartnerID).FirstName;

                    OrganizePNG LeftPData = SiblingPairPNG.transform.Find("LeftPPNG").GetComponent<OrganizePNG>();
                    OrganizePNG RightPData = SiblingPairPNG.transform.Find("RightPPNG").GetComponent<OrganizePNG>();

                    if(Sibling.Gender != Genders.Other && FindPerson(Sibling.PartnerID).Gender != Genders.Other) // Si ninguno de los dos son género comodín
                    {
                        if (Sibling.Gender == Genders.Male)
                        {
                            LeftPData.Humano = Sibling;
                            RightPData.Humano = FindPerson(Sibling.PartnerID);
                        }
                        else
                        {
                            LeftPData.Humano = FindPerson(Sibling.PartnerID);
                            RightPData.Humano = Sibling;
                        }
                    }
                    else // Si alguno es género comodín
                    {
                        if(Sibling.Gender == Genders.Other && FindPerson(Sibling.PartnerID).Gender != Genders.Other
                            || Sibling.Gender != Genders.Other && FindPerson(Sibling.PartnerID).Gender == Genders.Other) // Si solo hay un género comodín
                        {
                            if (Sibling.Gender == Genders.Male)
                            {
                                LeftPData.Humano = Sibling;
                                RightPData.Humano = FindPerson(Sibling.PartnerID);
                            }
                            else if (Sibling.Gender == Genders.Female)
                            {
                                LeftPData.Humano = FindPerson(Sibling.PartnerID);
                                RightPData.Humano = Sibling;
                            }
                        }
                        else // Ambos son género comodín
                        {
                            LeftPData.Humano = Sibling; // El famliar a la izquierda
                            RightPData.Humano = FindPerson(Sibling.PartnerID); // La pareja a la derecha
                        }
                    }

                    foreach (GameObject PNG in PNGs)
                    {
                        OrganizePNG PNGData = PNG.GetComponent<OrganizePNG>();
                        if (PNGData.ID == Person.ID)
                        {
                            SiblingPairPNG.transform.SetParent(PNG.transform);
                            SiblingPairPNG.transform.localPosition = SiblingPosition;
                        }
                    }
                }
            }
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
    static public Persona FindPerson(int ID)
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
public static class RandomVector3
{
    public static Vector3 GenerateRandomV3()
    {
        float rangoX = 15f, rangoZ = 15f;
        float x = Random.Range(-rangoX, rangoX);
        float y = 2.5f;
        float z = Random.Range(-rangoZ, rangoZ);

        return new Vector3(x, y, z);
    }
}