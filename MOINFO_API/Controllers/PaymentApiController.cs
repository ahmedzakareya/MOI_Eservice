using AutoMapper;
using Business.Enums;
using Business.Helpers;
using Business.Interfaces;
using Business.ModelWithSpecification;
using Business.Repository;
using Business.ViewModel;
using Business.ViewModel.ClassificationVM;
using Business.ViewModel.Dynamic;
using Business.ViewModel.HomePage;
using Business.ViewModel.Tourism;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using static Azure.Core.HttpHeader;

namespace MOINFO_API.Controllers
{
    [Route("api/PaymentFront")]
    public class PaymentApiController : BaseController
    {
        private readonly IUnitOfwork _unitOfwork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly GenerateLicNo _generateLicNo;
        private readonly EmailService _emailService;
        private readonly IDataFetchService _dataFetchService;
        private readonly IUpdateDataService _updateDataService;
        private readonly ILogger<PaymentApiController> _logger;


        public PaymentApiController(IUnitOfwork unitOfwork, IConfiguration configuration
            , IMapper mapper, GenerateLicNo generateLicNo, ILogger<PaymentApiController> logger, EmailService emailService, IDataFetchService dataFetchService, IUpdateDataService updateDataService)
        {
            _unitOfwork = unitOfwork;
            _configuration = configuration;
            _mapper = mapper;
            _generateLicNo = generateLicNo;
            _emailService = emailService;
            _dataFetchService = dataFetchService;
            _updateDataService = updateDataService;
            _logger = logger;

        }

  
        #region Payment

        [HttpPost]
        [Route("PostPayment")]
        public async Task<dynamic> PostPayment(PaymentRequestModel TourismPaymentModel)
        {
            string error = string.Empty;
            try
            {
                using (IDbContextTransaction dbTransaction = _unitOfwork.BeginTransaction())
                {
                    try
                    {
                        MoiEserviceRequestPaymentDetail ReqPayment = new MoiEserviceRequestPaymentDetail()
                        {
                            RequestId = TourismPaymentModel.reqID,
                            AppCivilId = TourismPaymentModel.ApplicantCivilId,
                            LicenceId = Convert.ToInt32(TourismPaymentModel.LicId),
                            TotalAmount = TourismPaymentModel.ServiceAmount,
                            Payed = 0,
                            ServiceId=(int) ServiceEnum.Tourism,
                            UserId=TourismPaymentModel.ApplicantId
                        };
                        var _mappedPayment = _mapper.Map<MoiEserviceRequestPaymentDetail>(ReqPayment);

                       await _unitOfwork.genericRepository<MoiEserviceRequestPaymentDetail>().Create(ReqPayment);
                        await _unitOfwork.Complete();


                        dbTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        return new ErrorMessage()
                        {
                            Error = true,
                            Status = "Failure",
                            Message = ex.Message + "" + ex.InnerException + "" + error,
                        };
                    }
                }
                return new ErrorMessage()
                {
                    Error = false,
                    Status = "Success",
                    Message = "inserted suceesfully",
                };
            }
            catch (Exception ex)
            {
                return new ErrorMessage()
                {
                    Error = true,
                    Status = "Failure",
                    Message = ex.Message + "" + ex.InnerException + "" + error,
                };
            }
        }

        [HttpPost]
        [Route("UpdatePayment")]
        public async Task<dynamic> UpdatePayment([FromBody] PaymentResponse PaymentResponse)
        {
            string error = string.Empty;
            try
            {

                //--------------- Start trunsaction -----------------------------------
                using (IDbContextTransaction dbTransaction = _unitOfwork.BeginTransaction())
                {
                    try
                    {
                        //------- Update in Payment table ------------
                        int reqidPayTbl = int.Parse(PaymentResponse.MerchantRequestID);
                        MoiEserviceRequestPaymentDetail UpdatePayTable = _unitOfwork.genericRepository<MoiEserviceRequestPaymentDetail>().GetByCondition(p => p.RequestId == reqidPayTbl).FirstOrDefault();
                        if (UpdatePayTable != null)
                        {
                            UpdatePayTable.PaymentId = PaymentResponse.PaymentID;
                            UpdatePayTable.Result = PaymentResponse.Result;
                            UpdatePayTable.TranId = PaymentResponse.TranID;
                            UpdatePayTable.Ref = PaymentResponse.Ref;
                            UpdatePayTable.Postdate = PaymentResponse.Postdate;
                            UpdatePayTable.Auth = PaymentResponse.Auth;
                            UpdatePayTable.TrackId = PaymentResponse.TrackID;
                            UpdatePayTable.Payed = PaymentResponse.Payed;
                            UpdatePayTable.Status = PaymentResponse.Status;
                        }

                        var _mappedUpdatePay = _mapper.Map<MoiEserviceRequestPaymentDetail>(UpdatePayTable);
                        await _unitOfwork.genericRepository<MoiEserviceRequestPaymentDetail>().Update(UpdatePayTable);
                        await _unitOfwork.Complete();

                        if (PaymentResponse.Payed == 1)
                        {
                            long reqId = long.Parse(PaymentResponse.MerchantRequestID);
                            //------- Update in Request table ------------
                            MoiEserviceLicensesRequest UpdateReqModel = _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().GetByCondition(c => c.RequestId == reqId).FirstOrDefault();
                            if (UpdateReqModel != null)
                            {
                                UpdateReqModel.RequestStatusId = (int)RequestStatusEnum.FinalLicenseIssued;
                                UpdateReqModel.Licpaystatus = "1";
                            }

                            var _mappedUpdateRequest = _mapper.Map<MoiEserviceLicensesRequest>(UpdateReqModel);
                            await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Update(UpdateReqModel);
                            await _unitOfwork.Complete();
                        }




                        dbTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        return new ErrorMessage()
                        {
                            Error = true,
                            Status = "Failure",
                            Message = ex.Message + "" + ex.InnerException + "" + error,
                        };
                    }
                }
                //--------------- End trunsaction -----------------------------------
                return new ErrorMessage()
                {
                    Error = false,
                    Status = "Success",
                    Message = "inserted suceesfully",
                };

            }
            catch (Exception ex)
            {

                return new ErrorMessage()
                {
                    Error = true,
                    Status = "Failure",
                    Message = ex.Message + "" + ex.InnerException + "" + error,
                };

            }


        }
        #endregion


        [HttpGet]
        [Route("GetAllRequestsForUser/{CivilId}")]
        public async Task<IEnumerable<RequestVM>> GetAllRequestsForUser(string CivilId)
        {
            var SpecRequest = new RequestWithSpecificService(CivilId, (int)ServiceEnum.Tourism);
            var AllRequest = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                            .GetTableWithSpecService(SpecRequest);

            return _mapper.Map<IEnumerable<MoiEserviceLicensesRequest>, IEnumerable<RequestVM>>(AllRequest);


        }

        [HttpGet]
        [Route("GetRequestDetails/{id}")]
        public async Task<RequestFrontVM> GetRequestDetails(long id)
        {
            var SpecRequest = new RequestWithSpecificService((int)id, (int)ServiceEnum.Tourism, false);
            var RequestDetails = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                              .GetByIdWithSpec(SpecRequest);
            var PaymentPerRequest = await _unitOfwork.genericRepository<MoiEserviceRequestPaymentDetail>()
                            .GetByCondition(x => x.RequestId == id).FirstOrDefaultAsync();
            var AttachmenttRequest = await _unitOfwork.genericRepository<MoiEserviceRequestsAttach>()
                           .GetByCondition(x => x.AttachRequestid == id).ToListAsync();
            var UserApplicant = await _unitOfwork.genericRepository<AspNetUser>()
                          .GetByCondition(x => x.CivilId == RequestDetails.AppCivilId).FirstOrDefaultAsync();


            return new RequestFrontVM
            {
                RequestVM = _mapper.Map<MoiEserviceLicensesRequest, RequestVM>(RequestDetails),
                PaymentDetailsVM = _mapper.Map<MoiEserviceRequestPaymentDetail, PaymentDetailsVM>(PaymentPerRequest),
                attachVMs = _mapper.Map<IEnumerable<MoiEserviceRequestsAttach>, IEnumerable<AttachVM>>(AttachmenttRequest),
                AspnetUserVM = _mapper.Map<AspNetUser, AspnetUserVM>(UserApplicant)

            };

        }

       

    }
}
