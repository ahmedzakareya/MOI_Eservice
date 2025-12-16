using Business.Enums;
using Business.Interfaces;
using Business.ViewModel.Tourism;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Helpers
{
    public  class GeneralReqNo
    {
        private readonly IUnitOfwork _unitOfwork;

        public GeneralReqNo(IUnitOfwork unitOfwork)
        {
            _unitOfwork = unitOfwork;
        }


        public async Task<Tuple<long, string>> GetRequestNo(int reqTypeId, string activityCode)
        {
            var sequenceNo = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                .GetByCondition(x => x.ServiceId==(int)ServiceEnum.Tourism)
                .OrderByDescending(x => x.SequenceNo)
                .Select(x => x.SequenceNo)
                .FirstOrDefaultAsync();
           
            sequenceNo = (sequenceNo == 0||sequenceNo==null) ? 1 : sequenceNo + 1;
            string countId = sequenceNo.ToString().PadLeft(6, '0');

            // Determine prefix based on ReqTypeId
            string prefix = reqTypeId switch
            {
                (int)RequestTypeEnum.PreApprovementConvert => "PreCon-",
                (int)RequestTypeEnum.PreApprovementNew=> "PreNew-",
                (int)RequestTypeEnum.Request => "Req-",
                (int)RequestTypeEnum.Renew => "Ren-",
                (int)RequestTypeEnum.EndLicences => "End-",
                (int)RequestTypeEnum.Classification=>"Classif-",
                (int)RequestTypeEnum.ReClassification => "ReClassif-",
                (int)RequestTypeEnum.WhoConc => "WhConc-",
                (int)RequestTypeEnum.AddMoIC=> "AdMOIC-",
                (int)RequestTypeEnum.DeleteMOIC => "DelMOIC-",
                (int)RequestTypeEnum.RenewMOIC => "RenMOIC-",
                (int)RequestTypeEnum.ChangeAddressMOIC => "ChAddMOIC-",
                (int)RequestTypeEnum.RenewOrChangeMOIC => "ReOrReMOIC-",
                (int)RequestTypeEnum.Renouncement => "Renounce-",
                (int)RequestTypeEnum.ChangeData=>"ED-",

                _ => "UNK-"
            };

            string reqNo = prefix + GetActivitySuffix(activityCode) + countId;

            return new Tuple<long, string>((long)sequenceNo, reqNo);
        }
        private string GetActivitySuffix(string activityCode)
        {
            return activityCode switch
            {
                "551011" => "MOITH", // فندق
                "551020" => "MOITA", // شقق فندقية
                "681015" => "MOITR", // منتجعات
                "932901" => "MOITP", // منتزهات الاستجمام
                "791207" => "MOITS", // تنظيم الرحلات
                _ => "MOIX"
            };
        }
        //موافقة مبدئية
        public async Task<Tuple<long,string>> GetPreApproveReqNo(int ReqTypeId, string ActivityCode) // ----------- الموافقة المبدئية
        {
            var SequenceNo = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                           .GetByCondition(x => x.ReqtypeId == ReqTypeId)
                           .OrderByDescending(x => x.SequenceNo).Select(x => x.SequenceNo).FirstOrDefaultAsync();

            string reqno = string.Empty;
            if(SequenceNo==null||SequenceNo==0)
            {
                SequenceNo = 1;

            }
            else
            {
                SequenceNo++;
            }
            string countid = SequenceNo.ToString().PadLeft(6, '0');
            switch (ActivityCode)
            {
                case "551011":
                    reqno = "Pre-MOITH" + countid; // ----- فندق
                    break;
                case "551020":
                    reqno = "Pre-MOITA" + countid; // ----- شقق فندقية
                    break;
                case "681015":
                    reqno = "Pre-MOITM" + countid; // ----- منتجعات
                    break;
                case "932901":
                    reqno = "Pre-MOITS" + countid; // ----- منتزهات الاستجمام والشواطئ والسواحل
                    break;
                case "791207":
                    reqno = "Pre-MOITO" + countid; // ----- تنظيم وتأجير الرحلات السياحية والبرية والبحرية والإرشاد السياحي الداخلي
                    break;
            }
        
            return new Tuple<long, string>((long)SequenceNo, reqno);
        }
        //إصدار
        public async Task<Tuple<long,string>> GetReqNoTourLic( int ReqTypeId, string ActivityCode) // ------- اصدار 
        {
            var SequenceNo = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                           .GetByCondition(x => x.ReqtypeId == ReqTypeId)
                           .OrderByDescending(x => x.SequenceNo).Select(x => x.SequenceNo).FirstOrDefaultAsync();
            string reqno = string.Empty;
            if (SequenceNo == null || SequenceNo == 0)
            {
                SequenceNo = 1;

            }
            else
            {
                SequenceNo++;
            }
            string countid = SequenceNo.ToString().PadLeft(6, '0');

            switch (ActivityCode)
            {
                case "551011":
                    reqno = "Req-MOITH" + countid; // ----- فندق
                    break;
                case "551020":
                    reqno = "Req-MOITA" + countid; // ----- شقق فندقية
                    break;
                case "681015":
                    reqno = "Req-MOITR" + countid; // ----- منتجعات
                    break;
                case "932901":
                    reqno = "Req-MOITP" + countid; // ----- منتزهات الاستجمام والشواطئ والسواحل
                    break;
                case "791207":
                    reqno = "Req-MOITS" + countid; // ----- تنظيم وتأجير الرحلات السياحية والبرية والبحرية والإرشاد السياحي الداخلي
                    break;
            }
         
            return new Tuple<long,string>((long)SequenceNo,reqno);
        }
        //تجديد
        public async Task<Tuple<long, string>> GetReqNoTourLicRenew( int ReqTypeId, string ActivityCode) // ------- تجديد 
        {
            var SequenceNo = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                           .GetByCondition(x => x.ReqtypeId == ReqTypeId)
                           .OrderByDescending(x => x.SequenceNo).Select(x => x.SequenceNo).FirstOrDefaultAsync();
            string reqno = string.Empty;
            if (SequenceNo == null || SequenceNo == 0)
            {
                SequenceNo = 1;

            }
            else
            {
                SequenceNo=SequenceNo+1;
            }
            string countid = SequenceNo.ToString().PadLeft(6, '0');
            switch (ActivityCode)
            {
                case "551011":
                    reqno = "Ren-MOITH" + countid; // ----- فندق
                    break;
                case "551020":
                    reqno = "Ren-MOITA" + countid; // ----- شقق فندقية
                    break;
                case "681015":
                    reqno = "Ren-MOITR" + countid; // ----- منتجعات
                    break;
                case "932901":
                    reqno = "Ren-MOITP" + countid; // ----- منتزهات الاستجمام والشواطئ والسواحل
                    break;
                case "791207":
                    reqno = "Ren-MOITS" + countid; // ----- تنظيم وتأجير الرحلات السياحية والبرية والبحرية والإرشاد السياحي الداخلي
                    break;
            }
         
            return new Tuple<long, string>((long)SequenceNo,reqno);
        }

        public async Task<Tuple<long, string>> GetReqNoTourLicEndLicences(int ReqTypeId, string ActivityCode) // ------- تجديد 
        {
            var SequenceNo = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                           .GetByCondition(x => x.ReqtypeId == ReqTypeId)
                           .OrderByDescending(x => x.SequenceNo).Select(x => x.SequenceNo).FirstOrDefaultAsync();
            string reqno = string.Empty;
            if (SequenceNo == null || SequenceNo == 0)
            {
                SequenceNo = 1;

            }
            else
            {
                SequenceNo = SequenceNo + 1;
            }
            string countid = SequenceNo.ToString().PadLeft(6, '0');
            switch (ActivityCode)
            {
                case "551011":
                    reqno = "End-MOITH" + countid; // ----- فندق
                    break;
                case "551020":
                    reqno = "End-MOITA" + countid; // ----- شقق فندقية
                    break;
                case "681015":
                    reqno = "End-MOITR" + countid; // ----- منتجعات
                    break;
                case "932901":
                    reqno = "End-MOITP" + countid; // ----- منتزهات الاستجمام والشواطئ والسواحل
                    break;
                case "791207":
                    reqno = "End-MOITS" + countid; // ----- تنظيم وتأجير الرحلات السياحية والبرية والبحرية والإرشاد السياحي الداخلي
                    break;
            }

            return new Tuple<long, string>((long)SequenceNo, reqno);
        }



        //تصنيف
        public async Task<Tuple<long, string>> GetReqNoTourLicClass( int ReqTypeId, string ActivityCode) // ------- تصنيف 
        {
            var SequenceNo = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                           .GetByCondition(x => x.ReqtypeId == ReqTypeId)
                           .OrderByDescending(x => x.SequenceNo).Select(x => x.SequenceNo).FirstOrDefaultAsync();
            string reqno = string.Empty;
            if (SequenceNo == null || SequenceNo == 0)
            {
                SequenceNo = 1;

            }
            else
            {
                SequenceNo = SequenceNo + 1;
            }
            string countid = SequenceNo.ToString().PadLeft(6, '0');
            switch (ActivityCode)
            {
                case "551011":
                    reqno = "CLASS-MOITA" + countid; // ----- فندق
                    break;
                case "551020":
                    reqno = "CLASS-MOITA" + countid; // ----- شقق فندقية
                    break;
                case "681015":
                    reqno = "CLASS-MOITM" + countid; // ----- منتجعات
                    break;
            }
            var newLicenseRequest = new MoiEserviceLicensesRequest
            {

                SequenceNo = SequenceNo, // Set the new sequence number
                                         // You can set other required properties here as needed
            };

            await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Create(newLicenseRequest);
            await _unitOfwork.Complete();
            return new Tuple<long, string>((long)SequenceNo,reqno);
        }
        //إعادة تصنيف
        public async Task<Tuple<long, string>> GetReqNoTourLicReClass( int ReqTypeId, string ActivityCode) // ------- إعادة تصنيف 
        {
            var SequenceNo = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                           .GetByCondition(x => x.ReqtypeId == ReqTypeId)
                           .OrderByDescending(x => x.SequenceNo).Select(x => x.SequenceNo).FirstOrDefaultAsync();
            string reqno = string.Empty;
            if (SequenceNo == null || SequenceNo == 0)
            {
                SequenceNo = 1;

            }
            else
            {
                SequenceNo = SequenceNo + 1;
            }
            string countid = SequenceNo.ToString().PadLeft(6, '0');
            switch (ActivityCode)
            {
                case "551011":
                    reqno = "ReCLASS-MOITA" + countid; // ----- فندق
                    break;
                case "551020":
                    reqno = "ReCLASS-MOITA" + countid; // ----- شقق فندقية
                    break;
                case "681015":
                    reqno = "ReCLASS-MOITM" + countid; // ----- منتجعات
                    break;
            }
            var newLicenseRequest = new MoiEserviceLicensesRequest
            {

                SequenceNo = SequenceNo, // Set the new sequence number
                                         // You can set other required properties here as needed
            };

            await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Create(newLicenseRequest);
            await _unitOfwork.Complete();
            return new Tuple<long, string>((long)SequenceNo, reqno);
        }

        public async Task<Tuple<long, string>> GetReqNoTourLicWhoConc( int ReqTypeId, string ActivityCode) // ------- إعادة تصنيف 
        {
            var SequenceNo = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                           .GetByCondition(x => x.ReqtypeId == ReqTypeId)
                           .OrderByDescending(x => x.SequenceNo).Select(x => x.SequenceNo).FirstOrDefaultAsync();
            string reqno = string.Empty;
            if (SequenceNo == null || SequenceNo == 0)
            {
                SequenceNo = 1;

            }
            else
            {
                SequenceNo = SequenceNo + 1;
            }
            string countid = SequenceNo.ToString().PadLeft(6, '0');
            switch (ActivityCode)
            {
                case "551011":
                    reqno = "WhConc-MOITH" + countid; // ----- فندق
                    break;
                case "551020":
                    reqno = "WhConc-MOITA" + countid; // ----- شقق فندقية
                    break;
                case "681015":
                    reqno = "WhConc-MOITR" + countid; // ----- منتجعات
                    break;
            }
            var newLicenseRequest = new MoiEserviceLicensesRequest
            {

                SequenceNo = SequenceNo, // Set the new sequence number
                                         // You can set other required properties here as needed
            };

            await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Create(newLicenseRequest);
            await _unitOfwork.Complete();
            return new Tuple<long, string>((long)SequenceNo, reqno);
        }

        //MOCI Func
        public async Task<Tuple<long, string>> GetReqNoTourLicMOCILetter( int ReqTypeId, string ActivityCode) // ------- طلب التجارة 
        {
            var SequenceNo = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                           .GetByCondition(x => x.ReqtypeId == ReqTypeId)
                           .OrderByDescending(x => x.SequenceNo).Select(x => x.SequenceNo).FirstOrDefaultAsync();
            string reqno = string.Empty;
            if (SequenceNo == null || SequenceNo == 0)
            {
                SequenceNo = 1;

            }
            else
            {
                SequenceNo = SequenceNo + 1;
            }
            string countid = SequenceNo.ToString().PadLeft(6, '0');
            switch (ActivityCode)
            {
                case "551011":
                    reqno = "MOIC-MOITH" + countid; // ----- فندق
                    break;
                case "551020":
                    reqno = "MOIC-MOITA" + countid; // ----- شقق فندقية
                    break;
                case "681015":
                    reqno = "MOIC-MOITR" + countid; // ----- منتجعات
                    break;
                case "932901":
                    reqno = "MOIC-MOITP" + countid; // ----- منتزهات الاستجمام والشواطئ والسواحل
                    break;
                case "791207":
                    reqno = "MOIC-MOITS" + countid; // ----- تنظيم وتأجير الرحلات السياحية والبرية والبحرية والإرشاد السياحي الداخلي
                    break;
            }
           
            return new Tuple<long, string>((long)SequenceNo, reqno);
        }


        public async Task<Tuple<long, string>> GetReqNoTourLicLost( int ReqTypeId, string ActivityCode) // ------- بدل فاقد 
        {
            var SequenceNo = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                           .GetByCondition(x => x.ReqtypeId == ReqTypeId)
                           .OrderByDescending(x => x.SequenceNo).Select(x => x.SequenceNo).FirstOrDefaultAsync();
            string reqno = string.Empty;
            if (SequenceNo == null || SequenceNo == 0)
            {
                SequenceNo = 1;

            }
            else
            {
                SequenceNo = SequenceNo + 1;
            }
            string countid = SequenceNo.ToString().PadLeft(6, '0');
            switch (ActivityCode)
            {
                case "551011":
                    reqno = "LO-MOITA" + countid; // ----- فندق
                    break;
                case "551020":
                    reqno = "LO-MOITA" + countid; // ----- شقق فندقية
                    break;
                case "681015":
                    reqno = "LO-MOITM" + countid; // ----- منتجعات
                    break;
                case "932901":
                    reqno = "LO-MOITS" + countid; // ----- منتزهات الاستجمام والشواطئ والسواحل
                    break;
                case "791207":
                    reqno = "LO-MOITO" + countid; // ----- تنظيم وتأجير الرحلات السياحية والبرية والبحرية والإرشاد السياحي الداخلي
                    break;
            }
            var newLicenseRequest = new MoiEserviceLicensesRequest
            {

                SequenceNo = SequenceNo, // Set the new sequence number
                                         // You can set other required properties here as needed
            };

            await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Create(newLicenseRequest);
            await _unitOfwork.Complete();
            return new Tuple<long, string>((long)SequenceNo, reqno);
        }

        public async Task<Tuple<long, string>> GetReqNoTourLicEdit( int ReqTypeId, string ActivityCode,int? TransactionTypeId) // ------- تعديل 
        {
            var SequenceNo = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                           .GetByCondition(x => x.ReqtypeId == ReqTypeId)
                           .OrderByDescending(x => x.SequenceNo).Select(x => x.SequenceNo).FirstOrDefaultAsync();
            string reqno = string.Empty;
            if (SequenceNo == null || SequenceNo == 0)
            {
                SequenceNo = 1;

            }
            else
            {
                SequenceNo = SequenceNo + 1;
            }
            string countid = SequenceNo.ToString().PadLeft(6, '0');
            switch (ActivityCode)
            {
                case "551011":
                    reqno = "ED-MOITA" + countid; // ----- فندق
                    break;
                case "551020":
                    reqno = "ED-MOITA" + countid; // ----- شقق فندقية
                    break;
                case "681015":
                    reqno = "ED-MOITM" + countid; // ----- منتجعات
                    break;
                case "932901":
                    reqno = "ED-MOITS" + countid; // ----- منتزهات الاستجمام والشواطئ والسواحل
                    break;
                case "791207":
                    reqno = "ED-MOITO" + countid; // ----- تنظيم وتأجير الرحلات السياحية والبرية والبحرية والإرشاد السياحي الداخلي
                    break;
            }
            var newLicenseRequest = new MoiEserviceLicensesRequest
            {

                SequenceNo = SequenceNo, // Set the new sequence number
                                         // You can set other required properties here as needed
            };

            await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Create(newLicenseRequest);
            await _unitOfwork.Complete();
            return new Tuple<long, string>((long)SequenceNo,reqno);
        }

        public async Task<Tuple<long, string>> GetRequestNoForElaw(int reqTypeId, int licTypeId)
        {
            var sequenceNo = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                .GetByCondition(x => x.ServiceId==(int)ServiceEnum.Elaw)
                .OrderByDescending(x => x.SequenceNo)
                .Select(x => x.SequenceNo)
                .FirstOrDefaultAsync();

            sequenceNo = (sequenceNo == 0 || sequenceNo == null) ? 1 : sequenceNo + 1;
            string countId = sequenceNo.ToString().PadLeft(6, '0');

            string prefix = GeneratePrefix(reqTypeId, licTypeId);
            string reqNo = prefix + countId;

            return new Tuple<long, string>((long)sequenceNo, reqNo);
        }

        private string GeneratePrefix(int requestTypeId, int licenseTypeId)
        {
            return ((RequestTypeEnum)requestTypeId) switch
            {
                RequestTypeEnum.Request => $"Req-{GetLicenseTypeSuffix(licenseTypeId)}",
                RequestTypeEnum.Renew => $"Ren-{GetLicenseTypeSuffix(licenseTypeId)}",
                RequestTypeEnum.EndLicences => $"End-{GetLicenseTypeSuffix(licenseTypeId)}",
                RequestTypeEnum.Renouncement => $"Renoun-{GetLicenseTypeSuffix(licenseTypeId)}",
                RequestTypeEnum.ReplacementOfLost => $"End-{GetLicenseTypeSuffix(licenseTypeId)}",

                RequestTypeEnum.ChangeData  => $"Cha-{GetLicenseTypeSuffix(licenseTypeId)}",

                _ => "UNK-"
            };
        }

        private string GetChangePrefix(TransactionTypesEnum transactionTypeId, int licenseTypeId)
        {
            var licenseSuffix = GetLicenseTypeSuffix(licenseTypeId);

            return transactionTypeId switch
            {
                TransactionTypesEnum.ChangeManager => $"ChMng-{licenseSuffix}",
                TransactionTypesEnum.ChangeLicencesName => $"ChLicName-{licenseSuffix}",
                TransactionTypesEnum.ChangeEmail => $"ChEmail-{licenseSuffix}",
                TransactionTypesEnum.ChangeSocialMedia => $"ChSocial-{licenseSuffix}",
                TransactionTypesEnum.ChangePartnerName => $"ChPartner-{licenseSuffix}",
                TransactionTypesEnum.ChangeAddress => $"ChAddr-{licenseSuffix}",
                TransactionTypesEnum.ChangeLicencesType => $"ChLicType-{licenseSuffix}",
                TransactionTypesEnum.ChangeCompaneName => $"ChComp-{licenseSuffix}",
                TransactionTypesEnum.ChangeCommercialName => $"ChTrade-{licenseSuffix}",
                _ => $"ChUnknown-{licenseSuffix}"
            };
        }

        private string GetLicenseTypeSuffix(int licenseTypeId)
        {
            return ((LicTypeEnum)licenseTypeId) switch
            {
                LicTypeEnum.Company => "CO",
                LicTypeEnum.Organization => "ORG",
                LicTypeEnum.Media_Organization_Company => "MCO",
                LicTypeEnum.Media_Organization_Individuals => "MPI",
                LicTypeEnum.Licensed_Media_Organization => "LMO",
                LicTypeEnum.Public_Benefit_Association => "PBA",
                LicTypeEnum.Government_Entity => "GOV",
                LicTypeEnum.OrganizationOrPerson => "IND",
                _ => "UNK"
            };
        }



    }
}
