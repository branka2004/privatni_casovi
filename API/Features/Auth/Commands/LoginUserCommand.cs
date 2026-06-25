    using API.Service;
    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using PrivateLessons.Infrastructure.Identity;
using PrivateLessons.Domain.Interfaces;

namespace API.Features.Auth.Commands;

    public class LoginUser
    {
        public class LoginUserCommand
            : IRequest<LoginResultDto?>
        {
            public string Email { get; set; } = string.Empty;

            public string Lozinka { get; set; } = string.Empty;
        }

        public class LoginResultDto
        {
            public string Token { get; set; } = string.Empty;

            public string Email { get; set; } = string.Empty;
        }

        public class LoginUserCommandHandler
            : IRequestHandler<LoginUserCommand, LoginResultDto?>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly JwtService _jwtService;
        private readonly IUnitOfWork _unitOfWork;

        public LoginUserCommandHandler(
    UserManager<ApplicationUser> userManager,
    JwtService jwtService,
    IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
        }

        public async Task<LoginResultDto?> Handle(
                LoginUserCommand request,
                CancellationToken cancellationToken)
            {
                var user =
                    await _userManager.FindByEmailAsync(
                        request.Email);

                if (user == null)
                    return null;

                var validPassword =
                    await _userManager.CheckPasswordAsync(
                        user,
                        request.Lozinka);

                if (!validPassword)
                    return null;

                var roles =
                    await _userManager.GetRolesAsync(user);


            int korisnikId = 0;

            if (roles.Contains("Predavac"))
            {
                var predavac =
                    (await _unitOfWork.Predavaci.GetAllAsync())
                    .FirstOrDefault(x =>
                        x.IdentityUserId == user.Id);

                if (predavac != null)
                {
                    korisnikId = predavac.KorisnikId;
                }
            }
            else if (roles.Contains("Ucenik"))
            {
                var ucenik =
                    (await _unitOfWork.Ucenici.GetAllAsync())
                    .FirstOrDefault(x =>
                        x.IdentityUserId == user.Id);

                if (ucenik != null)
                {
                    korisnikId = ucenik.KorisnikId;
                }
            }
            var token =
    _jwtService.CreateToken(
        user,
        roles,
        korisnikId);

            return new LoginResultDto
                {
                    Token = token,
                    Email = user.Email ?? ""
                };
            }
        }
    }