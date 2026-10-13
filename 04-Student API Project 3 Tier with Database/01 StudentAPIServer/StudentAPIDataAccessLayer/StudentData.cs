
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
namespace StudentAPIDataAccessLayer
{
    public class StudentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public double Grade { get; set; }

        public StudentDTO(int id, string name, int age, double grade)
        {
            Id = id;
            Name = name;
            Age = age;
            Grade = grade;
        }
    }
    public class StudentData
    {
        static string _connectionString = 
            "Server=localhost;Database=StudentsDB;Integrated Security=True;Encrypt=False;TrustServerCertificate=True;Connection Timeout=30;";

        public static List<StudentDTO> GetAllStudents()
        {
            List<StudentDTO> allStudents = new List<StudentDTO>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetAllStudents", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allStudents.Add(new StudentDTO(
                            reader.GetInt32(reader.GetOrdinal("Id")),
                            reader.GetString(reader.GetOrdinal("Name")),
                            reader.GetInt32(reader.GetOrdinal("Age")),
                            reader.GetInt32(reader.GetOrdinal("Grade"))
                        ));
                    }
                }
            }

            return allStudents;
        }
        public static StudentDTO GetStudentById(int studentId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetStudentById", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StudentId", studentId);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new StudentDTO(
                        reader.GetInt32(reader.GetOrdinal("Id")),
                        reader.GetString(reader.GetOrdinal("Name")),
                        reader.GetInt32(reader.GetOrdinal("Age")),
                        reader.GetInt32(reader.GetOrdinal("Grade"))
                    );
                }
                return null;
            }
        }
        public static int AddStudent(StudentDTO student)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("SP_AddStudent", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", student.Name);
                cmd.Parameters.AddWithValue("@Age", student.Age);
                cmd.Parameters.AddWithValue("@Grade", student.Grade);

                var outPutParameter = new SqlParameter("@NewStudentId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                cmd.Parameters.Add(outPutParameter);

                conn.Open();

                cmd.ExecuteNonQuery();

                return (int)outPutParameter.Value;
            }
            
        }
        public static bool UpdateStudent(StudentDTO student)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("SP_UpdateStudent", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StudentId",student.Id);
                cmd.Parameters.AddWithValue("@Name", student.Name);
                cmd.Parameters.AddWithValue("@Age", student.Age);
                cmd.Parameters.AddWithValue("@Grade", student.Grade);

                conn.Open();

                int rowsAffected = Convert.ToInt32(cmd.ExecuteScalar());

                return rowsAffected > 0;
            }
        }
        public static bool DeleteStudent(int studentId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("SP_DeleteStudent", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StudentId", studentId);

                conn.Open();

                int rowsAffected = Convert.ToInt32(cmd.ExecuteScalar());

                return rowsAffected == 1;
            }
        }
        public static double GetAverageGrade()
        {
            double averageGrade = 0;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetAverageGrade", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                conn.Open();

                object result = cmd.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    averageGrade = Convert.ToDouble(result);
                }
                else
                    averageGrade = 0;

                return averageGrade;
            }
        }
        public static List<StudentDTO> GetPassedStudents()
        {
            List<StudentDTO> passedStudents = new List<StudentDTO>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetPassedStudents", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        passedStudents.Add(new StudentDTO(
                           reader.GetInt32(reader.GetOrdinal("Id")),
                           reader.GetString(reader.GetOrdinal("Name")),
                           reader.GetInt32(reader.GetOrdinal("Age")),
                           reader.GetInt32(reader.GetOrdinal("Grade"))
                        ));
                    }
                }                

                return passedStudents;
            }
        }

    }
}
