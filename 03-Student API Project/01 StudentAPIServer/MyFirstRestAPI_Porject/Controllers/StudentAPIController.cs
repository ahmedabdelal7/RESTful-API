using Microsoft.AspNetCore.Mvc;
using StudentApi.Models;
using StudentApi.DataSimulation;
using System.Collections.Generic;

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

       
    }
}
