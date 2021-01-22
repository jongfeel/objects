# 설계 개선하기

이건 책의 Chatper 01 객체, 설계 > 03 설계 개선하기 부분에 대한 내용이다.
/Chapter01/Object_Design/Theater의 내용을 가져와서, 설계가 변경되고 코드가 변경되는 부분에 대한 C# 코드를 추가한다.

## No details

객체 지향적인 설계 기법을 적용하는 부분이기 때문에 기존에 작성한 문법에서 크게 차이가 나는 부분은 없다.

## Remarks

중요한건 기존에 작성한 Program.cs에 수정 없이 class의 설계의 변경만 있는 것이므로 실행 프로그램의 수정이 없다는 것에 주목해야 한다.
이건 기존에 자신이 작성한 코드가 객체지향적인가를 판단하는 중요한 기준이 되기도 한다.

## Refactoring

객체지향적인 설계로 개선한 것과 리팩토링을 해야 하는 부분은 다르기 때문에 마지막 코드에 추가로 리팩토링을 진행한다.
책에는 트레이드 오프에 대해 설명하고 다시 결합도를 낮추는 방향으로 코드를 되돌릴 것을 권장하지만\
여기서는 그냥 리팩토링을 진행한다.

### Ticket

Ticket.fee의 액면가를 안정해 놓은 건 실제 실행 프로그램을 작성하는데 불편함을 초래한다.\
설계된 코드를 설명하는데는 문제는 없지만, 실행 프로그램에서는 실제 티켓 가격을 책정하고 티켓을 팔고 계산하는 부분을 수행해야 하기 때문에 TestProgram 작성할 때도 언급했듯이 티켓 가격을 하드코딩으로 10000이라고 해 두었다.

하지만 티켓 가격은 항상 일정하지 않고 변경이 될 가능성이 매우 높으므로 생성할 때 가격을 설정할 수 있게 생성자 코드를 추가하는 것이 좋다고 본다.

``` csharp
public class Ticket {

    private long fee = 10000;

    public long Fee => fee;

    public Ticket(long fee) => this.fee = fee;
}
```

하지만 생성자 메소드, private field 값, getter 까지 하면 코드 라인이 3줄이나 된다. Ticket이 하는 일이라고는 fee 값 세팅 뿐이라면 fee를 Property로 설정하고 생성자를 없애는 것도 방법일 수 있다.

``` csharp
public class Ticket {

    public long Fee { get; set; } = 10000;
}
```

단, 티켓 가격을 10000이 아닌 액수로 세팅하기 위해서는 객체 생성과 동시에 Fee property 값을 변경해야 한다. 아래와 같은 코드로 간단하게 세팅 가능하다.

``` csharp
Ticket ticket = new Ticket() { Fee = 15000 };
```

> C#의 Property 문법은 2.0 버전부터 지원을 했는데,  getter, setter 메소드를 작성하는 수고로움을 덜어주는 좋은 문법 중에 하나라고 본다.

### Bag

Bag class의 설계 변경은 매우 훌륭하지만 Bag.Hold 메소드는 매우 눈에 띄는 refactoring point를 준다.

원래 코드

``` csharp
public long Hold(Ticket ticket)
{
    if (HasInvitation)
    {
        SetTicket(ticket);
        return 0;
    }
    else
    {
        SetTicket(ticket);
        MinusAmount(ticket.Fee);
        return ticket.Fee;
    }
}

private void SetTicket(Ticket ticket) => this.ticket = ticket;
```

첫번째는,\
SetTicket() 메소드인데, HasInvitation의 분기처리 여부와 상관 없이 SetTicket() 메소드를 호출하기 때문에, if else 문 위로 끌어올려 한 줄로 써볼 수 있다.

``` csharp
public long Hold(Ticket ticket)
{
    SetTicket(ticket);
    if (HasInvitation)
    {
        return 0;
    }
    else
    {
        MinusAmount(ticket.Fee);
        return ticket.Fee;
    }
}

private void SetTicket(Ticket ticket) => this.ticket = ticket;
```

두번째는,\
SetTicket는 setter의 역할 뿐인데다 private으로 선언되었기 때문에 Java로 하던 C# 으로 하던 메소드 호출로 처리 할 필요가 없다.\
그러므로 메소드를 삭제해 주고, ticket field를 직접 assign 해준다.

``` csharp
public long Hold(Ticket ticket)
{
    this.ticket = ticket;
    if (HasInvitation)
    {
        return 0;
    }
    else
    {
        MinusAmount(ticket.Fee);
        return ticket.Fee;
    }
}
```

세번째는,\
MinusAmount() 메소드 역시 private이고 ticket.Fee를 빼는 연산 뿐이므로 이것도 메소드를 삭제하고 직접 amount field 값을 연산하게 바꿀 수 있다.

``` csharp
public long Hold(Ticket ticket)
{
    this.ticket = ticket;
    if (HasInvitation)
    {
        return 0;
    }
    else
    {
        amount -= ticket.Fee;
        return ticket.Fee;
    }
}
```

네번째는,\
HasInvitation에 따라 0 또는 ticket.Fee를 리턴해 주는 코드인데 HasInvitation이 true일 때 amount -= 0 이라고 해도 amount의 값에는 변화가 없으므로 아래와 같이 간소화한 코드로 변경할 수 있다.

``` csharp
public long Hold(Ticket ticket)
{
    this.ticket = ticket;
    long fee = HasInvitation ? 0 : ticket.Fee;
    amount -= fee;
    return fee;
}
```

마지막으로,
HasInvitation property 역시 Invitation의 존재 유무를 판단하는 것이고 private getter이기 때문에 삭제하고 null 체크 해주는 로직으로 바꾼다.

``` csharp
public long Hold(Ticket ticket)
{
    this.ticket = ticket;
    long fee = invitation != null ? 0 : ticket.Fee;
    amount -= fee;
    return fee;
}
```

### TicketOffice

TicketOffice의 경우는 private method로 바뀐 PlusAmount, MinusAmount를 삭제하고 바로 field 값을 접근하는 코드로 변경해 볼 수 있다.

원래 코드

``` csharp
private long amount;

public void SellTicketTo(Audience audience) => PlusAmount(audience.Buy(Ticket));

private void MinusAmount(long amount) => this.amount -= amount;

private void PlusAmount(long amount) => this.amount += amount;
```

- MinusAmount는 필요가 없으므로 삭제한다. 아마 티켓 환불하는 용도로 쓸 수 있지만 그 때가 되더라도 바로 amount에 접근해서 계산하는 코드를 짠다.
- PlusAmount 역시 바로 계산하는 코드로 변경한다.
- 메소드의 호출이 영어를 읽는 느낌을 주기 때문에 직관적일 수 있다. 하지만 += 연산자 역시 기존 값에 더해서 저장한다는 뜻이고 거의 모든 개발자에게 익숙한 연산이므로 더 직관적일 수 있다.

``` csharp
private long amount;

public void SellTicketTo(Audience audience) => amount += audience.Buy(Ticket);

```

## Real improved design by the real world

이제 리팩토링은 여기까지 해도 될 것 같다. 하지만 진짜 중요한게 남아 있다. 여태까지의 class 설계는 책에 나와 있는 내용을 설명하기 위한 방향이었고, 다시 객체들의 행동들을 보면 실제 세계에서 일어날 법한 일을 하지 않고 있다.

### Analysis problem - buy ticket first, but audience has invitation

audience가 ticket을 buy 하기 위해 bag에 ticket을 hold 시킨다. 그리고 ticket 금액을 돌려준다. 이것만 보면 이미 티켓은 구입 했고 구입 금액을 return 해 줬으니까 최종 결과만 놓고 보면 별 문제가 없어 보인다.

``` csharp
public class Audience {
    public long Buy(Ticket ticket) => bag.Hold(ticket);
}
```

그러면 bag.Hold(ticket)를 호출했으니 이 메소드를 매우 자세히 분석해보자.

- ticket을 가방에 넣어 둠 (this.ticket = ticket)
- invitation이 있는지 조사를 함 (invitation != null)
  - invitation이 있다면 return 값이 0이다, 즉 공짜일 수 있지만 초대장과 교환한다는 의미로도 해석할 수 있다.
  - invitation이 없다면 ticket 비용만큼 bag.amount에서 차감하고 ticket 비용을 return 한다. 이건 정상적인 프로세스다.
  
그런데 `this.ticket = ticket;` 이 코드가 ticket을 bag에 넣었다는 걸 표현하는 건데 invitation이 있는지는 ticket을 할당한 이후에 있는지 판단한다.

현실 세계에서는 ticket을 받고 비용 지불을 안했다는 얘기다.

``` csharp
public class Bag
{
    public long Hold(Ticket ticket)
    {
        this.ticket = ticket;
        long fee = invitation != null ? 0 : ticket.Fee;
        amount -= fee;
        return fee;
    }
}
```

현실 세계를 잘 생각해 보자, audience가 invitation을 가지고 있다면 ticketSeller에게 먼저 invitation을 보여주고 난 이후 ticket을 받는 과정이 자연스럽다. 능청스럽게 ticket을 받고 가방에 넣었는데 "어라? invitation 있네? 계산 안해도 되죠? (라고 하면 다행이다. 그런데 그냥 먹튀 가능성도 있음)"

이런 프로세스는 현실 세계에는 없다. 있으면 양아치겠지.

그래서 ticketOffice에서 미리 invitation을 확인한다. 그리고 있으면 invitation 교환 ticket으로 구매한다고 생각해 보면 된다.

코드 상으로는 fee = 0을 세팅해 준다.

현실 세계에서는 "초대장으로 교환"과 같은 스탬프가 찍힌 ticket으로 교환해 줬다는 의미로 해석할 수 있다.

그리고 나서 ticket 구매 프로세스를 진행한다.

``` csharp
public class Ticket {
    
    public long Fee { get; set; } = 10000;

    public void InvitationExchanged() => Fee = 0;
}
```

``` csharp
public class TicketOffice
{
    private long amount;
    private List<Ticket> tickets = new List<Ticket>();

    public TicketOffice(long amount, params Ticket[] tickets)
    {
        this.amount = amount;
        this.tickets.AddRange(tickets);
    }

    public void SellTicketTo(Audience audience)
    {
        if (audience.Invitation != null)
        {
            Ticket.InvitationExchanged();
        }
        
        amount += audience.Buy(Ticket);
    }

    public Ticket Ticket
    {
        get
        {
            Ticket t = tickets[0];
            tickets.RemoveAt(0);
            return t;
        }
    }
}
```

Bag class는 아래와 같이 변경한다.

Hold method는 ticket을 넣고 티켓 액면가 만큼 내 가방에서 액수를 제외하고 티켓 가격을 return해 준다. Hold method가 이제서야 한가지 일을 하는 method로 단순화가 되었다. (SRP: Single Responsibility Principle, 단일 책임 원칙)

Invitation의 경우는 constructor에서 주입하므로 Invitation을 property로 설정하고  private setter, public getter로 만들어 준다.

그러면 invitation은 ticket과 상관 없이 가방에 넣고 빼낼 수 있는 독립적인 객체가 된다.

``` csharp
public class Bag {
    public Invitation Invitation { private set; get; }
    private Ticket ticket;

    public Bag(Invitation invitation, long amount)
    {
        Invitation = invitation;
        this.amount = amount;
    }

    public long Hold(Ticket ticket)
    {
        this.ticket = ticket;
        amount -= ticket.Fee;
        return ticket.Fee;
    }
}
```

Audience 역시 invitation과 ticket을 선택할 수 있어야 하므로 Invitation getter property를 추가한다.

``` csharp
public class Audience {
    public Invitation Invitation => bag.Invitation;
}
```

### Readable code

읽을 수 있는 코드는 코드의 흐름을 읽었을 때 결과와 상관 없이 순서에 맞게 처리가 되었는지도 중요하다고 생각한다. 어차피 처리 결과가 똑같은데 처리 순서가 무슨 상관이냐 라고 생각한다면 진짜 리팩토링을 한다는 것에 대해서 다시 생각해 볼 필요가 있다. 왜냐하면 리팩토링을 하는게 뭘 하는 건지도 모르고 하는 얘기니까.

리팩토링은 단순히 중복 코드 제거를 넘어서 읽을 수 있는 코드로의 정리까지 포함해야 한다고 본다. 영어 단어 선택 및 동사의 적절한 사용은 기본이고, 읽으면 이해가 되는 로직의 흐름이 중요하다.

간혹 읽기 쉬운 코드가 어렵고 쉬운 문법 사용의 차이라고 착각하는 사람도 있는데, 자기가 모르는 문법을 쓴다고 해서 읽기 어려운 코드는 아니다. 예를 들면, C#의 LINQ 문법이 익숙하지 않고 잘 모르기 때문에 이걸 모두 for문과 if문으로 처리하는 코드로 바꾸는게 읽기 쉬운 코드로의 리팩토링이 아니라는 뜻이다.

한번 쯤 본인이 작성한 코드를 다시 리뷰해 보면서 읽어보자. 내가 어떤 생각으로 코드를 짰는지 다시 한번 돌아보게 되는 소중한 시간일 수 있다.