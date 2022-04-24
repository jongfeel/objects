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

### 4단계 [employees.rb](https://github.com/eternity-oop/object/blob/master/chapter07/d_module/employees.rb) and [Program.cs](https://github.com/jongfeel/objects/blob/main/Chapter07/Employees_module/Program.cs)

<details>
<summary>Code</summary>
<p>

``` ruby
#encoding: UTF-8
module Employees
  $employees = ["직원A", "직원B", "직원C", "아르바이트D", "아르바이트E", "아르바이트F"]
  $basePays = [400, 300, 250, 1, 1, 1.5]
  $hourlys = [false, false, false, true, true, true]
  $timeCards = [0, 0, 0, 120, 120, 120]

  def Employees.calculatePay(name, taxRate)
    if (Employees.hourly?(name)) then
      pay = Employees.calculateHourlyPayFor(name, taxRate)
    else
      pay = Employees.calculatePayFor(name, taxRate)
    end
  end

  def Employees.hourly?(name)
    return $hourlys[$employees.index(name)]
  end

  def Employees.calculateHourlyPayFor(name, taxRate)
    index = $employees.index(name)
    basePay = $basePays[index] * $timeCards[index]
    return basePay - (basePay * taxRate)
  end

  def Employees.calculatePayFor(name, taxRate)
    return basePay - (basePay * taxRate)
  end

  def Employees.sumOfBasePays()
    result = 0
    for name in $employees
      if (not Employees.hourly?(name)) then
        result += $basePays[$employees.index(name)]
      end
    end
    return result
  end
end

def main(operation, args={})
  case(operation)
  when :pay then calculatePay(args[:name])
  when :basePays then sumOfBasePays()
  end
end

def calculatePay(name)
  taxRate = getTaxRate()
  pay = Employees.calculatePay(name, taxRate)
  puts(describeResult(name, pay))
end

def getTaxRate()
  print("세율을 입력하세요: ")
  return gets().chomp().to_f()
end

def describeResult(name, pay)
  return "이름 : #{name}, 급여 : #{pay}"
end

def sumOfBasePays()
  puts(Employees.sumOfBasePays())
end

main(:basePays)
main(:pay, name:"아르바이트F")
```

``` c#
// See https://aka.ms/new-console-template for more information

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
    double pay = Employees.CalculatePay(name, taxRate);
    Console.WriteLine(DescribeResult(name, pay));
}

double GetTaxRate()
{
    Console.Write("Input tax rate: ");
    string? taxRate = Console.ReadLine();
    double.TryParse(taxRate, out double result);
    return result;
}

string DescribeResult(string name, double pay) => $"Name : {name}, Pay : {pay}";

void SumOfBasePays() => Console.WriteLine(Employees.SumOfBasePays());

struct Employees
{
    static string[] employees = new [] { "EmployeeA", "EmployeeB", "EmployeeC", "ParttimeD", "ParttimeE", "ParttimeF" };
    static double[] basePays = new [] { 400, 300, 250, 1, 1, 1.5 };
    static bool[] hourlys = new [] { false, false, false, true, true, true };
    static int[] timeCards = new [] { 0, 0, 0, 120, 120, 120 };

    public static double CalculatePay(string name, double taxRate) => IsHourly(name) ? CalculateHourlyPayFor(name, taxRate) : CalculatePayFor(name, taxRate);

    static bool IsHourly(string name)
    {
        int index = Array.IndexOf<string>(employees, name);
        return index >= 0 ? hourlys[index] : false;
    }

    static double CalculateHourlyPayFor(string name, double taxRate)
    {
        int index = Array.IndexOf<string>(employees, name);
        double basePay = index >= 0 ? basePays[index] * timeCards[index] : 0;
        return basePay - (basePay * taxRate);
    }

    static double CalculatePayFor(string name, double taxRate)
    {
        int index = Array.IndexOf<string>(employees, name);
        double basePay = index >= 0 ? basePays[index] : 0;
        return basePay - (basePay * taxRate);
    }

    public static double SumOfBasePays() => employees.Select((name, index) => IsHourly(name) ? 0 : basePays[index]).Sum();
}
```

</p>
</details>

#### module vs struct

ruby의 module에 해당하는 C#의 module이 무엇이 있을까 고민하다가 struct를 사용하는 것으로 결정했다. static 키워드를 쓰는 것으로 모듈 느낌을 낼 수 있다고 판단했기 때문이다.

관심사의 분리를 위해 struct 구조를 썼을 뿐 로직의 흐름이나 문법은 달라진 것이 없다

### 5단계 [employees.rb](https://github.com/eternity-oop/object/blob/master/chapter07/e_abstract_data%2Btype/employees.rb) and [Program.cs](https://github.com/jongfeel/objects/blob/main/Chapter07/Employees_abstract_data_and_type/Program.cs)

<details>
<summary>Code</summary>
<p>

``` ruby
#encoding: UTF-8
Employee = Struct.new(:name, :basePay, :hourly, :timeCard) do
  def calculatePay(taxRate)
    if (hourly) then
      return calculateHourlyPay(taxRate)
    end
    return calculateSalariedPay(taxRate)
  end

  def monthlyBasePay()
    if (hourly) then return 0 end
    return basePay
  end
  
private  
  def calculateHourlyPay(taxRate)
    return (basePay * timeCard) - (basePay * timeCard) * taxRate
  end
  
  def calculateSalariedPay(taxRate)
    return basePay - (basePay * taxRate)
  end
end

$employees = [
  Employee.new("직원A", 400, false, 0),
  Employee.new("직원B", 300, false, 0),
  Employee.new("직원C", 250, false, 0),
  Employee.new("아르바이트D", 1, true, 120),
  Employee.new("아르바이트E", 1, true, 120),
  Employee.new("아르바이트F", 1, true, 120),
]

def main(operation, args={})
  case(operation)
  when :pay then calculatePay(args[:name])
  when :basePays then sumOfBasePays()
  end
end

def calculatePay(name)
  taxRate = getTaxRate()
  for each in $employees
    if (each.name == name) then employee = each; break end
  end
  pay = employee.calculatePay(taxRate)
  puts(describeResult(name, pay))
end

def getTaxRate()
  print("세율을 입력하세요: ")
  return gets().chomp().to_f()
end

def describeResult(name, pay)
  return "이름 : #{name}, 급여 : #{pay}"
end

def sumOfBasePays()
  result = 0
  for each in $employees
    result += each.monthlyBasePay()
  end
  puts(result)
end
```

``` c#
// See https://aka.ms/new-console-template for more information

Employee[] employees = new Employee[6]
{
  new Employee("EmployeeA", 400, false, 0),
  new Employee("EmployeeB", 300, false, 0),
  new Employee("EmployeeC", 250, false, 0),
  new Employee("ParttimeD", 1, true, 120),
  new Employee("ParttimeE", 1, true, 120),
  new Employee("ParttimeF", 1, true, 120)
};

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
    Employee? matchEmployee = employees.FirstOrDefault(employee => employee.Name.Equals(name));
    double? pay = matchEmployee?.CalculatePay(taxRate);
    Console.WriteLine(DescribeResult(name, pay));
}

double GetTaxRate()
{
    Console.Write("Input tax rate: ");
    string? taxRate = Console.ReadLine();
    double.TryParse(taxRate, out double result);
    return result;
}

string DescribeResult(string name, double? pay) => $"Name : {name}, Pay : {pay}";

void SumOfBasePays() => Console.WriteLine(employees.Sum(employee => employee.MonthlyBasePay));

class Employee
{
    public string Name { private set; get; }
    private double basePay;
    private bool isHourly;
    private int timeCard;

    public Employee(string name, double basePay, bool isHourly, int timeCard)
    {
        this.Name = name;
        this.basePay = basePay;
        this.isHourly = isHourly;
        this.timeCard = timeCard;
    }

    public double CalculatePay(double taxRate) => isHourly ? CalculateHourlyPay(taxRate) : CalculateSalariedPay(taxRate);

    public double MonthlyBasePay => isHourly ? 0 : basePay;

    private double CalculateHourlyPay(double taxRate) => (basePay * timeCard) - (basePay * timeCard) * taxRate;

    private double CalculateSalariedPay(double taxRate) => basePay - (basePay * taxRate);
}
```

</p>
</details>

#### struct vs class

ruby의 struct는 C#의 class의 개념과 거의 같으므로 class를 사용해서 코딩했다.

new로 객체 생성하는 것도 유사하고, 내부에서 private, public으로 캡슐화를 하는 방법도 같다.

CalculatePay() 함수에서 C#으로 옮길 때 FirstOrDefault를 사용해서 name에 해당하는 employee가 없을 때의 예외 처리를 추가했다. Ruby 코드에는 그런 처기가 없으므로 name이 일치하지 않으면 예외가 발생한다.

``` c#
void CalculatePay(string name)
{
    double taxRate = GetTaxRate();
    Employee? matchEmployee = employees.FirstOrDefault(employee => employee.Name.Equals(name));
    double? pay = matchEmployee?.CalculatePay(taxRate);
    Console.WriteLine(DescribeResult(name, pay));
}
```