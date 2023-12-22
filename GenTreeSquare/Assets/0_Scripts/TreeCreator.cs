using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TreeCreator : MonoBehaviour
{
    //public Persona RootPerson;
    public int RootPersonID;
    public GameObject PersonaPNG;
    public List<GameObject> PersonasPNG = new List<GameObject>();

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
        PersonasPNG = PeopleList.ExistingPeople;
    }
    private void OnEnable()
    {
        foreach (GameObject Persona in PersonasPNG)
        {
            //Persona.transform.parent = transform;
            //Persona.transform.position = new Vector3(0f, 0f, 0f);
            Debug.Log(Persona.GetComponent<OrganizePNG>().Humano.FirstName + MovementHorizontal(Persona));
            if (Persona.GetComponent<OrganizePNG>().Humano.ID == RootPersonID)
            {
                OrganizeSquare(Persona);
                Debug.Log(Persona.GetComponent<OrganizePNG>().Humano.FirstName);// + " " + Horizontal(Persona));
            }
        }
    }
    /*private void OnDisable()
    {
        foreach (GameObject Persona in PersonasPNG)
        {
            if (Persona != null)
            {
                Destroy(Persona);
            }
        }
    }*/
    public void CreatePersonPNG(GameObject Persona)
    {
        GameObject PersonPNG = Instantiate(PersonaPNG, new Vector3(0f, 0f, 0f), Quaternion.identity);
        PersonPNG.GetComponent<OrganizePNG>().ID = Persona.GetComponent<Person>().Humano.ID;
        PersonPNG.GetComponent<OrganizePNG>().AssignHuman();


        Persona PersonData = Persona.GetComponent<Person>().Humano;

        //PersonPNG.GetComponent<OrganizePNG>().Humano = PersonData;
        PersonPNG.GetComponent<OrganizePNG>().TC = GetComponent<TreeCreator>();
        //PersonPNG.GetComponent<OrganizePNG>().RootPerson = GetComponent<TreeCreator>().RootPerson.GetComponent<Person>().Humano;
        PersonPNG.transform.parent = transform;


        PersonPNG.transform.Find("Data").Find("Name").gameObject.transform.GetComponent<TextMeshProUGUI>().text = PersonData.FirstName + PersonData.SecondName;
        PersonPNG.transform.Find("Data").Find("Surnames").gameObject.transform.GetComponent<TextMeshProUGUI>().text = PersonData.Surname1 + " " + PersonData.Surname2;

        PersonPNG.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        PersonPNG.transform.localPosition = new Vector3(0f, 0f, 0f);

        PersonasPNG.Add(PersonPNG);
        //PersonaPNG.transform.localPosition = TreeOrganize(Persona);

        PersonPNG.name = PersonData.ID + "_" + PersonData.FirstName + PersonData.Surname1;
    }
    public void OrganizeSquare(GameObject Persona) //Persona en PNG
    {
        float Horizontal = 25f,
            Vertical = 75f;
        Persona PersonData = Persona.GetComponent<OrganizePNG>().Humano;

        if (PersonData.FatherID != 0) //Considera al pare com a monigote
        {
            foreach (GameObject HumanPNG in PersonasPNG)
            {
                if (PersonData.FatherID == HumanPNG.GetComponent<OrganizePNG>().Humano.ID) //Conseguixc al pare en PNG
                {
                    HumanPNG.transform.parent = Persona.transform;
                    /*if(MovementHorizontal(Persona) >= 1)
                    {
                        HumanPNG.transform.position = new Vector3(Persona.transform.position.x - Horizontal * MovementHorizontal(Persona),
                            Persona.transform.position.y + (Vertical), 0f);
                    }
                    else
                    {*/
                        HumanPNG.transform.position = new Vector3(Persona.transform.position.x - Horizontal * MovementHorizontal(Persona),
                            Persona.transform.position.y + (Vertical), 0f);
                    //}
                    OrganizeSquare(HumanPNG);
                }
            }
        }
        if (PersonData.MotherID != 0) //Considera a la mare com a monigote
        {
            foreach (GameObject HumanPNG in PersonasPNG)
            {
                if (PersonData.MotherID == HumanPNG.GetComponent<OrganizePNG>().Humano.ID) //Conseguixc a la mare en PNG
                {
                    HumanPNG.transform.parent = Persona.transform;
                    /*if (MovementHorizontal(Persona) >= 1)
                    {
                        HumanPNG.transform.position = new Vector3(Persona.transform.position.x + Horizontal * MovementHorizontal(Persona),
                            Persona.transform.position.y + (Vertical), 0f);
                    }
                    else
                    {*/
                        HumanPNG.transform.position = new Vector3(Persona.transform.position.x + Horizontal * MovementHorizontal(Persona),
                            Persona.transform.position.y + (Vertical), 0f);
                    //}
                    OrganizeSquare(HumanPNG);
                }
            }
        }
    }
    public int MovementHorizontal(GameObject Human)
    {
        int HMovement = 1;
        Persona PersonData = Human.GetComponent<OrganizePNG>().Humano;
        if(PersonData.Gender == Genders.Male && PersonData.MotherID != null)
        {
            foreach (GameObject HumanPNG in PersonasPNG)
            {
                if (PersonData.MotherID == HumanPNG.GetComponent<OrganizePNG>().Humano.ID)
                {
                    return HMovement * 2 * MovementHorizontal(HumanPNG);
                    //return "_" + HumanPNG.GetComponent<OrganizePNG>().Humano.FirstName + Horizontal(HumanPNG);
                }
            }
        }
        else if (PersonData.Gender == Genders.Female && PersonData.FatherID != null)
        {
            foreach (GameObject HumanPNG in PersonasPNG)
            {
                if (PersonData.FatherID == HumanPNG.GetComponent<OrganizePNG>().Humano.ID)
                {

                    return HMovement * 2 * MovementHorizontal(HumanPNG);
                    //return "_" + HumanPNG.GetComponent<OrganizePNG>().Humano.FirstName + Horizontal(HumanPNG);
                }
            }
        }
        return 1;
    }
}
