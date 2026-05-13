using Diplom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Diplom.Classes
{
    // ViewModel
    public class EmployeeViewModel
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public string FullName => $"{LastName} {FirstName} {Patronymic}";
        public int? PlannedHours { get; set; }       // теперь nullable
        public int? ActualHours { get; set; }        // nullable
        public int? WorkExperience { get; set; }     // nullable
        public DateTime? HireDate { get; set; }      // nullable
        public DateTime? MedicalExamDate { get; set; } // nullable
        public int DepartmentId { get; set; }
        public int PositionId { get; set; }
        public int EmploymentTypeId { get; set; }
        public string DepartmentName { get; set; }
        public string PositionName { get; set; }
        public string EmploymentTypeName { get; set; }

        public double? LoadPercent
        {
            get
            {
                if (PlannedHours == null) return null;
                int norm = PositionName != null && PositionName.ToLower().Contains("мастер") ? 900 : 720;
                return Math.Round((double)PlannedHours.Value / norm * 100, 1);
            }
        }

        public SolidColorBrush LoadBrush
        {
            get
            {
                if (PlannedHours == null) return new SolidColorBrush(Colors.Gray);
                double? percent = LoadPercent;
                if (percent == null) return new SolidColorBrush(Colors.Gray);
                if (percent > 130) return new SolidColorBrush(Color.FromRgb(0xe7, 0x4c, 0x3c));
                if (percent < 90) return new SolidColorBrush(Color.FromRgb(0xf3, 0x9c, 0x12));
                return new SolidColorBrush(Color.FromRgb(0x27, 0xae, 0x60));
            }
        }

        public EmployeeViewModel(Employees employee)
        {
            if (employee == null) throw new ArgumentNullException(nameof(employee));

            LastName = employee.LastName ?? "";
            FirstName = employee.FirstName ?? "";
            Patronymic = employee.Patronymic ?? "";
            PlannedHours = employee.PlannedHours;
            ActualHours = employee.ActualHours;
            WorkExperience = employee.WorkExperience;
            HireDate = employee.HireDate;
            MedicalExamDate = employee.MedicalExamDate;
            DepartmentId = employee.DepartmentID.Value;
            PositionId = employee.PositionID.Value;
            EmploymentTypeId = employee.EmploymentTypesID.Value;

            DepartmentName = employee.Departments?.Name ?? "Не указан";
            PositionName = employee.Positions?.Name ?? "Не указана";
            EmploymentTypeName = employee.EmploymentTypes?.Name ?? "Не указан";
        }
    }
}
