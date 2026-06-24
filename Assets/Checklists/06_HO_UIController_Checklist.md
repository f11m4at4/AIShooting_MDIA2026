# 06. HO_UIController 검증 체크리스트

## 컴파일 확인

- [x] `HO_UIController` 클래스명이 파일명과 일치한다.
- [x] 새 클래스명에 `HO_` prefix가 붙어 있다.
- [x] Inspector 노출 값은 `[SerializeField] private` 필드를 우선 사용한다.
- [x] Unity Console에 컴파일 오류가 없다.

## HP UI 검증

- [x] HP Slider 참조를 Inspector에서 할당할 수 있다.
- [x] HP 갱신 메서드를 호출하면 Slider 최대값이 갱신된다.
- [x] HP 갱신 메서드를 호출하면 Slider 현재값이 갱신된다.
- [x] 플레이어 HP가 감소하면 Slider 표시가 함께 감소한다.

## 점수 UI 검증

- [x] 점수 Text 참조를 Inspector에서 할당할 수 있다.
- [x] 점수 갱신 메서드를 호출하면 Text 값이 갱신된다.
- [x] 점수가 증가하면 화면의 점수 표시가 함께 증가한다.

## 해상도 및 배치 검증

- [x] Canvas가 Screen Space 기준으로 구성되어 있다.
- [ ] Game View 기준 해상도를 `768 x 1024`로 설정해 확인했다.
- [x] HP Slider가 왼쪽 상단에 정상 표시된다.
- [x] 점수 Text가 오른쪽 상단에 정상 표시된다.
- [x] UI가 게임플레이 로직을 직접 판단하지 않고 전달받은 값만 표시한다.
