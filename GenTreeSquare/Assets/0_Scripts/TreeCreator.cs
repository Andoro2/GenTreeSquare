using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TreeCreator : MonoBehaviour
{
    //public Persona RootPerson;
    public GameObject RootPerson, PersonaPNG;
    public List<GameObject> ExistingPeople = new List<GameObject>(),
        PersonasPNG = new List<GameObject>();

    private int CounterID = 1;

    // Suscribe la función a OnNuevoElementoAgregado

    // Start is called before the first frame update
    void Start()
    {
        //PeopleList PeopleManager = FindObjectOfType<PeopleList>();
        //PeopleManager.OnPeopleCreated += CreatePersonPNG;
    }

    // Update is called once per frame
    void Update()
    {
        ExistingPeople = PeopleList.ExistingPeople;
    }
    /*private void OnEnable()
    {
        foreach (GameObject Persona in PersonasPNG)
        {
            Persona.transform.localPosition = new Vector3(GetAncestors(Persona.GetComponent<Person>().Humano), GetDescendents(Persona.GetComponent<Person>().Humano), 0f);
        }
    }*/

    public void CreatePersonPNG(GameObject Persona)
    {
        GameObject PersonPNG = Instantiate(PersonaPNG, new Vector3(0f, 0f, 0f), Quaternion.identity);
        PersonPNG.GetComponent<OrganizePNG>().Humano = Persona.GetComponent<Person>().Humano;
        PersonPNG.transform.parent = this.transform;

        Persona PersonData = Persona.GetComponent<Person>().Humano;

        PersonPNG.transform.Find("Name").gameObject.transform.GetComponent<TextMeshProUGUI>().text = PersonData.FirstName + PersonData.SecondName;
        PersonPNG.transform.Find("Surnames").gameObject.transform.GetComponent<TextMeshProUGUI>().text = PersonData.FirstSurname + " " + PersonData.SecondSurname;

        PersonPNG.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        PersonPNG.transform.localPosition = new Vector3(0f, 0f, 0f);

        PersonasPNG.Add(PersonPNG);
        //PersonaPNG.transform.localPosition = TreeOrganize(Persona);

        PersonPNG.name = CounterID.ToString() + "_" + PersonData.FirstName + PersonData.FirstSurname;
        CounterID++;
    }
    /*public Vector3 TreeOrganize()
    {
        
        Vector3 position = new Vector3(0f, 0f, 0f);
        return position;
    }*/
}
