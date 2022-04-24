// See https://aka.ms/new-console-template for more information

Employee[] employees = new Employee[6]
{
  new SalariedEmployee("EmployeeA", 400),
  new SalariedEmployee("EmployeeB", 300),
  new SalariedEmployee("EmployeeC", 250),
  new HourlyEmployee("ParttimeD", 1, 120),
  new HourlyEmployee("ParttimeE", 1, 120),
  new HourlyEmployee("ParttimeF", 1, 120)
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

abstract class Employee
{
    public string Name { private set; get; }
    protected double basePay;

    public Employee(string name, double basePay)
    {
        this.Name = name;
        this.basePay = basePay;
    }

    public abstract double CalculatePay(double taxRate);
    public abstract double MonthlyBasePay { get; }
}

class SalariedEmployee : Employee
{
    public SalariedEmployee(string name, double basePay) : base(name, basePay) { }

    public override double MonthlyBasePay => basePay;

    public override double CalculatePay(double taxRate) => basePay - (basePay * taxRate);
}

class HourlyEmployee : Employee
{
    private double timeCard;
    public HourlyEmployee(string name, double basePay, double timeCard) : base(name, basePay) => this.timeCard = timeCard;

    public override double MonthlyBasePay => 0;

    public override double CalculatePay(double taxRate) => (basePay * timeCard) - (basePay * timeCard) * taxRate;
}