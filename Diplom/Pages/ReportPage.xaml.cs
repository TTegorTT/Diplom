using ClosedXML.Excel;
using Diplom.Classes;
using Diplom.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Diplom.Pages
{
    public partial class ReportPage : Page
    {
        private List<EmployeeReportViewModel> _allEmployees;
        private List<EmployeeReportViewModel> _currentReport;

        public ReportPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var employees = DB.context.Employees
                    .Include(e => e.Departments)
                    .Include(e => e.Positions)
                    .Include(e => e.EmploymentTypes)
                    .ToList();

                _allEmployees = employees.Select(e => new EmployeeReportViewModel(e)).ToList();

                // Заполняем фильтр отделов
                DepartmentFilterCombo.Items.Clear();
                DepartmentFilterCombo.Items.Add(new ComboBoxItem { Content = "Все отделы", IsSelected = true });
                foreach (var dep in DB.context.Departments.ToList())
                    DepartmentFilterCombo.Items.Add(new ComboBoxItem { Content = dep.Name, Tag = dep.ID });

                // Первоначальная загрузка отчёта
                ApplyReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplyReport()
        {
            if (_allEmployees == null) return;

            // Фильтр по отделу
            var filtered = new List<EmployeeReportViewModel>(_allEmployees);
            if (DepartmentFilterCombo.SelectedItem is ComboBoxItem depItem && depItem.Tag != null)
            {
                int depId = (int)depItem.Tag;
                filtered = filtered.Where(e => e.DepartmentId == depId).ToList();
            }

            // Тип отчёта
            string reportType = (ReportTypeCombo.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "";

            if (reportType.Contains("Медосмотр"))
            {
                var today = DateTime.Today;
                var deadline = today.AddDays(30);
                filtered = filtered.Where(e => e.MedicalExamDate.HasValue &&
                                               e.MedicalExamDate.Value >= today &&
                                               e.MedicalExamDate.Value <= deadline)
                                   .OrderBy(e => e.MedicalExamDate).ToList();
            }
            else if (reportType.Contains("Аттестация"))
            {
                var today = DateTime.Today;
                var deadline = today.AddDays(60);
                filtered = filtered.Where(e => e.CertificationDate.HasValue &&
                                               e.CertificationDate.Value >= today &&
                                               e.CertificationDate.Value <= deadline)
                                   .OrderBy(e => e.CertificationDate).ToList();
            }
            else // Список сотрудников по отделу
            {
                filtered = filtered.OrderBy(e => e.DepartmentName).ThenBy(e => e.FullName).ToList();
            }

            _currentReport = filtered;
            ReportGrid.ItemsSource = _currentReport;
        }

        private void ReportTypeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e) => ApplyReport();
        private void DepartmentFilterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e) => ApplyReport();

        private void ExportBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_currentReport == null || _currentReport.Count == 0)
            {
                MessageBox.Show("Нет данных для экспорта.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var saveDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx",
                FileName = "Отчёт_персонал.xlsx"
            };

            if (saveDialog.ShowDialog() == true)
            {
                try
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var ws = workbook.Worksheets.Add("Отчёт");
                        ws.Cell(1, 1).Value = "ФИО";
                        ws.Cell(1, 2).Value = "Отдел";
                        ws.Cell(1, 3).Value = "Должность";
                        ws.Cell(1, 4).Value = "Стаж";
                        ws.Cell(1, 5).Value = "Дата приёма";
                        ws.Cell(1, 6).Value = "Медосмотр до";
                        ws.Cell(1, 7).Value = "Аттестация до";
                        ws.Cell(1, 8).Value = "Нагрузка (ч)";

                        int row = 2;
                        foreach (var emp in _currentReport)
                        {
                            ws.Cell(row, 1).Value = emp.FullName;
                            ws.Cell(row, 2).Value = emp.DepartmentName;
                            ws.Cell(row, 3).Value = emp.PositionName;
                            ws.Cell(row, 4).Value = emp.WorkExperience?.ToString() ?? "";
                            ws.Cell(row, 5).Value = emp.HireDate?.ToShortDateString() ?? "";
                            ws.Cell(row, 6).Value = emp.MedicalExamDate?.ToShortDateString() ?? "";
                            ws.Cell(row, 7).Value = emp.CertificationDate?.ToShortDateString() ?? "";
                            ws.Cell(row, 8).Value = emp.PlannedHours?.ToString() ?? "";
                            row++;
                        }
                        ws.Columns().AdjustToContents();
                        workbook.SaveAs(saveDialog.FileName);
                    }
                    MessageBox.Show("Экспорт завершён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка экспорта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

    // ViewModel для отчётов
    public class EmployeeReportViewModel
    {
        public string FullName { get; set; }
        public string DepartmentName { get; set; }
        public string PositionName { get; set; }
        public string EmploymentTypeName { get; set; }
        public int? WorkExperience { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? MedicalExamDate { get; set; }
        public DateTime? CertificationDate { get; set; }
        public int? PlannedHours { get; set; }
        public int? ActualHours { get; set; }
        public int DepartmentId { get; set; }
        public int PositionId { get; set; }
        public int EmploymentTypeId { get; set; }

        public EmployeeReportViewModel(Employees employee)
        {
            if (employee == null) throw new ArgumentNullException(nameof(employee));

            FullName = $"{employee.LastName} {employee.FirstName} {employee.Patronymic}";
            DepartmentName = employee.Departments?.Name ?? "Не указан";
            PositionName = employee.Positions?.Name ?? "Не указана";
            EmploymentTypeName = employee.EmploymentTypes?.Name ?? "Не указан";
            WorkExperience = employee.WorkExperience;
            HireDate = employee.HireDate;
            MedicalExamDate = employee.MedicalExamDate;

            // Аттестация – каждые 5 лет от найма (если есть дата приёма)
            if (employee.HireDate.HasValue)
                CertificationDate = employee.HireDate.Value.AddYears(5 * ((DateTime.Now.Year - employee.HireDate.Value.Year) / 5 + 1));
            else
                CertificationDate = null;

            PlannedHours = employee.PlannedHours;
            ActualHours = employee.ActualHours;
            DepartmentId = employee.DepartmentID.Value;
            PositionId = employee.PositionID.Value;
            EmploymentTypeId = employee.EmploymentTypesID.Value;
        }
    }
}