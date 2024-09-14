//using AutoMapper;
//using Koi.Repositories.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Koi.Services.Services
//{
//    public class PaymentService
//    {
//        private readonly IMapper _mapper;
//        private readonly IClaimsService _claimsService;
//        private readonly IUnitOfWork _unitOfWork;

//        public PaymentService(IMapper mapper, IClaimsService claimsService, IUnitOfWork unitOfWork)
//        {
//            _mapper = mapper;
//            _claimsService = claimsService;
//            _unitOfWork = unitOfWork;
//        }

//        public async Task<Payment> CreatePaymentAsync(Appointment appointment)
//        {
//            try
//            {
//                var newPayment = new Payment
//                {
//                    AppointmentId = appointment.Id,
//                    PaymentMethod = "VNPAY",
//                    PaymentDate = DateTime.UtcNow.AddHours(7),
//                    Status = "PENDING",
//                    UserId = _claimsService.GetCurrentSessionUserId,
//                    TotalAmount = appointment.Price.Value
//                };

//                var check = await _unitOfWork.PaymentRepository.AddAsync(newPayment);
//                var result = await _unitOfWork.SaveChangeAsync();
//                if (result > 0)
//                {
//                    return check;
//                }
//                else
//                {
//                    throw new Exception("Something has been failed while adding payment");
//                    //return null;
//                }
//            }
//            catch (Exception ex)
//            {
//                throw new Exception(ex.Message);
//            }
//        }

//        public async Task<List<Payment>> GetPaymentsAsync()
//        {
//            var paymentsUser = await _unitOfWork.PaymentRepository.GetAllPaymentsAsync();
//            if (_claimsService.GetCurrentSessionUserId == 0)
//            {
//                throw new Exception("Non-signup user");
//            }
//            return paymentsUser.Where(x => x.UserId == _claimsService.GetCurrentSessionUserId).ToList();
//        }

//        public async Task<List<Payment>> GetPaymentByAppointmentIdAsync(int id)
//        {
//            var paymentsUser = await _unitOfWork.PaymentRepository.GetAllAsync(x => x.AppointmentId == id);
//            if (_claimsService.GetCurrentSessionUserId == 0)
//            {
//                throw new Exception("Non-signup user");
//            }

//            return paymentsUser;
//        }

//        public async Task<PaymentResult> CreateTransactionOfPaymentAsync(PaymentResponseModel response)
//        {
//            var existingPayment = await _unitOfWork.PaymentRepository.GetByIdAsync(int.Parse(response.PaymentId), null, x => x.Appointment, x => x.User);
//            if (existingPayment == null)
//            {
//                throw new Exception("This payment is not existing please check again");
//            }

//            var newTracsaction = new Transaction
//            {
//                PaymentId = existingPayment.Id,
//                Date = response.PayDate,
//                AccountNumber = response.BanKTranNo,
//                TransactionToken = response.TransactionToken,
//                AmountOfMoney = (decimal)response.AmountOfMoney,
//                TransactionNo = response.TransactionNo,
//                PaymentType = "NCB",
//                Status = response.Success ? "SUCCESS" : "FAILED",
//            };

//            newTracsaction = await _unitOfWork.TransactionRepository.AddAsync(newTracsaction);
//            var checkAppointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(existingPayment.AppointmentId);

//            if (response.Success)
//            {
//                existingPayment.Status = "COMPLETED";
//                checkAppointment.Status = "ASSIGNING";
//                //existingPayment.
//            }
//            else
//            {
//                existingPayment.Status = "FAILED";
//            }

//            _unitOfWork.PaymentRepository.Update(existingPayment);

//            if (await _unitOfWork.SaveChangeAsync() > 0)
//            {
//                var result = new PaymentResult
//                {
//                    Payment = existingPayment,
//                    Transaction = newTracsaction
//                };

//                return result;
//            }
//            else
//            {
//                throw new Exception("Something has happened while adding transaction");
//            }
//        }
//    }
//}