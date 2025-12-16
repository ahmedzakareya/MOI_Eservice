using Business.Enums;
using Business.Interfaces;
using Business.ModelWithSpecification;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace Business.Helpers
{
    public  class GenerateLicNo
    {

        private readonly IUnitOfwork _unitOfwork;

        public GenerateLicNo(IUnitOfwork unitOfwork)
        {
            _unitOfwork = unitOfwork;
        }
        public async Task<Tuple<long,string>> GenerateUniqueLicenseNumberTourism(int ServiceId,int ReqTypeId,int ActivityTypeId)
        {
           
           
            var licences =await _unitOfwork.genericRepository<Licence>()
                                        .GetByCondition(l => l.ServiceId == ServiceId )
                                        .OrderByDescending(l => l.SequenceNo)
                                        .FirstOrDefaultAsync();
            long? SequenceNo =licences.SequenceNo ;

           
                if (SequenceNo == null || SequenceNo == 0)
                {
                     SequenceNo = 1;
                  


                }
                else
                {
                    SequenceNo = ++SequenceNo; // Handle nullable long safely
                }
            
            string licensePrefix = "TL-MOIRH";

            //if (ReqTypeId == (int)RequestTypeEnum.PreApprovement)
            //{
            //    licensePrefix = "TL-MOIPA";
            //}
            //else if (ReqTypeId == (int)RequestTypeEnum.Request)
            //{
            //    switch (ActivityTypeId)
            //    {
            //        case (int)ActivityTypeEnum.Hotel:
            //            licensePrefix = "TL-MOIRH";
            //            break;
            //        case (int)ActivityTypeEnum.ApartmentHotel:
            //            licensePrefix = "TL-MOIRAH";
            //            break;
            //        case (int)ActivityTypeEnum.Resorts:
            //            licensePrefix = "TL-MOIRR";
            //            break;
            //        case (int)ActivityTypeEnum.Parks:
            //            licensePrefix = "TL-MOIRP";
            //            break;
            //        case (int)ActivityTypeEnum.Sailing:
            //            licensePrefix = "TL-MOIRS";
            //            break;
            //        default:
            //            throw new ArgumentException("Invalid activity code.");
            //    }
            //}
           
            string newLicenseNumber = $"{licensePrefix}{SequenceNo:D6}";


            return new Tuple<long,string>((long)SequenceNo, newLicenseNumber);
        }

        public async Task<Tuple<long, string>> GenerateUniqueLicenseNumberTourismPreApproval(int ReqTypeId,int ServiceId)
        {


            var licences = await _unitOfwork.genericRepository<MoiPreApprovement>().GetAllAsync();
            var sequencelicences = licences.OrderByDescending(x => x.SequenceNo).Select(x=>x.SequenceNo).FirstOrDefault();
            //long? SequenceNo = sequencelicences.SequenceNo;              
            //long? SequenceNo = licences.SequenceNo;


            if (sequencelicences == null || sequencelicences == 0)
            {
                sequencelicences = 1;



            }
            else
            {
                sequencelicences = ++sequencelicences; // Handle nullable long safely
            }

            string licensePrefix = ReqTypeId switch
            {
                (int)RequestTypeEnum.PreApprovementNew => "TL-MOIPA-NEW",
                (int)RequestTypeEnum.PreApprovementConvert => "TL-MOIPA-CONV",
                _ => "TL-MOIPA"
            };



            string newLicenseNumber = $"{licensePrefix}{sequencelicences:D6}";


            return new Tuple<long, string>((long)sequencelicences, newLicenseNumber);
        }


        public async Task<Tuple<long, string>> GenerateUniqueLicenseNumberElaw(int ServiceId, int ReqTypeId, int ActivityTypeId)
        {

            var licences = await _unitOfwork.genericRepository<Licence>()
                                         .GetByCondition(l => l.ServiceId == ServiceId)
                                         .OrderByDescending(l => l.SequenceNo)
                                         .FirstOrDefaultAsync();
            long? SequenceNo = licences.SequenceNo;


            if (SequenceNo == null || SequenceNo == 0)
            {
                SequenceNo = 1;



            }
            else
            {
                SequenceNo = ++SequenceNo; // Handle nullable long safely
            }
            //string numberPart = requestid.ToString().PadLeft(7, '0');

            string licensePrefix = "L-ELaw";


            //if (ReqTypeId == 2)
            //{
            //    switch (ActivityTypeId)
            //    {
            //        case(int) ActivityTypeEnum.NewsService:
            //            licensePrefix = "EL-News";
            //            break;
            //        case (int)ActivityTypeEnum.ElectronicPress:
            //            licensePrefix = "EL-ElecPr";
            //            break;
            //        case (int)ActivityTypeEnum.ElectronicWeb:
            //            licensePrefix = "El-ElecWeb";
            //            break;
            //        case (int)ActivityTypeEnum.ElectronicElaw:
            //            licensePrefix = "El-ElecElw";
            //            break;
            //        case (int)ActivityTypeEnum.ElectronicNews:
            //            licensePrefix = "El-ElecNews";
            //            break;
            //        default:
            //            throw new ArgumentException("Invalid activity code.");
            //    }
            //}
            string newLicenseNumber = $"{licensePrefix}{SequenceNo:D6}";


            return new Tuple<long, string>((long)SequenceNo, newLicenseNumber);
        }




    }
}
