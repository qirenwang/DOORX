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
    public class EnrollmentController : BaseController
    {
        public EnrollmentController(DOOROracleContext _DBcontext,
            OraTransMsgs _OraTransMsgs)
        : base(_DBcontext, _OraTransMsgs)

        {
        }


        [HttpGet]
        [Route("GetEnrollment")]
        public async Task<IActionResult> GetEnrollment()
        {
            List<EnrollmentDTO> lst = await _context.Enrollments
                .Select(sp => new EnrollmentDTO
                {
                    CreatedBy = sp.CreatedBy,
                    FinalGrade = sp.FinalGrade,
                    EnrollDate = sp.EnrollDate,
                    CreatedDate = DateTime.Now,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = DateTime.Now,
                    SchoolId = sp.SchoolId,
                    SectionId = sp.SectionId,
                    StudentId = sp.StudentId,


                }).ToListAsync();
            return Ok(lst);
        }


        [HttpGet]
        [Route("GetEnrollment/{_StudentId}/{_SectionId}/{_SchoolId}")]
        public async Task<IActionResult> GetEnrollment(int _StudentId, int _SectionId, int _SchoolId)
        {
            EnrollmentDTO? lst = await _context.Enrollments
                .Where(x => x.StudentId == _StudentId)
                .Select(sp => new EnrollmentDTO
                {
                    CreatedBy = sp.CreatedBy,
                    FinalGrade = sp.FinalGrade,
                    EnrollDate = sp.EnrollDate,
                    CreatedDate = DateTime.Now,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = DateTime.Now,
                    StudentId = sp.StudentId,
                    SectionId = sp.SectionId,
                    SchoolId = sp.SchoolId,

                }).FirstOrDefaultAsync();
            return Ok(lst);

        }


        [HttpPost]
        [Route("PostEnrollment")]
        public async Task<IActionResult> PostEnrollment([FromBody] EnrollmentDTO _EnrollmentDTO)
        {
            try
            {
                Enrollment enrollment = await _context.Enrollments.Where(x => x.StudentId == _EnrollmentDTO.StudentId).FirstOrDefaultAsync();

                if (enrollment == null)
                {
                    enrollment = new Enrollment
                    {
                        SchoolId = _EnrollmentDTO.SchoolId,
                        SectionId = _EnrollmentDTO.SectionId,
                        StudentId = _EnrollmentDTO.StudentId,
                        ModifiedDate = DateTime.Now,
                        ModifiedBy = _EnrollmentDTO.ModifiedBy,
                        CreatedDate = DateTime.Now,
                        EnrollDate = DateTime.Now,
                        FinalGrade = _EnrollmentDTO.FinalGrade,
                        CreatedBy = _EnrollmentDTO.CreatedBy,

                    };
                    _context.Enrollments.Add(enrollment);
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
        [Route("PutEnrollment")]
        public async Task<IActionResult> PutEnrollment([FromBody] EnrollmentDTO _EnrollmentDTO)
        {
            try
            {
                Enrollment enrollment = await _context.Enrollments.Where(x => x.StudentId == _EnrollmentDTO.StudentId).FirstOrDefaultAsync();

                if (enrollment != null)
                {
                    enrollment.SchoolId = _EnrollmentDTO.SchoolId;
                    enrollment.SectionId = _EnrollmentDTO.SectionId;
                    enrollment.StudentId = _EnrollmentDTO.StudentId;
                    enrollment.ModifiedDate = DateTime.Now;
                    enrollment.ModifiedBy = _EnrollmentDTO.ModifiedBy;
                    enrollment.CreatedDate = DateTime.Now;
                    enrollment.EnrollDate = DateTime.Now;
                    enrollment.FinalGrade = _EnrollmentDTO.FinalGrade;
                    enrollment.CreatedBy = _EnrollmentDTO.CreatedBy;

                    _context.Enrollments.Update(enrollment);
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
        [Route("DeleteCourse/{_StudentId}/{_SectionId}/{_SchoolId}")]
        public async Task<IActionResult> DeleteCourse(int _StudentId, int _SectionId, int _SchoolId)
        {
            try
            {
                Enrollment enroll = await _context.Enrollments.Where(x => x.StudentId == _StudentId).FirstOrDefaultAsync();

                if (enroll != null)
                {
                    _context.Enrollments.Remove(enroll);
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