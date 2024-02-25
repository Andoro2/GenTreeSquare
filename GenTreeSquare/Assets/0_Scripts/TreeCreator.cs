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

        //OrganizeUp(FindPerson(RootPersonID));

        if (FindPerson(RootPersonID).PartnerID != 0) OrganizeUp(FindPerson(RootPersonID), FindPerson(FindPerson(RootPersonID).PartnerID));
        else OrganizeUp(FindPerson(RootPersonID));
        OrganizeDown(FindPerson(RootPersonID));
    }
    private void OnDisable()
    {
        if (RootPersonPNG != null) Destroy(RootPersonPNG);
    }
    void OrganizeDown(Persona Person)
    {
        if(Person.ChildrenID.Count > 0)
        {
            GameObject Children = new GameObject("Children");

            GameObject[] PNGs = GameObject.FindGameObjectsWithTag("PersonPNG");
            foreach (GameObject PNG in PNGs)
            {
                OrganizePNG PNGData = PNG.GetComponent<OrganizePNG>();
                if (PNGData.Humano.ID == Person.ID) // Una vez obtenido el PNG de la persona a posicionar
                {
                    Children.transform.SetParent(PNG.transform.parent);
                    Children.transform.localPosition = new Vector3(0f, 0f, 0f);
                    Children.transform.localScale = new Vector3(1f, 1f, 1f);
                }
            }

            List<Persona> ChildrenList = new List<Persona>();
            foreach (int ChildID in Person.ChildrenID)
            {
                ChildrenList.Add(FindPerson(ChildID));
            }

            var ChildrenOrdered = ChildrenList
                    .OrderBy(p => p.BirthDate.Year)
                    .ThenBy(p => p.BirthDate.Month)
                    .ThenBy(p => p.BirthDate.Day)
                    .ThenBy(p => p.FirstName)
                    .ToList();

            bool PrevChildPair = false;

            float HOffset = 0f;

            foreach (Persona Child in ChildrenOrdered)
            {
                // Cálculo de la posición

                if ((CountDescendants(Person).Item1 + CountDescendants(Person).Item2) > 2) // Más de dos descendencies
                {
                    if (ChildrenOrdered.IndexOf(Child) == 0) // Primer hermano
                    {
                        HOffset += -60;

                        for (int i = 0; i < (ChildrenOrdered.Count / 2); i++)
                        {
                            HOffset += (Child.PartnerID == 0) ? -120 : -60;
                        }
                    }
                }
                else // Dos descendientes o menos
                {
                    if (ChildrenOrdered.IndexOf(Child) == 0 && Child.SiblingsID.Count > 0)
                    {
                        if (Child.PartnerID == 0) // Primer hermano sin pareja
                        {
                            if (FindPerson(Child.SiblingsID[0]).PartnerID == 0) // Ambos hermanos sin pareja
                            {
                                HOffset += -30;
                            }
                            else // Primero sin segundo con pareja
                            {
                                HOffset += -120;
                            }
                        }
                        else // Primero hermano con pareja
                        {
                            if(FindPerson(Child.SiblingsID[0]).PartnerID == 0) // Primero con segundo sin
                            {
                                HOffset += -30;
                            }
                            else // Ambos hermanos con pareja
                            {
                                HOffset += -60;
                            }
                        }
                    }
                }

                if (ChildrenOrdered.IndexOf(Child) != 0) 
                { 
                    if(Child.PartnerID != 0) HOffset += (PrevChildPair) ? 120 : 60;
                    else HOffset += (PrevChildPair) ? 90 : 60;
                }

                //----CREAR EL OBJETO PAREJA O PERSONA----//

                if (Child.PartnerID == 0) // Hijo/a sin pareja
                {
                    PrevChildPair = false;

                    Vector3 ChildPosition = new Vector3(HOffset, -VOffset, 0f);
                    GameObject ChildSinglePNG = Instantiate(PersonaPNG, ChildPosition, Quaternion.identity);

                    OrganizePNG ChildData = ChildSinglePNG.GetComponent<OrganizePNG>();

                    ChildData.Humano = Child;

                    ChildSinglePNG.transform.name = Child.FirstName;

                    ChildSinglePNG.transform.SetParent(Children.transform);
                    ChildSinglePNG.transform.localPosition = ChildPosition;
                }
                else // Hijo/a con pareja
                {
                    PrevChildPair = true;

                    Vector3 ChildPosition = new Vector3(HOffset, -VOffset, 0f);

                    GameObject ChildPairPNG = Instantiate(ParejaPNG, ChildPosition, Quaternion.identity);

                    OrganizePNG LeftPData = ChildPairPNG.transform.Find("LeftPPNG").GetComponent<OrganizePNG>();
                    OrganizePNG RightPData = ChildPairPNG.transform.Find("RightPPNG").GetComponent<OrganizePNG>();

                    if (Child.Gender != Genders.Other && FindPerson(Child.PartnerID).Gender != Genders.Other) // Si ninguno de los dos son género comodín
                    {
                        if (Child.Gender == Genders.Male)
                        {
                            LeftPData.Humano = Child;
                            RightPData.Humano = FindPerson(Child.PartnerID);
                        }
                        else
                        {
                            LeftPData.Humano = FindPerson(Child.PartnerID);
                            RightPData.Humano = Child;
                        }
                    }
                    else // Si alguno es género comodín
                    {
                        if (Child.Gender == Genders.Other && FindPerson(Child.PartnerID).Gender != Genders.Other
                            || Child.Gender != Genders.Other && FindPerson(Child.PartnerID).Gender == Genders.Other) // Si solo hay un género comodín
                        {
                            if (Child.Gender == Genders.Male)
                            {
                                LeftPData.Humano = Child;
                                RightPData.Humano = FindPerson(Child.PartnerID);
                            }
                            else if (Child.Gender == Genders.Female)
                            {
                                LeftPData.Humano = FindPerson(Child.PartnerID);
                                RightPData.Humano = Child;
                            }
                        }
                        else // Ambos son género comodín
                        {
                            LeftPData.Humano = Child; // El famliar a la izquierda
                            RightPData.Humano = FindPerson(Child.PartnerID); // La pareja a la derecha
                        }
                    }

                    ChildPairPNG.transform.Find("LeftPPNG").GetComponent<OrganizePNG>().Humano.ID = LeftPData.Humano.ID;
                    ChildPairPNG.transform.Find("RightPPNG").GetComponent<OrganizePNG>().Humano.ID = RightPData.Humano.ID; ;
                    ChildPairPNG.transform.Find("LeftPPNG").name = LeftPData.Humano.ID + "_" + LeftPData.Humano.FirstName + LeftPData.Humano.Surname1;
                    ChildPairPNG.transform.Find("RightPPNG").name = RightPData.Humano.ID + "_" + RightPData.Humano.FirstName + RightPData.Humano.Surname1;

                    ChildPairPNG.name = Child.FirstName + "&" + FindPerson(Child.PartnerID).FirstName;

                    ChildPairPNG.transform.SetParent(Children.transform);
                    ChildPairPNG.transform.localPosition = ChildPosition;
                }

                OrganizeDown(Child);
            }
        }    
    }
    void OrganizeUp(Persona FirstPersonData, Persona SecondPersonData = null)
    {
        float VOffset = 125f;
        // Crear a el PNG de la persona individual en caso de no tener asignada una pareja,
        // mientras que si tiene pareja asignada, creara el PNG combinado
        if (SecondPersonData == null)
        {
            GameObject PersonPNG = Instantiate(PersonaPNG, new Vector3(0f, 0f, 0f), Quaternion.identity);

            OrganizePNG SingleData = PersonPNG.GetComponent<OrganizePNG>();
            SingleData.Humano = FirstPersonData;
            SingleData.Humano.ID = FirstPersonData.ID;

            GameObject[] PNGs = GameObject.FindGameObjectsWithTag("PersonPNG");
            foreach (GameObject PNG in PNGs)
            {
                OrganizePNG PNGData = PNG.GetComponent<OrganizePNG>();
                if (SingleData.Humano.ChildrenID.Contains(PNGData.Humano.ID))
                {
                    PersonPNG.transform.SetParent(PNG.transform);
                }
            }

            //PersonPNG.transform.SetParent(transform);
            if (FirstPersonData != FindPerson(RootPersonID)) PersonPNG.transform.localPosition = new Vector3(60f, VOffset, 0f);
            else // Si eres RootPerson
            {
                PersonPNG.transform.SetParent(transform);
                PersonPNG.transform.localPosition = new Vector3(0f, 0f, 0f);
                RootPersonPNG = PersonPNG;
                //CreateSiblings(FirstPersonData);
            }
        }
        else
        {
            Vector3 PairPosition = new Vector3(0f, 0f, 0f);

            // Si no eres el pare o mare de la RootPerson
            if (FirstPersonData.ID != FindPerson(RootPersonID).FatherID && FirstPersonData.ID != FindPerson(RootPersonID).MotherID &&
                SecondPersonData.ID != FindPerson(RootPersonID).FatherID && SecondPersonData.ID != FindPerson(RootPersonID).MotherID)
            {
                int HOffset = 0;
                if (FirstPersonData.ChildrenID.Count > 0 && SecondPersonData.ChildrenID.Count > 0)
                {
                    if (FindPerson(FirstPersonData.ChildrenID[0]).Gender == Genders.Male)
                    {
                        HOffset = -60 - 120 * (CountAncestors(FindPerson(SecondPersonData.ID)));
                    }
                    else if (FindPerson(FirstPersonData.ChildrenID[0]).Gender == Genders.Female)
                    {
                        HOffset = 60 + 120 * (CountAncestors(FindPerson(FirstPersonData.ID)));
                    }
                }

                PairPosition = new Vector3(HOffset, VOffset, 0f);
            }
            // Si eres el pare o mare de la RootPerson
            else
            {
                int HOffset = 0;
                GameObject[] Parents = GameObject.FindGameObjectsWithTag("PersonPNG");
                foreach (GameObject PNG in Parents)
                {
                    OrganizePNG PNGData = PNG.GetComponent<OrganizePNG>();
                    if(FindPerson(RootPersonID).PartnerID == 0)
                    {
                        HOffset = 0;
                    }
                    // Si eres el padre o la madre y el hijo tiene pareja
                    else if (PNGData.Humano.FatherID == FirstPersonData.ID || PNGData.Humano.MotherID == FirstPersonData.ID &&
                        PNGData.Humano.FatherID == SecondPersonData.ID || PNGData.Humano.MotherID == SecondPersonData.ID)
                    {
                        HOffset = (PNG.gameObject.transform.GetSiblingIndex() == 1) ? 60 : -60;
                    }
                }

                PairPosition = new Vector3(HOffset, VOffset, 0f);
                //CreateSiblings(FirstPersonData);
                //if(FirstPersonData.ID != RootPersonID) CreateSiblings(SecondPersonData);
            }

            GameObject PairPNG = Instantiate(ParejaPNG, PairPosition, Quaternion.identity);
            
            PairPNG.transform.name = FirstPersonData.FirstName + "&" + SecondPersonData.FirstName;

            OrganizePNG LeftPData = PairPNG.transform.Find("LeftPPNG").GetComponent<OrganizePNG>();
            OrganizePNG RightPData = PairPNG.transform.Find("RightPPNG").GetComponent<OrganizePNG>();

            if (FirstPersonData.Gender == Genders.Male)
            {
                LeftPData.Humano = FirstPersonData;
                LeftPData.Humano.ID = FirstPersonData.ID;

                RightPData.Humano = SecondPersonData;
                RightPData.Humano.ID = SecondPersonData.ID;
            }
            else if (FirstPersonData.Gender == Genders.Female)
            {
                RightPData.Humano = FirstPersonData;
                RightPData.Humano.ID = FirstPersonData.ID;

                LeftPData.Humano = SecondPersonData;
                LeftPData.Humano.ID = SecondPersonData.ID;
            }

            GameObject[] PNGs = GameObject.FindGameObjectsWithTag("PersonPNG");
            foreach (GameObject PNG in PNGs)
            {
                OrganizePNG PNGData = PNG.GetComponent<OrganizePNG>();
                if (LeftPData.Humano.ChildrenID.Contains(PNGData.Humano.ID) && RightPData.Humano.ChildrenID.Contains(PNGData.Humano.ID))
                {
                    PairPNG.transform.SetParent(PNG.transform);
                    PairPNG.transform.localPosition = PairPosition;
                }
            }

            if (FirstPersonData == FindPerson(RootPersonID))
            {
                PairPNG.transform.SetParent(transform);
                PairPNG.transform.localPosition = new Vector3(0f, 0f, 0f);
                RootPersonPNG = PairPNG;
                //CreateSiblings(FirstPersonData);
            }
        }

        //----------------------------------------//

        CreateParents(FirstPersonData);
        CreateSiblings(FirstPersonData);

        if (FirstPersonData.ID != RootPersonID && SecondPersonData != null)
        {
            CreateParents(SecondPersonData);
            CreateSiblings(SecondPersonData);
        }
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
            if (Person.ID == RootPersonID) SiblingsList.Add(FindPerson(RootPersonID));

            var SiblingsOrdered = SiblingsList
                .OrderBy(p => p.BirthDate.Year)
                .ThenBy(p => p.BirthDate.Month)
                .ThenBy(p => p.BirthDate.Day)
                .ThenBy(p => p.FirstName)
                .ToList();

            float HOffset = 0f;

            HOffset -= 60;

            GameObject[] PNGs = GameObject.FindGameObjectsWithTag("PersonPNG");

            if (Person.ID == FindPerson(RootPersonID).FatherID) SiblingsOrdered.Reverse();

            bool PrevSiblingPair = false;

            int PersonIndex = SiblingsOrdered.IndexOf(Person);

            foreach (Persona Sibling in SiblingsOrdered)
            {
                // Cálculo de la posición
                foreach (GameObject PNG in PNGs)
                {
                    OrganizePNG PNGData = PNG.GetComponent<OrganizePNG>();
                    if (PNGData.Humano.ID == Person.ID)
                    {
                        int ChildrenIndex = PNG.gameObject.transform.GetSiblingIndex(); // Detectar si es le padre o la madre
                        if (Person.ID == RootPersonID) // Eres la RootPerson
                        {
                            HOffset = 0f;

                            if (SiblingsOrdered.IndexOf(Sibling) < SiblingsOrdered.IndexOf(Person))
                            {
                                for (int i = PersonIndex; i > SiblingsOrdered.IndexOf(Sibling); i--) // Crear anteriores
                                {
                                    HOffset -= (FindPerson(Sibling.ID).PartnerID == 0) ? 120f : 240;
                                }
                            }
                            else
                            {
                                for (int i = PersonIndex; i < SiblingsOrdered.IndexOf(Sibling); i++) // Crear siguientes
                                {
                                    HOffset += (FindPerson(Sibling.ID).PartnerID == 0) ? 120 : 240;
                                }
                            }

                            if (Person.PartnerID != 0) // Si el hermano raíz no tiene pareja
                            {
                                HOffset += (ChildrenIndex == 1) ? 60 : -60;
                            }

                            PrevSiblingPair = ((FindPerson(Sibling.ID).PartnerID == 0)) ? true : false;
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
                                    if (ChildrenIndex == 1) // Si es el padre
                                    {
                                        HOffset += PrevSiblingPair ? -240f : -180;
                                    }
                                    else if (ChildrenIndex == 2) // Si es la madre
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
                                    if (ChildrenIndex == 1) // Si es el padre
                                    {
                                        HOffset += PrevSiblingPair ? -180f : -120;
                                    }
                                    else if (ChildrenIndex == 2) // Si es la madre
                                    {
                                        HOffset += PrevSiblingPair ? 180f : 120;
                                    }
                                }
                                PrevSiblingPair = false;
                            }

                            if (Person.PartnerID != 0 && SiblingsOrdered.IndexOf(Sibling) == 0) // Si el hermano raíz no tiene pareja
                            {
                                HOffset += (ChildrenIndex == 1) ? -60 : 60;
                            }
                        }
                    }
                }

                //----CREAR EL OBJETO PAREJA O PERSONA----//
                if(Sibling != FindPerson(RootPersonID))
                {
                    if (Sibling.PartnerID == 0) // Hermano/a soltero
                    {
                        Vector3 SiblingPosition = new Vector3(HOffset, 0f, 0f);

                        GameObject SiblingSinglePNG = Instantiate(PersonaPNG, SiblingPosition, Quaternion.identity);

                        OrganizePNG SiblingData = SiblingSinglePNG.GetComponent<OrganizePNG>();

                        SiblingData.Humano = Sibling;
                        SiblingData.Humano.ID = Sibling.ID;

                        SiblingSinglePNG.transform.name = Sibling.FirstName;

                        foreach (GameObject PNG in PNGs)
                        {
                            OrganizePNG PNGData = PNG.GetComponent<OrganizePNG>();
                            if (PNGData.Humano.ID == Person.ID)
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

                        SiblingPairPNG.transform.Find("LeftPPNG").GetComponent<OrganizePNG>().Humano.ID = LeftPData.Humano.ID;
                        SiblingPairPNG.transform.Find("RightPPNG").GetComponent<OrganizePNG>().Humano.ID = RightPData.Humano.ID; ;
                        SiblingPairPNG.transform.Find("LeftPPNG").name = LeftPData.Humano.ID + "_" + LeftPData.Humano.FirstName + LeftPData.Humano.Surname1;
                        SiblingPairPNG.transform.Find("RightPPNG").name = RightPData.Humano.ID + "_" + RightPData.Humano.FirstName + RightPData.Humano.Surname1;

                        foreach (GameObject PNG in PNGs)
                        {
                            OrganizePNG PNGData = PNG.GetComponent<OrganizePNG>();
                            if (PNGData.Humano.ID == Person.ID)
                            {
                                SiblingPairPNG.transform.SetParent(PNG.transform);
                                SiblingPairPNG.transform.localPosition = SiblingPosition;
                            }
                        }
                    }
                }

                /*if (Person.ID == FindPerson(RootPersonID).FatherID ||
                    Person.ID == FindPerson(RootPersonID).MotherID)
                    OrganizeDown(Sibling);*/
                OrganizeDown(Sibling);
            }
        }
    }
    (int,int) CountDescendants(Persona Person)
    {
        int SingleDescendants = 0, PairDescendants = 0;

        if (Person.ChildrenID.Count > 0)
        {
            foreach(int ChildID in Person.ChildrenID)
            {
                if (FindPerson(ChildID).PartnerID != 0) PairDescendants++;
                else SingleDescendants++;
            }
        }

        return (SingleDescendants, PairDescendants);
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