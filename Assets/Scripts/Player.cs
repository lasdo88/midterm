using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Joystick joystick;
    public GameObject bullet;
    public GameObject firePoint;
    public float ControlTime = 0;

    private float passedTime = 0;
    private float timerInterval = 0.3f;
    private CharacterController cc;

    private GameObject focusEnemy;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (ControlTime != 1)
        {
            GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");

        float miniDist = 9999;
        foreach (GameObject enemy in enemys)
        {
            // 計算距離
            float d = Vector3.Distance(transform.position, enemy.transform.position);

            // 跟上一個最近的比較，有比較小就記錄下來
            if (d < miniDist)
            {
                miniDist = d;
                focusEnemy = enemy;
            }
        }

            passedTime += Time.deltaTime;
            float h = joystick.Horizontal;
            float v = joystick.Vertical;
            Vector3 dir = new Vector3(h, 0, v);

            if (dir.magnitude > 0.1f)
        {
            // 將方向向量轉為角度
            float faceAngle = Mathf.Atan2(h, v) * Mathf.Rad2Deg;

            // 使用 Lerp 漸漸轉向
            Quaternion targetRotation = Quaternion.Euler(0, faceAngle, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.3f);
        }
        else
        {
            // 沒有移動輸入，並且有鎖定的敵人，漸漸面向敵人
            if (focusEnemy)
            {
                var targetRotation = Quaternion.LookRotation(focusEnemy.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 20 * Time.deltaTime);
            }
        }

            if (!cc.isGrounded)
            {
                dir.y = -9.8f * 30 * Time.deltaTime;
            }
            cc.Move(dir * Time.deltaTime * 10);

            if (Input.GetKey(KeyCode.Space))
            {
                if (passedTime >= timerInterval)
                {
                    Instantiate(bullet, firePoint.transform.position, firePoint.transform.rotation);
                    passedTime = 0;
                }
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Teleport")
            StartCoroutine(MyCoroutine());
    }

    IEnumerator MyCoroutine()
    {
        ControlTime = 1;
        yield return new WaitForSeconds(1);
        ControlTime = 0;
    }
}