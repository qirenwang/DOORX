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
    public class GradeTypeWeightController : BaseController
    {
        public GradeTypeWeightController(DOOROracleContext DBcontext,
            OraTransMsgs _OraTransMsgs) :
        base(DBcontext, _OraTransMsgs)
        {
        }

        [HttpGet]
        [Route("GetGradeTypeWeight")]
        public async Task<IActionResult> GetGradeTypeWeight()
        {
            List<GradeTypeWeightDTO> lst = await _context.GradeTypeWeights
                .Select(sp => new GradeTypeWeightDTO
                {
                    SchoolId = sp.SchoolId,
                    SectionId = sp.SectionId,
                    GradeTypeCode = sp.GradeTypeCode,
                    NumberPerSection = sp.NumberPerSection,
                    PercentOfFinalGrade = sp.PercentOfFinalGrade,
                    DropLowest = sp.DropLowest,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate,
                }).ToListAsync();
            return Ok(lst);
        }

        [HttpGet]
        [Route("GetGradeTypeWeight/{_SchoolId}/{_SectionId}/{_GradeTypeCode}")]
        public async Task<IActionResult> GetGradeTypeWeight(int _SchoolId, int _SectionId, string _GradeTypeCode)
        {
            GradeTypeWeightDTO? lst = await _context.GradeTypeWeights
                .Where(x => x.SchoolId == _SchoolId)
                .Where(x => x.SectionId == _SectionId)
                .Where(x => x.GradeTypeCode == _GradeTypeCode)
                .Select(sp => new GradeTypeWeightDTO
                {
                    SchoolId = sp.SchoolId,
                    SectionId = sp.SectionId,
                    GradeTypeCode = sp.GradeTypeCode,
                    NumberPerSection = sp.NumberPerSection,
                    PercentOfFinalGrade = sp.PercentOfFinalGrade,
                    DropLowest = sp.DropLowest,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate,
                }).FirstOrDefaultAsync();
            return Ok(lst);

        }

        [HttpPost]
        [Route("PostGradeTypeWeight")]
        public async Task<IActionResult> PostGradeTypeWeight([FromBody] GradeTypeWeightDTO _GradeTypeWeightDTO)
        {
            try
            {
                GradeTypeWeight? gradetw = await _context.GradeTypeWeights
                    .Where(x => x.SchoolId == _GradeTypeWeightDTO.SchoolId)
                    .Where(x => x.SectionId == _GradeTypeWeightDTO.SectionId)
                    .Where(x => x.GradeTypeCode == _GradeTypeWeightDTO.GradeTypeCode).FirstOrDefaultAsync();

                if (gradetw == null)
                {
                    gradetw = new GradeTypeWeight
                    {
                        SchoolId = _GradeTypeWeightDTO.SchoolId,
                        SectionId = _GradeTypeWeightDTO.SectionId,
                        GradeTypeCode = _GradeTypeWeightDTO.GradeTypeCode,
                        NumberPerSection = _GradeTypeWeightDTO.NumberPerSection,
                        PercentOfFinalGrade = _GradeTypeWeightDTO.PercentOfFinalGrade,
                        DropLowest = _GradeTypeWeightDTO.DropLowest
                    };
                    _context.GradeTypeWeights.Add(gradetw);
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
        [Route("PutGradeTypeWeight")]
        public async Task<IActionResult> PutGradeTypeWeight([FromBody] GradeTypeWeightDTO _GradeTypeWeightDTO)
        {
            try
            {
                GradeTypeWeight? gradetw = await _context.GradeTypeWeights
                    .Where(x => x.SchoolId == _GradeTypeWeightDTO.SchoolId)
                    .Where(x => x.SectionId == _GradeTypeWeightDTO.SectionId)
                    .Where(x => x.GradeTypeCode == _GradeTypeWeightDTO.GradeTypeCode).FirstOrDefaultAsync();

                if (gradetw != null)
                {
                    gradetw.SchoolId = _GradeTypeWeightDTO.SchoolId;
                    gradetw.SectionId = _GradeTypeWeightDTO.SectionId;
                    gradetw.GradeTypeCode = _GradeTypeWeightDTO.GradeTypeCode;
                    gradetw.NumberPerSection = _GradeTypeWeightDTO.NumberPerSection;
                    gradetw.PercentOfFinalGrade = _GradeTypeWeightDTO.PercentOfFinalGrade;
                    gradetw.DropLowest = _GradeTypeWeightDTO.DropLowest;

                    _context.GradeTypeWeights.Update(gradetw);
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
        [Route("DeleteGradeTypeWeight/{_SchoolId}/{_SectionId}/{_GradeTypeCode}")]
        public async Task<IActionResult> DeleteGradeTypeWeight(int _SchoolId, int _SectionId, string _GradeTypeCode)
        {
            try
            {
                GradeTypeWeight? gradetw = await _context.GradeTypeWeights
                    .Where(x => x.SchoolId == _SchoolId)
                .Where(x => x.SectionId == _SectionId)
                    .Where(x => x.GradeTypeCode == _GradeTypeCode).FirstOrDefaultAsync();

                if (gradetw != null)
                {
                    _context.GradeTypeWeights.Remove(gradetw);
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