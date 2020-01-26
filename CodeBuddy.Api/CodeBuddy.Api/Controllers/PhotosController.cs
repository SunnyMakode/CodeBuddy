using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CodeBuddy.Api.Context.Repository;
using AutoMapper;
using Microsoft.Extensions.Options;
using CodeBuddy.Api.Helpers;
using CloudinaryDotNet;
using System.Threading.Tasks;
using CodeBuddy.Api.Dtos;
using CodeBuddy.Api.Model;
using System.Security.Claims;
using CloudinaryDotNet.Actions;
using System.Linq;

namespace CodeBuddy.Api.Controllers
{
    [Authorize]
    [Route("api/user/{userId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IGenericRepository _genericRepository;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        //IOptions is use to access the configuration service that we've in startup.cs
        public PhotosController(IGenericRepository genericRepository,
                                IMapper mapper,
                                IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
            _cloudinaryConfig = cloudinaryConfig;

            Account acc = new Account
            (
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret    
            );

            _cloudinary = new Cloudinary(acc);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, [FromForm] PhotoForCreationDto photoForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var userFromRepo = await _genericRepository.Get<User>(userId
                , i => i.Photos
                , i => i.Id == userId);

            var file = photoForCreationDto.File;

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreationDto);

            if (!userFromRepo.Photos.Any(u => u.IsMainPhoto))
            {
                photo.IsMainPhoto = true;
            }

            userFromRepo.Photos.Add(photo);

            if (await _genericRepository.SaveAll())
            {
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);

                //return Ok();
                // calling GetPhoto action method
                return CreatedAtRoute(
                    "GetPhoto",
                    new
                    {
                        userId = userId,
                        id = photo.Id
                    },
                    photoToReturn);
            }

            return BadRequest("Could not add the photo");
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _genericRepository.Get<Photo>(id);

            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo);
        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var userFromRepo = await _genericRepository.Get<User>(userId
                , i => i.Photos
                , i => i.Id == userId);

            if (!userFromRepo.Photos.Any(p => p.Id == id))
            {
                return Unauthorized();
            }

            var photoFromRepo = await _genericRepository.Get<Photo>(id);

            if (photoFromRepo.IsMainPhoto)
            {
                return BadRequest("This is already a main photo");
            }

            var currentMainPhoto = await _genericRepository.Get<Photo>(userId,
                i => i.UserId == userId,
                j => j.IsMainPhoto);

            currentMainPhoto.IsMainPhoto = false;

            photoFromRepo.IsMainPhoto = true;

            if (await _genericRepository.SaveAll())
            {
                return NoContent();
            }

            return BadRequest("Could not set photo as main");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var userFromRepo = await _genericRepository.Get<User>(userId
                , i => i.Photos
                , i => i.Id == userId);

            if (!userFromRepo.Photos.Any(p => p.Id == id))
            {
                return Unauthorized();
            }

            var photoFromRepo = await _genericRepository.Get<Photo>(id);

            if (photoFromRepo.IsMainPhoto)
            {
                return BadRequest("You cannot delete a main photo");
            }

            if (photoFromRepo.PublicId != null)
            {
                var deleteParams = new DeletionParams(photoFromRepo.PublicId);

                var cloudinaryResult = _cloudinary.Destroy(deleteParams);

                if (cloudinaryResult.Result == "ok")
                {
                    _genericRepository.Delete(photoFromRepo);
                }
            }

            if (photoFromRepo.PublicId == null)
            {
                _genericRepository.Delete(photoFromRepo);
            }

            if (await _genericRepository.SaveAll())
            {
                return Ok();
            }

            return BadRequest("Failed to delete photo");
        }
    }    
}