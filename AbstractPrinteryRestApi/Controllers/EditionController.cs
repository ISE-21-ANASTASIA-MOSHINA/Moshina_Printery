using PrinterySVC.BindingModel;
using PrinterySVC.Inteface;
using System;
using System.Web.Http;

namespace AbstractPrinteryRestApi.Controllers
{
    public class EditionController : ApiController
    {

        
            private readonly IEditionSVC _service;

            public EditionController(IEditionSVC service)
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
            public void AddElement(EditionBindingModel model)
            {
                _service.AddElement(model);
            }

            [HttpPost]
            public void UpdElement(EditionBindingModel model)
            {
                _service.UpElement(model);
            }

            [HttpPost]
            public void DelElement(EditionBindingModel model)
            {
                _service.DelElement(model.Number);
            }
        }
}
