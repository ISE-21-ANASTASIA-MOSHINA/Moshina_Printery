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
    public class ReportController : ApiController
    {

        private readonly IReportSVC _service;

        public ReportController(IReportSVC service)
        {
            _service = service;
        }

        [HttpGet]
        public IHttpActionResult GetRacksLoad()
        {
            var list = _service.GetRacksLoad();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpPost]
        public IHttpActionResult GetCustomerOrders(ReportBindingModel model)
        {
            var list = _service.GetCustomerOrders(model);
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpPost]
        public void SaveEditionPrice(ReportBindingModel model)
        {
            _service.SaveProductPrice(model);
        }

        [HttpPost]
        public void SaveRacksLoad(ReportBindingModel model)
        {
            _service.SaveRacksLoad(model);
        }

        [HttpPost]
        public void SaveCustomerOrders(ReportBindingModel model)
        {
            _service.SaveCustomerOrders(model);
        }

    }
}
