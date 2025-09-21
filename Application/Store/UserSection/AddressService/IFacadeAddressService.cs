using Application.Commons.Interfaces.Localization;
using Application.Store.UserSection.AddressService.Commands;
using Application.Store.UserSection.AddressService.Queries;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.UserSection.AddressService
{
    public interface IFacadeAddressService
    {
        IAddressCommandsService AddressCommandsService { get; }
        IAddressQueriesService AddressQueriesService { get; }
    }
    public class FacadeAddressService : IFacadeAddressService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        private readonly UserManager<User> _userManager;
        public FacadeAddressService(IMediator mediator, ILocalizationService localizationService, UserManager<User> userManager)
        {
            _mediator = mediator;
            _localizationService = localizationService;
            _userManager = userManager;
        }

        //Commands
        private IAddressCommandsService _AddressCommandsService;
        public IAddressCommandsService AddressCommandsService
        {
            get
            {
                return _AddressCommandsService = _AddressCommandsService ?? new AddressCommandsService(_mediator, _localizationService, _userManager);
            }
        }

        //Queries
        private IAddressQueriesService _AddressQueriesService;
        public IAddressQueriesService AddressQueriesService
        {
            get
            {
                return _AddressQueriesService = _AddressQueriesService ?? new AddressQueriesService(_mediator, _localizationService, _userManager);
            }
        }
    }
}
