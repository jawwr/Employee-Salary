using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileCompare
{
    class Employee
    {
        public string Name { get; }
        public string Work { get; }
        public bool IsChief { get; }
        public int Salary { get; }

        public Employee(string name, string work, int salary, bool isChief = false)
        {
            Name = name;
            Work = work;
            Salary = salary;
            IsChief = isChief;
        }
    }
    class Program
    {
        private const int NameId = 0;
        private const int WorkId = 1;
        private const int SalaryId = 2;
        private const int ChiefId = 3;
        
        static void Main(string[] args)
        {
            List<Employee> employees = ReadFile();
            CheckChiefCount(employees);
            AverageSalary(employees);
            MaxSalaryChief(employees);
        }
        static List<Employee> ReadFile()
        {
            string[] file = File.ReadAllLines("File1.txt");
            List<Employee> employees = new List<Employee>();
            foreach (var line in file)
            {
                Employee newEmployee;
                string[] lineSplit = line.Split(';');
                if (lineSplit.Length == 4)
                {
                    newEmployee = new Employee(lineSplit[NameId],lineSplit[WorkId],int.Parse(lineSplit[SalaryId]),bool.Parse(lineSplit[ChiefId]));
                }
                else
                {
                    newEmployee = new Employee(lineSplit[NameId], lineSplit[WorkId], int.Parse(lineSplit[SalaryId]));
                }
                employees.Add(newEmployee);
            }

            return employees;
        }
        static void CheckChiefCount(List<Employee> employees)
        {
            
            List<string> works = new List<string>();
            for (int i = 0; i < employees.Count; i++)
            {
                string work = employees[i].Work;
                if (!works.Contains(work))
                {
                    works.Add(work);
                    var countChief = employees.Where(x => x.IsChief && x.Work == work);
                    if (countChief.Count() > 2 || countChief.Count() == 0)
                        throw new Exception("Количество начальников не верно");
                }
            }
        }
        static void AverageSalary(List<Employee> employees)
        {
            List<string> works = new List<string>();
            for (int i = 0; i < employees.Count; i++)
            {
                string work = employees[i].Work;
                if (!works.Contains(work))
                {
                    works.Add(work);
                    var employeeOnWork = employees.Where(x => !x.IsChief && x.Work == work);
                    int sum = employeeOnWork.Sum(x => x.Salary);
                    int count = employeeOnWork.Count();
                    Console.WriteLine($"{work}, средняя зарплата: {sum / count}");
                }
            }
        }
        static void MaxSalaryChief(List<Employee> employees)
        {
            var max = employees.Max(x => x.Salary);
            Console.WriteLine($"Максимальная зарплата среди руководителей: {max}, {employees.Find(x => x.Salary == max).Work}");
        }
    }
}