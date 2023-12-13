using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleList : MonoBehaviour
{
    [SerializeField]
    public static List<GameObject> ExistingPeople = new List<GameObject>();
    public List<GameObject> ExistingPeopleShow = new List<GameObject>();
    public int VerticalSeparation;
    
    public static int VerticalS;

    public TreeCreator TC;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        VerticalS = VerticalSeparation;

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

                TC.CreatePersonPNG(NewPerson);
            }
        }

        ExistingPeople = UpdatedList;

        ExistingPeopleShow = ExistingPeople;
    }
}
