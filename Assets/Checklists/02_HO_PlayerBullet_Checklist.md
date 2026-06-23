# 02. HO_PlayerBullet 검증 체크리스트

## 컴파일 확인

- [x] `HO_PlayerBullet` 클래스명이 파일명과 일치한다.
- [x] 새 클래스명에 `HO_` prefix가 붙어 있다.
- [x] Inspector 노출 값은 `[SerializeField] private` 필드를 우선 사용한다.
- [ ] Unity Console에 컴파일 오류가 없다.

## 이동 검증

- [x] 총알이 생성 후 월드 좌표 기준 `Vector3.up` 방향으로 이동한다.
- [x] 총알 이동은 XY 평면에서만 처리되고 XZ 평면 이동이 없다.
- [ ] 이동 속도를 Inspector에서 변경하면 Play Mode에서 반영된다.
- [x] 이동 중 Z 위치가 게임플레이 이동 값으로 변하지 않는다.

## 제거 검증

- [x] 화면 위쪽 제거 Y 값을 Inspector에서 조정할 수 있다.
- [x] 총알이 제거 Y 값을 넘으면 자동 제거된다.

## 충돌 검증

- [ ] 총알이 `HO_Enemy`와 충돌하면 적 피격 메서드를 호출한다.
- [x] 총알 대미지를 Inspector에서 조정할 수 있다.
- [ ] 적과 충돌한 총알은 제거된다.
- [x] 총알 관통, 유도, 폭발 같은 제외 기능이 포함되지 않았다.
