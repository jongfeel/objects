# Chapter 06 메시지와 인터페이스

Event and recurring schedule program

## Overview

class가 Event와 RecurringSchedule 두 개인 아주 간단한 프로그램 이다.

역시 C#으로 옮기면서 달라진 점과 리팩토링한 내용에 대해 적어 본다.

이번 챕터 부터 조금 다르게 한 점은 실행 프로그램과 unit test 코드가 중복으로 작성되는 부분이 있다 보니, 실행 프로그램은 작성하지 않고 실행에 ㅐ한 결과는 모두 unit test 코드로 대체한다.

## Code review

### [Event.java](https://github.com/eternity-oop/object/blob/master/chapter06/src/main/java/org/eternity/event/step02/Event.java) and [Event.cs](https://github.com/jongfeel/objects/blob/main/Chapter06/Event/Event.cs)

<details>
<summary>Code</summary>
<p>

``` java
package org.eternity.event.step02;

import java.time.Duration;
import java.time.LocalDateTime;

public class Event {
    private String subject;
    private LocalDateTime from;
    private Duration duration;

    public Event(String subject, LocalDateTime from, Duration duration) {
        this.subject = subject;
        this.from = from;
        this.duration = duration;
    }

    public boolean isSatisfied(RecurringSchedule schedule) {
        if (from.getDayOfWeek() != schedule.getDayOfWeek() ||
                !from.toLocalTime().equals(schedule.getFrom()) ||
                !duration.equals(schedule.getDuration())) {
            return false;
        }

        return true;
    }

    public void reschedule(RecurringSchedule schedule) {
        from = LocalDateTime.of(from.toLocalDate().plusDays(daysDistance(schedule)),
                schedule.getFrom());
        duration = schedule.getDuration();
    }

    private long daysDistance(RecurringSchedule schedule) {
        return schedule.getDayOfWeek().getValue() - from.getDayOfWeek().getValue();
    }
}
```

``` csharp
public class Event
{
    private string subject;
    private DateTime from;
    private TimeSpan duration;

    public Event(string subject, DateTime from, TimeSpan duration)
    {
        this.subject = subject;
        this.from = from;
        this.duration = duration;
    }

    public bool IsSatisfied(RecurringSchedule schedule)
     => from.DayOfWeek != schedule.DayOfWeek ||
        from.Hour != schedule.From.Hour || from.Minute != schedule.From.Minute ||
        duration.TotalMinutes != schedule.Duration.TotalMinutes
        ? false : true;

    public void Reschedule(RecurringSchedule schedule)
    {
        from = from.AddDays(DaysDistance(schedule));
        duration = schedule.Duration;
    }

    private int DaysDistance(RecurringSchedule schedule) => schedule.DayOfWeek - from.DayOfWeek;
}
```

</p>
</details>

#### IsSatisfied에서 비교하는 방법

OR 조건이 3개가 통과되어야 하는 조건에서 C#은 4개로 작성했는데  
그 이유는 Java의 경우 LocalDateTime 과 LocalTime class를 사용해 event 날짜와 시간 그리고 반복되는 일정의 시간을 구분해서 사용하는데 비해  
C#의 경우는 DateTime class 하나를 쓰다 보니 같은 객체이지만 다른 데이터를 넣어서 사용하게 됐다.

그래서 java는

``` java
!from.toLocalTime().equals(schedule.getFrom())
```

이런 식으로 같은지를 비교할 수 있지만

``` c#
from.Hour != schedule.From.Hour || from.Minute != schedule.From.Minute
```

C#은 시간과 분을 따로 체크해야 해서 이렇게 코드가 나오게 되었다.

#### Reschedule 리팩토링

IsSatisfied에서의 복잡함은 Reschedule에서 덜어주게 되는데  
java의 경우는 날짜의 차이를 더하고 거기에 시간까지 추가해서 변경된 LocalDateTime을 만들어줘야 하는데 비해

C#은 처음부터 DateTime을 사용했으므로 같은 시간이라는 조건을 따로 더 계산해 줄 필요가 없다. 그래서 요일 차이에 따른 날짜를 더하는 것으로 비교적 쉽게 변경이 가능하다.

### [RecurringSchedule.java](https://github.com/eternity-oop/object/blob/master/chapter06/src/main/java/org/eternity/event/step02/RecurringSchedule.java) and [RecurringSchedule.cs](https://github.com/jongfeel/objects/blob/main/Chapter06/Event/RecurringSchedule.cs)

<details>
<summary>Code</summary>
<p>

``` java
package org.eternity.event.step02;

import java.time.DayOfWeek;
import java.time.Duration;
import java.time.LocalTime;

public class RecurringSchedule {
    private String subject;
    private DayOfWeek dayOfWeek;
    private LocalTime from;
    private Duration duration;

    public RecurringSchedule(String subject, DayOfWeek dayOfWeek,
                             LocalTime from, Duration duration) {
        this.subject = subject;
        this.dayOfWeek = dayOfWeek;
        this.from = from;
        this.duration = duration;
    }

    public DayOfWeek getDayOfWeek() {
        return dayOfWeek;
    }

    public LocalTime getFrom() {
        return from;
    }

    public Duration getDuration() {
        return duration;
    }
}
```

``` csharp
public class RecurringSchedule
{
    private string subject;
    public DayOfWeek DayOfWeek { private set; get; }
    public DateTime From { private set; get; }
    public TimeSpan Duration { private set; get; }

    public RecurringSchedule(string subject, DayOfWeek dayOfWeek, DateTime from, TimeSpan duration)
    {
        this.subject = subject;
        this.DayOfWeek = dayOfWeek;
        this.From = from;
        this.Duration = duration;
    }
}
```

</p>
</details>

#### Property

C#은 getter, setter에 대한 문법으로 property를 제공하므로 불필요한 getter, setter 메소드를 작성할 필요가 없다.