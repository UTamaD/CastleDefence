using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum Monster1Notify
{
	None,
	Stun
}


public class StunData : NotifyBase
{
	
}


public class Monster1 : CharacterBase<FSM_Monster1>
{
	
	[System.Serializable]
	public class DamageTypeEffect
	{
		public GameObject effectPrefab;
		public string soundName;
	}
	
	
	public float Speed = 5.0f;
	public int DestinationIndex;
	public float MaxHealth = 100;
	public float CurrentHealth;
	
    
	private Animator animator;
	
	private SlowDebuff slowDebuff;
	private float originalSpeed;
	private bool isStunned = false;
	private float stunDuration = 0f;
	
	
	public GameObject slowDebuffEffectPrefab;
	public GameObject stunEffectPrefab;
	private GameObject currentDebuffEffect;
	private Renderer monsterRenderer;
	private Color originalColor;
	
	
	public DamageTypeEffect fireEffect;
	public DamageTypeEffect iceEffect;
	public DamageTypeEffect lightningEffect;

	public string deathSoundName = "MonsterDeath";
	private bool isDying = false;
	private bool isFullyDead = false;

	public int goldIncrease = 5;

	/// <summary>
	/// 몬스터의 스턴 또는 사망 상태 여부를 확인하는 함수
	/// </summary>
	/// <returns>스턴 또는 사망 상태면 true, 아니면 false</returns>
	public bool IsDeadOrStun()
	{
		return (isStunned || isDying);
	}
	
	protected override void Awake()
	{
		base.Awake();
		DestinationIndex = 0;
		CurrentHealth = MaxHealth;
		animator = GetComponent<Animator>();
		
		originalSpeed = Speed;
		
		monsterRenderer = GetComponentInChildren<Renderer>();
		originalColor = monsterRenderer.material.color;
		
		Speed *= UpgradeManager.Instance.GetEnemySpeedMultiplier();
	}
	private void Update()
	{
		if (slowDebuff != null)
		{
			slowDebuff.Update(Time.deltaTime);
			if (slowDebuff.IsExpired())
			{
				slowDebuff = null;
				UpdateMovementSpeed();
				if (currentDebuffEffect != null)
				{
					Destroy(currentDebuffEffect);
				}
			}
		}

		if (isStunned)
		{
			stunDuration -= Time.deltaTime;
			if (stunDuration <= 0)
			{
				isStunned = false;
				UpdateMovementSpeed();
				monsterRenderer.material.color = originalColor;
				animator.speed = 1;
				if (currentDebuffEffect != null)
				{
					Destroy(currentDebuffEffect);
				}
			}
		}

		if (CurrentHealth < 0)
		{
			Fsm.ChangeState(FSM_Monster1State.FSM_Monster1_Death);
		}
	}


    /// <summary>
    /// 몬스터에게 스턴 상태를 적용하는 함수
    /// </summary>
	public void OnStun()
	{
		Fsm.OnNotify(Monster1Notify.Stun, new StunData());
	}
	
	/// <summary>
	/// 몬스터의 스턴 상태가 종료될 때 호출되는 함수
	/// </summary>
	public void OnStunFInish()
	{
		Fsm.ChangeState(FSM_Monster1State.FSM_Monster1_LoopingMove);
	}
	
	
	/// <summary>
	/// 몬스터를 목적지로 이동시키는 함수
	/// </summary>
	/// <param name="destination">이동할 목적지 위치</param>
	/// <returns>목적지 도착 여부 (도착: true, 이동 중: false)</returns>
	public bool MoveToDestination(Vector3 destination)
	{
		if (isStunned) return false;
		if (isDying) return false;
		
		
		Vector3 nextPosition = Vector3.MoveTowards(transform.position, destination, Speed * Time.deltaTime);
		_rb.MovePosition(nextPosition);
		if (Vector3.Distance(destination, transform.position) <= 1.0f)
		{
			return true;
		}
		_rb.MoveRotation(Quaternion.LookRotation((nextPosition - transform.position).normalized));

		
		return false;
	}

	/// <summary>
	/// 데미지 타입에 따라 적절한 효과를 반환하는 함수
	/// </summary>
	/// <param name="damageType">데미지 타입 (fire, ice, lightning)</param>
	/// <returns>해당 데미지 타입에 맞는 효과</returns>
	private DamageTypeEffect GetDamageTypeEffect(string damageType)
	{
		switch (damageType.ToLower())
		{
			case "fire":
				return fireEffect;
			case "ice":
				return iceEffect;
			case "lightning":
				return lightningEffect;
			default:
				Debug.LogWarning($"Unknown damage type: {damageType}. Using default effect.");
				return fireEffect;
		}
	}

	/// <summary>
	/// 몬스터가 데미지를 받는 함수
	/// </summary>
	/// <param name="damage">받을 데미지 양</param>
	/// <param name="damageType">데미지 타입</param>
	public void TakeDamage(float damage, string damageType)
	{
		if (isDying) return;

		CurrentHealth -= damage;

		animator.SetTrigger("Hit");

		DamageTypeEffect effect = GetDamageTypeEffect(damageType);

		if (effect.effectPrefab != null)
		{
			Instantiate(effect.effectPrefab, transform.position, Quaternion.identity);
		}

		SoundManager.Instance.PlaySFX(effect.soundName);

		if (CurrentHealth <= 0)
		{
			StartCoroutine(Die());
		}
	}
	
	/// <summary>
	/// 몬스터에게 이동 속도 감소 디버프를 적용하는 함수
	/// </summary>
	/// <param name="duration">디버프 지속 시간</param>
	public void ApplySlowDebuff(float duration)
	{
		if (slowDebuff == null)
		{
			slowDebuff = new SlowDebuff(duration);
			ShowDebuffEffect(slowDebuffEffectPrefab);
		}
		else
		{
			slowDebuff.Refresh(duration);
		}

		UpdateMovementSpeed();
	}
	
	/// <summary>
	/// 디버프에 따라 몬스터의 이동 속도를 갱신하는 함수
	/// </summary>
	private void UpdateMovementSpeed()
	{
		if (slowDebuff != null)
		{
			if (slowDebuff.StackCount == 3)
			{
				ApplyStun(3f);
			}
			else
			{
				Speed = originalSpeed * (1f - 0.3f * slowDebuff.StackCount);
			}
		}
		else
		{
			Speed = originalSpeed;
		}
	}
	
	/// <summary>
	/// 몬스터에게 스턴 효과를 적용하는 함수
	/// </summary>
	/// <param name="duration">스턴 지속 시간</param>
	private void ApplyStun(float duration)
	{
		isStunned = true;
		stunDuration = duration;
		Speed = 0;
		ShowDebuffEffect(stunEffectPrefab);
		monsterRenderer.material.color = Color.blue;
		animator.speed = 0;
	}

	/// <summary>
	/// 디버프 시각 효과를 표시하는 함수
	/// </summary>
	/// <param name="effectPrefab">표시할 이펙트 프리팹</param>
	private void ShowDebuffEffect(GameObject effectPrefab)
	{
		if (currentDebuffEffect != null)
		{
			Destroy(currentDebuffEffect);
		}
		currentDebuffEffect = Instantiate(effectPrefab, transform.position, Quaternion.identity, transform);
	}
	
	/// <summary>
	/// 몬스터 사망 처리 코루틴
	/// </summary>
	private IEnumerator Die()
	{
		isDying = true;
		
		animator.SetTrigger("Die");
		
		SoundManager.Instance.PlaySFX(deathSoundName);

		MyPlayerController.Instance.AddGold(goldIncrease);

		_collider.enabled = false;

		isFullyDead = true;

		yield return new WaitForSeconds(1.5f);
		
		Destroy(gameObject);
	}
	
	/// <summary>
	/// 몬스터가 살아있는지 확인하는 함수
	/// </summary>
	/// <returns>몬스터가 살아있으면 true, 죽었으면 false</returns>
	public bool IsAlive()
	{
		return !isFullyDead;
	}
}
