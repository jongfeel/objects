# objects

![objects](https://image.aladin.co.kr/product/19368/10/cover500/k972635015_1.jpg)

오브젝트, 코드로 이해하는 객체지향 설계 책의 예제 코드를 보고 이해

## Overview

조영호님의 이전 책인 [객체지향의 사실과 오해](http://aladin.kr/p/aGexE)라는 책을 읽고 좋은 책을 쓰셨다고 생각했는데
이번에 더 완성판인 [오브젝트](http://aladin.kr/p/kLQlP)를 읽다 보니국내에도 이런 좋은 책을 볼 수 있다는 것에 감사하고 있었다.
중간 쯤 읽다가 예제 코드를 C# 버전으로 한번 바꿔보고 이해해보면 좋겠다는 생각이 들었다.
이미 소스코드가 [object](https://github.com/eternity-oop/object) 에 공개되어 있었고 이걸 다시 C#으로 코드를 만들고 github에 올려보자는 생각에 책의 저자인 조영호님에게 메일을 보냈고 흔쾌히 허락을 받았다. (2020-03-18)

## Purpose

- 당연하게도 Java와 C# 코드가 어떻게 문법적으로 다른지 비교 (우열을 가리는 비교 아님 주의)
- 리팩토링이나 재설계가 필요하다면 추가

## Environment

C#이기 때문에 자연스럽게 .NET Core로 진행하는게 맞을 텐데 .NET 5의 공개가 임박해 있고
현 시점에도 v5.0.0-rc.1이 release 되었기 때문에 특별히 .NET 5로 진행해 본다.
또 실행을 시켜서 결과를 확인해볼 수 있게 구성할 예정이다.

- [https://dotnet.microsoft.com/download/dotnet/5.0](https://dotnet.microsoft.com/download/dotnet/5.0)
- 계속해서 preview 버전이 업데이트 되었다가, 2020-09-14에 rc1이 release 되었음
- 최신 업데이트 버전이 공개되면 이 문서를 계속 수정할 예정임.

## Contents

### Chapter 01 객체, 설계

- [Java and C# code review](/Chapter01/Object_Design/)
- [Test program](/Chapter01/TestProgram/)
- [Improved design](/Chapter01/ImprovedDesign/)

### Chapter 02 객체지향 프로그래밍

- [Java and C# code review](/Chapter02/)

### Chapter 04 설계 품질과 트레이드오프

- [Java and C# code review](/Chapter04/)