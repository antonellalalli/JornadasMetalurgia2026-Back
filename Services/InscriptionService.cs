using AutoMapper;
using Jornadas_Metalurgia_2026.Models.Inscription;
using Jornadas_Metalurgia_2026.Models.Inscription.DTO;
using Jornadas_Metalurgia_2026.Repositories;
using Jornadas_Metalurgia_2026.Utils;
using Jornadas_Metalurgia_2026.Enum;
using System.Net;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

namespace Jornadas_Metalurgia_2026.Services
{
    public class InscriptionService
    {


        private readonly IInscriptionRepository _repo;
        private readonly IMapper _mapper;

        public InscriptionService(IInscriptionRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        //En este servicio se pueden crear, modidicar, poner inactivas y traer todas las inscripciones


        public async Task<Inscription> CreateOneInscription(InscriptionCreateDTO dto)
        {
            Inscription newInscription;

            //En primer lugar le preguntas el tipo
            if (dto.InscriptionType == INSCRIPTION.PRESENTATION)
            {
                string presentationPath = string.Empty;

                if (dto.Presentation != null && dto.Presentation.Length > 0)
                {
                    var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    var fileExtension = Path.GetExtension(dto.Presentation.FileName);
                    var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                    var filePath = Path.Combine(folder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create)) { await dto.Presentation.CopyToAsync(stream); }
                    presentationPath = $"/uploads/{uniqueFileName}";
                }
                newInscription = new PresentationInscription
                {
                    StudentName = dto.StudentName,
                    StudentEmail = dto.StudentEmail,
                    StudentDni = dto.StudentDni,
                    StudentInstitution = dto.StudentInstitution,

                    PresentationTitle = dto.PresentationTitle!,
                    PresentationParticipants = dto.Participants!,
                    Presentation = presentationPath
                };
                
            }
            else
            {
                newInscription = new AttendanceInscription
                {
                    StudentName = dto.StudentName,
                    StudentEmail = dto.StudentEmail,
                    StudentDni = dto.StudentDni,
                    StudentInstitution = dto.StudentInstitution

                };
            }
            await _repo.CreateOneAsync(newInscription);
            return newInscription;
        }


        ///--------------------------------------------------
        public async Task DeleteOneById(string id)
        {
            if (!int.TryParse(id, out int inscriptionId))
            {
                throw new HttpResponseError(HttpStatusCode.BadRequest, "ID de inscripcin inválido");

            }

            var inscription = await _repo.GetOneAsync(I => I.Id == inscriptionId);
            if (inscription == null)
            {
                throw new HttpResponseError(HttpStatusCode.NotFound, "Inscripcón no encontrada");

            }
            
            
                inscription.IsActive = false;

            await _repo.DeleteOneAsync(inscription);
            
        }

        ///--------------------------------------------------



        //metodo que trae todas las inscripciones segun filtros 
        public async Task<List<InscriptionResponseDTO>> GetAll(string? type, bool? isactive = true)
        {

            var query = _repo.GetAllQueryable();


            if (isactive.HasValue)
            {
                query = query.Where(i => i.IsActive == isactive.Value);
            }
            if (!string.IsNullOrEmpty(type))
            {

                switch (type)
                {
                    case INSCRIPTION.PRESENTATION:

                        query = query.OfType<PresentationInscription>();
                        break;

                    case INSCRIPTION.ATTENDANCE:

                        query = query.OfType<AttendanceInscription>();

                        break;
                }


            }
            var list = await query.ToListAsync();
            return  _mapper.Map<List<InscriptionResponseDTO>>(list);
        }
    }
}
