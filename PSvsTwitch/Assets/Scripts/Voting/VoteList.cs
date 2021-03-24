using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Vote List", menuName = "Quiz/new Vote List")]
public class VoteList : ScriptableObject
{
    public List<Vote> VotesList = new List<Vote>(0);
}
