using Application.Commons.Interfaces.Localization;
using Application.Store.UserSection.PaymentService.Commands;
using Application.Store.UserSection.PaymentService.Queries;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Store.UserSection.PaymentService
{
    public interface IFacadePaymentService
    {
        IPaymentCommandsService PaymentCommandsService { get; }
        IPaymentQueriesService PaymentQueriesService { get; }
    }
    public class FacadePaymentService : IFacadePaymentService
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _localizationService;
        private readonly UserManager<User> _userManager;
        public FacadePaymentService(IMediator mediator, ILocalizationService localizationService, UserManager<User> userManager)
        {
            _mediator = mediator;
            _localizationService = localizationService;
            _userManager = userManager;
        }

        //Commands
        private IPaymentCommandsService _PaymentCommandsService;
        public IPaymentCommandsService PaymentCommandsService
        {
            get
            {
                return _PaymentCommandsService = _PaymentCommandsService ?? new PaymentCommandsService(_mediator, _localizationService, _userManager);
            }
        }

        //Queries
        private IPaymentQueriesService _PaymentQueriesService;
        public IPaymentQueriesService PaymentQueriesService
        {
            get
            {
                return _PaymentQueriesService = _PaymentQueriesService ?? new PaymentQueriesService(_mediator, _localizationService, _userManager);
            }
        }
    }
}
