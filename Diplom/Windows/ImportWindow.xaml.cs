using ClosedXML.Excel;
using Diplom.Classes;
using Diplom.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Diplom.Windows
{
    public partial class ImportWindow : Window
    {
        private string _filePath;
        private int _addedCount = 0;
        private int _skippedCount = 0;
        private StringBuilder _log = new StringBuilder();

        public ImportWindow()
        {
            InitializeComponent();
        }

        private void ChooseFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx",
                Title = "Выберите файл с данными сотрудников"
            };
            if (dlg.ShowDialog() == true)
            {
                _filePath = dlg.FileName;
                FilePathText.Text = _filePath;
                ImportButton.IsEnabled = true;
                _log.Clear();
                LogTextBox.Text = "";
                _addedCount = 0;
                _skippedCount = 0;
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_filePath))
            {
                MessageBox.Show("Сначала выберите файл.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            _log.Clear();
            LogTextBox.Text = "Начинаю импорт...";
            _addedCount = 0;
            _skippedCount = 0;

            try
            {
                using (var workbook = new XLWorkbook(_filePath))
                {
                    var ws = workbook.Worksheet(1);
                    int rowCount = ws.LastRowUsed()?.RowNumber() ?? 0;
                    if (rowCount < 2)
                    {
                        Log("Файл пуст или содержит только заголовки.");
                        return;
                    }

                    // Предполагаем, что первая строка — заголовки, данные со второй
                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            // Чтение ячеек (индексы с 1)
                            string lastName = ws.Cell(row, 1).GetString().Trim();
                            string firstName = ws.Cell(row, 2).GetString().Trim();
                            string patronymic = ws.Cell(row, 3).GetString().Trim();
                            string departmentName = ws.Cell(row, 4).GetString().Trim();
                            string positionName = ws.Cell(row, 5).GetString().Trim();
                            string employmentTypeName = ws.Cell(row, 6).GetString().Trim();

                            // Проверка обязательных полей
                            if (string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(firstName))
                            {
                                Log($"Строка {row}: пропущена — не указаны фамилия или имя.");
                                _skippedCount++;
                                continue;
                            }

                            // Поиск отдела
                            var department = DB.context.Departments.FirstOrDefault(d => d.Name.ToLower() == departmentName.ToLower());
                            if (department == null)
                            {
                                Log($"Строка {row} ({lastName} {firstName}): отдел '{departmentName}' не найден.");
                                _skippedCount++;
                                continue;
                            }

                            // Поиск должности
                            var position = DB.context.Positions.FirstOrDefault(p => p.Name.ToLower() == positionName.ToLower());
                            if (position == null)
                            {
                                Log($"Строка {row} ({lastName} {firstName}): должность '{positionName}' не найдена.");
                                _skippedCount++;
                                continue;
                            }

                            // Поиск типа занятости
                            var employmentType = DB.context.EmploymentTypes.FirstOrDefault(et => et.Name.ToLower() == employmentTypeName.ToLower());
                            if (employmentType == null)
                            {
                                Log($"Строка {row} ({lastName} {firstName}): тип занятости '{employmentTypeName}' не найден.");
                                _skippedCount++;
                                continue;
                            }

                            // Создание сотрудника
                            var employee = new Employees
                            {
                                ID = Guid.NewGuid(),
                                LastName = lastName,
                                FirstName = firstName,
                                Patronymic = patronymic,
                                DepartmentID = department.ID,
                                PositionID = position.ID,
                                EmploymentTypesID = employmentType.ID,
                                HireDate = DateTime.Now, // или можно оставить NULL, но поле NOT NULL? У нас DATE NULL допускается
                                // MedicalExamDate, WorkExperience, PlannedHours, ActualHours — оставляем NULL
                            };

                            DB.context.Employees.Add(employee);
                            _addedCount++;
                        }
                        catch (Exception ex)
                        {
                            Log($"Строка {row}: ошибка обработки — {ex.Message}");
                            _skippedCount++;
                        }
                    }

                    DB.context.SaveChanges();
                    Log($"\nИмпорт завершён: добавлено {_addedCount}, пропущено {_skippedCount}.");
                }
            }
            catch (Exception ex)
            {
                Log($"Критическая ошибка: {ex.Message}");
                MessageBox.Show($"Ошибка при импорте: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                LogTextBox.Text = _log.ToString();
            }
        }

        private void Log(string message)
        {
            _log.AppendLine(message);
            LogTextBox.Text = _log.ToString();
            LogTextBox.ScrollToEnd();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}