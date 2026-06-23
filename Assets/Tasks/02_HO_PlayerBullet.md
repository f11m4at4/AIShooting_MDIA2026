# 02. HO_PlayerBullet 구현 Task

## 목표

플레이어 총알의 위쪽 이동, 화면 밖 자동 제거, 적 충돌 시 대미지 전달을 처리하는 `HO_PlayerBullet`을 구현한다.

## 구현 범위

- 런타임 스크립트 위치: `Assets/Scripts/Gameplay/HO_PlayerBullet.cs`
- 클래스명: `HO_PlayerBullet`
- 부모 클래스: `MonoBehaviour`
- 이동 평면: XY 평면, 위치는 `Vector3(x, y, 0)` 기준

## 구현 내용

1. 생성 후 매 프레임 월드 좌표 기준 `Vector3.up` 방향으로 이동한다.
2. `Speed` 성격의 `[SerializeField] private float` 값으로 이동 속도를 조정할 수 있게 한다.
3. `Damage` 성격의 `[SerializeField] private int` 값으로 총알 대미지를 조정할 수 있게 한다.
4. `DestroyY` 성격의 `[SerializeField] private float` 값으로 화면 위쪽 제거 기준을 조정할 수 있게 한다.
5. 총알 이동 중 Z 위치가 게임플레이 이동에 사용되지 않도록 XY 이동만 처리한다.
6. `HO_Enemy`와 충돌하면 적의 피격 처리 메서드를 호출한다.
7. 적과 충돌한 뒤 총알 자신을 제거한다.
8. 화면 위쪽 제거 기준을 넘으면 총알 자신을 제거한다.

## Inspector 조정 값

- 이동 속도
- 대미지
- 화면 위쪽 제거 Y 값

## 완료 기준

- 총알이 위쪽으로 단순 이동한다.
- 총알 이동 속도를 Inspector에서 바꿀 수 있다.
- 화면 위쪽 제거 기준을 넘으면 제거된다.
- 적과 충돌하면 적 피격 처리가 호출된다.
- 충돌 후 총알이 제거된다.
