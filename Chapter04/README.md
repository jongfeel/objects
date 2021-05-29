# Chapter 04

Movie program

## Overview

영화 예매 시스템을 데이터 중심으로 바라보면서 발생하는 문제점에 대해서 짚어보는 내용이다.

책에서 잘못됐다는 내용을 얘기해주지 않으면 뭐가 잘못된건지 모를 코드가 있어서 리팩토링을 진행하고 싶은 욕구가 상당히 많이 느껴진다.

이후 챕터들에서 개선된 코드로 변경되긴 하지만 이번 챕터 내에서 진행될 만한 부분들은 진행해볼 예정이다.

물론 이런 내용들은 책을 보고 이해하면 되고 내가 지금부터 하려는 건 이 repository readme에도 언급이 되어 있는 내용이지만

- Java와 C#의 비교
- (필요시) 리팩토링 및 재설계

를 진행할 것이다.

이번 챕터 역시 test program이 없으므로 test program을 만들 예정이며 unit test 역시 추가로 포함시켜보려 한다.

## Code review

### [Movie.java](https://github.com/eternity-oop/object/blob/master/chapter04/src/main/java/org/eternity/movie/step01/Movie.java) and [Movie.cs](https://github.com/jongfeel/objects/blob/main/Chapter04/Movie/Movie.cs)

<details>
<summary>Code</summary>
<p>

``` java
package org.eternity.movie.step01;

import org.eternity.money.Money;

import java.time.Duration;
import java.util.Arrays;
import java.util.Collections;
import java.util.List;

public class Movie {
    private String title;
    private Duration runningTime;
    private Money fee;
    private List<DiscountCondition> discountConditions;

    private MovieType movieType;
    private Money discountAmount;
    private double discountPercent;

    public Movie(String title, Duration runningTime, Money fee, double discountPercent, DiscountCondition... discountConditions) {
        this(MovieType.PERCENT_DISCOUNT, title, runningTime, fee, Money.ZERO, discountPercent, discountConditions);
    }

    public Movie(String title, Duration runningTime, Money fee, Money discountAmount, DiscountCondition... discountConditions) {
        this(MovieType.AMOUNT_DISCOUNT, title, runningTime, fee, discountAmount, 0, discountConditions);
    }

    public Movie(String title, Duration runningTime, Money fee) {
        this(MovieType.NONE_DISCOUNT, title, runningTime, fee, Money.ZERO, 0);
    }

    private Movie(MovieType movieType, String title, Duration runningTime, Money fee, Money discountAmount, double discountPercent,
                  DiscountCondition... discountConditions) {
        this.movieType = movieType;
        this.title = title;
        this.runningTime = runningTime;
        this.fee = fee;
        this.discountAmount = discountAmount;
        this.discountPercent = discountPercent;
        this.discountConditions = Arrays.asList(discountConditions);
    }

    public MovieType getMovieType() {
        return movieType;
    }

    public void setMovieType(MovieType movieType) {
        this.movieType = movieType;
    }

    public Money getFee() {
        return fee;
    }

    public void setFee(Money fee) {
        this.fee = fee;
    }

    public List<DiscountCondition> getDiscountConditions() {
        return Collections.unmodifiableList(discountConditions);
    }

    public void setDiscountConditions(List<DiscountCondition> discountConditions) {
        this.discountConditions = discountConditions;
    }

    public Money getDiscountAmount() {
        return discountAmount;
    }

    public void setDiscountAmount(Money discountAmount) {
        this.discountAmount = discountAmount;
    }

    public double getDiscountPercent() {
        return discountPercent;
    }

    public void setDiscountPercent(double discountPercent) {
        this.discountPercent = discountPercent;
    }
}
```

``` csharp
using System;
using System.Collections.ObjectModel;

public class Movie {
    private string title;
    private TimeSpan runningTime;
    public Money Fee { private set; get; }
    public ReadOnlyCollection<DiscountCondition> DiscountConditions { private set; get;}

    public MovieType MovieType { private set; get; }
    public Money DiscountAmount { private set; get; }
    public double DiscountPercent { private set; get; }

    public Movie(string title, TimeSpan runningTime, Money fee, double discountPercent, params DiscountCondition[] discountConditions)
        : this(MovieType.PERCENT_DISCOUNT, title, runningTime, fee, Money.ZERO, discountPercent, discountConditions)
    {
    }

    public Movie(string title, TimeSpan runningTime, Money fee, Money discountAmount, params DiscountCondition[] discountConditions)
        : this(MovieType.AMOUNT_DISCOUNT, title, runningTime, fee, discountAmount, 0, discountConditions)
    {    
    }

    public Movie(string title, TimeSpan runningTime, Money fee)
        : this(MovieType.NONE_DISCOUNT, title, runningTime, fee, Money.ZERO, 0)
    {    
    }

    private Movie(MovieType movieType, string title, TimeSpan runningTime, Money fee, Money discountAmount, double discountPercent, params DiscountCondition[] discountConditions)
    {
        this.MovieType = movieType;
        this.title = title;
        this.runningTime = runningTime;
        this.Fee = fee;
        this.DiscountAmount = discountAmount;
        this.DiscountPercent = discountPercent;
        this.DiscountConditions = Array.AsReadOnly(discountConditions);
    }
}
```

</p>
</details>

#### Getter,Setter vs Property

Movie.java를 Movie.cs로 바꾸면서 느낄 수 있는 가장 큰 변화는 역시 getter, setter를 한번에 묶어주는 C#의 property 문법이다.

Java의 이런 문법은 Lombok을 쓰고 싶게 만드는 매우 흔한 getter, setter 문법이다.

``` java
private Money fee;

public Money getFee() {
    return fee;
}

public void setFee(Money fee) {
    this.fee = fee;
}
```

Lombok을 쓰면 아래와 같이 쓸 수 있을 것이다.

``` java
@Getter
@Setter
public class Movie {
    private Money fee;
}
```

하지만 C#은 lombok과 같은 외부 패키지 없이 단 한줄로 이 코드를 구현할 수 있다. C# 모르는 사람이 보면 Fee라는 field 값을 public으로 선언하다니? 객체지향적이지 못하다! 라고 주장할 수 있으나 set; get; 이라는 키워드 안에는 setter, getter method가 구현이 되어 있으므로 객체지향의 본질인 캡슐화를 깨뜨리지 않는 아름다운 문법임을 알아야 한다.

``` csharp
public Money Fee { set; get; }
```

#### Collections.unmodifiableList vs ReadOnlyCollection

Java의 unmodifiableList는 이름 자체만으로도 매우 java스러운 메소드인데, 이름에서 추측 가능하듯이 list를 수정 불가능한 객체로 반환해 준다. 이 넘겨받은 객체는 read-only 형태의 객체로 감싸서 주는데 add 메소드라도 부르게 되면 exception을 발생시키 때문에 원본 list 객체 대신 reference list 객체를 주고 싶을 때 사용한다.

C#은 ReadOnlyCollection이라는 type이 있고 만약 이를 변환하는 코드를 짠다면 아래와 같은 코드로 짤 수 있다. 마찬가지로 add 메소드나 assign이라도 하려고 하면 exeption이 발생한다.

``` csharp
DiscountCondition[] discountConditions;
this.DiscountConditions = Array.AsReadOnly(discountConditions);
```

#### Call constructor from other constructor

Constructor(생성자) 코드가 overloading 되어 있다면 서로 호출해서 코드의 중복을 막을 수 있다.

여기서 java와 c#의 미묘한 문법의 차이가 존재하는데

- Java는 constructor body에 this overload constructor를 사용
- C#은 `:` 키워드를 사용해서 body에 진입하기 전에 this overload constructor를 호출

코드로 살펴 보면

``` java
public Movie(String title, Duration runningTime, Money fee, double discountPercent, DiscountCondition... discountConditions) {
    this(MovieType.PERCENT_DISCOUNT, title, runningTime, fee, Money.ZERO, discountPercent, discountConditions);
}
```

``` csharp
public Movie(string title, TimeSpan runningTime, Money fee, double discountPercent, params DiscountCondition[] discountConditions)
    : this(MovieType.PERCENT_DISCOUNT, title, runningTime, fee, Money.ZERO, discountPercent, discountConditions)
{
}
```

그런데 this overload constructor 호출 타이밍을 제어할 수 있다는 점에서 봤을 때 java 쪽이 조금 더 유리한 코드를 작성할 수 있다.

C#은 body에 진입하기 전에 this overload constructor call을 하기 때문에 미리 손 쓸 기회도 없이 시작하기 때문이다.

### [DiscountCondition.java](https://github.com/eternity-oop/object/blob/master/chapter04/src/main/java/org/eternity/movie/step01/DiscountCondition.java) and [Movie.cs](https://github.com/jongfeel/objects/blob/main/Chapter04/Movie/DiscountCondition.cs)

<details>
<summary>Code</summary>
<p>

``` java
package org.eternity.movie.step01;

import java.time.DayOfWeek;
import java.time.LocalTime;

public class DiscountCondition {
    private DiscountConditionType type;

    private int sequence;

    private DayOfWeek dayOfWeek;
    private LocalTime startTime;
    private LocalTime endTime;

    public DiscountConditionType getType() {
        return type;
    }

    public void setType(DiscountConditionType type) {
        this.type = type;
    }

    public DayOfWeek getDayOfWeek() {
        return dayOfWeek;
    }

    public void setDayOfWeek(DayOfWeek dayOfWeek) {
        this.dayOfWeek = dayOfWeek;
    }

    public LocalTime getStartTime() {
        return startTime;
    }

    public void setStartTime(LocalTime startTime) {
        this.startTime = startTime;
    }

    public LocalTime getEndTime() {
        return endTime;
    }

    public void setEndTime(LocalTime endTime) {
        this.endTime = endTime;
    }

    public int getSequence() {
        return sequence;
    }

    public void setSequence(int sequence) {
        this.sequence = sequence;
    }
}
```

``` csharp
using System;

public class DiscountCondition
{
    public DiscountConditionType Type { set; get; }
    public int Sequence { set; get; }
    public DayOfWeek DayOfWeek { set; get; }
    public TimeSpan StartTime { set; get; }
    public TimeSpan EndTime { set; get; }
}
```

</p>
</details>

DiscountCondition은 C#의 Property로 그대로 바꾸기만 했다.

### [Screening.java](https://github.com/eternity-oop/object/blob/master/chapter04/src/main/java/org/eternity/movie/step01/Screening.java) and [Movie.cs](https://github.com/jongfeel/objects/blob/main/Chapter04/Movie/Screening.cs)

<details>
<summary>Code</summary>
<p>

``` java
package org.eternity.movie.step01;

import java.time.LocalDateTime;

public class Screening {
    private Movie movie;
    private int sequence;
    private LocalDateTime whenScreened;

    public Movie getMovie() {
        return movie;
    }

    public void setMovie(Movie movie) {
        this.movie = movie;
    }

    public LocalDateTime getWhenScreened() {
        return whenScreened;
    }

    public void setWhenScreened(LocalDateTime whenScreened) {
        this.whenScreened = whenScreened;
    }

    public int getSequence() {
        return sequence;
    }

    public void setSequence(int sequence) {
        this.sequence = sequence;
    }
}
```

``` csharp
using System;

public class Screening
{
    public Movie movie { set; get; }
    public int sequence { set; get; }
    public DateTime whenScreened { set; get; }
}
```

</p>
</details>

Screening 역시 Property로 변경한 것 밖에 없다. 왠지 망해가는 class 설계라는게 눈에 보이기 시작한다.

### [Reservation.java](https://github.com/eternity-oop/object/blob/master/chapter04/src/main/java/org/eternity/movie/step01/Reservation.java) and [Movie.cs](https://github.com/jongfeel/objects/blob/main/Chapter04/Movie/Reservation.cs)

<details>
<summary>Code</summary>
<p>

``` java
package org.eternity.movie.step01;

import org.eternity.money.Money;

public class Reservation {
    private Customer customer;
    private Screening screening;
    private Money fee;
    private int audienceCount;

    public Reservation(Customer customer, Screening screening, Money fee,
                       int audienceCount) {
        this.customer = customer;
        this.screening = screening;
        this.fee = fee;
        this.audienceCount = audienceCount;
    }

    public Customer getCustomer() {
        return customer;
    }

    public void setCustomer(Customer customer) {
        this.customer = customer;
    }

    public Screening getScreening() {
        return screening;
    }

    public void setScreening(Screening screening) {
        this.screening = screening;
    }

    public Money getFee() {
        return fee;
    }

    public void setFee(Money fee) {
        this.fee = fee;
    }

    public int getAudienceCount() {
        return audienceCount;
    }

    public void setAudienceCount(int audienceCount) {
        this.audienceCount = audienceCount;
    }
}
```

``` csharp
using System;

public class Reservation
{
    public Customer Customer { set; get; }
    public Screening Screening { set; get; }
    public Money Fee { set; get; }
    public int AudienceCount { set; get; }

    public Reservation(Customer customer, Screening screening, Money fee,
                       int audienceCount) {
        Customer = customer;
        Screening = screening;
        Fee = fee;
        AudienceCount = audienceCount;
    }
}
```

</p>
</details>

Reservation은 생성자가 추가된거 빼고는 역시 망해가는 class 설계로 가고 있다.

### [Customer.java](https://github.com/eternity-oop/object/blob/master/chapter04/src/main/java/org/eternity/movie/step01/Customer.java) and [Movie.cs](https://github.com/jongfeel/objects/blob/main/Chapter04/Movie/Customer.cs)

<details>
<summary>Code</summary>
<p>

``` java
package org.eternity.movie.step01;

public class Customer {
    private String name;
    private String id;

    public Customer(String name, String id) {
        this.id = id;
        this.name = name;
    }
}
```

``` csharp
public class Customer
{
    private string name;
    private string id;

    public Customer(string name, string id)
    {
        this.name = name;
        this.id = id;
    }
}
```

</p>
</details>

Customer는 그냥 보여주기식으로 만들었는데 이유는 private field 값 name, id에 constructor로 세팅하는 거 빼고는 하는게 없는 class이기 떄문이다. 그래서 string type에 대해 처음에 대문자냐 소문자냐만 다를 뿐 코드가 똑같다.