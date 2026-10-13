using StudentAPIDataAccessLayer;

namespace StudentAPIBusinessLayer
{
    public class Student
    {
        public enum enMode
        {
            AddNew,
            Update
        }

        public enMode Mode { get; private set; } = enMode.AddNew;

        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public double Grade { get; set; }

        public StudentDTO SDTO =>
            new StudentDTO(Id, Name, Age, Grade);

        public Student(StudentDTO student, enMode mode = enMode.AddNew)
        {
            ArgumentNullException.ThrowIfNull(student);

            Id = student.Id;
            Name = student.Name;
            Age = student.Age;
            Grade = student.Grade;

            Mode = mode;
        }

        private bool _AddNewStudent()
        {
            Id = StudentData.AddStudent(SDTO);

            return Id > 0;
        }

        private bool _UpdateStudent()
        {
            return StudentData.UpdateStudent(SDTO);
        }

        public static bool DeleteStudent(int studentId)
        {
            return StudentData.DeleteStudent(studentId);
        }

        public static Student Find(int studentId)
        {
            StudentDTO studentDTO =
                StudentData.GetStudentById(studentId);

            if (studentDTO == null)
                return null;

            return new Student(studentDTO, enMode.Update);
        }

        public static List<StudentDTO> GetAllStudents()
        {
            return StudentData.GetAllStudents();
        }

        public static List<StudentDTO> GetPassedStudents()
        {
            return StudentData.GetPassedStudents();
        }

        public static double GetAverageGrade()
        {
            return StudentData.GetAverageGrade();
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (!_AddNewStudent())
                        return false;

                    Mode = enMode.Update;
                    return true;

                case enMode.Update:
                    return _UpdateStudent();

                default:
                    return false;
            }
        }
    }
}