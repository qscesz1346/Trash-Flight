using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    
    [SerializeField]
    private GameObject[] weapons;
    private int weaponIndex = 0;

    [SerializeField]
    private Transform shootTransform;

    [SerializeField]
    private float shootInterval = 0.05f;
    private float lastShotTime = 0f;

    // Update is called once per frame
    void Update()
    {
        // float horizontalInput = Input.GetAxisRaw("Horizontal");
        // float verticalInput = Input.GetAxisRaw("Vertical");
        // Vector3 moveTo = new Vector3(horizontalInput, verticalInput, 0f);
        // transform.position += moveTo * moveSpeed * Time.deltaTime;

        // Vector3 moveTo = new Vector3(moveSpeed * Time.deltaTime, 0, 0);
        // if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
        //     transform.position -= moveTo;
        // } else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
        //     transform.position += moveTo;
        // }
        
        // Debug.Log(Input.mousePosition); 현재 마우스 위치 좌표 확인

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //현재 카메라 화면에 맞춰 좌표 재설정
        float toX = Mathf.Clamp(mousePos.x, -2.35f, 2.35f);  // Mathf.Clamp : value가 최대값보다 높으면 최대값을 최소값보다 작으면 최소값 적용
        transform.position = new Vector3(toX, transform.position.y, transform.position.z);

        if(GameManager.instance.isGameOver == false) {
            Shoot();
        }

    }

    void Shoot() {
        // 10 - 0 > 0.05
        // lastShotTime = 10;

        // 10.01 - 10 > 0.05 ? false
        // 10.02 - 10 > 0.05 ? false
        // ...

        // 10.06 - 10 > 0.05 true
        // lastShotTime = 10.06;

        if(Time.time - lastShotTime > shootInterval) {
            Instantiate(weapons[weaponIndex], shootTransform.position, Quaternion.identity);  // Instantiate : 어떤 오브젝트를 포지션에 어떤방식으로 만드는가
            lastShotTime = Time.time;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Boss") {
            GameManager.instance.SetGameOver();
            Destroy(gameObject);
        } else if (other.gameObject.tag == "Coin") {
            GameManager.instance.IncreaseCoin();
            Destroy(other.gameObject);
        }
    }

    public void Upgrade() {
        weaponIndex += 1;
        if(weaponIndex >= weapons.Length) {
            weaponIndex = weapons.Length - 1;
        }
    }

}
