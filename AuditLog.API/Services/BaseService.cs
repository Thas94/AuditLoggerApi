using AuditLog.API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AuditLog.API.Services
{
    public class BaseService
    {
        public readonly IMapper _mapper;
        public readonly ILogger _logger;
        public readonly DatabaseContext _databaseContext;
        public readonly UserManager<UserIdentity> _userManager;
        private readonly IConfiguration _configuration;

        public BaseService(IMapper mapper, ILogger logger, DatabaseContext databaseContext, UserManager<UserIdentity> userManager, IConfiguration configuration)
        {
            _mapper = mapper;
            _logger = logger;
            _databaseContext = databaseContext;
            _userManager = userManager;
            _configuration = configuration;
        }
    }
}
