# Chapter 02

Movie program

## Overview

영화 예매 시스템을 예제로 객체지향 프로그래밍에서 다루는 중요한 개념들에 대해 설명한다.

주로 객체들의 협력, 할인 요금을 구하기 위해 협력하는 객체에 대한 이야기가 나오고, 객체지향에서 빠지지 않는 키워드인 상속, 다형성, 인터페이스, 추상화 등등에 대해 얘기한다.

물론 이런 내용들은 책을 보고 이해하면 되고 내가 지금부터 하려는 건 이 repository readme에도 언급이 되어 있는 내용이지만

- Java와 C#의 비교
- (필요시) 리팩토링 및 재설계

를 진행할 것이다.

이번 챕터 역시 test program이 없으므로 test program을 만들 예정이며 unit test 역시 추가로 포함시켜보려 한다. 얘기가 나온 김에 챕터01도 unit test를 추가해 보면 좋을 것 같다.

## Code review

### [Money.java](https://github.com/eternity-oop/object/blob/master/chapter02/src/main/java/org/eternity/money/Money.java) and [Money.cs](https://github.com/jongfeel/objects/blob/main/Chapter02/Movie/Money.cs)

<details>
<summary>Code</summary>
<p>

``` java
package org.eternity.money;

import java.math.BigDecimal;
import java.util.Objects;

public class Money {
    public static final Money ZERO = Money.wons(0);

    private final BigDecimal amount;

    public static Money wons(long amount) {
        return new Money(BigDecimal.valueOf(amount));
    }

    public static Money wons(double amount) {
        return new Money(BigDecimal.valueOf(amount));
    }

    Money(BigDecimal amount) {
        this.amount = amount;
    }

    public Money plus(Money amount) {
        return new Money(this.amount.add(amount.amount));
    }

    public Money minus(Money amount) {
        return new Money(this.amount.subtract(amount.amount));
    }

    public Money times(double percent) {
        return new Money(this.amount.multiply(BigDecimal.valueOf(percent)));
    }

    public boolean isLessThan(Money other) {
        return amount.compareTo(other.amount) < 0;
    }

    public boolean isGreaterThanOrEqual(Money other) {
        return amount.compareTo(other.amount) >= 0;
    }

    public boolean equals(Object object) {
        if (this == object) {
            return true;
        }

        if (!(object instanceof Money)) {
            return false;
        }

        Money other = (Money)object;
        return Objects.equals(amount.doubleValue(), other.amount.doubleValue());
    }

    public int hashCode() {
        return Objects.hashCode(amount);
    }

    public String toString() {
        return amount.toString() + "원";
    }
}
```

``` csharp
public class Money {
    
    public static Money ZERO = Money.Wons(0);

    private decimal amount;

    public static Money Wons(long amount) => new Money((decimal)amount);

    public static Money Wons(double amount) => new Money((decimal)amount);

    private Money(decimal amount) => this.amount = amount;

    public static Money operator +(Money a, Money b) => new Money(a.amount + b.amount);

    public static Money operator -(Money a, Money b) => new Money(a.amount - b.amount);

    public static Money operator *(Money a, double b) => new Money(a.amount * (decimal)b);

    public static bool operator <(Money a, Money b) => a.amount < b.amount;

    public static bool operator >(Money a, Money b) => a.amount > b.amount;

    public static bool operator >=(Money a, Money b) => a.amount >= b.amount;

    public static bool operator <=(Money a, Money b) => a.amount >= b.amount;

    public override string ToString() => $"{amount}원";
}
```

</p>
</details>

C++, C# 프로그래머라면 Money class를 보고 operator overloading을 적용하면 좋다는 생각이 떠오를 것이다.

그래서 operator overloading을 적용해서 Money class에 대한 [리팩토링을 진행했다](https://github.com/jongfeel/objects/pull/5).

이후 Money class를 사용하는 부분은 operator overloading이 적용된 코드로 변경하였다.

plus(), minus(), times() 메소드해 해당하는 부분은 각각 +, -, * 연산자로 직관적으로 사용할 수 있다.

isLessThan()은 < 연산자만 구현하면 될 것 같은데, > 연산자 역시 함께 구현해야 한다. 둘 중 하나만 구현하면 에러가 난다.

isGreaterThanOrEqual() 역시 <=, >= 연산자 두개가 필요하다.

### [Screening.java](https://github.com/eternity-oop/object/blob/master/chapter02/src/main/java/org/eternity/movie/step01/Screening.java) and [Screening.cs](https://github.com/jongfeel/objects/blob/main/Chapter02/Movie/Screening.cs)

<details>
<summary>Code</summary>
<p>

``` java
package org.eternity.movie.step01;

import org.eternity.money.Money;

import java.time.LocalDateTime;

public class Screening {
    private Movie movie;
    private int sequence;
    private LocalDateTime whenScreened;

    public Screening(Movie movie, int sequence, LocalDateTime whenScreened) {
        this.movie = movie;
        this.sequence = sequence;
        this.whenScreened = whenScreened;
    }

    public LocalDateTime getStartTime() {
        return whenScreened;
    }

    public boolean isSequence(int sequence) {
        return this.sequence == sequence;
    }

    public Money getMovieFee() {
        return movie.getFee();
    }

    public Reservation reserve(Customer customer, int audienceCount) {
        return new Reservation(customer, this, calculateFee(audienceCount),
                audienceCount);
    }

    private Money calculateFee(int audienceCount) {
        return movie.calculateMovieFee(this).times(audienceCount);
    }
}
```

``` csharp
using System;

public class Screening
{
    private Movie movie;
    private int sequence;
    private DateTime whenScreened;

    public Screening(Movie movie, int sequence, DateTime whenScreened)
    {
        this.movie = movie;
        this.sequence = sequence;
        this.whenScreened = whenScreened;
    }

    public DateTime StartTime => whenScreened;

    public bool IsSequence(int sequence) => this.sequence == sequence;

    public Money MovieFee => movie.Fee;

    public Reservation Reserve(Customer customer, int audienceCount) =>
        new Reservation(customer, this, CalculateFee(audienceCount), audienceCount);

    private Money CalculateFee(int audienceCount) => movie.CalculateMovieFee(this) * audienceCount;
}
```

</p>
</details>

눈에 띄는 차이점은 역시 [ExpressionBody](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/expression-bodied-members#read-only-properties)이다. C#의 코드를 매우 간략하고 함축적으로 설명해 주는 좋은 문법이라고 생각한다.

또 chapter01에도 언급한 얘기지만 LocalDateTime은 C#에서는 DateTime으로 대체한다.

### [Resevration.java](https://github.com/eternity-oop/object/blob/master/chapter02/src/main/java/org/eternity/movie/step01/Reservation.java) and [Resevration.cs](https://github.com/jongfeel/objects/blob/main/Chapter02/Movie/Reservation.cs)

<details>
<summary>Code</summary>
<p>

``` java
package org.eternity.movie.step01;

import org.eternity.money.Money;

public class Reservation {
    private Customer customer;
    private Screening Screening;
    private Money fee;
    private int audienceCount;

    public Reservation(Customer customer, Screening Screening, Money fee, int audienceCount) {
        this.customer = customer;
        this.Screening = Screening;
        this.fee = fee;
        this.audienceCount = audienceCount;
    }
}
```

``` csharp
public class Reservation
{
    private Customer customer;
    private Screening screening;
    private Money fee;
    private int audienceCount;

    public Reservation(Customer customer, Screening screening, Money fee, int audienceCount)
    {
        this.customer = customer;
        this.screening = screening;
        this.fee = fee;
        this.audienceCount = audienceCount;
    }
}
```

</p>
</details>

너무 Class 기본 코드로만 작성하다 보니 문법 마저도 똑같은 코드다. 할 얘기가 없으므로 패스

### [Movie.java](https://github.com/eternity-oop/object/blob/master/chapter02/src/main/java/org/eternity/movie/step01/Movie.java) and [Movie.cs](https://github.com/jongfeel/objects/blob/main/Chapter02/Movie/Movie.cs)

<details>
<summary>Code</summary>
<p>

``` java
  
package org.eternity.movie.step01;

import org.eternity.money.Money;

import java.time.Duration;

public class Movie {
    private String title;
    private Duration runningTime;
    private Money fee;
    private DiscountPolicy discountPolicy;

    public Movie(String title, Duration runningTime, Money fee, DiscountPolicy discountPolicy) {
        this.title = title;
        this.runningTime = runningTime;
        this.fee = fee;
        this.discountPolicy = discountPolicy;
    }

    public Money getFee() {
        return fee;
    }

    public Money calculateMovieFee(Screening screening) {
        if (discountPolicy == null) {
            return fee;
        }
        return fee.minus(discountPolicy.calculateDiscountAmount(screening));
    }
}
```

``` csharp
using System;

public class Movie
{
    private string title;
    private TimeSpan runningTime;
    private Money fee;
    private DiscountPolicy discountPolicy;

    public Movie(string title, TimeSpan runningTime, Money fee, DiscountPolicy discountPolicy) {
        this.title = title;
        this.runningTime = runningTime;
        this.fee = fee;
        this.discountPolicy = discountPolicy;
    }

    public Money Fee => fee;

    public Money CalculateMovieFee(Screening screening) =>
        discountPolicy == null ? fee : fee - discountPolicy.CalculateDiscountAmount(screening);
}
```

</p>
</details>

하나 언급하자면 유연한 설계를 설명하면서 Movie.calculateMovieFee(Screening screening)를 설명하는데 책에는 discountPolicy에 대해 null 체크 하는 부분이 있지만 github의 [Movie.java](https://github.com/eternity-oop/object/blob/master/chapter02/src/main/java/org/eternity/movie/step01/Movie.java)에는 빠져있다. C#으로 옮기면서 그냥 conditional operator로 처리했다. 이건 Java에도 있는 문법이다.

또 이걸 그냥 둘 수 없어 pull request를 만들었다.

https://github.com/eternity-oop/object/pull/4

하지만 예제를 설명하기 위해 추가된 임시 코드라고 하여 closed 당함 ㅜㅜ

### [DiscountPolicy.java](https://github.com/eternity-oop/object/blob/master/chapter02/src/main/java/org/eternity/movie/step01/DiscountPolicy.java) and [DiscountPolicy.cs](https://github.com/jongfeel/objects/blob/main/Chapter02/Movie/DiscountPolicy.cs)

<details>
<summary>Code</summary>
<p>

``` java
package org.eternity.movie.step01;

import org.eternity.money.Money;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

public abstract class DiscountPolicy {
    private List<DiscountCondition> conditions = new ArrayList<>();

    public DiscountPolicy(DiscountCondition ... conditions) {
        this.conditions = Arrays.asList(conditions);
    }

    public Money calculateDiscountAmount(Screening screening) {
        for(DiscountCondition each : conditions) {
            if (each.isSatisfiedBy(screening)) {
                return getDiscountAmount(screening);
            }
        }

        return Money.ZERO;
    }

    abstract protected Money getDiscountAmount(Screening Screening);
}
```

``` csharp
using System.Collections.Generic;
using System.Linq;

public abstract class DiscountPolicy
{
    private List<DiscountCondition> conditions = new List<DiscountCondition>();

    public DiscountPolicy(params DiscountCondition[] conditions) => this.conditions.AddRange(conditions);

    public virtual Money CalculateDiscountAmount(Screening screening) =>
        conditions.Count(condition => condition.IsSatisfiedBy(screening)) > 0 ?
        GetDiscountAmount(screening) : Money.ZERO;

    abstract protected Money GetDiscountAmount(Screening screening);
}
```

</p>
</details>

#### DiscountPolicy constructor

chapter01의 theater 예제에서도 나왔지만 Java는 Arrays.asList 메소드 호출을 통해 List<> type의 객체로 가져온다.
반면 C#은 List<> 객체의 AddRange 메소드를 통해 추가하는 형태로 진행할 수 있다.
표현 방식은 다르지만 list 형태로 받는 conditions를 간단하게 추가해 주는 문법이라고 보면 된다.

#### CalculateDiscountAmount

여기에서도 logic의 의도를 잘 파악하고 구현한다면 변형이 얼마든지 가능하다.
conditions 중에 screening을 만족 시키는(isSatisfiedBy) condition을 찾으면 getDiscountAmount라는 메소드를 호출해서 할인 가격을 return 하는 코드이다.
C#에서는 Count() 라는 linq를 사용해 조건에 부합하는 condition이 존재하면 메소드를 호출하는 식으로 변형했다.

Java 역시 stream을 사용하면 C#과 비슷한 문법으로 변형 가능하다.

``` java
return conditions.stream().filter(condition -> condition.isSatisfiedBy(screening)).count() > 0 ? getDiscountAmount(screening) : Money.ZERO;
```

### [DiscountCondition.java](https://github.com/eternity-oop/object/blob/master/chapter02/src/main/java/org/eternity/movie/step01/DiscountCondition.java) and [DiscountCondition.cs](https://github.com/jongfeel/objects/blob/main/Chapter02/Movie/DiscountCondition.cs)

<details>
<summary>Code</summary>
<p>

``` java
package org.eternity.movie.step01;

public interface DiscountCondition {
    boolean isSatisfiedBy(Screening screening);
}
```

``` csharp
  
public interface DiscountCondition
{
    bool IsSatisfiedBy(Screening screening);
}
```

</p>
</details>

Java는 boolean이고 C#은 bool을 씀. 더 이상의 자세한 설명은 없음.