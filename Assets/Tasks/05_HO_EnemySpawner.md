# 05. HO_EnemySpawner 구현 Task

## 목표

화면 위쪽 스폰 영역에서 적 프리팹을 일정 간격으로 생성하는 `HO_EnemySpawner`를 구현한다.

## 구현 범위

- 런타임 스크립트 위치: `Assets/Scripts/Gameplay/HO_EnemySpawner.cs`
- 클래스명: `HO_EnemySpawner`
- 부모 클래스: `MonoBehaviour`
- 스폰 위치: XY 평면, 위치는 `Vector3(x, y, 0)` 기준

## 구현 내용

1. `EnemyPrefab`을 일정 간격으로 생성한다.
2. `SpawnInterval` 성격의 `[SerializeField] private float` 값으로 스폰 간격을 조정할 수 있게 한다.
3. `SpawnXMin`, `SpawnXMax` 성격의 `[SerializeField] private float` 값으로 X 스폰 범위를 조정할 수 있게 한다.
4. `SpawnY` 성격의 `[SerializeField] private float` 값으로 Y 스폰 위치를 조정할 수 있게 한다.
5. 스폰 위치는 `Vector3(randomX, spawnY, 0)` 기준으로 만든다.
6. 시간 기반 반복 스폰만 사용한다.
7. `HO_GameManager`의 게임 종료 상태를 확인할 수 있으면, 종료 상태에서 추가 스폰을 중지한다.
8. 웨이브 테이블, 난이도 곡선, 복합 패턴 스폰은 구현하지 않는다.

## Inspector 조정 값

- 적 프리팹
- 스폰 간격
- 스폰 X 최소값
- 스폰 X 최대값
- 스폰 Y 값
- 게임 매니저 참조

## 완료 기준

- 적 프리팹이 일정 간격으로 생성된다.
- 스폰 간격을 Inspector에서 바꿀 수 있다.
- 적이 설정한 X 범위 안에서 생성된다.
- 적이 설정한 Y 위치에서 생성된다.
- 생성 위치가 XY 평면 기준을 유지한다.
- 게임 종료 상태에서는 추가 스폰이 중지된다.
