## Castle Gardians (졸업 프로젝트)

[Youtube](https://youtu.be/eCI3BYK4hy4)

[Notion](https://www.notion.so/6d24e70871254eb8a9e825f7ba365cec?pvs=4#c90de980030c4f73a410d5e5bc8f0abe)

제작 시기 : 2023.03 ~ 2023.11

- 졸업 프로젝트
    - 2D 디펜스 게임
    - Unity
    - Android
    - 프로그래머
    - 인원: 프로그래머 2
    
- **설명**
    - 졸업 프로젝트로 Unity로 구현 중인 2D 디펜스 게임입니다.
    - C# .NET 을 이용해 Async 한 서버를 구현하여 멀티 플레이를 지원합니다.
    - 원활한 소통과 높은 퀄리티를 위하여 모든 설계를 완료한 뒤 구현 하였습니다.

- 담당 역할
    - [**Server**](https://www.notion.so/6d24e70871254eb8a9e825f7ba365cec?pvs=4#73a73ad4976644cf930fe40ba4ae99cf)
        - [**Server Listener**](https://www.notion.so/6d24e70871254eb8a9e825f7ba365cec?pvs=4#9d8f0169d18e413685e3f65a3bfd7065)
        - [JobQueue](https://www.notion.so/6d24e70871254eb8a9e825f7ba365cec?pvs=4#44f212f6f6334906ad8c9d96a7400477)
        - [**Packet 송수신**](https://www.notion.so/6d24e70871254eb8a9e825f7ba365cec?pvs=4#68b2fe5f6a274a1a9a4cfac22686f5c3)
    - [**Client**](https://www.notion.so/6d24e70871254eb8a9e825f7ba365cec?pvs=4#aa32cb1e5cb64bdb98d85e7c2158ed03)
        - [Manger](https://www.notion.so/6d24e70871254eb8a9e825f7ba365cec?pvs=4#a66123265c32456199838d7daac68d1e)
        - [Entity](https://www.notion.so/6d24e70871254eb8a9e825f7ba365cec?pvs=4#bf65e65ecdc94365a2b95525b53e84b9)
        - [**Json**](https://www.notion.so/6d24e70871254eb8a9e825f7ba365cec?pvs=4#85def84d00de4b70a983bab33d0a562d)

### **Server**


**Server Listener**

![졸프-서버구조.drawio (2).png](https://www.notion.so/6d24e70871254eb8a9e825f7ba365cec?pvs=4#80b1c8095fbc43bfae22cd2ad7339d4e)

**Server 전체 UML**

![졸프-서버구조.drawio (3).png](https://s3-us-west-2.amazonaws.com/secure.notion-static.com/a82a47ce-74c6-47bd-8098-d601150fd488/%EC%A1%B8%ED%94%84-%EC%84%9C%EB%B2%84%EA%B5%AC%EC%A1%B0.drawio_(3).png)

Listener 파트 확대

[GraduationProject/Listener.cs at main · ParkSungTaek/GraduationProject](https://github.com/ParkSungTaek/GraduationProject/blob/main/Server/ServerCore/Listener.cs)

실시간 멀티플레이어 게임 서버는 효율적인 자원 활용과 고성능을 위해 비동기(Async) 방식으로 설계되어야 합니다. 

메인 스레드에서 개별 유저의 입력을 순차적으로 대기하는 동기(Sync) 방식은 자원 활용 측면에서 제한적입니다. 그러므로, 메인 스레드의 블로킹 없이 클라이언트의 연결 요청을 처리하고 데이터를 송수신할 수 있는 비동기 방식이 필요합니다.

C#에서 제공하는 `Socket.Send()`와 `Socket.Accept()` 메서드들은 동기 방식으로 작동하며, 이는 코드가 직관적이고 이해하기 쉽지만, 높은 트래픽 상황에서는 성능 저하를 초래할 수 있습니다. 따라서 우리는 `Socket.SendAsync()`와 `Socket.AcceptAsync()`를 사용하여 이 문제를 해결하였습니다.

게임 서버가 시작될 때, Listener는 별도의 스레드에 클라이언트 접속 대기 명령을 수행합니다. 해당 스레드는 클라이언트 접속을 감지하고 콜백 함수를 통해 메인 스레드에 알립니다. Delegate를 사용하여 클라이언트의 데이터를 전달 받아 처리합니다.

JobQueue

[GraduationProject/Server/ServerCore/JobQueue.cs at main · ParkSungTaek/GraduationProject](https://github.com/ParkSungTaek/GraduationProject/blob/main/Server/ServerCore/JobQueue.cs)

RoomManager와 4인 이하의 유저들이 참여하는 Room은 여러 스레드에 의해 동시에 접근될 수 있는 구조입니다.
이러한 환경에서 레이스 컨디션을 방지하기 위해, JobQueue를 도입하였습니다.

JobQueue는 작업을 큐에 추가(Push)하거나, 가장 앞선 작업을 제거(Pop)하는 경우에만 Lock을 사용합니다.
이는 모든 레이스 컨디션이 발생 가능한 자료구조에 접근할 때마다 Lock을 사용하는 것보다 효율적입니다.

왜냐하면, Lock은 자원에 대한 동시 접근을 막기 위해 사용되지만, 과도한 사용은 성능 저하를 초래할 수 있기 때문입니다. 따라서 우리는 필요한 시점에서만 Lock을 걸어서 최소한의 성능 손실로 안정성과 일관성을 보장하는 방향으로 설계를 진행했습니다.

결과적으로, JobQueue를 통해 우리는 높은 동시성과 데이터 일관성 사이에서 균형잡힌 솔루션을 제공할 수 있게 되었습니다. 이로써 멀티 스레드 환경에서도 안정적인 게임 서버 운영이 가능해져 전반적인 사용자 경험이 향상되었습니다.

**Packet 송수신**

[GraduationProject/PacketManager.cs at main · ParkSungTaek/GraduationProject](https://github.com/ParkSungTaek/GraduationProject/blob/main/Server/Server/Packet/Manage/PacketManager.cs)

[GraduationProject/SendBuffer.cs at main · ParkSungTaek/GraduationProject](https://github.com/ParkSungTaek/GraduationProject/blob/main/Server/ServerCore/Buffer/SendBuffer.cs)

![졸프-패킷.drawio.png](https://s3-us-west-2.amazonaws.com/secure.notion-static.com/4226fd70-1ae7-46aa-826e-d9e936378a85/%EC%A1%B8%ED%94%84-%ED%8C%A8%ED%82%B7.drawio.png)

![졸프-서버구조.drawio (4).png](https://s3-us-west-2.amazonaws.com/secure.notion-static.com/894473a1-a954-48e1-a000-afa9dbbf4ce7/%EC%A1%B8%ED%94%84-%EC%84%9C%EB%B2%84%EA%B5%AC%EC%A1%B0.drawio_(4).png)

실시간 멀티플레이어 게임 서버에서는 클라이언트로부터 수신한 패킷(Client To Server Packet, CTS)을 처리하고, 이 정보를 다른 클라이언트에게 게임 상태를 동기화하기 위한 패킷(Server To Client Packet, STC)을 전송해야 합니다. 그러나, 각 CTS 수신 시마다 동기화를 위해 STC를 즉시 전송한다면 네트워크 통신에 지나치게 많은 시간을 소비하게 될 것입니다. 이는 서버의 전체적인 성능 저하를 초래할 수 있습니다.

따라서 우리는 '청크(Chunk)'라는 큰 자료구조를 활용하여 이 문제를 해결하였습니다. 청크 형태의 임시 저장 공간에 STC 패킷을 직렬화하여 모아둔 후, 주기적으로(본 서버에서는 250ms 간격으로) 해당 청크의 데이터들을 클라이언트에게 일괄 전송하는 방식을 채택하였습니다.

이렇게 함으로써 네트워크 통신 비용은 크게 줄여 서버 성능을 향상할 수 있었습니다.

### **Client**


**Client Class Diagram**

![졸프-클라이언트.drawio (1).png](https://s3-us-west-2.amazonaws.com/secure.notion-static.com/d6a0042b-b678-428b-a38d-bc9166e1b4a3/%EC%A1%B8%ED%94%84-%ED%81%B4%EB%9D%BC%EC%9D%B4%EC%96%B8%ED%8A%B8.drawio_(1).png)

Manger

[GraduationProject/GameManager.cs at main · ParkSungTaek/GraduationProject](https://github.com/ParkSungTaek/GraduationProject/blob/main/Client/Assets/Scripts/Managers/GameManager.cs)

[GraduationProject/Client/Assets/Scripts/Managers/SoundManager.cs at main · ParkSungTaek/GraduationProject](https://github.com/ParkSungTaek/GraduationProject/blob/main/Client/Assets/Scripts/Managers/SoundManager.cs)

[](https://github.com/ParkSungTaek/GraduationProject/blob/main/Client/Assets/Scripts/Managers/ResourceManager.cs)

![졸프-클라이언트.drawio (4).png](https://s3-us-west-2.amazonaws.com/secure.notion-static.com/97909d31-f09d-42af-9316-df2450251771/%EC%A1%B8%ED%94%84-%ED%81%B4%EB%9D%BC%EC%9D%B4%EC%96%B8%ED%8A%B8.drawio_(4).png)

우리의 게임 아키텍처는 최상위 Manager인 GameManager를 싱글톤 패턴으로 구현하였습니다. 이는 게임 내 다른 Manager들을 통합적으로 관리하는 역할을 수행합니다.

GameManager를 요청(Get)하는 시점에, 객체 반환 전 초기화 함수(Init)를 호출하여 GameManager의 존재가 항상 보장되도록 설계하였습니다. 또한, 다른 Manager들의 초기화 작업은 GameManager의 초기화 함수 내에서 수행됩니다. 이렇게 함으로써, 정확한 순서로 초기화를 할 수 있게 구현하였습니다.

SoundManager는 게임 내에서 지속적으로 사용되는 효과음 등을 Caching함으로써 로딩 시간과 가비지 컬렉션(GC) 빈도를 줄이는 역할을 합니다.

ResourceManager는 Prefab을 Load할 때 우선 Caching Dictionary를 확인 후 없다면 Load하고 Dictionary에 삽입합니다. 따라서, 한번 Load한 객체는 다시  Load하지 않아 로딩 시간을 줄일 수 있습니다.

Entity

![스크린샷(327).png](https://s3-us-west-2.amazonaws.com/secure.notion-static.com/d9bfc2c4-e92b-4e09-ab57-ebbdc74cd2ce/%EC%8A%A4%ED%81%AC%EB%A6%B0%EC%83%B7(327).png)

[GraduationProject/Entity.cs at main · ParkSungTaek/GraduationProject](https://github.com/ParkSungTaek/GraduationProject/blob/main/Client/Assets/Scripts/Statuses/Entity.cs)

[GraduationProject/MonsterController.cs at main · ParkSungTaek/GraduationProject](https://github.com/ParkSungTaek/GraduationProject/blob/main/Client/Assets/Scripts/Controllers/MonsterController.cs)

![졸프-클라이언트.drawioPlayer.png](https://s3-us-west-2.amazonaws.com/secure.notion-static.com/0adb0974-9217-4f01-a1ce-9daf442f8d5c/%EC%A1%B8%ED%94%84-%ED%81%B4%EB%9D%BC%EC%9D%B4%EC%96%B8%ED%8A%B8.drawioPlayer.png)

코드의 중복을 최소화하고 다형성을 증가시키기 위해, 'Entity'를 추상 클래스로 정의하였습니다. 이렇게 함으로써, 공통적인 행동이나 속성을 'Entity' 추상 클래스에 정의함으로써 코드 재사용성을 향상시킬 수 있었습니다.

구체적인 구현은 각각의 직업 클래스와 컨트롤러에서 담당합니다. 예를 들어, 'Wizard', 'Warrior' 등의 직업 클래스는 각자 고유한 능력과 행동 방식을 구현합니다. 이와 마찬가지로, 'TowerController', 'MonsterController'도 자신들만의 동작 방식과 로직을 구현합니다.

이렇게 하면서 우리는 객체 지향 프로그래밍의 핵심 원칙 중 하나인 다형성을 활용하였습니다. 같은 Entity 타입이라도 실제 인스턴스에 따라서 서로 다른 동작 방식과 로직을 가지게 되므로 확장성과 유연성이 높아집니다.

**Json**

![졸프-클라이언트.drawio (1) (1).png](https://s3-us-west-2.amazonaws.com/secure.notion-static.com/750ec849-a40f-4426-a69d-23dd049c27dc/%EC%A1%B8%ED%94%84-%ED%81%B4%EB%9D%BC%EC%9D%B4%EC%96%B8%ED%8A%B8.drawio_(1)_(1).png)

[GraduationProject/Client/Assets/Scripts/Utils/Util.cs at main · ParkSungTaek/GraduationProject](https://github.com/ParkSungTaek/GraduationProject/blob/main/Client/Assets/Scripts/Utils/Util.cs)

[GraduationProject/Client/Assets/Resources/Jsons at main · ParkSungTaek/GraduationProject](https://github.com/ParkSungTaek/GraduationProject/tree/main/Client/Assets/Resources/Jsons)

각 데이터의 세부 정보는 Json을 이용하여 저장하고 파싱 코드를 이용해 불러들였습니다. Json파싱은 유니티 라이브러리 UnityEngine의 JsonUtility를 사용하여 구현했습니다. 

TextAsset은 사용 후 GC의 대상이 되지 않도록 Resources.UnloadAsset를 사용하여 삭제해 주었습니다.
