using System.Collections;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [Header("스태미나 설정")]
    public float maxStamina = 100f;

    [Header("회복 설정")]
    public float exhaustDuration = 3f; // 스태미나 0일 때 행동 제한 시간

    [HideInInspector] public float currentStamina;
    private bool isExhausted = false;

    void Start()
    {
        currentStamina = maxStamina;
    }




    // 공격/스킬 등 특정 행동 시 호출
    public void UseStamina(float amount)
    {
        if (isExhausted)
        {
            Debug.Log("⚠ 지쳐서 스태미나 사용 불가");
            return;
        }


        if (currentStamina >= amount)
        {
            currentStamina -= amount;
            Debug.Log($"⚡ 스태미나 사용: -{amount}, 현재 {currentStamina}");
            if (currentStamina <= 0)
            {
                Exhaust(); // 부족하면 지친 상태
            }
        }
        else

        {
            Exhaust(); // 부족하면 지친 상태
        }
    }



    // 달리기나 행동 시 스태미나 닳지 않음 → UseStamina 호출 제거
    // 회복은 수동
    public void Heal(float amount)
    {
        currentStamina += amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        Debug.Log($"💊 스태미나 회복: +{amount}, 현재 {currentStamina}");
    }

    public void Exhaust()
    {
        if (!isExhausted)
            StartCoroutine(ExhaustRoutine());
    }

    private IEnumerator ExhaustRoutine()
    {
        isExhausted = true;
        Debug.Log("⚡ 스태미나 0! 지친 상태 시작");
        yield return new WaitForSeconds(exhaustDuration);
        isExhausted = false;
        Debug.Log("💪 지친 상태 종료");
    }

    public bool IsExhausted() => isExhausted;
    public bool HasStamina(float amount) => currentStamina >= amount;
}
