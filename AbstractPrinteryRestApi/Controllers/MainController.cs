using PrinterySVC.BindingModel;
using PrinterySVC.Inteface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AbstractPrinteryRestApi.Controllers
{
    public class MainController : ApiController
    {

        private readonly IMainSVC _service;

        public MainController(IMainSVC service)
        {
            _service = service;
        }

        [HttpGet]
        public IHttpActionResult GetList()
        {
            var list = _service.GetList();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpPost]
        public void CreateBooking(BookingBindingModel model)
        {
            _service.CreateBooking(model);
        }

        [HttpPost]
        public void TakeBookingInWork(BookingBindingModel model)
        {
            _service.TakeBookingInWork(model);
        }

        [HttpPost]
        public void FinishBooking(BookingBindingModel model)
        {
            _service.FinishBooking(model.Number);
        }

        [HttpPost]
        public void PayBooking(BookingBindingModel model)
        {
            _service.PayBooking(model.Number);
        }

        [HttpPost]
        public void PutMaterialOnRack(RackMaterialBindingModel model)
        {
            _service.PutMaterialOnRack(model);
        }

    }
}
