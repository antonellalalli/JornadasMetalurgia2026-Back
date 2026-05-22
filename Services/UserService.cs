using AutoMapper;
using Jornadas_Metalurgia_2026.Models.User;
using Jornadas_Metalurgia_2026.Models.User.DTO;
using Jornadas_Metalurgia_2026.Repositories;
using Jornadas_Metalurgia_2026.Utils;
using System.Net;

namespace Jornadas_Metalurgia_2026.Services
{
    public class UserService
    {
        private readonly IUserRepository _repo;
        private readonly IEncoderServices _encoderService;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repo, IEncoderServices encoderService, IMapper mapper)
        {
            _repo = repo;
            _encoderService = encoderService;
            _mapper = mapper;

        }

        //PASAR EL USER A DTO SIN CONTRASEÑA
        async public Task<List<UserWithoutPasswordDTO>> GetAll()
        {
            var users = await _repo.GetAllAsync();
            //pasar a dto
            return _mapper.Map<List<UserWithoutPasswordDTO>>(users);
        }

        async public Task<User> GetOneByEmailOrUsername(string? email, string? username)
        {
            if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(email))
            {
                throw new HttpResponseError(HttpStatusCode.BadRequest, "Email and Username are empty");
            }
            var user = await _repo.GetOneAsync(x => x.Email == email || x.UserName == username);
            return user;
        }
        async public Task<User> UpdateUser(string id, UpdateUserDTO dto)
        {
            //Buscar el usuario por id
            if (!int.TryParse(id, out int userId))
            {
                throw new HttpResponseError(HttpStatusCode.BadRequest, "ID de usuario inválido");
            }
            var user = await _repo.GetOneAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new HttpResponseError(HttpStatusCode.NotFound, "Usuario no encontrado");
            }
            if (!string.IsNullOrWhiteSpace(dto.UserName)){

               user.UserName = dto.UserName;
            }
            if (!string.IsNullOrWhiteSpace(dto.Email))
            {
            user.Email = dto.Email;

            }

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                user.Password = _encoderService.Encode(dto.Password);
            }
            await _repo.UpdateOneAsync(user);

            return user;
        }


    }
}
