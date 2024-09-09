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


    
	public void OnStun()
	{
		Fsm.OnNotify(Monster1Notify.Stun, new StunData());
	}
	
	public void OnStunFInish()
	{
		Fsm.ChangeState(FSM_Monster1State.FSM_Monster1_LoopingMove);
	}
	
	
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
				return fireEffect; // 기본값으로 fire 효과 사용
		}
	}
	public void TakeDamage(float damage, string damageType)
	{
		if (isDying) return;

		CurrentHealth -= damage;

		// 피격 애니메이션 재생
		animator.SetTrigger("Hit");

		// 피해 타입에 따른 이펙트 및 사운드 재생
		DamageTypeEffect effect = GetDamageTypeEffect(damageType);

		// 피격 이펙트 생성
		if (effect.effectPrefab != null)
		{
			Instantiate(effect.effectPrefab, transform.position, Quaternion.identity);
		}

		// 피격 사운드 재생
		SoundManager.Instance.PlaySFX(effect.soundName);

		// 체력이 0 이하가 되면 사망 처리
		if (CurrentHealth <= 0)
		{
			StartCoroutine(Die());
		}
	}
	
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
	
	private void ApplyStun(float duration)
	{
		isStunned = true;
		stunDuration = duration;
		Speed = 0;
		ShowDebuffEffect(stunEffectPrefab);
		monsterRenderer.material.color = Color.blue;
		animator.speed = 0;
	}

	
	private void ShowDebuffEffect(GameObject effectPrefab)
	{
		if (currentDebuffEffect != null)
		{
			Destroy(currentDebuffEffect);
		}
		currentDebuffEffect = Instantiate(effectPrefab, transform.position, Quaternion.identity, transform);
	}
	
	private IEnumerator Die()
	{
		isDying = true;
		
		animator.SetTrigger("Die");
		
		
		SoundManager.Instance.PlaySFX(deathSoundName);

		// 사망 애니메이션 길이만큼 대기
		float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
		//Debug.Log($"Waiting for death animation to finish. Animation length: {animationLength}");
		yield return new WaitForSeconds(animationLength);
		
		// 몬스터 제거 직전 처리
		MyPlayerController.Instance.RemoveMonster(gameObject);

		// 골드 증가 및 킬 증가
		MyPlayerController.Instance.AddGold(goldIncrease + UpgradeManager.Instance.GetExtraGold());
		UpgradeManager.Instance.IncrementKillCount();

		Debug.Log(MyPlayerController.Instance.Gold);
		// 몬스터 제거
		Destroy(gameObject);
	}


	public bool IsAlive()
	{
		return !isDying && !isFullyDead;
	}
	

}
