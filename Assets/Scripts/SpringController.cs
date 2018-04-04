using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpringController : MonoBehaviour {

    public SpringJoint spring;
    public Slider springCoefficientSlider;

    // Use this for initialization
    void Start () {
        this.spring.spring = this.springCoefficientSlider.value;
    }
	
	// Update is called once per frame
	void Update () {
        this.spring.spring = this.springCoefficientSlider.value;
	}
}
