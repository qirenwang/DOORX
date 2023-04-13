using DOOR.EF.Data;
using DOOR.EF.Models;
using DOOR.Server.Controllers.Common;
using DOOR.Shared.DTO;
using DOOR.Shared.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DOOR.Server.Controllers.UD
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : BaseController
    {
        public StudentController(DOOROracleContext _DBcontext,
            OraTransMsgs _OraTransMsgs)
            : base(_DBcontext, _OraTransMsgs)
        {
        }

        [HttpGet]
        [Route("GetStudent")]
        public async Task<IActionResult> GetStudent()
        {
            List<StudentDTO> lst = await _context.Students
                .Select(sp => new StudentDTO
                {
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = DateTime.Now,
                    Employer = sp.Employer,
                    FirstName = sp.FirstName,
                    LastName = sp.LastName,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = DateTime.Now,
                    Phone = sp.Phone,
                    RegistrationDate = DateTime.Now,
                    Salutation = sp.Salutation,
                    SchoolId = sp.SchoolId,
                    StreetAddress = sp.StreetAddress,
                    StudentId = sp.StudentId,
                    Zip = sp.Zip,


                }).ToListAsync();
            return Ok(lst);
        }


        [HttpGet]
        [Route("GetStudent/{_StudentId}/{_SchoolId}")]
        public async Task<IActionResult> GetCourse(int _StudentId, int _SchoolId)
        {
            StudentDTO? lst = await _context.Students
                .Where(x => x.StudentId == _StudentId)
                .Where(x => x.SchoolId == _SchoolId)

                .Select(sp => new StudentDTO
                {
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = DateTime.Now,
                    Employer = sp.Employer,
                    FirstName = sp.FirstName,
                    LastName = sp.LastName,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = DateTime.Now,
                    Phone = sp.Phone,
                    RegistrationDate = DateTime.Now,
                    Salutation = sp.Salutation,
                    SchoolId = sp.SchoolId,
                    StreetAddress = sp.StreetAddress,
                    StudentId = sp.StudentId,
                    Zip = sp.Zip,


                }).FirstOrDefaultAsync();
            return Ok(lst);
        }

        [HttpPost]
        [Route("PostStudent")]
        public async Task<IActionResult> PostStudent([FromBody] Student _StudentDTO)
        {
            try
            {
                Student student = await _context.Students.Where(x => x.StudentId == _StudentDTO.StudentId).FirstOrDefaultAsync();

                if (student == null)
                {
                    student = new Student
                    {
                        CreatedBy = _StudentDTO.CreatedBy,
                        CreatedDate = DateTime.Now,
                        Employer = _StudentDTO.Employer,
                        FirstName = _StudentDTO.FirstName,
                        LastName = _StudentDTO.LastName,
                        ModifiedBy = _StudentDTO.ModifiedBy,
                        ModifiedDate = DateTime.Now,
                        Phone = _StudentDTO.Phone,
                        RegistrationDate = DateTime.Now,
                        Salutation = _StudentDTO.Salutation,
                        SchoolId = _StudentDTO.SchoolId,
                        StreetAddress = _StudentDTO.StreetAddress,
                        StudentId = _StudentDTO.StudentId,
                        Zip = _StudentDTO.Zip,

                    };
                    _context.Students.Add(student);
                    await _context.SaveChangesAsync();
                }
            }

            catch (DbUpdateException Dex)
            {
                List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, Newtonsoft.Json.JsonConvert.SerializeObject(DBErrors));
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                List<OraError> errors = new List<OraError>();
                errors.Add(new OraError(1, ex.Message.ToString()));
                string ex_ser = Newtonsoft.Json.JsonConvert.SerializeObject(errors);
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex_ser);
            }

            return Ok();
        }
        [HttpPut]
        [Route("PutStudent")]
        public async Task<IActionResult> PutStudent([FromBody] StudentDTO _StudentDTO)
        {
            try
            {
                Student student = await _context.Students.Where(x => x.StudentId == _StudentDTO.StudentId).FirstOrDefaultAsync();

                if (student != null)
                {

                    student.CreatedBy = _StudentDTO.CreatedBy;
                    student.CreatedDate = DateTime.Now;
                    student.Employer = _StudentDTO.Employer;
                    student.FirstName = _StudentDTO.FirstName;
                    student.LastName = _StudentDTO.LastName;
                    student.ModifiedBy = _StudentDTO.ModifiedBy;
                    student.ModifiedDate = DateTime.Now;
                    student.Phone = _StudentDTO.Phone;
                    student.RegistrationDate = DateTime.Now;
                    student.Salutation = _StudentDTO.Salutation;
                    student.SchoolId = _StudentDTO.SchoolId;
                    student.StreetAddress = _StudentDTO.StreetAddress;
                    student.StudentId = _StudentDTO.StudentId;
                    student.Zip = _StudentDTO.Zip;

                    _context.Students.Update(student);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException Dex)
            {
                List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, Newtonsoft.Json.JsonConvert.SerializeObject(DBErrors));
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                List<OraError> errors = new List<OraError>();
                errors.Add(new OraError(1, ex.Message.ToString()));
                string ex_ser = Newtonsoft.Json.JsonConvert.SerializeObject(errors);
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex_ser);
            }

            return Ok();
        }


        [HttpDelete]
        [Route("DeleteStudent/{_StudentId}/{_SchoolId}")]
        public async Task<IActionResult> DeleteCourse(int _StudentId)
        {
            try
            {
                Student student = await _context.Students.Where(x => x.StudentId == _StudentId).FirstOrDefaultAsync();

                if (student != null)
                {
                    _context.Students.Remove(student);
                    await _context.SaveChangesAsync();
                }
            }

            catch (DbUpdateException Dex)
            {
                List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
                return StatusCode(StatusCodes.Status417ExpectationFailed, Newtonsoft.Json.JsonConvert.SerializeObject(DBErrors));
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                List<OraError> errors = new List<OraError>();
                errors.Add(new OraError(1, ex.Message.ToString()));
                string ex_ser = Newtonsoft.Json.JsonConvert.SerializeObject(errors);
                return StatusCode(StatusCodes.Status417ExpectationFailed, ex_ser);
            }

            return Ok();
        }



    }
}
