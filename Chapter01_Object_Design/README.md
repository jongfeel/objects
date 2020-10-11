# Chatpter 01

Ticket sale application

## Overview

기존 java로 되어 있는 코드를 C# 코드로 변환하는 과정 정리

## Code review

### Invitation.java and Invitation.cs

- Java의 경우는 LocalDateTime
- C#의 경우는 DateTime
- 둘의 큰 차이는 아마 없을 것이다.

<details><summary>Code</summary>
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

### Ticket.java and Ticket.cs

- long type
  - Java에서 Long으로 쓴 건 java.lang.Long class를 쓰는 것이다. 즉 메소드 호출을 통한 값의 변화나 변환된 값을 얻기 위해 사용한다. 주로 type간 변환, string으로 변환에 사용할 필요가 있을 때 쓴다.
  - C#의 경우 primitive type 자체가 struct로 정의되어 있고 System.Int64이다. Java와 마찬가지로 메소드가 제공된다.
- getter
  - Java는 field값을 얻기 위한 public method로 get이라는 이름을 붙여 메소드로 구현한다. 명시적이다.
  - C#의 경우는 property라는 문법이 제공되고 get을 구현하면 getter가 되므로 함축적이면서 간결하게 구현이 가능하다. 거기에 Read-only body expression을 사용하면 더 함축적인 문법적 구현이 가능해진다. [참고](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/expression-bodied-members#read-only-properties)

<details><summary>Code</summary>
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

### Bag.java and Bag.cs

- call other constructor
  - Java에서 다른 constructor를 호출하는 건 this 키워드를 사용해서 parameter를 signature에 맞게 넘겨주면 호출된다.
  - C#의 경우 constructor body가 아닌 `:` 을 사용해서 호출한다. body의 경우는 필요에 따라 생략 가능하게 만들어 줄 수 있다.
- body expression
  - Ticket.cs에서도 설명했듯이, C#의 경우는 한 줄로 코드가 되어 있는 부분은 body expression으로 함축적으로 표현이 가능해진다.
  - 이런 경우는 return 키워드 생략, `{}` body 기호도 생략할 수 있다.

<details><summary>Code</summary>
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