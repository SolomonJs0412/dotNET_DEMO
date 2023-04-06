using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnetEFAndJWT.classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnetEFAndJWT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IMapper _mapper;


        public StudentController(DataContext context, IMapper mapper)
        {
            this.context = context;
            _mapper = mapper;
        }

        //Get all students
        [HttpGet]
        [Route("students")]
        public async Task<ActionResult<List<Student>>> GetAllStudents()
        {
            var students = await context.Students.ToListAsync();
            if (students.Count == 0)
            {
                return NotFound("No data available");
            }
            return Ok(students.Select(std => _mapper.Map<StudentDto>(std)));
        }

        //Get one student with id
        [HttpGet]
        [Route("student/{id}")]
        public async Task<ActionResult<List<Student>>> GetStudent(int id)
        {
            var student = await context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound("Student not found");
            }

            return Ok(student);
        }

        //Create a new Student
        [HttpPost]
        [Route("new")]
        public async Task<ActionResult<List<Student>>> CreateNewStudent([FromBody] Student student)
        {
            if (student == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            context.Add(student);
            await context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(CreateNewStudent),
                new { student_id = student.student_id },
                student);
        }
        //Delete a student with id
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult<Student>> DeleteStudent(int id)
        {
            var isCreated = await context.Students.FindAsync(id);
            if (isCreated == null)
            {
                return NotFound("Not found student with id " + id);
            }
            context.Students.Remove(isCreated);
            context.SaveChangesAsync();
            return Ok("Student deleted successfully");
        }
        //Update a student with id
        [HttpPut]
        [Route("update/{id}")]
        public async Task<ActionResult<Student>> UpdateStudent(int id, [FromBody] Student studentUpdatesInfo)
        {
            var student = await context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound("Student with id not found");
            }

            student.name = studentUpdatesInfo.name;
            student.major = studentUpdatesInfo.major;
            student.score = studentUpdatesInfo.score;

            context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(CreateNewStudent),
                new { student_id = id },
                studentUpdatesInfo);
        }
    }
}