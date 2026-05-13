using Diplom.Classes;
using Diplom.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Diplom.Pages
{
    public partial class EmployeesPage : Page
    {
        private List<EmployeeViewModel> _allEmployees;
        private List<EmployeeViewModel> _filteredEmployees;
        private bool _isLoaded = false;

        public EmployeesPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // Проверка контекста БД
                if (DB.context == null)
                {
                    MessageBox.Show("Контекст базы данных не инициализирован. Проверьте строку подключения.", "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var employeesFromDb = DB.context.Employees
                    .Include(e => e.Departments)
                    .Include(e => e.Positions)
                    .Include(e => e.EmploymentTypes)
                    .ToList();

                if (employeesFromDb == null)
                {
                    MessageBox.Show("Не удалось загрузить список сотрудников из базы данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    _allEmployees = new List<EmployeeViewModel>();
                }
                else
                {
                    _allEmployees = employeesFromDb.Select(e => new EmployeeViewModel(e)).ToList();
                }

                LoadFilters();
                _isLoaded = true;
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}\n\nСтек вызовов: {ex.StackTrace}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadFilters()
        {
            // Сортировка
            SortComboBox.Items.Clear();
            SortComboBox.Items.Add(new ComboBoxItem { Content = "Без сортировки", IsSelected = true });
            SortComboBox.Items.Add(new ComboBoxItem { Content = "По фамилии (А-Я)" });
            SortComboBox.Items.Add(new ComboBoxItem { Content = "По фамилии (Я-А)" });
            SortComboBox.Items.Add(new ComboBoxItem { Content = "По стажу (возр.)" });
            SortComboBox.Items.Add(new ComboBoxItem { Content = "По стажу (убыв.)" });
            SortComboBox.Items.Add(new ComboBoxItem { Content = "По нагрузке (возр.)" });
            SortComboBox.Items.Add(new ComboBoxItem { Content = "По нагрузке (убыв.)" });

            // Фильтр по отделу
            FilterDepartmentComboBox.Items.Clear();
            FilterDepartmentComboBox.Items.Add(new ComboBoxItem { Content = "Все отделы", IsSelected = true });
            foreach (var dep in DB.context.Departments.ToList())
                FilterDepartmentComboBox.Items.Add(new ComboBoxItem { Content = dep.Name, Tag = dep.ID });

            // Фильтр по должности
            FilterPositionComboBox.Items.Clear();
            FilterPositionComboBox.Items.Add(new ComboBoxItem { Content = "Все должности", IsSelected = true });
            foreach (var pos in DB.context.Positions.ToList())
                FilterPositionComboBox.Items.Add(new ComboBoxItem { Content = pos.Name, Tag = pos.ID });

            // Фильтр по типу занятости
            FilterEmploymentTypeComboBox.Items.Clear();
            FilterEmploymentTypeComboBox.Items.Add(new ComboBoxItem { Content = "Все типы занятости", IsSelected = true });
            foreach (var et in DB.context.EmploymentTypes.ToList())
                FilterEmploymentTypeComboBox.Items.Add(new ComboBoxItem { Content = et.Name, Tag = et.ID });
        }

        private void ApplyFilters()
        {
            if (!_isLoaded || _allEmployees == null) return;

            _filteredEmployees = new List<EmployeeViewModel>(_allEmployees);

            // Текстовый поиск
            string search = Search?.Text?.ToLower() ?? "";
            if (!string.IsNullOrWhiteSpace(search))
            {
                _filteredEmployees = _filteredEmployees
                    .Where(e =>
                        e.FullName.ToLower().Contains(search) ||
                        (e.DepartmentName != null && e.DepartmentName.ToLower().Contains(search)) ||
                        (e.PositionName != null && e.PositionName.ToLower().Contains(search)) ||
                        (e.EmploymentTypeName != null && e.EmploymentTypeName.ToLower().Contains(search)) ||
                        e.WorkExperience.ToString().Contains(search) ||
                        e.HireDate.Value.ToString("dd.MM.yyyy").Contains(search) ||
                        (e.MedicalExamDate != DateTime.MinValue && e.MedicalExamDate.Value.ToString("dd.MM.yyyy").Contains(search))
                    ).ToList();
            }

            // Фильтр по отделу
            if (FilterDepartmentComboBox?.SelectedItem is ComboBoxItem depItem && depItem.Tag != null)
            {
                int depId = (int)depItem.Tag;
                _filteredEmployees = _filteredEmployees.Where(e => e.DepartmentId == depId).ToList();
            }

            // Фильтр по должности
            if (FilterPositionComboBox?.SelectedItem is ComboBoxItem posItem && posItem.Tag != null)
            {
                int posId = (int)posItem.Tag;
                _filteredEmployees = _filteredEmployees.Where(e => e.PositionId == posId).ToList();
            }

            // Фильтр по типу занятости
            if (FilterEmploymentTypeComboBox?.SelectedItem is ComboBoxItem etItem && etItem.Tag != null)
            {
                int etId = (int)etItem.Tag;
                _filteredEmployees = _filteredEmployees.Where(e => e.EmploymentTypeId == etId).ToList();
            }

            ApplySorting();
            PersonellItemsControl.ItemsSource = _filteredEmployees;
        }

        private void EmployeeCard_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                var border = sender as Border;
                if (border?.DataContext is EmployeeViewModel selectedEmployee)
                {
                    NavigationService?.Navigate(new EmployeeInfoPage(selectedEmployee));
                }
            }
        }
        private void ApplySorting()
        {
            if (_filteredEmployees == null || SortComboBox?.SelectedItem == null) return;

            if (SortComboBox.SelectedItem is ComboBoxItem sortItem)
            {
                switch (sortItem.Content?.ToString())
                {
                    case "По фамилии (А-Я)":
                        _filteredEmployees = _filteredEmployees.OrderBy(e => e.LastName).ToList(); break;
                    case "По фамилии (Я-А)":
                        _filteredEmployees = _filteredEmployees.OrderByDescending(e => e.LastName).ToList(); break;
                    case "По стажу (возр.)":
                        _filteredEmployees = _filteredEmployees.OrderBy(e => e.WorkExperience).ToList(); break;
                    case "По стажу (убыв.)":
                        _filteredEmployees = _filteredEmployees.OrderByDescending(e => e.WorkExperience).ToList(); break;
                    case "По нагрузке (возр.)":
                        _filteredEmployees = _filteredEmployees.OrderBy(e => e.PlannedHours ?? 0).ToList(); break;
                    case "По нагрузке (убыв.)":
                        _filteredEmployees = _filteredEmployees.OrderByDescending(e => e.PlannedHours ?? 0).ToList(); break;
                }
            }
        }

        // Обработчики событий
        private void Search_TextChanged(object sender, TextChangedEventArgs e) => ApplyFilters();
        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => ApplyFilters();
        private void FilterDepartmentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => ApplyFilters();
        private void FilterPositionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => ApplyFilters();
        private void FilterEmploymentTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => ApplyFilters();
    }
}
