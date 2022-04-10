# Chapter 07 객체 분해

Employees program

- Regular or Parttime
- Calculate tax rate of base pay
- Sum of employees pays

## Overview

Ruby base로 된 프로그램으로 main 함수에서 부터 출발해서 하향식 접근 프로그램의 예제를 보여주는 프로그램이다.
class로 작성된 프로그램도 아니고 main 함수에서 분해해서 함수 호출하는 방식이라 특별히 unit test는 필요가 없을 것으로 판단해서 작성하지는 않았다.

여기서는 Ruby 코드를 C#으로 옮기면서 달라진 점에 대해 적어 본다.

## Code review

### 1단계 [employees.rb](https://github.com/eternity-oop/object/blob/master/chapter07/a_functional_decomposition/employees.rb) and [Program.cs](https://github.com/jongfeel/objects/blob/main/Chapter07/Employees_basic/Program.cs)

<details>
<summary>Code</summary>
<p>

``` ruby
#encoding: UTF-8
$employees = ["직원A", "직원B", "직원C"]
$basePays = [400, 300, 250]

def main(name)
  taxRate = getTaxRate()
  pay = calculatePayFor(name, taxRate)
  puts(describeResult(name, pay))
end

def getTaxRate()
  print("세율을 입력하세요: ")
  return gets().chomp().to_f()
end

def calculatePayFor(name, taxRate)
  index = $employees.index(name)
  basePay = $basePays[index]  
  return basePay - (basePay * taxRate)
end

def describeResult(name, pay)
  return "이름 : #{name}, 급여 : #{pay}"
end

main("직원A")
```

``` c#
// See https://aka.ms/new-console-template for more information

string[] employees = new [] { "EmployeeA", "EmployeeB", "EmployeeC" };
int[] basePays = new [] { 400, 300, 250 };

string name = args.Length > 0 ? args[0] : string.Empty;
float taxRate = GetTaxRate();
float pay = CalculatePayFor(name, taxRate);
Console.WriteLine(DescribeResult(name, pay));

float GetTaxRate()
{
    Console.Write("Input tax rate: ");
    string? taxRate = Console.ReadLine();
    float.TryParse(taxRate, out float result);
    return result;
}

float CalculatePayFor(string name, float taxRate)
{
    int index = Array.IndexOf<string>(employees, name);
    int basePay = index >= 0 ? basePays[index] : 0;
    return basePay - (basePay * taxRate);
}

string DescribeResult(string name, float pay) => $"Name : {name}, Pay : {pay}";
```

</p>
</details>

#### Array에서 index 값 가져오는 방법

ruby의 코드가 워낙에 simple 함을 지향하다 보니 index를 가져오는 코드가 직관적이다.

``` ruby
index = $employees.index(name)
```

C#의 경우는 list class라면 모를까 array를 쓰고 있기 때문에 static 함수를 사용하게 되는 점이 다르다.

``` c#
int index = Array.IndexOf<string>(employees, name);
```

#### Exceptions handling

ruby 코드는 예외 처리에 대해서 고려하지 않고 동작하는 case에 대해서만 코드가 작성이 되어 있다.

크게 예외가 두 군데 존재하는데

- $employees에 없는 이름을 입력 받았을 때
- 세율을 float 값이 아닌 값으로 입력 받았을 때

이렇게 이다.

그래서 C# 코드로 작성하면서 예외 코드를 추가했다. try catch 구문을 쓰면 코드가 길어지고 번잡스럽게 보이므로 try catch를 사용하지 않고도 처리할 수 있는 방향으로 진행했다.

employees에 없는 이름일 경우 반드시 예외가 발생하므로, 없는 이름일 때 array index의 결과가 -1이 오는 것을 이용한다.

index >= 0 이면 이름이 있는 것이므로 basePays에서 값을 가져오고 그렇지 않다면 그냥 0으로 처리해 버리면 이후 tax rate에 대해 어떤 계산을 하게 되더라도 0이 된다.

``` c#
int index = Array.IndexOf<string>(employees, name);
int basePay = index >= 0 ? basePays[index] : 0;
```

세율 입력도 float type으로 변환할 수 있게 TryParse 함수를 사용했다. 별도의 try catch 코드를 사용하지 않고 float type이면 변환이 되고 아니면 그냥 기본값이 0으로 결과를 얻을 수 있다.

``` c#
string? taxRate = Console.ReadLine();
float.TryParse(taxRate, out float result);
```

### 2단계 [employees.rb](https://github.com/eternity-oop/object/blob/master/chapter07/b_add_function/employees.rb) and [Program.cs](https://github.com/jongfeel/objects/blob/main/Chapter07/Employees_add_sumOfBasePay_function/Program.cs)

<details>
<summary>Code</summary>
<p>

``` ruby
#encoding: UTF-8
$employees = ["직원A", "직원B", "직원C"]
$basePays = [400, 300, 250]

def main(operation, args={})
  case(operation)
  when :pay then calculatePay(args[:name])
  when :basePays then sumOfBasePays()
  end
end

def calculatePay(name)
  taxRate = getTaxRate()
  pay = calculatePayFor(name, taxRate)
  puts(describeResult(name, pay))
end

def getTaxRate()
  print("세율을 입력하세요: ")
  return gets().chomp().to_f()
end

def calculatePayFor(name, taxRate)
  index = $employees.index(name)
  basePay = $basePays[index]  
  return basePay - (basePay * taxRate)
end

def describeResult(name, pay)
  return "이름 : #{name}, 급여 : #{pay}"
end

def sumOfBasePays()
  result = 0
  for basePay in $basePays
    result += basePay
  end  
  puts(result)
end

main(:basePays)
main(:pay, name:"직원A")
```

``` c#
// See https://aka.ms/new-console-template for more information

string[] employees = new [] { "EmployeeA", "EmployeeB", "EmployeeC" };
int[] basePays = new [] { 400, 300, 250 };

string operation = args.Length > 0 ? args[0] : string.Empty;
string name = args.Length > 1 ? args[1] : string.Empty;

switch (operation.ToLower())
{
    case "pay":
        CalculatePay(name);
        break;
    case "basepay":
        SumOfBasePays();
        break;
}

void CalculatePay(string name)
{
    float taxRate = GetTaxRate();
    float pay = CalculatePayFor(name, taxRate);
    Console.WriteLine(DescribeResult(name, pay));
}

float GetTaxRate()
{
    Console.Write("Input tax rate: ");
    string? taxRate = Console.ReadLine();
    float.TryParse(taxRate, out float result);
    return result;
}

float CalculatePayFor(string name, float taxRate)
{
    int index = Array.IndexOf<string>(employees, name);
    int basePay = index >= 0 ? basePays[index] : 0;
    return basePay - (basePay * taxRate);
}

string DescribeResult(string name, float pay) => $"Name : {name}, Pay : {pay}";

void SumOfBasePays() => Console.WriteLine(basePays.Sum());
```

</p>
</details>

#### Array의 Sum 구하는 방법

독자의 수준을 고려해서 sum 함수를 사용하지 않았는지 모르겠지만, ruby도 sum 함수를 지원한다.

따라서 sumOfBasePays 함수는 아래와 같이 작성할 수 있다.

``` ruby
def sumOfBasePays()
  puts($basePays.sum)
end
```

마찬가지로 C# linq도 Sum 함수를 지원하므로 아래와 같이 구현이 가능하다.

``` c#
void SumOfBasePays() => Console.WriteLine(basePays.Sum());
```

### 3단계 [employees.rb](https://github.com/eternity-oop/object/blob/master/chapter07/c_data_change/employees.rb) and [Program.cs](https://github.com/jongfeel/objects/blob/main/Chapter07/Employees_add_parttime_employees/Program.cs)

<details>
<summary>Code</summary>
<p>

``` ruby
#encoding: UTF-8
$employees = ["직원A", "직원B", "직원C", "아르바이트D", "아르바이트E", "아르바이트F"]
$basePays = [400, 300, 250, 1, 1, 1.5]
$hourlys = [false, false, false, true, true, true]
$timeCards = [0, 0, 0, 120, 120, 120]

def main(operation, args={})
  case(operation)
  when :pay then calculatePay(args[:name])
  when :basePays then sumOfBasePays()
  end
end

def calculatePay(name)
  taxRate = getTaxRate()
  if (hourly?(name)) then
    pay = calculateHourlyPayFor(name, taxRate)
  else
    pay = calculatePayFor(name, taxRate)
  end
  puts(describeResult(name, pay))
end

def hourly?(name)
  return $hourlys[$employees.index(name)]
end

def calculateHourlyPayFor(name, taxRate)
  index = $employees.index(name)
  basePay = $basePays[index] * $timeCards[index]
  return basePay - (basePay * taxRate)
end

def getTaxRate()
  print("세율을 입력하세요: ")
  return gets().chomp().to_f()
end

def calculatePayFor(name, taxRate)
  index = $employees.index(name)
  basePay = $basePays[index]  
  return basePay - (basePay * taxRate)
end

def describeResult(name, pay)
  return "이름 : #{name}, 급여 : #{pay}"
end

def sumOfBasePays()
  result = 0
  for name in $employees
    if (not hourly?(name)) then 
      result += $basePays[$employees.index(name)]
    end
  end  
  puts(result)
end

main(:basePays)
main(:pay, name:"아르바이트F")
```

``` c#
// See https://aka.ms/new-console-template for more information

string[] employees = new [] { "EmployeeA", "EmployeeB", "EmployeeC", "ParttimeD", "ParttimeE", "ParttimeF" };
double[] basePays = new [] { 400, 300, 250, 1, 1, 1.5 };
bool[] hourlys = new [] { false, false, false, true, true, true };
int[] timeCards = new [] { 0, 0, 0, 120, 120, 120 };

string operation = args.Length > 0 ? args[0] : string.Empty;
string name = args.Length > 1 ? args[1] : string.Empty;

switch (operation.ToLower())
{
    case "pay":
        CalculatePay(name);
        break;
    case "basepay":
        SumOfBasePays();
        break;
}

void CalculatePay(string name)
{
    double taxRate = GetTaxRate();
    double pay = IsHourly(name) ? CalculateHourlyPayFor(name, taxRate) : CalculatePayFor(name, taxRate);
    Console.WriteLine(DescribeResult(name, pay));
}

bool IsHourly(string name)
{
    int index = Array.IndexOf<string>(employees, name);
    return index >= 0 ? hourlys[index] : false;
}

double CalculateHourlyPayFor(string name, double taxRate)
{
    int index = Array.IndexOf<string>(employees, name);
    double basePay = index >= 0 ? basePays[index] * timeCards[index] : 0;
    return basePay - (basePay * taxRate);
}

double GetTaxRate()
{
    Console.Write("Input tax rate: ");
    string? taxRate = Console.ReadLine();
    double.TryParse(taxRate, out double result);
    return result;
}

double CalculatePayFor(string name, double taxRate)
{
    int index = Array.IndexOf<string>(employees, name);
    double basePay = index >= 0 ? basePays[index] : 0;
    return basePay - (basePay * taxRate);
}

string DescribeResult(string name, double pay) => $"Name : {name}, Pay : {pay}";

void SumOfBasePays() => Console.WriteLine(employees.Select((name, index) => !IsHourly(name) ? basePays[index] : 0).Sum());
```

</p>
</details>

#### SumOfBasePays Select linq

이 부분은 C#좀 했다는 사람도 linq로 만들 수 있을까? 에 대한 의문을 품어볼 수 있다. 왜냐하면 for문에 if문 그리고 다시 index를 구한 것에서 sum을 해야 하니까 그대로 ruby 코드에서 c# 코드로 옮기는게 최선이라고 생각할 수 있다.

즉, 이정도 되는 ruby 코드의 경우

``` ruby
def sumOfBasePays()
  result = 0
  for name in $employees
    if (not hourly?(name)) then 
      result += $basePays[$employees.index(name)]
    end
  end  
  puts(result)
end
```

C#으로 바꾼다고 하면 그대로 바꿔볼 수 있다.

``` c#
void SumOfBasePays()
{
    int result = 0;
    foreach (string name in employees)
    {
        if (!IsHourly(name))
        {
            int index = Array.IndexOf<string>(employees, name);
            result += basePays[index];
        }
    }
    Console.WriteLine(result);
}
```

하지만 C#에서 기교를 좀 부려본다면 아래와 같이 한 줄로 코드 작성이 가능하다.  
Select linq 함수는 python의 array를 enumerate를 시키면 value와 index를 함꼐 얻어올 수 있듯이, Select 역시 value와 index 파라미터가 함께 오므로 IsHourly에서 걸러진 판단의 결과로 바로 basePays의 index에 해당하는 값을 select(선택) 할 수 있고 그 결과들을 Sum까지 할 수 있다.

``` c#
void SumOfBasePays() => Console.WriteLine(employees.Select((name, index) => !IsHourly(name) ? basePays[index] : 0).Sum());
```