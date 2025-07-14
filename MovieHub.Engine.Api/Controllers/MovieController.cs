using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieHub.Engine.Api.Models.Requests;
using MovieHub.Engine.Domain.UseCases.AddMedia;
using MovieHub.Engine.Domain.UseCases.AddMedia.FinalizeMovieAddition;
using MovieHub.Engine.Domain.UseCases.AddMedia.FinalizeMovieUpload;
using MovieHub.Engine.Domain.UseCases.AddMedia.GetMoviePartUploadUrl;
using MovieHub.Engine.Domain.UseCases.AddMedia.InitiateMovieAddition;
using MovieHub.Engine.Domain.UseCases.AddMedia.StartMovieUpload;

namespace MovieHub.Engine.Api.Controllers;

[ApiController]
[Route("api/movies")]
public class MovieController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateMovieRequest([FromBody] CreateMovieRequestDto requestDto)
    {
        var requestId = await mediator.Send(
            new InitiateMovieAdditionCommand(
                requestDto.Title,
                requestDto.Description,
                requestDto.ReleasedAt,
                requestDto.Countries,
                requestDto.Genres,
                requestDto.DirectorIds,
                requestDto.ActorsIds,
                requestDto.AgeRating,
                requestDto.Budget));

        return Ok(new { RequestId = requestId });
    }

    [HttpPost]
    [Route("{requestId}/complete")]
    public async Task<IActionResult> CompleteAddMovie(Guid requestId)
    { 
        await mediator.Send(new FinalizeMovieAdditionCommand(requestId));
        
        return Ok();
    }
    
    [HttpPost]
    [Route("{requestId}/upload/init")]
    public async Task<ActionResult> InitUpload(Guid requestId, string contentType,string fileName)
    {
        var command = new StartMovieUploadCommand(requestId, contentType,fileName);
        var (key, uploadId) = await mediator.Send(command);

        return Ok(new { Key = key, UploadId = uploadId });
    }

    [HttpGet]
    [Route("upload/{uploadId}/part")]
    public async Task<ActionResult> GetPresignedUrl(
        [FromQuery] string key,
        string uploadId,
        [FromQuery] int partNumber)
    {
        var command = new GetMoviePartUploadUrlQuery(
            uploadId,
            partNumber,
            key);

        var url = await mediator.Send(command);

        return Ok(new { Url = url });
    }

    [HttpPost]
    [Route("{requestId}/upload/{uploadId}/complete")]
    public async Task<ActionResult> CompleteUpload(
        Guid requestId,
        [FromQuery] string key,
        string uploadId,
        [FromBody] List<FilePartDto> parts)
    {
        var command = new FinalizeMovieUploadCommand(
            requestId,
            uploadId,
            parts.Select(x=>new FilePart(x.PartNumber,x.PartName)),
            key);

        await mediator.Send(command);

        return Ok();
    }
    
    
}