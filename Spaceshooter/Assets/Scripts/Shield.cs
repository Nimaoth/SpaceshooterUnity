using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Shield : MonoBehaviour {

    public Slider shieldSlider;
    private Material material;
    private Image image;

    private Renderer renderer;
    private Collider collider;

    private int previousShield = 0;

	// Use this for initialization
	void Start () {
        var go = shieldSlider.transform.Find("Fill Area").Find("Fill");
        image = go.GetComponent<Image>();

        renderer = GetComponent<Renderer>();
        collider = GetComponent<Collider>();

        material = renderer.material;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (previousShield != Player.ShieldAmount)
        {
            UpdateStats();
        }

        previousShield = Player.ShieldAmount;
	}

    public void UpdateStats()
    {
        shieldSlider.value = Player.ShieldAmount;

        Color newColor = Color.Lerp(Color.red, Color.green, Player.ShieldAmount / 100.0f);

        image.color = newColor;
        material.color = new Color(newColor.r, newColor.g, newColor.b, 0.5f);
    }
}
