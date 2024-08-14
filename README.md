# Unity_Network-1
# 240814
1. Cinemachine: 추적 카메라 구성
2. IK 역운동학(Inverse Kinematic)
: 플레이어나 적 캐릭터에 무기 장착시 무기를 기준으로 애니메이션이 재생되는 것을 의미

3. Post Processing - 후처리
: 게임 중 빛이나 직접광이나 간접광을 어떻게 디자이어너처럼 잘 표현하는지
4. PUN2(Photon NetWork2)을 사용해 네크워크 게임 만들기

5. 움직임
- A, D: 회전
- W, S: 전진 후진

* 현재 Animator Controller는 2개의 Layer 존재.
* Animator Controller는 Layer를 두 개 이상(BaseLayer, UpperLayer) 만들면 각 Layer에서 재생하는 Animation이 위에서 아래 순서로 덮어쓰기(override) 하며 반영됨.
이 과정에서 두 개의 레이어 부위 중 겹치지 않은 부위는 애니메이션이 합쳐짐. 즉 UpperLayer의 Animation은 BaseLayer Animation을 덮어쓰기도 함.
* 레이어를 나눈 이유는 더 적은 Animation Clip으로 다양한 경우에 대응하기 위해서.
* 레이어를 나누지 않았다면 뛰면서 재장전하는 새로운 Animation Clip을 제작해야 하기 때문