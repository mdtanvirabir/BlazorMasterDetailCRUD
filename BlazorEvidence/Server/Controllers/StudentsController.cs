using BlazorEvidence.Server.Data;
using BlazorEvidence.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazorEvidence.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public StudentsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]

        public async Task<ActionResult<List<Student>>> Get()
        {

            return await _db.Students.Include(s => s.Modules).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetById(int id)
        {
            var result = await _db.Students.Include(x => x.Modules).FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        [HttpPost]

        public async Task<ActionResult<int>> Post(Student student)
        {
            _db.Students.Add(student);
            await _db.SaveChangesAsync();
            return student.Id;
        }

        [HttpPut]
        public async Task<ActionResult> Put(Student student)
        {
            _db.Entry(student).State = EntityState.Modified;
            foreach (var modules in student.Modules)
            {
                if (modules.Id != 0)
                {
                    _db.Entry(modules).State = EntityState.Modified;
                }
                else
                {
                    _db.Entry(modules).State = EntityState.Added;
                }
            }
            var idsOfModules = student.Modules.Select(x => x.Id).ToList();
            var modulesToDelete = await _db
                .Modules
                .Where(x => !idsOfModules.Contains(x.Id) && x.StudentId == student.Id).ToListAsync();
            _db.RemoveRange(modulesToDelete);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            var delStudent = await _db.Students.FindAsync(id);
            if (delStudent == null)
            {
                return false;
            }
            _db.Remove(delStudent);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
