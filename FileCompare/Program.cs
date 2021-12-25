using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Channels;

namespace FileCompare
{
    class Employee
    {
        public string Name { get; set; }
        public string Work { get; set; }
        public bool IsChief { get; set; }
        public int Salary { get; set; }
    }
    class Program
    {
        private const int NameID = 0;
        private const int WorkID = 1;
        private const int SalaryID = 2;
        private const int ChiefID = 3;
        
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
                Employee newEmployee = new Employee();
                string[] lineSplit = line.Split(';');
                if (lineSplit.Length == 4)
                {
                    newEmployee.Name = lineSplit[NameID];
                    newEmployee.Work = lineSplit[WorkID];
                    newEmployee.Salary = int.Parse(lineSplit[SalaryID]);
                    newEmployee.IsChief = bool.Parse(lineSplit[ChiefID]);
                }
                else
                {
                    newEmployee.Name = lineSplit[NameID];
                    newEmployee.Work = lineSplit[WorkID];
                    newEmployee.Salary = int.Parse(lineSplit[SalaryID]);
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
                int countChief = 0;
                string work = employees[i].Work;
                if (!works.Contains(work))
                {
                    works.Add(work);
                    for (int j = 0; j < employees.Count; j++)
                    {
                        if (work == employees[j].Work && employees[j].IsChief)
                            countChief++;
                    }
                    if (countChief > 2 || countChief == 0)
                        throw new Exception("Клдичество начальников не верно");
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
                    int count = 0;
                    works.Add(work);
                    int sum = 0;
                    for (int j = 0; j < employees.Count; j++)
                    {
                        if (work == employees[j].Work && !employees[j].IsChief)
                        {
                            sum += employees[j].Salary;
                            count++;
                        }
                    }
                    Console.WriteLine($"{work}, средняя зарплата: {sum / count}");
                }
            }
        }

        static void MaxSalaryChief(List<Employee> employees)
        {
            int max = 0;
            string work = null;
            foreach (var employee in employees)
            {
                if (employee.IsChief)
                    if (max < employee.Salary)
                    {
                        max = employee.Salary;
                        work = employee.Work;
                    }
            }

            Console.WriteLine($"Максимальная зарплата среди руководителей: {max}, {work}");
        }
    }
}