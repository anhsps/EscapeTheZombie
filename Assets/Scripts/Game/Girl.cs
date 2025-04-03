using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Girl : MonoBehaviour
{
    SpriteRenderer sr;
    [SerializeField] private Sprite s_free, s_climb;
    [SerializeField] private float speed = 2f;
    private bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
            transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SoundManager19.Instance.PlaySound(6);
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(GirlFree());
            collision.GetComponent<Player>().IncreaseGirl();
        }
    }

    private IEnumerator GirlFree()
    {
        sr.sprite = s_free;
        yield return new WaitForSeconds(0.5f);
        sr.sprite = s_climb;
        canMove = true;
        Destroy(gameObject, 3f);
    }
}
