using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using StudentAPIDataAccessLayer;
using StudentAPIBusinessLayer;

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

        public ActionResult<IEnumerable<StudentDTO>> GetAllStudents() // Define a method to get all students.
        {
            //StudentDataSimulation.StudentsList.Clear();
            //if (StudentDataSimulation.StudentsList.Count == 0)
            //{
            //    return NotFound("No Students Found!");
            //}

            //return Ok(StudentDataSimulation.StudentsList); // Returns the list of students.

            List<StudentDTO> students = Student.GetAllStudents();
            if(students.Count == 0)
            {
                return NotFound("No Students Found!");
            }

            return Ok(students);

        }

        [HttpGet("Passed", Name = "GetPassedStudents")]
       // [ProducesResponseType(typeof(IEnumerable<Student>), 200)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<IEnumerable<StudentDTO>> GetPassedStudents()
        {

            List<StudentDTO> students = Student.GetPassedStudents();
            if (students.Count == 0)
            {
                return NotFound("No Students Passed!");
            }

            return Ok(students);
        }

        [HttpGet("AverageGrade",Name = "GetAverageGrade")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<double> GetAverageGrade()
        { 

            double averageGrade = Student.GetAverageGrade();
            return Ok(averageGrade);

        }

        [HttpGet("{id}",Name = "GetStudentByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<StudentDTO> GetStudentByID(int id)
        { 
            

            if(id < 1)
            {
                return BadRequest($"Not Accepted Id {id}");
            }

            Student student = Student.Find(id);

            if (student == null)
                return NotFound($"Student With Id {id} Not Found!");

            return Ok(student.SDTO);

        }

        [HttpPost(Name = "AddStudent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        
        public ActionResult<StudentDTO> AddStudent(StudentDTO newStudent) 
        {
            if (newStudent == null || string.IsNullOrEmpty(newStudent.Name)
            || newStudent.Age < 0 || newStudent.Grade < 0)
            {
                return BadRequest("Invalid Student Data!");
            }

            Student student = new Student(newStudent);
            student.Save();

            newStudent.Id = student.Id;

            return CreatedAtRoute("GetStudentById", new { id = newStudent.Id }, newStudent);


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
            if(!Student.DeleteStudent(id))
            {
                return NotFound($"Student with ID {id} not found.");
            }

            return Ok($"Student with ID {id} has been deleted.");

        }

        [HttpPut("{id}", Name = "UpdateStudent")]//Update
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]


        public ActionResult<StudentDTO> UpdateStudent(int id, StudentDTO UpdatedStudent)
        {
            
            if (id < 1 || string.IsNullOrEmpty(UpdatedStudent.Name) || UpdatedStudent.Age < 1 || UpdatedStudent.Grade < 0)
            {
                return BadRequest("Invalid Student Data!");
            }

            Student student = Student.Find(id);

            if (student == null) 
            {
                return NotFound("Student Not Found!");
            }

            student.Name = UpdatedStudent.Name;
            student.Age = UpdatedStudent.Age;
            student.Grade = UpdatedStudent.Grade;    
            if(!student.Save())
            {
                return StatusCode(500, new { message = "Error Updating Student!" });

            }

            return Ok(student.SDTO);

        }
    }
}
