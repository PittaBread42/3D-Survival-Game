using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Blueprint : MonoBehaviour
{

    public string ItemName;

    public string Req1;
    public string Req2;

    public int Req1Amount;
    public int Req2Amount;

    public int numOfRequirements;

    public Blueprint(string name, int reqNUM, string R1, int R1num, string R2, int R2num)
    {
        ItemName = name;

        numOfRequirements = reqNUM;

        Req1 = R1;
        Req2 = R2;

        Req1Amount = R1num;
        Req2Amount = R2num;

    }
}
