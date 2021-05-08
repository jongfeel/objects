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
using System.Collections.Generic;
using System.Linq;

public class Movie {
    private string title;
    private TimeSpan runningTime;
    public Money Fee { private set; get; }
    public IEnumerable<DiscountCondition> DiscountConditions { private set; get;}

    public MovieType MovieType { private set; get; }
    public Money DiscountAmount { private set; get; }
    public double DiscountPercent { private set; get; }

    public Movie(string title, TimeSpan runningTime, Money fee, double discountPercent, params DiscountCondition[] discountConditions)
    {
        this(MovieType.PERCENT_DISCOUNT, title, runningTime, fee, Money.ZERO, discountPercent, discountConditions);
    }

    public Movie(string title, TimeSpan runningTime, Money fee, Money discountAmount, params DiscountCondition[] discountConditions)
    {
        this(MovieType.AMOUNT_DISCOUNT, title, runningTime, fee, discountAmount, 0, discountConditions);
    }

    public Movie(string title, TimeSpan runningTime, Money fee)
    {
        this(MovieType.NONE_DISCOUNT, title, runningTime, fee, Money.ZERO, 0);
    }

    private Movie(MovieType movieType, string title, TimeSpan runningTime, Money fee, Money discountAmount, double discountPercent,
                  params DiscountCondition[] discountConditions) {
        this.MovieType = movieType;
        this.title = title;
        this.runningTime = runningTime;
        this.Fee = fee;
        this.DiscountAmount = discountAmount;
        this.DiscountPercent = discountPercent;
        this.DiscountConditions = discountConditions.AsEnumerable();
    }
}
```

</p>
</details>

Movie.java를 Movie.cs로 바꾸면서 느낄 수 있는 가장 큰 변화는 역시 getter, setter를 한번에 묶어주는 C#의 property 문법이다.