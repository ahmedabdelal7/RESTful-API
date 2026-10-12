using Microsoft.AspNetCore.Mvc;
using StudentApi.Models;
using StudentApi.DataSimulation;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace StudentApi.Controllers
{
    [ApiController] // Marks the class as a Web API controller with enhanced features.
                    // [Route("[controller]")] // Sets the route for this controller to "students", based on the controller name.
    [Route("api/Students")]

    public class StudentsController : ControllerBase // Declare the controller class inheriting from ControllerBase.
    {

        [HttpGet("All", Name = "GetAllStudents")] // Marks this method to respond to HTTP GET requests.
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<IEnumerable<Student>> GetAllStudents() // Define a method to get all students.
        {
            //StudentDataSimulation.StudentsList.Clear();
            if (StudentDataSimulation.StudentsList.Count == 0)
            {
                return NotFound("No Students Found!");
            }

            return Ok(StudentDataSimulation.StudentsList); // Returns the list of students.
        }

        [HttpGet("Passed", Name = "GetPassedStudents")]
       // [ProducesResponseType(typeof(IEnumerable<Student>), 200)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<IEnumerable<Student>> GetPassedStudents()
        {
            //StudentDataSimulation.StudentsList.Clear();
            if (StudentDataSimulation.StudentsList.Count == 0)
            {
                return NotFound("No Students Passed!");
            }

            var passedStudents = StudentDataSimulation.StudentsList.Where(student => student.Grade >= 50).ToList();
            return Ok(passedStudents);
        }

        [HttpGet("AverageGrades",Name = "GetAverageGrade")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<double> GetAverageGrade()
        { 
            //StudentDataSimulation.StudentsList.Clear();

            if(StudentDataSimulation.StudentsList.Count == 0)
            {
                return NotFound("No students found!");
            }

            var avgGrades =  StudentDataSimulation.StudentsList.Average(student => student.Grade);  
            return Ok(avgGrades);
        }

        [HttpGet("{id}",Name = "GetStudentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<Student> GetStudentByID(int id) { 
            

            if(id < 1)
            {
                return BadRequest($"Not Accepted Id {id}");
            }

            var student = StudentDataSimulation.StudentsList.FirstOrDefault(student => student.Id == id);
            if(student == null)
            {
                return NotFound($"Student With Id {id} Not Found!");
            }

            return Ok(student);
        }

        [HttpPost(Name = "AddStudent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<Student> AddStudent(Student newStudent) 
        {
            if (newStudent == null || string.IsNullOrEmpty(newStudent.Name)
            || newStudent.Age < 0 || newStudent.Grade < 0)
            {
                return BadRequest("Invalid Student Data!");
            }

            newStudent.Id = StudentDataSimulation.StudentsList.Count > 0 ? StudentDataSimulation.StudentsList.Max(student => student.Id) + 1 : 1;

            StudentDataSimulation.StudentsList.Add(newStudent);

            return CreatedAtRoute("GetStudentByID",new { id = newStudent.Id}, newStudent);

        }

        [HttpDelete("{id}", Name = "DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteStudent(int id)
        {
            if (id < 1)
            {
                return BadRequest($"Not accepted ID {id}");
            }

            var student = StudentDataSimulation.StudentsList.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            StudentDataSimulation.StudentsList.Remove(student);
            return Ok($"Student with ID {id} has been deleted.");
        }

        [HttpPut("{id}", Name = "UpdateStudent")]//Update
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]


        public ActionResult<Student> UpdateStudent(int id, Student UpdatedStudent)
        {
            if(id < 1 || string.IsNullOrEmpty(UpdatedStudent.Name) || UpdatedStudent.Age < 1 || UpdatedStudent.Grade < 0)
            {
                return BadRequest("Invalid Student Data!");
            }

            var student = DataSimulation.StudentDataSimulation.StudentsList.FirstOrDefault(student => student.Id == id);

            if (student == null)
            {
                return NotFound("Student Not Found!");
            }

            student.Name = UpdatedStudent.Name;
            student.Age = UpdatedStudent.Age;
            student.Grade = UpdatedStudent.Grade;

            //return CreatedAtRoute("GetStudentByID", new {id = student.Id}, student);
            return Ok(student);
        }
    }
}
