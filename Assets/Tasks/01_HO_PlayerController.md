# 01. HO_PlayerController 구현 Task

## 목표

플레이어의 XY 평면 이동, 화면 경계 제한, 공격 입력, HP 상태, 사망 상태를 관리하는 `HO_PlayerController`를 구현한다.

## 구현 범위

- 런타임 스크립트 위치: `Assets/Scripts/Gameplay/HO_PlayerController.cs`
- 클래스명: `HO_PlayerController`
- 부모 클래스: `MonoBehaviour`
- 이동 평면: XY 평면, 위치는 `Vector3(x, y, 0)` 기준
- 입력 API: Old Input Manager 기반 `UnityEngine.Input`

## 구현 내용

1. `Input.GetAxisRaw("Horizontal")`, `Input.GetAxisRaw("Vertical")`로 이동 입력을 읽는다.
2. 입력값을 `Vector2`로 만들고, 크기가 1을 초과하면 정규화한다.
3. `MoveSpeed` 성격의 `[SerializeField] private float` 값으로 이동 속도를 조정할 수 있게 한다.
4. `MinBounds`, `MaxBounds` 성격의 `[SerializeField] private Vector2` 값으로 이동 가능 범위를 조정할 수 있게 한다.
5. 이동 후 `Mathf.Clamp`로 X, Y 위치를 화면 경계 안에 제한한다.
6. Z 위치는 기존 값을 유지하거나 `0` 기준에서 벗어나지 않게 처리한다.
7. `Input.GetButtonDown("Fire1")`로 공격 입력을 읽는다.
8. 공격 입력 시 `BulletPrefab`을 `FirePoint` 위치에 생성한다.
9. `FirePoint`가 비어 있으면 플레이어 위치를 사용하고 `Debug.LogWarning`으로 경고를 남긴다.
10. 최대 HP와 현재 HP를 분리해 관리하고, 최대 HP는 Inspector에서 조정 가능하게 한다.
11. 적이 호출할 수 있는 공개 피격 메서드를 제공한다.
12. HP가 0 이하가 되면 사망 상태로 전환하고 이동과 공격 입력을 무시한다.

## Inspector 조정 값

- 이동 속도
- 이동 가능 최소 좌표
- 이동 가능 최대 좌표
- 최대 HP
- 총알 프리팹
- 발사 위치 Transform

## 완료 기준

- 플레이어가 XY 평면에서 이동한다.
- 대각선 이동 시 속도가 과하게 증가하지 않는다.
- 설정한 경계 밖으로 나가지 않는다.
- `Fire1` 입력 시 총알 프리팹이 생성된다.
- HP가 0 이하가 되면 이동과 공격이 중지된다.
