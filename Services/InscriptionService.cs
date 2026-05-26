using AutoMapper;
using Jornadas_Metalurgia_2026.Models.Inscription;
using Jornadas_Metalurgia_2026.Models.Inscription.DTO;
using Jornadas_Metalurgia_2026.Repositories;
using Jornadas_Metalurgia_2026.Utils;
using Jornadas_Metalurgia_2026.Enum;
using System.Net;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Jornadas_Metalurgia_2026.Services
{
    public class InscriptionService
    {


        private readonly IInscriptionRepository _repo;
        private readonly IMapper _mapper;
        private readonly EmailService _emailService;
        private readonly Cloudinary _cloudinary;
        private readonly IConfiguration _config;
        public InscriptionService(IInscriptionRepository repo, IMapper mapper, EmailService emailService, IConfiguration config)
        {
            _repo = repo;
            _mapper = mapper;
            _emailService = emailService;

            var account = new Account(
                config["CLOUDINARY_CLOUD_NAME"],
                config["CLOUDINARY_API_KEY"],
                config["CLOUDINARY_API_SECRET"]);
            _cloudinary = new Cloudinary(account);
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
                    using var stream = dto.Presentation.OpenReadStream();

                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(dto.Presentation.FileName, stream),
                        Folder = "jornadas_metalurgia_uploads",
                      Format ="pdf"
                        
                    };

                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    string secureUrl = uploadResult.SecureUrl.ToString();
                    if(!secureUrl.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                    {
                        secureUrl += ".pdf";
                    }
                    presentationPath = secureUrl;
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
            _= Task.Run (async () =>
            {
            try
            {
                await _emailService.SendInscriptionMail(newInscription.StudentEmail, newInscription.StudentName, newInscription.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error enviando confirmación de inscripción, {ex}");
            }

            }
            
            
            
            )
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
        public async Task<List<InscriptionResponseDTO>> GetAll(string? type, string? search, bool? isactive = true)
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
            if (!string.IsNullOrWhiteSpace(search))
            {
                string lowerSearch = search.ToLower();
                string pattern = $"%{lowerSearch}%";

                if(query is IQueryable<PresentationInscription> queryPresentation)
                {
                    query = queryPresentation.Where(i =>
                    EF.Functions.ILike( i.StudentName, pattern) ||
                    EF.Functions.ILike(i.StudentDni.ToString(), pattern) ||
                    EF.Functions.ILike( i.StudentInstitution, pattern)||
                    EF.Functions.ILike(i.PresentationTitle, pattern) ||
                    i.PresentationParticipants.Any(participant => EF.Functions.ILike(participant, pattern)));


                }
                else  {

                    query = query.Where(i =>
                    EF.Functions.ILike(i.StudentName, pattern) ||
                    EF.Functions.ILike(i.StudentDni.ToString(), pattern) ||
                    EF.Functions.ILike( i.StudentInstitution, pattern));

                }

               
            }
            var list = await query.ToListAsync();
            return  _mapper.Map<List<InscriptionResponseDTO>>(list);
        }
    }
}
