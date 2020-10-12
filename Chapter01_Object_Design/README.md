# Chatpter 01

Ticket sale application

## Overview

기존 java로 되어 있는 코드를 C# 코드로 변환하는 과정 정리

## Code review

### Invitation.java and Invitation.cs

<details>
<summary>Code</summary>
<p>

``` java
import java.time.LocalDateTime;

public class Invitation {
    private LocalDateTime when;
}
```

``` csharp
using System;

public class Invitation {
    private DateTime when;
}
```

</p>
</details>

- Java의 경우는 LocalDateTime
- C#의 경우는 DateTime
- 둘의 큰 차이는 아마 없을 것이다.

### Ticket.java and Ticket.cs

<details>
<summary>Code</summary>
<p>

``` java
public class Ticket {
    private Long fee;

    public Long getFee() {
        return fee;
    }
}
```

``` csharp
public class Ticket {

    private long fee;

    public long Fee => fee;
}
```

</p>
</details>

- long type
  - Java에서 Long으로 쓴 건 java.lang.Long class를 쓰는 것이다. 즉 메소드 호출을 통한 값의 변화나 변환된 값을 얻기 위해 사용한다. 주로 type간 변환, string으로 변환에 사용할 필요가 있을 때 쓴다.
  - C#의 경우 primitive type 자체가 struct로 정의되어 있고 System.Int64이다. Java와 마찬가지로 메소드가 제공된다.
- getter
  - Java는 field값을 얻기 위한 public method로 get이라는 이름을 붙여 메소드로 구현한다. 명시적이다.
  - C#의 경우는 property라는 문법이 제공되고 get을 구현하면 getter가 되므로 함축적이면서 간결하게 구현이 가능하다. 거기에 Read-only body expression을 사용하면 더 함축적인 문법적 구현이 가능해진다. [참고](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/expression-bodied-members#read-only-properties)

### Bag.java and Bag.cs

<details>
<summary>Code</summary>
<p>

``` java
public class Bag {
    private Long amount;
    private Invitation invitation;
    private Ticket ticket;

    public Bag(long amount) {
        this(null, amount);
    }

    public Bag(Invitation invitation, long amount) {
        this.invitation = invitation;
        this.amount = amount;
    }

    public boolean hasInvitation() {
        return invitation != null;
    }

    public boolean hasTicket() {
        return ticket != null;
    }

    public void setTicket(Ticket ticket) {
        this.ticket = ticket;
    }

    public void minusAmount(Long amount) {
        this.amount -= amount;
    }

    public void plusAmount(Long amount) {
        this.amount += amount;
    }
}
```

``` csharp
public class Bag {

    private long amount;
    private Invitation invitation;
    private Ticket ticket;

    public Bag(long amount) : this(null, amount) {}

    public Bag(Invitation invitation, long amount)
    {
        this.invitation = invitation;
        this.amount = amount;
    }

    public bool HasInvitation => invitation != null;

    public bool HasTicket => ticket != null;

    public void SetTicket(Ticket ticket) => this.ticket = ticket;

    public void MinusAmount(long amount) => this.amount -= amount;

    public void PlusAmount(long amount) => this.amount += amount;
}
```

</p>
</details>

- call other constructor
  - Java에서 다른 constructor를 호출하는 건 this 키워드를 사용해서 parameter를 signature에 맞게 넘겨주면 호출된다.
  - C#의 경우 constructor body가 아닌 `:` 을 사용해서 호출한다. body의 경우는 필요에 따라 생략 가능하게 만들어 줄 수 있다.
- body expression
  - Ticket.cs에서도 설명했듯이, C#의 경우는 한 줄로 코드가 되어 있는 부분은 body expression으로 함축적으로 표현이 가능해진다.
  - 이런 경우는 return 키워드 생략, `{}` body 기호도 생략할 수 있다.

### Audience.java and Audience.cs

<details>
<summary>Code</summary>
<p>

``` java
public class Audience {
    private Bag bag;

    public Audience(Bag bag) {
        this.bag = bag;
    }

    public Bag getBag() {
        return bag;
    }
}
```

``` csharp
public class Audience {

    private Bag bag;

    public Audience(Bag bag) => this.bag = bag;

    public Bag Bag => bag;
}
```

</p>
</details>

- Ticket의 방식과 크게 다르지 않다. 생성자를 통해 Bag을 전달하는 게 추가된 것 정도다.

### TicketOffice.java and TicketOffice.cs

<details>
<summary>Code</summary>
<p>

``` java
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

public class TicketOffice {
    private Long amount;
    private List<Ticket> tickets = new ArrayList<>();

    public TicketOffice(Long amount, Ticket ... tickets) {
        this.amount = amount;
        this.tickets.addAll(Arrays.asList(tickets));
    }

    public Ticket getTicket() {
        return tickets.remove(0);
    }

    public void minusAmount(Long amount) {
        this.amount -= amount;
    }

    public void plusAmount(Long amount) {
        this.amount += amount;
    }
}
```

``` csharp
using System.Collections.Generic;
using System.Linq;

public class TicketOffice {

    private long amount;
    private List<Ticket> tickets = new List<Ticket>();

    public TicketOffice(long amount, params Ticket[] tickets)
    {
        this.amount = amount;
        this.tickets.AddRange(tickets);
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

    public void MinusAmount(long amount) => this.amount -= amount;

    public void PlusAmount(long amount) => this.amount += amount;
}
```

</p>
</details>

#### Java의 경우

- private List< Ticket > tickets = new ArrayList<>();
  - List< Ticket >은 List < E > type의 generic interface이므로 반드시 List interface를 상속받은 class의 객체가 와야 한다.
  - ArrayList < E >는 List< E > interface를 상속받았으므로 [리스코프 치환 원칙](https://en.wikipedia.org/wiki/Liskov_substitution_principle)이 성립한다.
  - ArrayList를 생성할 때 <> 안에 아무 타입도 넣지 않아도 되는 이유는 java의 generic class의 타입 추론 (type inference)이 가능하기 때문이다.
- this.tickets.addAll(Arrays.asList(tickets));
  - addAll 메소드는 파라미터로 Collection < Ticket > 를 받는다
  - 생성자의 ... 은 Varargs라고 하는 문법으로 Ticket type의 가변 array를 받은 것이라고 보면 된다.
  - 호출하는 쪽에서는 Ticket[] 타입이 아니라 Ticket type을 하나씩 나열해서 호출하는게 가능하다. [문서](https://docs.oracle.com/javase/8/docs/technotes/guides/language/varargs.html) 참고.
  - Arrays.asList(T ... a)로 정의되어 있는데 Ticket 타입의 array를 List< Ticket >로 return해 준다.
  - List < T >는 Collection < T >를 상속받은 interface이므로 역시 리스코프 치환 원칙에 따라서 assign이 가능해진다.
- 이 두 줄의 코드를 보고 이정도 얘기할 수 있다면 java를 겉핥기 식으로 공부한게 아니라고 얘기할 수 있다.
- tickets.remove(0)
  - remove 메소드는 0번째 index에 해당하는 element를 삭제하고 return 값으로 삭제한 element 값을 준다. 그러므로 remove 메소드 호출한 결과를 Ticket type으로 return해 줄 수 있다.

#### C#의 경우

- List generic class를 사용해 객체를 생성하고 같은 타입으로 변수가 선언되었다. Java와 같이 interface 타입으로 IList< Ticket >으로 할 수 있지만 Java와 같은 addAll() 메소드에 대응하는 List.AddRange 메소드로 호출하기 위해 같은 타입으로 선언해서 생성하였다.
- params keyword의 경우 Java의 varargs와 같은 문법이다.
- List에는 AddRange 메소드가 있는데 parameter type이 IEnumerable< T > 이다. 하지만 Java와 달리 parameter의 type을 변경하기 위해 Arrays.asList 메소드를 호출하여 타입을 변경하지 않아도 타입 추론이 가능하다.
- 왜냐하면 C#에서는 [] type은 Array abstract class를 상속 받고 여기서 IEnumerable interface를 구현했기 때문이다.
- C# 2.0 부터 Array는 여러 interface를 구현했는데 이들 interface를 참조하기 위해 별도로 캐스팅 작업을 하지 않아도 되게 암시적으로 구현했기 때문이다.
- 따라서 Ticket[] 타입의 변수를 명시적으로 캐스팅 문법을 작성하지 않아도 IEnumerable< Ticket > 타입으로 캐스팅이 되며 AddRange 메소드의 파라미터로 사용할 수 있다.
- 마지막으로 Ticket을 하나 얻는 메소드의 경우 Java와 달리 remove()와 같은 메소드를 제공하지 않는다. 따라서 해당 index의 element를 가져오고 RemoveAt() 메소드를 호출하여 삭제한 후에 가져온 값을 return하는 세 줄의 코드를 작성해야 한다.

#### 적고 나서 정리해보는 두 언어의 문법적 차이

- 어느 문법에 우열이 있다고 보기 어렵다.
- Java의 경우 List.remove 메소드는 index 접근해서 element를 가져오고 동시에 삭제가 되는 매우 효율적인 메소드를 제공하지만 C#은 이런 메소드가 없기 때문에 indexer를 통해 element를 가져오고 List.RemoveAt 메소드를 통해 삭제를 진행해야 한다.
- 반면 List 계열의 interface들 간의 캐스팅에서 Java의 경우 Arrays.asList와 같은 명시적 메소드 호출을 통한 명시적 inteface type 캐스팅이 진행되어야 하지만, C#의 경우는 Array와 List 간의 interface 구현이 IEnumerable로 동일하기 때문에 캐스팅 없이 암시적으로 문법적 구현이 가능하다.

### TicketSeller.java and TicketSeller.cs

- Audience와 구현 방식이 같기 때문에 생략

### Theater.java and Theater1.cs

<details>
<summary>Code</summary>
<p>

``` java
public class Theater {
    private TicketSeller ticketSeller;

    public Theater(TicketSeller ticketSeller) {
        this.ticketSeller = ticketSeller;
    }

    public void enter(Audience audience) {
        if (audience.getBag().hasInvitation()) {
            Ticket ticket = ticketSeller.getTicketOffice().getTicket();
            audience.getBag().setTicket(ticket);
        } else {
            Ticket ticket = ticketSeller.getTicketOffice().getTicket();
            audience.getBag().minusAmount(ticket.getFee());
            ticketSeller.getTicketOffice().plusAmount(ticket.getFee());
            audience.getBag().setTicket(ticket);
        }
    }
}
```

``` csharp
public class Theater1 {

    private TicketSeller ticketSeller;

    public Theater1(TicketSeller ticketSeller) => this.ticketSeller = ticketSeller;

    public void Enter(Audience audience)
    {
        if (audience.Bag.HasInvitation)
        {
            Ticket ticket = ticketSeller.TicketOffice.Ticket;
            audience.Bag.SetTicket(ticket);
        }
        else
        {
            Ticket ticket = ticketSeller.TicketOffice.Ticket;
            audience.Bag.MinusAmount(ticket.Fee);
            ticketSeller.TicketOffice.PlusAmount(ticket.Fee);
            audience.Bag.SetTicket(ticket);
        }
    }
}
```

</p>
</details>

- 어느 언어가 익숙하느냐에 따라 가독성의 차이가 있다고 보면 된다.
- 사실 C#의 경우 property로 구현하면 set 문법으로 setter도 구현할 수 있으므로 메소드 구현도 변경이 가능한데, 우선 getter만 비교할 수 있게 setter는 메소드 그대로 뒀다.
- 예를 들어 아래와 같은 Java의 setter의 경우

``` java
audience.Bag.SetTicket(ticket);
```

- 아래와 같은 C#의 property set으로 표현할 수 있다.

``` csharp
audience.Bag.Ticket = ticket;
```

- Java는 값을 얻어오고 변경하는 작업이 모두 메소드인 반면
- C#은 마치 public field를 접근하는 것 마냥 (Java 개발자 입장에서는) 매우 불편해 보이는건 사실이지만, 엄연히 property라는 C#의 문법을 통해 setter, getter를 구현한 것이므로 객체지향의 패러다임을 깨뜨리지 않는 문법이다.
- 이쯤에서 Java 개발자라면 [Project Lombok](https://projectlombok.org/)을 언급하고 싶겠지만, native sdk가 아닌 건 여기선 논외로 한다.
