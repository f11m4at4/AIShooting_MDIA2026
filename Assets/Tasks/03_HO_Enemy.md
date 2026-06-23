# 03. HO_Enemy 구현 Task

## 목표

적의 아래쪽 이동, 화면 밖 자동 제거, 플레이어 충돌 공격, 피격 사망 처리를 담당하는 `HO_Enemy`를 구현한다.

## 구현 범위

- 런타임 스크립트 위치: `Assets/Scripts/Gameplay/HO_Enemy.cs`
- 클래스명: `HO_Enemy`
- 부모 클래스: `MonoBehaviour`
- 이동 평면: XY 평면, 위치는 `Vector3(x, y, 0)` 기준

## 구현 내용

1. 생성 후 매 프레임 월드 좌표 기준 `Vector3.down` 방향으로 이동한다.
2. `MoveSpeed` 성격의 `[SerializeField] private float` 값으로 이동 속도를 조정할 수 있게 한다.
3. `ContactDamage` 성격의 `[SerializeField] private int` 값으로 플레이어 접촉 대미지를 조정할 수 있게 한다.
4. `DestroyY` 성격의 `[SerializeField] private float` 값으로 화면 아래쪽 제거 기준을 조정할 수 있게 한다.
5. 적 이동 중 Z 위치가 게임플레이 이동에 사용되지 않도록 XY 이동만 처리한다.
6. 화면 아래쪽 제거 기준을 넘으면 적 자신을 제거한다.
7. 플레이어와 충돌하면 `HO_PlayerController`의 피격 메서드를 호출한다.
8. 플레이어와 충돌한 적은 제거한다.
9. 총알이 호출할 수 있는 공개 피격 메서드를 제공한다.
10. 프로토타입 기준으로 총알에 맞은 적은 즉시 제거한다.

## Inspector 조정 값

- 이동 속도
- 접촉 대미지
- 화면 아래쪽 제거 Y 값

## 완료 기준

- 적이 아래쪽으로 단순 이동한다.
- 적 이동 속도를 Inspector에서 바꿀 수 있다.
- 화면 아래쪽 제거 기준을 넘으면 제거된다.
- 플레이어와 충돌하면 플레이어 HP가 감소한다.
- 플레이어와 충돌한 적은 제거된다.
- 플레이어 총알에 맞은 적은 제거된다.
