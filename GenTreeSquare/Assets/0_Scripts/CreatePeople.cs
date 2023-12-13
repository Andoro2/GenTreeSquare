using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class CreatePeople : MonoBehaviour
{
    public List<Persona> Personas = new List<Persona>();
    public GameObject Persona;
    private int CounterID = 1;

    public TMP_InputField FirstNameInput, SecondNameInput, NickNameInput,
        FirstSurnameInput, SecondSurnameInput;
    public TextMeshProUGUI BirthDateInput;
    //BirthDayInput, BirthMonthInput, BirthYearInput;
    public TMP_Dropdown SexDropDown, FatherDropdown, MotherDropdown, PartnerDropdown;//, SiblingsDropdown, ChildrenDropdown;

    public List<string> People = new List<string> { "Ninguno" };
    public List<GameObject> ExistingPeople = new List<GameObject>();
    public int PeopleCounter = 0;

    void Start()
    {
        ExistingPeople = PeopleList.ExistingPeople;
        //AddToDropdown(FatherDropdown);
        //AddToDropdown(MotherDropdown);
        //AddToDropdown(PartnerDropdown);
        //PeopleCounter = ExistingPeople.Count;
        //FatherDropdown.AddOptions(People);
        //MotherDropdown.AddOptions(People);
        //SiblingsDropdown.AddOptions(People);
        //ChildrenDropdown.AddOptions(People);
    }

    void Update()
    {
        if(PeopleCounter != ExistingPeople.Count)
        {
            GameObject[] PersonasArray = GameObject.FindGameObjectsWithTag("Person");
            foreach (GameObject persona in PersonasArray)
            {
                if (!ExistingPeople.Contains(persona))
                {
                    ExistingPeople.Add(persona);
                }
            }
            AddToDropdown(FatherDropdown);
            AddToDropdown(MotherDropdown);
            AddToDropdown(PartnerDropdown);

            PeopleCounter = ExistingPeople.Count;
        }
    }
    public void AddToDropdown(TMP_Dropdown PeopleDropdown)
    {
        for (int i = 0; i < ExistingPeople.Count; i++)
        {
            if (!People.Contains(ExistingPeople[i].GetComponent<Person>().Humano.FirstName))
            {
                People.Add(ExistingPeople[i].GetComponent<Person>().Humano.FirstName);
            }
        }
        PeopleDropdown.ClearOptions();
        PeopleDropdown.AddOptions(People);
    }

    public void Create()
    {
        if (FirstNameInput.text != "" && FirstSurnameInput.text != "" && ((FatherDropdown.value == 0 ||  MotherDropdown.value == 0) || FatherDropdown.value != MotherDropdown.value))
        {
            GameObject Penya = Instantiate(Persona, new Vector3(0f, 0f, 0.5f), Quaternion.identity);
            Penya.transform.Find("Canvas").Find("NameTMP").GetComponent<TextMeshProUGUI>().text = FirstNameInput.text;
            Penya.transform.parent = GameObject.FindWithTag("PeopleManager").gameObject.transform;

            Persona PersonData = Penya.GetComponent<Person>().Humano;
            Penya.name = (PeopleCounter + 1).ToString() + "_" + FirstNameInput.text + SecondNameInput.text;


            PersonData.FirstName = FirstNameInput.text;
            FirstNameInput.text = "";
            PersonData.SecondName = SecondNameInput.text;
            SecondNameInput.text = "";
            PersonData.NickName = NickNameInput.text;
            NickNameInput.text = "";
            PersonData.FirstSurname = FirstSurnameInput.text;
            FirstSurnameInput.text = "";
            PersonData.SecondSurname = SecondSurnameInput.text;
            SecondSurnameInput.text = "";

            if (BirthDateInput.text != "Select a date")
            {
                string FechaNacimiento = BirthDateInput.text;

                string anioString = FechaNacimiento.Substring(0, 4);
                string mesString = FechaNacimiento.Substring(5, 2);
                string diaString = FechaNacimiento.Substring(8, 2);

                if (int.TryParse(anioString, out int BirthYear) &&
                int.TryParse(mesString, out int BirthMonth) &&
                int.TryParse(diaString, out int BirthDay))
                {
                    PersonData.BirthDay = BirthDay;
                    PersonData.BirthMonth = BirthMonth;
                    PersonData.BirthYear = BirthYear;
                }

                BirthDateInput.text = "";
            }
            
            switch (SexDropDown.value)
                {
                    case 0:
                        PersonData.Gender = global::Persona.Genders.Male;
                        break;
                    case 1:
                        PersonData.Gender = global::Persona.Genders.Female;
                        break;
                    case 2:
                        PersonData.Gender = global::Persona.Genders.Other;
                        break;
                }

            if (FatherDropdown.value != 0) PersonData.Father = ExistingPeople[FatherDropdown.value - 1];
            if (MotherDropdown.value != 0) PersonData.Mother = ExistingPeople[MotherDropdown.value - 1];
            if (PartnerDropdown.value != 0) PersonData.Partner = ExistingPeople[PartnerDropdown.value - 1];

            //CounterID++;

            ExistingPeople.Add(Penya);
        }
        else if (FatherDropdown.value == MotherDropdown.value)
        {
            Debug.Log("No puede tener el mismo padre y madre.");
        }
        else if (FirstNameInput.text != "" || FirstSurnameInput.text != "")
        {
            Debug.Log("Rellena campo tanto nombre como apellido.");
        }
    }
    public void AutoCreate()
    {
        if ((CounterID - 1) != Personas.Count)
        {
            GameObject Penya = Instantiate(Persona, new Vector3(0f, 0f, 0.5f), Quaternion.identity);
            Penya.transform.Find("Canvas").Find("NameTMP").GetComponent<TextMeshProUGUI>().text = Personas[CounterID - 1].FirstName;
            Penya.transform.parent = GameObject.FindWithTag("PeopleManager").gameObject.transform;

            Persona PersonData = Penya.GetComponent<Person>().Humano;
            Penya.name = (CounterID + 1).ToString() + "_" + Personas[CounterID - 1].FirstName + Personas[CounterID - 1].FirstSurname;

            PersonData.FirstName = Personas[CounterID - 1].FirstName;
            PersonData.SecondName = Personas[CounterID - 1].SecondName;
            PersonData.NickName = Personas[CounterID - 1].NickName;
            PersonData.FirstSurname = Personas[CounterID - 1].FirstSurname;
            PersonData.SecondSurname = Personas[CounterID - 1].SecondSurname;
            PersonData.BirthDay = Personas[CounterID - 1].BirthDay;
            PersonData.BirthMonth = Personas[CounterID - 1].BirthMonth;
            PersonData.BirthYear = Personas[CounterID - 1].BirthYear;
            PersonData.Gender = Personas[CounterID - 1].Gender;

            CounterID++;
        }
    }
}

[System.Serializable]
public class Persona
{
    public string FirstName,
        SecondName,
        NickName,
        FirstSurname,
        SecondSurname;
    public Sprite Photo;
    public enum Genders { Male, Female, Other }
    [SerializeField] public Genders Gender;
    public int BirthDay, BirthMonth, BirthYear;
    public GameObject Father,
        Mother,
        Partner;
    public List<GameObject> Siblings = new List<GameObject>(),
        Children = new List<GameObject>();
}