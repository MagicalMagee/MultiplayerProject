using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText, pointsText;

    // Start is called before the first frame update
    public void Initialize(string name)
    {
        nameText.text = name;
    }

    public void setPoints(int points)
    {
        pointsText.text = points.ToString();
    }
}
