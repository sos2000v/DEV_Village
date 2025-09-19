using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Transform target;           // ���� Transform
    public Vector3 offset = new Vector3(0, 2f, 0);

    void LateUpdate()
    {
        if (target == null) return;

        // �Ӹ� �� ��ġ
        transform.position = target.position + offset;

        if (Camera.main != null)
        {
            Vector3 dir = transform.position - Camera.main.transform.position;

            // y�� ȸ���� ���� (HP�ٰ� ���� ����)
            dir.y = 0;

            if (dir != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    public void SetMaxHealth(float maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }
    }

    public void SetHealth(float health)
    {
        if (healthSlider != null)
        {
            healthSlider.value = health;
        }
    }
}

