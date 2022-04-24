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