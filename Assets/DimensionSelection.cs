using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DimensionSelection : MonoBehaviour
{
    public GameObject TwoDimensionOptions;
    public GameObject ThreeDimensionOptions;
    public Text Label;

    // Start is called before the first frame update
    void Start()
    {
        ThreeDimensionOptions.SetActive(false);
        TwoDimensionOptions.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Label.text == "2D" && ThreeDimensionOptions.activeSelf)
        {
            ThreeDimensionOptions.SetActive(false);
            TwoDimensionOptions.SetActive(true);
        }

        if (Label.text == "3D" && TwoDimensionOptions.activeSelf)
        {
            ThreeDimensionOptions.SetActive(true);
            TwoDimensionOptions.SetActive(false);
        }
    }
}
