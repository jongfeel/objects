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

    private Money CalculateFee(int audienceCount) => movie.CalculateMovieFee(this).Times(audienceCount);
}
```

</p>
</details>

눈에 띄는 차이점은 역시 [ExpressionBody](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/expression-bodied-members#read-only-properties)이다. C#의 코드를 매우 간략하고 함축적으로 설명해 주는 좋은 문법이라고 생각한다.

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
        discountPolicy == null ? fee : fee.Minus(discountPolicy.CalculateDiscountAmount(screening));
}
```

</p>
</details>

하나 언급하자면 유연한 설계를 설명하면서 Movie.calculateMovieFee(Screening screening)를 설명하는데 책에는 null 체크 하는 부분이 있지만 github에는 빠져있다. C#으로 옮기면서 그냥 conditional operator로 처리했다. Java에도 있는 문법이다.