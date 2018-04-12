using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpringController : MonoBehaviour {

    public Spring spring;
    public Rigidbody load;
    
    public Slider springCoefficientSlider;
    public Slider loadMassSlider;

    void setSpringValues()
    {
        this.spring.constant = this.springCoefficientSlider.value;
        this.load.mass = this.loadMassSlider.value;
    }

    // Use this for initialization
    void Start ()
    {
        this.setSpringValues();
    }
	
	// Update is called once per frame
	void Update ()
    {
        this.setSpringValues();
    }
}
