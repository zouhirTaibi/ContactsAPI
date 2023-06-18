using ContactsAPI.Data;
using ContactsAPI.Moldes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly ContactApiDbContext dbContext;
        public ContactsController(ContactApiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            return Ok(await dbContext.Contacts.ToListAsync());
            
        }
        [HttpPost]  
        public async Task<IActionResult> AddContact(AddContactRequest addContactRequest)
        {
            //mapping
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                Address = addContactRequest.Address,
                FirstName = addContactRequest.FirstName,
                Phone = addContactRequest.Phone,
                Email = addContactRequest.Email,
            };

            await dbContext.Contacts.AddAsync(contact);
            await dbContext.SaveChangesAsync();
            return Ok(contact);
        }
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {
            var contact = await dbContext.Contacts.FindAsync(id);
            if(contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }


        [HttpPut]
        [Route("{id:guid}")]
        //the id is coming from the route
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id,UpdateContactRequest updateContactRequest)
        {
            var contact=await dbContext.Contacts.FindAsync(id);
            if(contact!=null) { 
                contact.Address = updateContactRequest.Address;
                contact.Email = updateContactRequest.Email;
                contact.Phone = updateContactRequest.Phone;
                contact.FirstName = updateContactRequest.FirstName;
                await dbContext.SaveChangesAsync();
                return Ok(contact);
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            //creer un objet pour recevoir les donnees trouves a base de l'id
            var contact = await dbContext.Contacts.FindAsync(id);
            if(contact!= null)
            {

                dbContext.Remove(contact);
                await dbContext.SaveChangesAsync();
                return Ok(contact);
            }
            return NotFound();

        }

    }
}
