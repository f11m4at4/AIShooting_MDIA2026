# 05. HO_EnemySpawner 검증 체크리스트

## 컴파일 확인

- [x] `HO_EnemySpawner` 클래스명이 파일명과 일치한다.
- [x] 새 클래스명에 `HO_` prefix가 붙어 있다.
- [x] Inspector 노출 값은 `[SerializeField] private` 필드를 우선 사용한다.
- [x] Unity Console에 컴파일 오류가 없다.

## 스폰 검증

- [x] 적 프리팹을 Inspector에서 할당할 수 있다.
- [x] 스폰 간격을 Inspector에서 조정할 수 있다.
- [x] 설정한 간격마다 적이 생성된다.
- [x] X 스폰 최소값과 최대값을 Inspector에서 조정할 수 있다.
- [x] 적이 설정한 X 범위 안에서 생성된다.
- [x] Y 스폰 위치를 Inspector에서 조정할 수 있다.
- [x] 적이 설정한 Y 위치에서 생성된다.
- [x] 생성 위치가 `Vector3(x, y, 0)` 기준을 유지한다.

## 게임 상태 연동 검증

- [x] 게임 매니저 참조를 Inspector에서 할당할 수 있다.
- [x] 게임 진행 상태에서는 스폰이 계속된다.
- [x] 게임 종료 상태에서는 추가 스폰이 중지된다.
- [x] 웨이브 테이블, 난이도 곡선, 복합 패턴 스폰이 포함되지 않았다.
