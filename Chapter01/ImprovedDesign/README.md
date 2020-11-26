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

Bag class의 설계 변경은 매우 훌륭하지만 Bag.Hold 메소드는 매우 눈에 띄는 refactoring point를 준다.\

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
SetTicket() 메소드인데, HasInvitation의 여부와 상관 없이 SetTicket() 메소드는 호출하기 때문에 한 줄로 바꿔볼 수 있다.

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
SetTicket는 setter의 역할 뿐이므로 Java로 하던 C# 으로 하던 메소드 호출로 할 필요가 없다.\
그러므로 메소드를 삭제해 주고, private field를 직접 set 해준다.

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
MinusAmount() 메소드 역시 private인데 ticket.Fee를 빼는 연산 뿐이므로 이것도 메소드를 삭제하고 직접 amount field 값을 연산하게 바꿀 수 있다.

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

네번째는,
HasInvitation에 따라 0 또는 ticket.Fee를 리턴해 주는 코드인데 amount의 값에 영향을 주는 값이라고 생각해 보면 HasInvitation이 true일 때 amount -= 0 이라고 해도 amount의 값에는 변화가 없으므로 아래와 같이 간소화한 코드로 변경할 수 있다.

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
HasInvitation 역시 Invitation의 존재 유무를 판단하는 것이고 private이기 때문에 삭제하고 null 체크 해주는 로직으로 바꾼다.

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
- 메소드의 호출이 영어를 읽는 느낌을 주기 때문에 직관적일 수 있다. 하지만 += 연산자 역시 기존 값에 더해서 저장한다는 뜻이므로 더 직관적일 수 있다. 

``` csharp
private long amount;

public void SellTicketTo(Audience audience) => amount += audience.Buy(Ticket);

```

