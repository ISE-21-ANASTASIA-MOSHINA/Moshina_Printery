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
    public class MaterialController : ApiController
    {

        private readonly IMaterialSVC _service;

        public MaterialController(IMaterialSVC service)
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

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var element = _service.GetElement(id);
            if (element == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(element);
        }

        [HttpPost]
        public void AddElement(PrinterySVC.BindingModel.MaterialBindingModel model)
        {
            _service.AddElement(model);
        }

        [HttpPost]
        public void UpdElement(PrinterySVC.BindingModel.MaterialBindingModel model)
        {
            _service.UpElement(model);
        }

        [HttpPost]
        public void DelElement(MaterialBindingModel model)
        {
            _service.DelElement(model.Number);
        }
    }
}
