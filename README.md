# 3D Vampire Survival

Unity 6로 만든 3D 뱀서라이크(Vampire Survivors-like) 게임입니다. 몰려오는 몬스터를 자동 발동 스킬로 처치하고, 경험치 오브를 모아 레벨업하며 카드(스킬)를 선택해 성장합니다.

- **제품명**: vampire3D (v0.1.0)
- **엔진**: Unity `6000.3.18f1`
- **렌더 파이프라인**: URP 17.3.0

## 게임 플레이

| 요소 | 내용 |
| --- | --- |
| 이동 | `WASD` / 방향키 (`Horizontal`, `Vertical` 축). 캐릭터는 가장 가까운 몬스터를 자동으로 바라봅니다. |
| 공격 | 보유한 액티브 스킬이 쿨타임마다 자동 발동됩니다. |
| 성장 | 몬스터가 떨어뜨린 경험치 오브를 획득 → 레벨업 시 카드 3장 중 1장 선택 (게임 시작 시에는 액티브 카드 3장 제시) |
| 카드 레벨 | 카드당 최대 5레벨 |
| 몬스터 | 플레이어 주변 원 둘레에서 주기적으로 스폰. 체력은 플레이 시간에 따라 증가합니다. |
| 보스 | `LevelDesign.BossSpawnRate` 주기마다 `Skeleton_Boss` 등장. 플레이어 주변에 범위 공격(낙뢰)을 시전합니다. |
| 아이템 | 체력 회복(최대 체력의 30%), 폭탄(범위 피해), 자석(모든 오브 흡수), 보물상자(보유 카드 1/3/5장 강화) |

### 액티브 스킬

| 카드 | 클래스 | 동작 |
| --- | --- | --- |
| 낙뢰 | `Skill01_Lighting` | 범위 내 무작위 몬스터에게 레벨 수만큼 낙뢰 |
| 화염구 | `Skill02_Fireball` | 전방으로 투사체 발사, 레벨업 시 45° 부채꼴로 발사 수 증가 |
| 폭발 | `Skill03_Explosion` | 플레이어 중심 범위 폭발, 레벨에 따라 범위 증가 |
| 푸른마법 | `Skill04_BlueField` | 플레이어를 따라다니는 장판으로 주기적 피해 |
| 유성 | `Skill05_Meteo` | 주변 무작위 위치에 유성 낙하, 무작위 상태이상 부여 |
| 연속베기 | `Skill06_MultiSlash` | 전방 부채꼴(45°)을 연속으로 베기 |

### 패시브 카드

| 카드 | 효과 |
| --- | --- |
| 끌림의 정수 | 오브 획득 범위 증가 |
| 강한 한 방 | 공격력 증가 |
| 성장의 비법 | 경험치 획득량 증가 |
| 한방의 기회 | 치명타 확률 증가 |
| 치명적인 한방 | 치명타 피해 증가 |
| 강인한 체력 | 최대 체력 증가 |

### 상태이상

| 상태 | 효과 |
| --- | --- |
| Burn (화상) | 4초간 1초마다 지속 피해 |
| Freeze (빙결) | 스택에 비례해 감속, 스택이 가득 차면 2초간 완전 빙결 |
| Shock (감전) | 3초간 받는 피해 20% 증가 |
| Stun (기절) | 2초간 행동 불가 |
| Knockback (넉백) | 플레이어 반대 방향으로 밀려남 |

## 프로젝트 구조

```
Assets/
├── 01.Scenes/          InGame(메인 게임)
├── 02.Scripts/
│   ├── Manager/        MANAGER, PoolManager, DBManager, SessionManager, SkillManager
│   ├── Skill/          SkillBase, 액티브 스킬 6종, PassiveManager, Monster/(보스 스킬)
│   ├── StatusEffect/   StatusEffect 및 Burn/Freeze/Shock/Stun
│   ├── Item/           Item, IItemEffect 구현체(Health/Explotion/Magnet/Treasure)
│   ├── ScriptableObject/ CardDB, PartDB, LevelDesign
│   ├── Factory/        GenericPartFactory
│   ├── UI/             BaseCanvas, CardSelector, Card, Treasure, SkillFrame, DamageTMP
│   ├── Interface/      IPool, IFactory, IItemEffect, IStatusEffect
│   ├── Utills/         UtilsCoroutine, UtilsWorld, UtilsUI
│   └── Player*.cs, Monster*.cs, Spawner.cs, Bullet.cs, Orb.cs
├── 03.Prefabs/         Item, Monster, UI 프리팹
├── Resources/
│   ├── DB/             Card(Actives/Passives/None), Part(Monster/Item), LevelDesign
└── └── POOL/           오브젝트 풀에서 로드하는 프리팹
```

## 아키텍처

- **MANAGER (싱글턴)** — `DontDestroyOnLoad` 오브젝트로, 자식 컴포넌트인 `PoolManager` / `DBManager` / `SessionManager` / `SkillManager`를 정적 필드로 노출합니다.
- **오브젝트 풀링** — `MANAGER.poolManager.PoolingObj("이름")`으로 `Resources/POOL/이름` 프리팹을 풀링합니다. 몬스터, 투사체, 이펙트, 데미지 폰트, 오브 모두 풀을 사용합니다.
- **데이터 주도 설계** — 카드(`CardDB`), 몬스터/아이템 외형(`PartDB`), 밸런스(`LevelDesign`)를 ScriptableObject로 관리합니다.
- **스킬 시스템** — `CardDB.className`에 적힌 이름으로 리플렉션(`Type.GetType`)을 통해 `SkillBase` 파생 컴포넌트를 생성합니다. 패시브는 `PassiveManager`가 `SessionManager`의 스탯 배율을 갱신합니다.
- **팩토리 패턴** — `GenericPartFactory<T>`가 하나의 풀링된 `Monster`/`Item` 오브젝트에 ID에 맞는 외형 프리팹을 붙여 재사용합니다.
- **상태이상** — `IStatusEffect` 구현체를 몬스터의 `StatusEffect` 컴포넌트가 매 프레임 Tick/End 처리합니다.
- **이벤트 기반 UI** — `SessionManager`의 델리게이트(`onHPChanged`, `onExpChanged`, `onBossTime` 등)를 `BaseCanvas`가 구독합니다.

### 밸런스 공식

- 몬스터 체력: `baseHP + grawRate × (분)^exponent`, 보스는 `× bossHPMultiplier`
- 필요 경험치: `floor(5 × level^PlayerExpExponent)`
- 스킬 피해: `플레이어 공격력 × (baseDamage + damagePerLevel × (level-1)) / 100`

수치는 `Assets/Resources/DB/LevelDesign.asset`과 각 카드 에셋에서 조정합니다.

## 주요 의존성

| 패키지 | 버전 |
| --- | --- |
| Universal Render Pipeline | 17.3.0 |
| Input System | 1.19.0 |
| UniTask | Git (Cysharp/UniTask) |
| DOTween | `Assets/Plugins/Demigiant` |
| TextMesh Pro | uGUI 2.0.0 내장 |

<h1>[Unity3D] VampireSurvival 3D</h1>
