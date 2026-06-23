# 06. HO_UIController 구현 Task

## 목표

HP Slider와 점수 Text UI를 갱신하는 `HO_UIController`를 구현한다.

## 구현 범위

- 런타임 스크립트 위치: `Assets/Scripts/UI/HO_UIController.cs`
- 클래스명: `HO_UIController`
- 부모 클래스: `MonoBehaviour`
- 기준 해상도: 세로형 `768 x 1024`

## 구현 내용

1. 플레이어 HP 표시용 Slider 참조를 Inspector에서 할당받는다.
2. 점수 표시용 Text UI 참조를 Inspector에서 할당받는다.
3. HP Slider는 화면 왼쪽 상단에 배치되는 것을 전제로 갱신한다.
4. 점수 Text는 화면 오른쪽 상단에 배치되는 것을 전제로 갱신한다.
5. HP 값 갱신 메서드를 제공하고 Slider의 최대값과 현재값을 설정한다.
6. 점수 값 갱신 메서드를 제공하고 Text UI에 현재 점수를 표시한다.
7. UI는 게임플레이 로직을 직접 판단하지 않고 전달받은 값만 표시한다.
8. Canvas 구성은 Screen Space 기준으로 만들고 `768 x 1024` Game View에서 확인한다.

## Inspector 조정 값

- HP Slider 참조
- 점수 Text 참조

## 완료 기준

- HP Slider가 현재 HP를 표시한다.
- HP Slider의 최대값과 현재값이 정상 갱신된다.
- 점수 Text가 현재 점수를 표시한다.
- 점수 변경 시 Text UI가 갱신된다.
- `768 x 1024` Game View 기준에서 HP와 점수 UI가 화면 상단에 정상 표시된다.
