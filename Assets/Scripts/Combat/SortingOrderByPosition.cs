using UnityEngine;

public class SortingOrderByPosition : MonoBehaviour {

    private SpriteRenderer _renderer;

	void Awake ()
    {
        _renderer = GetComponent<SpriteRenderer>();
        RefreshOrder();
    }
	
    public void RefreshOrder()
    {
        float y = -transform.position.y;
        _renderer.sortingOrder = (int) y * 2;
    }
}
