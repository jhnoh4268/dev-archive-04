<h1>[Unity3D] VampireSurvival 3D</h1>

🖥️ 프로젝트 소개

3D로 만든 뱀서라이크장르의 게임입니다

🕰️ 개발 기간

2달

⚙️ 개발 환경

Unity, C#

<h2>1. 플레이어 방법</h2>

방향키로 이동 가능 공격은 자동공격이다. 적이 가까이오게 되면 가장 가까이있는 적을 바라보며

스킬은 레벨업시에 여러가지 스킬이 선택가능하다. 캐릭터는 액티브, 패시브 스킬을 선택가능하고 첫 스킬은 액티브 스킬로 고정된다.

액티브스킬들은 스킬마다 공격외에도 상태이상 효과들이 있다.

<h2>2.스킬</h2>
<h3>번개</h3>
플레이어 주변 일정범위내 랜덤 적에게 번개를 떨어뜨림

맞은 적은 일정시간 받는 데미지가 증가하는 디버프에 걸림

<img width="400" height="225" alt="Lighting" src="https://github.com/user-attachments/assets/45ab3536-f87b-4655-8818-f2c3876f4794" />

<h3>폭발</h3>
플레이어를 중심으로 폭발을 일으킴

맞은 적은 일정시간 스턴에 걸림

<img width="400" height="225" alt="Explotion" src="https://github.com/user-attachments/assets/940f06e1-df49-459a-9570-454af65b2759" />

<h3>연속베기</h3>
플레이어 전방으로 칼을 휘두름

맞은 적은 넉백으로 인해 밀려남

<img width="400" height="225" alt="sword" src="https://github.com/user-attachments/assets/22314339-172f-4894-9c0f-70108907501e" />

<h3>화염볼</h3>
플레이어 전방으로 화염볼을 날림

맞은 적은 화상에 걸리며 일정시간동안 지속피해를 받음

<img width="400" height="225" alt="Fireball" src="https://github.com/user-attachments/assets/65ccb941-4eb6-4082-b33e-7a72e929d19b" />

<h3>푸른마법</h3>
플레이어를 중심으로 영역을 생성하며 범위안으로 들어온 적에게 피해를 임힘

맞은 적은 동결에 걸리며 일정시간동안 속도가 느려짐

<img width="400" height="225" alt="BlueField" src="https://github.com/user-attachments/assets/e9105584-0382-4dae-8560-883bccefc258" />

<h3>유성</h3>
플레이어 주변에 유성을 떨어뜨림

맞은 적은 랜덤한 상태이상효과를 일으킴

<img width="400" height="225" alt="Meteo" src="https://github.com/user-attachments/assets/45a3ce43-9ab4-413a-96f2-e0f612e0922e" />

<h2>3.보스</h2>
일정시간이 지날때마다 보스가 출현

보스 출현시 출현 문구가 나타나고 일정거리 접근시 플레이어 주변으로 번개를 떨어뜨림
<img width="480" height="270" alt="Boss" src="https://github.com/user-attachments/assets/d6775c1d-2f01-4bc6-acf7-f6709a66345b" />


<h2>기술적 요소</h2>
자세한 소스코드 확인은 깃허브에서 확인가능

1. 오브젝트 풀링
2. ScriptableObject
