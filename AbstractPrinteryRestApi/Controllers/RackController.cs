using PrinterySVC.BindingModel;
using PrinterySVC.Inteface;
using System;
using System.Web.Http;

namespace AbstractPrinteryRestApi.Controllers
{
    public class RackController : ApiController
    {
        private readonly IRackSVC _service;

        public RackController(IRackSVC service)
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
        public void AddElement(RackBindingModel model)
        {
            _service.AddElement(model);
        }

        [HttpPost]
        public void UpdElement(RackBindingModel model)
        {
            _service.UpdElement(model);
        }

        [HttpPost]
        public void DelElement(RackBindingModel model)
        {
            _service.DelElement(model.Number);
        }
    }
}
