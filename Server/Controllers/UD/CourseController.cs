using DOOR.EF.Data;
using DOOR.EF.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text.Json;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Hosting.Internal;
using System.Net.Http.Headers;
using System.Drawing;
using Microsoft.AspNetCore.Identity;
using DOOR.Server.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Data;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Numerics;
using DOOR.Shared.DTO;

namespace CSBA6.Server.Controllers.app
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        protected DOOROracleContext _context { get; set; }
        public CourseController(DOOROracleContext _DBcontext)
        {
            _context = _DBcontext;
        }

        //[HttpPost]
        //[Route("PostPlayerAssigned/{_SeasonID}")]
        //public async Task<IActionResult> PostPlayerAssigned(string _SeasonID, [FromBody] string _PlayerDTO_JSON)
        //{
        //    List<PlayerDTO> _PlayerDTO = JsonSerializer.Deserialize<List<PlayerDTO>>(_PlayerDTO_JSON);


        //    HashSet<string> _ExistingItems = _context.SeasonPlayers
        //        .Where(x => x.SeasonPlayerSeasonId == _SeasonID)
        //        .Select(x => x.SeasonPlayerPlayerId).ToHashSet();

        //    //                    HashSet<string> _UpdatePosts = new HashSet<string>(_ProjectDetailsDTO.FndgDocmtPostIDs);
        //    HashSet<string> _UpdateItems = _PlayerDTO.Select(x => x.PlayerId).ToHashSet();


        //    IEnumerable<string> _ThingsToRemove = _ExistingItems.Except(_UpdateItems);
        //    //  This should be just A

        //    IEnumerable<string> _ThingsToAdd = _UpdateItems.Except(_ExistingItems);
        //    //  This should be just D

        //    foreach (var RemoveItem in _ThingsToRemove)
        //    {
        //        var Item = _context.SeasonPlayers
        //            .Where(x => x.SeasonPlayerSeasonId == _SeasonID)
        //            .Where(x => x.SeasonPlayerPlayerId == RemoveItem).FirstOrDefault();
        //        if (Item != null)
        //            _context.SeasonPlayers.Remove(Item);
        //    }

        //    foreach (var Item in _ThingsToAdd)
        //    {
        //        var AddItem = new SeasonPlayer
        //        {
        //            SeasonPlayerId = Guid.NewGuid().ToString().ToUpper().Replace("-", ""),
        //            SeasonPlayerSeasonId = _SeasonID,
        //            SeasonPlayerPlayerId = Item
        //        };
        //        _context.SeasonPlayers.Add(AddItem);
        //    }
        //    await _context.SaveChangesAsync();

        //    return Ok();

        //}

        [HttpGet]
        [Route("GetCourse")]
        public async Task<IActionResult> GetCourse()
        {
            List<CourseDTO> lst = await _context.Courses
                .Select(sp => new CourseDTO
                {
                    Cost = sp.Cost,
                    CourseNo = sp.CourseNo,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    Description = sp.Description,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate,
                    Prerequisite = sp.Prerequisite
                }).ToListAsync();
            return Ok(lst);
        }


        [HttpGet]
        [Route("GetCourse/{_CourseNo}")]
        public async Task<IActionResult> GetCourse(int _CourseNo)
        {
            CourseDTO? lst = await _context.Courses
                .Where(x => x.CourseNo == _CourseNo)
                .Select(sp => new CourseDTO
                {
                    Cost = sp.Cost,
                    CourseNo = sp.CourseNo,
                    CreatedBy = sp.CreatedBy,
                    CreatedDate = sp.CreatedDate,
                    Description = sp.Description,
                    ModifiedBy = sp.ModifiedBy,
                    ModifiedDate = sp.ModifiedDate,
                    Prerequisite = sp.Prerequisite
                }).FirstOrDefaultAsync();
            return Ok(lst);
        }


        [HttpPost]
        [Route("PostCourse")]
        public async Task<IActionResult> PostCourse([FromBody] CourseDTO _CourseDTO)
        {

            try
            {

                Course c = await _context.Courses
                    .Where(x => x.CourseNo == _CourseDTO.CourseNo)
                    .FirstOrDefaultAsync();

                if (c == null)
                {
                    c = new Course
                    {
                        CourseNo = _CourseDTO.CourseNo,
                        Cost = _CourseDTO.Cost,
                        Description = _CourseDTO.Description,
                        Prerequisite = _CourseDTO.Prerequisite
                    };
                    _context.Courses.Add(c);
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
                return StatusCode(StatusCodes.Status500InternalServerError, ex_ser);
            }

            return Ok();
        }



        //[HttpPut]
        //[Route("PutPlayer")]
        //public async Task<IActionResult> PutPlayer([FromBody] string _PlayerDTO_JSON)
        //{
        //    PlayerDTO _PlayerDTO = JsonSerializer.Deserialize<PlayerDTO>(_PlayerDTO_JSON);

        //    try
        //    {
        //        await _context.Database.BeginTransactionAsync();
        //        _context.SetUserID(this._CurrUser.UserName);

        //        var item = await _context.Players.Where(x => x.PlayerId == _PlayerDTO.PlayerId).FirstOrDefaultAsync();

        //        if (item != null)
        //        {
        //            item.PlayerLastName = _PlayerDTO.PlayerLastName;
        //            item.PlayerFirstName = _PlayerDTO.PlayerFirstName;
        //            _context.Players.Update(item);
        //            await _context.SaveChangesAsync();
        //        }

        //        var itmPP = await _context.PlayerPositions.Where(x => x.PlayerId == _PlayerDTO.PlayerId).FirstOrDefaultAsync();
        //        if (itmPP == null)
        //        {
        //            itmPP = new PlayerPosition
        //            {
        //                PlayerId = _PlayerDTO.PlayerId,
        //                PlayerPositionId = Guid.NewGuid().ToString().ToUpper().Replace("-", ""),
        //                PrimaryPositionId = _PlayerDTO.PrimaryPositionId,
        //                SecondaryPositionId = _PlayerDTO.SecondaryPositionId
        //            };
        //            _context.PlayerPositions.Add(itmPP);
        //        }
        //        else
        //        {
        //            itmPP.PrimaryPositionId = _PlayerDTO.PrimaryPositionId;
        //            itmPP.SecondaryPositionId = _PlayerDTO.SecondaryPositionId;
        //            _context.PlayerPositions.Update(itmPP);
        //        }
        //        await _context.SaveChangesAsync();



        //        await _context.Database.CommitTransactionAsync();
        //    }

        //    catch (DbUpdateException Dex)
        //    {
        //        List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
        //        return StatusCode(StatusCodes.Status417ExpectationFailed, Newtonsoft.Json.JsonConvert.SerializeObject(DBErrors));
        //    }
        //    catch (Exception ex)
        //    {
        //        _context.Database.RollbackTransaction();
        //        List<OraError> errors = new List<OraError>();
        //        errors.Add(new OraError(1, ex.Message.ToString()));
        //        string ex_ser = Newtonsoft.Json.JsonConvert.SerializeObject(errors);
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex_ser);
        //    }

        //    return Ok();
        //}

        //[HttpDelete]
        //[Route("DeletePlayer/{PlayerID}")]
        //public async Task<IActionResult> DeletePlayer(String PlayerID)
        //{
        //    try
        //    {
        //        await _context.Database.BeginTransactionAsync();
        //        _context.SetUserID(this._CurrUser.UserName);
        //        var item = await _context.Players.Where(x => x.PlayerId == PlayerID).FirstOrDefaultAsync();

        //        if (item != null)
        //        {
        //            _context.Players.Remove(item);
        //            await _context.SaveChangesAsync();
        //        }
        //        await _context.Database.CommitTransactionAsync();
        //    }

        //    catch (DbUpdateException Dex)
        //    {
        //        List<OraError> DBErrors = ErrorHandling.TryDecodeDbUpdateException(Dex, _OraTranslateMsgs);
        //        return StatusCode(StatusCodes.Status417ExpectationFailed, Newtonsoft.Json.JsonConvert.SerializeObject(DBErrors));
        //    }
        //    catch (Exception ex)
        //    {
        //        _context.Database.RollbackTransaction();
        //        List<OraError> errors = new List<OraError>();
        //        errors.Add(new OraError(1, ex.Message.ToString()));
        //        string ex_ser = Newtonsoft.Json.JsonConvert.SerializeObject(errors);
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex_ser);
        //    }

        //    return Ok();
        //}
    }
}