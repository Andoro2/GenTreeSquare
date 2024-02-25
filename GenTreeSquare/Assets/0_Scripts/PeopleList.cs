using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using TMPro;

public class PeopleList : MonoBehaviour
{
    [SerializeField]
    public static List<GameObject> ExistingPeople = new List<GameObject>();
    public List<GameObject> ExistingPeopleShow = new List<GameObject>();
    [SerializeField]
    public static ListaPersonas PeopleRegistry = new ListaPersonas();
    public List<Persona> PeopleRegistryShow = new List<Persona>();

    public TreeCreator TC;

    private bool FirstActivation = true;

    // Start is called before the first frame update
    void Start()
    {
        //SaveToJSON();
        LoadFromJSON();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] People = GameObject.FindGameObjectsWithTag("Person");

        List<GameObject> UpdatedList = new List<GameObject>(People);

        for (int i = 0; i < UpdatedList.Count; i++)
        {
            for (int j = i + 1; j < UpdatedList.Count; j++)
            {
                if (UpdatedList[i] == UpdatedList[j])
                {
                    UpdatedList.RemoveAt(j);
                    j--;
                }
            }
        }

        foreach (GameObject NewPerson in UpdatedList)
        {
            if (!ExistingPeople.Contains(NewPerson))
            {
                ExistingPeople.Add(NewPerson);

                PeopleRegistry.Registry.Add(NewPerson.GetComponent<Person>().Humano);
                SaveToJSON();
            }
        }

        ExistingPeople = UpdatedList;

        ExistingPeopleShow = ExistingPeople;
        PeopleRegistryShow = PeopleRegistry.Registry;

        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveToJSON();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadFromJSON();
        }
        if (FirstActivation)
        {
            SaveToJSON();
            LoadFromJSON(FirstActivation);
            FirstActivation = false;
        }
    }
    public static void SaveToJSON()
    {
        string RegistryData = JsonUtility.ToJson(PeopleRegistry);
        string filePath = Application.persistentDataPath + "/RegistryData.json";

        Debug.Log(filePath);

        File.WriteAllText(filePath, RegistryData);
        //Debug.Log("Guardado");
    }
    private void LoadFromJSON(bool Repeat = true)
    {
        PeopleRegistry.Registry.Clear();

        string filePath = Application.persistentDataPath + "/RegistryData.json";
        string RegistryData = File.ReadAllText(filePath);
        PeopleRegistry = JsonUtility.FromJson<ListaPersonas>(RegistryData);

        HashSet<int> existingIDs = new HashSet<int>(ExistingPeople.Select(personObject => personObject.GetComponent<Person>().Humano.ID));

        List<Persona> PeopleToCreate = new List<Persona>();

        foreach (Persona loadedPerson in PeopleRegistry.Registry)
        {

            if (!existingIDs.Contains(loadedPerson.ID))
            {
                PeopleToCreate.Add(loadedPerson);
            }
            else
            {
                GameObject existingPersonObject = ExistingPeople.Find(personObject => personObject.GetComponent<Person>().Humano.ID == loadedPerson.ID);

                if (existingPersonObject != null)
                {
                    existingPersonObject.GetComponent<Person>().Humano = loadedPerson;
                }
            }
        }

        foreach (Persona PersonToCreate in PeopleToCreate)
        {
            CreatePersonFromJSON(PersonToCreate);
        }

        if (Repeat)
        {
            Repeat = false;
            LoadFromJSON(Repeat);
        }

        //Debug.Log("Cargado");
    }

    public GameObject PersonaPreFab;
    public void CreatePersonFromJSON(Persona Humano)
    {
        GameObject Penya = Instantiate(PersonaPreFab, RandomVector3.GenerateRandomV3(), Quaternion.identity);
        Penya.transform.Find("Canvas").Find("NameTMP").GetComponent<TextMeshProUGUI>().text = Humano.FirstName;
        Penya.transform.parent = GameObject.FindWithTag("PeopleManager").gameObject.transform;

        Persona PersonData = Penya.GetComponent<Person>().Humano;
        Penya.name = Humano.ID.ToString() + "_" + Humano.FirstName + Humano.Surname1;

        PersonData.ID = Humano.ID;
        PersonData.FirstName = Humano.FirstName;
        PersonData.SecondName = Humano.SecondName;
        PersonData.NickName = Humano.NickName;
        PersonData.Surname1 = Humano.Surname1;
        PersonData.Surname2 = Humano.Surname2;
        PersonData.BirthDate.Day = Humano.BirthDate.Day;
        PersonData.BirthDate.Month = Humano.BirthDate.Month;
        PersonData.BirthDate.Year = Humano.BirthDate.Year;
        PersonData.Gender = Humano.Gender;

        ExistingPeople.Add(Penya);

        //TC.CreatePersonPNG(Penya);

        PeopleRegistry.Registry.Add(Penya.GetComponent<Person>().Humano);
    }
}
/// <summary>
/// /////////////////////////////////
/// </summary>
public class ListaPersonas
{
    public List<Persona> Registry = new List<Persona>();
}
[Serializable]
public class Persona
{
    public string FirstName;
    public int ID;
    public string? SecondName;
    public string NickName;
    public string? Surname1;
    public string? Surname2;
    public Genders Gender;
    [SerializeField]
    public DateInfo BirthDate;
    [SerializeField]
    public DateInfo DeathDate;
    public int FatherID;
    public int MotherID;
    public int PartnerID;
    public List<int> SiblingsID = new List<int>();
    public List<int> ChildrenID = new List<int>();
}
public enum Genders { Male, Female, Other }
[Serializable]
public class DateInfo
{
    public int Day;
    public int Month;
    public int Year;
}