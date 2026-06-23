# AGENTS.md

## 프로젝트 개요

- 프로젝트명: `AIShooting`
- Unity 버전: `6000.4.10f1`
- 장르: 3D 탑뷰 로그라이크 슈팅 게임
- 이동 평면: XY 평면만 사용한다. 사용자가 명시적으로 요청하지 않는 한 플레이어, 적, 투사체, 게임플레이 이동을 XZ 평면 기준으로 구현하지 않는다.
- Unity 버전에 민감한 변경을 할 때는 `ProjectSettings/ProjectVersion.txt`를 기준으로 확인한다.

## Unity 설정 규칙

- 주요 게임플레이 카메라는 반드시 `Projection = Orthographic`을 사용한다.
- 기준 해상도는 세로형 `768 x 1024`로 잡는다.
- 새 카메라와 UI 작업은 `768 x 1024` 비율을 기준으로 구성한다.
- 카메라의 orthographic size를 정해야 할 때는 `768 x 1024` 플레이 영역이 일관되게 보이도록 값을 선택하고, 필요한 경우 근거를 남긴다.
- 게임플레이 입력은 레거시 Old Input Manager API를 사용한다.
- 사용자가 명시적으로 변경을 요청하지 않는 한 새 게임플레이 코드에서 New Input System API를 사용하지 않는다.
- 입력 처리는 `Input.GetAxisRaw`, `Input.GetKey`, `Input.GetButton` 등 `UnityEngine.Input` 기반 API를 우선 사용한다.

## 코딩 컨벤션

- Unity C# 컨벤션을 따른다.
- 클래스, 메서드, 프로퍼티, public 필드는 `PascalCase`를 사용한다.
- 지역 변수와 파라미터는 `camelCase`를 사용한다.
- Inspector에 값을 노출할 때는 public 필드보다 `[SerializeField] private` 필드를 우선 사용한다.
- MonoBehaviour 생명주기 메서드는 작고 명확하게 유지한다.
- 반복해서 사용하는 컴포넌트 참조는 캐싱한다.
- 새로 만드는 클래스 이름에는 `HO_` prefix를 붙인다.
- 예시: `HO_PlayerController`, `HO_EnemySpawner`, `HO_Bullet`
- 런타임 코드는 적절한 `Assets` 하위 폴더에 두고, 에디터 전용 코드는 `Editor` 폴더 아래에 둔다.
- 요청한 게임플레이 변경과 무관한 리팩터링은 하지 않는다.

## 게임플레이 구현 지침

- 탑뷰 게임플레이는 XY 평면 기준으로 구현한다.
- 평면 이동에는 `Vector2` 또는 `Vector3(x, y, 0)`을 사용한다.
- 기능상 깊이 축이 꼭 필요한 경우가 아니라면 게임플레이용 Transform은 XY 이동 기준에 맞춘다.
- 카메라는 orthographic projection으로 XY 플레이 평면을 바라보게 구성한다.
- 이동 속도, 발사 속도, 대미지, 스폰 간격, 경계값 같은 핵심 값은 Inspector에서 조정할 수 있게 만든다.
- 먼저 단순하고 읽기 쉬운 게임플레이 시스템을 선호한다. 실제 중복을 줄이거나 책임을 명확히 할 때만 추상화를 추가한다.

## 검증

- 코드나 씬을 변경한 뒤에는 프로젝트가 Unity `6000.4.10f1`에서 열리는지 확인한다.
- 게임플레이 스크립트는 먼저 컴파일 오류를 확인하고, Unity 사용이 가능하면 Play Mode에서 동작을 검증한다.
- 카메라나 해상도 관련 변경을 했을 때는 Game View를 `768 x 1024` 기준으로 확인한다.
