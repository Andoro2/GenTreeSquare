using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraFollow : MonoBehaviour
{
    private static Transform target;
    public static int PersonIndex = 0;
    public TextMeshProUGUI Name;
    public float smoothTime = 0.3f;
    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    public static Transform Target { get => target; set => target = value; }

    void Update()
    {
        if(Target != null)
        {
            Persona Target = PeopleList.PeopleRegistry.Registry[PersonIndex];
            if(Target.SecondName != "") Name.text = Target.FirstName + " " + Target.SecondName + " " + Target.Surname1;
            else Name.text = Target.FirstName + " " + Target.Surname1;

            Vector3 targetPos = CameraFollow.Target.position + offset;

            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        }
        else Target = PeopleList.ExistingPeople[PersonIndex].transform;
    }
    public void NextPerson()
    {
        if (PersonIndex < PeopleList.ExistingPeople.Count - 1) PersonIndex++;
        Target = PeopleList.ExistingPeople[PersonIndex].transform;
    }
    public void PrevPerson()
    {
        if (PersonIndex > 0) PersonIndex--;
        Target = PeopleList.ExistingPeople[PersonIndex].transform;
    }
    static public void ChangeTarget(int ID)
    {
        foreach(GameObject Persona in PeopleList.ExistingPeople)
        {
            if (Persona.GetComponent<Person>().Humano.ID == ID) Target = Persona.transform;
            PersonIndex = ID - 1;
        }
    }
}
