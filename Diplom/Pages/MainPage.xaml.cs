using Diplom.Classes;
using Diplom.Models;
using Diplom.Windows;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

namespace Diplom.Pages
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            LoadDashboard();
        }

        private void LoadDashboard()
        {
            try
            {
                // Загружаем всех сотрудников с их отделами и должностями
                var employees = DB.context.Employees
                    .Include(e => e.Departments)
                    .Include(e => e.Positions)
                    .Include(e => e.EmploymentTypes)
                    .ToList();

                // Статистика по нагрузке (только для тех, у кого есть PlannedHours)
                var withLoad = employees.Where(e => e.PlannedHours.HasValue).ToList();
                int total = employees.Count;
                int overloaded = withLoad.Count(e => e.PlannedHours > GetNorm(e.Positions?.Name) * 1.3);
                int underloaded = withLoad.Count(e => e.PlannedHours < GetNorm(e.Positions?.Name) * 0.9);
                int normal = withLoad.Count - overloaded - underloaded;

                TotalEmployeesText.Text = total.ToString();
                OverloadedText.Text = overloaded.ToString();
                UnderloadedText.Text = underloaded.ToString();
                NormalText.Text = normal.ToString();

                // Загрузка данных для графика по отделам
                var departments = DB.context.Departments.ToList();
                var depStats = new List<DepartmentLoadViewModel>();
                foreach (var dep in departments)
                {
                    var empInDep = employees.Where(e => e.DepartmentID == dep.ID && e.PlannedHours.HasValue).ToList();
                    if (empInDep.Count == 0) continue;
                    double avgPercent = empInDep.Average(e =>
                    {
                        int norm = GetNorm(e.Positions?.Name);
                        return (double)e.PlannedHours.Value / norm * 100;
                    });
                    depStats.Add(new DepartmentLoadViewModel
                    {
                        DepartmentName = dep.Name,
                        LoadPercent = Math.Round(avgPercent, 0),
                        Count = empInDep.Count
                    });
                }
                DepartmentLoadList.ItemsSource = depStats;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Норма часов по названию должности (упрощённо)
        private int GetNorm(string positionName)
        {
            if (positionName != null && positionName.ToLower().Contains("мастер"))
                return 900;
            return 720;
        }

        // Навигация
        private void EmployeesButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new EmployeesPage());
        }

        private void ReportsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ReportPage());
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            ImportWindow importWindow = new ImportWindow();
            importWindow.ShowDialog();
            LoadDashboard(); // обновить статистику после закрытия окна импорта
        }
    }

    // ViewModel для отображения загрузки отдела
    public class DepartmentLoadViewModel
    {
        public string DepartmentName { get; set; }
        public double LoadPercent { get; set; }
        public int Count { get; set; }
        public string CountText => $"({Count} чел.)";
        public SolidColorBrush BarColor
        {
            get
            {
                if (LoadPercent > 130) return new SolidColorBrush(Color.FromRgb(0xe7, 0x4c, 0x3c));   // Красный
                if (LoadPercent < 90) return new SolidColorBrush(Color.FromRgb(0xf3, 0x9c, 0x12));   // Жёлтый
                return new SolidColorBrush(Color.FromRgb(0x27, 0xae, 0x60));                         // Зелёный
            }
        }
    }
}