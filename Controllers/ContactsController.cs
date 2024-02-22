using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Coach.Data;
using Coach.Models;

namespace Coach.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ContactsController : Controller
    {
        private CoachContext _context;

        public ContactsController(CoachContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var contacts = _context.Contacts.Select(i => new {
                i.ContactId,
                i.Email,
                i.FullName,
                i.Mobile,
                i.Msg,
                i.TransDate
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "ContactId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(contacts, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Contact();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Contacts.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.ContactId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Contacts.FirstOrDefaultAsync(item => item.ContactId == key);
            if(model == null)
                return StatusCode(409, "Object not found");

            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task Delete(int key) {
            var model = await _context.Contacts.FirstOrDefaultAsync(item => item.ContactId == key);

            _context.Contacts.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(Contact model, IDictionary values) {
            string CONTACT_ID = nameof(Contact.ContactId);
            string EMAIL = nameof(Contact.Email);
            string FULL_NAME = nameof(Contact.FullName);
            string MOBILE = nameof(Contact.Mobile);
            string MSG = nameof(Contact.Msg);
            string TRANS_DATE = nameof(Contact.TransDate);

            if(values.Contains(CONTACT_ID)) {
                model.ContactId = Convert.ToInt32(values[CONTACT_ID]);
            }

            if(values.Contains(EMAIL)) {
                model.Email = Convert.ToString(values[EMAIL]);
            }

            if(values.Contains(FULL_NAME)) {
                model.FullName = Convert.ToString(values[FULL_NAME]);
            }

            if(values.Contains(MOBILE)) {
                model.Mobile = Convert.ToString(values[MOBILE]);
            }

            if(values.Contains(MSG)) {
                model.Msg = Convert.ToString(values[MSG]);
            }

            if(values.Contains(TRANS_DATE)) {
                model.TransDate = values[TRANS_DATE] != null ? Convert.ToDateTime(values[TRANS_DATE]) : (DateTime?)null;
            }
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState) {
            var messages = new List<string>();

            foreach(var entry in modelState) {
                foreach(var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }
    }
}