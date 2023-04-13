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
    public class InstructorController : BaseController
    {
        public InstructorController(DOOROracleContext _DBcontext,
            OraTransMsgs _OraTransMsgs)
        : base(_DBcontext, _OraTransMsgs)

        {
        }


        [HttpGet]
        [Route("GetInstructor")]
        public async Task<IActionResult> GetInstructor()
        {
            List<InstructorDTO> lst = await _context.Instructors
                .Select(sp => new InstructorDTO
                {
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = DateTime.Now,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = DateTime.Now,
                    FirstName = sp.FirstName,
                    LastName = sp.LastName,
                    InstructorId = sp.InstructorId,
                    Phone = sp.Phone,
                    Salutation = sp.Salutation,
                    SchoolId = sp.SchoolId,
                    StreetAddress = sp.StreetAddress,
                    Zip = sp.Zip,


                }).ToListAsync();
            return Ok(lst);
        }


        [HttpGet]
        [Route("GetInstructor/{_InstructorId}/{_SchoolId}")]
        public async Task<IActionResult> GetInstructor(int _InstructorId, int _SchoolId)
        {
            InstructorDTO? lst = await _context.Instructors
                .Where(x => x.InstructorId == _InstructorId)
                .Select(sp => new InstructorDTO
                {
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = DateTime.Now,
                    Zip = sp.Zip,
                    FirstName = sp.FirstName,
                    InstructorId = sp.InstructorId,
                    Phone = sp.Phone,
                    LastName = sp.LastName,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = DateTime.Now,
                    StreetAddress = sp.StreetAddress,
                    Salutation = sp.Salutation,
                    SchoolId = sp.SchoolId,

                }).FirstOrDefaultAsync();
            return Ok(lst);
        }


        [HttpPost]
        [Route("PostInstructor")]
        public async Task<IActionResult> PostInstructor([FromBody] InstructorDTO _InstructorDTO)
        {
            try
            {
                Instructor instructor = await _context.Instructors.Where(x => x.InstructorId == _InstructorDTO.InstructorId).FirstOrDefaultAsync();

                if (instructor == null)
                {
                    instructor = new Instructor
                    {
                        ModifiedDate = DateTime.Now,
                        Phone = _InstructorDTO.Phone,
                        Salutation = _InstructorDTO.Salutation,
                        InstructorId = _InstructorDTO.InstructorId,
                        FirstName = _InstructorDTO.FirstName,
                        LastName = _InstructorDTO.LastName,
                        CreatedBy = _InstructorDTO.CreatedBy,
                        CreatedDate = DateTime.Now,
                        SchoolId = _InstructorDTO.SchoolId,
                        ModifiedBy = _InstructorDTO.ModifiedBy,
                        StreetAddress = _InstructorDTO.StreetAddress,
                        Zip = _InstructorDTO.Zip,


                    };
                    _context.Instructors.Add(instructor);
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
        [Route("PutInstructor")]
        public async Task<IActionResult> PutInstructor([FromBody] InstructorDTO _InstructorDTO)
        {
            try
            {
                Instructor instructor = await _context.Instructors.Where(x => x.InstructorId == _InstructorDTO.InstructorId).FirstOrDefaultAsync();

                if (instructor != null)
                {
                    instructor.ModifiedDate = DateTime.Now;
                    instructor.ModifiedDate = DateTime.Now;
                    instructor.Phone = _InstructorDTO.Phone;
                    instructor.Salutation = _InstructorDTO.Salutation;
                    instructor.InstructorId = _InstructorDTO.InstructorId;
                    instructor.FirstName = _InstructorDTO.FirstName;
                    instructor.LastName = _InstructorDTO.LastName;
                    instructor.CreatedBy = _InstructorDTO.CreatedBy;
                    instructor.CreatedDate = DateTime.Now;
                    instructor.SchoolId = _InstructorDTO.SchoolId;
                    instructor.ModifiedBy = _InstructorDTO.ModifiedBy;
                    instructor.StreetAddress = _InstructorDTO.StreetAddress;
                    instructor.Zip = _InstructorDTO.Zip;

                    _context.Instructors.Update(instructor);
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
        [Route("DeleteInstructor/{_InstructorId}/{_SchoolId}")]
        public async Task<IActionResult> DeleteCourse(int _InstructorId)
        {
            try
            {
                Instructor instructor = await _context.Instructors.Where(x => x.InstructorId == _InstructorId).FirstOrDefaultAsync();

                if (instructor != null)
                {
                    _context.Instructors.Remove(instructor);
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
