using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Http;
using System;
using MongoDB.Bson.Serialization.Attributes;
using System.Linq;
using MongoDB.Bson;
using My_Profile.Services;
using RabbitMQ.Client;

namespace My_Profile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
         private RabbitMQConnection rabbit;
        private readonly IUserRepository _userRepository;
        public ValuesController(IUserRepository userRepository, RabbitMQConnection rabbit)
        {
            _userRepository = userRepository;
             this.rabbit=rabbit;
        }

        // GET api/values
        //Get all users
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await _userRepository.GetAllUsers();
            if (user == null)
            {
                return NotFound("Null database");
            }
            else
            {

                return Ok(user);
            }

        }
        // GET: api/values/id
        //Get user by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
           // ObjectId id = new ObjectId(_id);
            var user = await _userRepository.GetUser(id);
            if (user == null)
                return NotFound("user with this id not found");
            return Ok(user);
        }

        //Get Resource Status
        [HttpGet("status/{id}/resource/{resourceId}")]
        public async Task<IActionResult> GetStatusById(string id, string resourceId)
        {
          //  ObjectId id = new ObjectId(_id);
            var status = await _userRepository.GetStatus(id, resourceId);
            return Ok(status);
        }
        // POST: api/values
        //post user
        [HttpPost("{UserNode}")]
        public async Task<IActionResult> Post([FromBody]User user)
        {
            if (ModelState.IsValid)
            {
                bool result = await _userRepository.PostNote(user);
                if (!result)
                {
                    await _userRepository.Create(user);
                    return Ok(user);
                }
                else
                {
                    return BadRequest("Note already exists, please try again.");
                }
            }
            return BadRequest("Invalid Format");

        }
        [HttpPost("RatingLearningPlan")]
        public void RatingLearningPlanAsync([FromBody] LearningPlanFeedBack learningPlanFeedback)
        {
            var command = new LearningPlanFeedBack
            {
                LearningPlanFeedBackId = learningPlanFeedback.LearningPlanFeedBackId,
                LearningPlanId = learningPlanFeedback.LearningPlanId,
                UserId = learningPlanFeedback.UserId,
                Star = learningPlanFeedback.Star,
                // Password = "examplePassword"
            };

            var body = ObjectSerialize.Serialize(command);

            rabbit.Model.BasicPublish(
                                exchange: rabbit.ExchangeNme,
                                routingKey: "Send.LearningPlanFeedBack",
                                basicProperties: null,
                                body: body
            );
            Console.WriteLine(" [x] Sent {0}", command.LearningPlanFeedBackId);
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();

        }
        [HttpPost("RatingResource")]
        public void RatingResourceAsync([FromBody] ResourceFeedBack resourceFeedBack)
        {
            var command = new ResourceFeedBack
            {
                ResourceFeedBackId = resourceFeedBack.ResourceFeedBackId,
                ResourceId = resourceFeedBack.ResourceId,
                UserId = resourceFeedBack.UserId,
                Star = resourceFeedBack.Star,
                // Password = "examplePassword"
            };

            var body = ObjectSerialize.Serialize(command);

            rabbit.Model.BasicPublish(
                                exchange: rabbit.ExchangeNme,
                                routingKey: "Send.ResourceFeedBack",
                                basicProperties: null,
                                body: body
            );
            Console.WriteLine(" [x] Sent {0}", command.ResourceFeedBackId);
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();

        }
        [HttpPost("SubscriberLearningPlan")]
        public void SubscriberLearningPlanAsync([FromBody] LearningPlanFeedBack learningPlanFeedback)
        {
            var command = new LearningPlanFeedBack
            {
                LearningPlanFeedBackId = learningPlanFeedback.LearningPlanFeedBackId,
                LearningPlanId = learningPlanFeedback.LearningPlanId,
                UserId = learningPlanFeedback.UserId,
                Subscribe = learningPlanFeedback.Subscribe,
                // Password = "examplePassword"
            };

            var body = ObjectSerialize.Serialize(command);

            rabbit.Model.BasicPublish(
                                exchange: rabbit.ExchangeNme,
                                routingKey: "Send.LearningPlanFeedBack",
                                basicProperties: null,
                                body: body
            );
            Console.WriteLine(" [x] Sent {0}", command.LearningPlanFeedBackId);
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
        [HttpPost("UnSubscriberLearningPlan")]
        public void UnSubscriberLearningPlanAsync([FromBody] LearningPlanFeedBack learningPlanFeedback)
        {
            var command = new LearningPlanFeedBack
            {
                LearningPlanFeedBackId = learningPlanFeedback.LearningPlanFeedBackId,
                LearningPlanId = learningPlanFeedback.LearningPlanId,
                UserId = learningPlanFeedback.UserId,
                //  Subscribe = learningPlanFeedback.Subscribe,
                // Password = "examplePassword"
            };

            var body = ObjectSerialize.Serialize(command);

            rabbit.Model.BasicPublish(
                                exchange: rabbit.ExchangeNme,
                                routingKey: "Send.LearningPlanFeedBack",
                                basicProperties: null,
                                body: body
            );
            Console.WriteLine(" [x] Sent {0}", command.LearningPlanFeedBackId);
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
        [HttpPost("ReportQuestion")]
        public void ReportQuestionAsync([FromBody] QuestionFeedBack questionFeedBack)
        {
            var command = new QuestionFeedBack
            {
                QuestionFeedBackId = questionFeedBack.QuestionFeedBackId,
                QuestionId = questionFeedBack.QuestionId,
                UserId = questionFeedBack.UserId,
                Ambiguity = questionFeedBack.Ambiguity
                //  Subscribe = learningPlanFeedback.Subscribe,
                // Password = "examplePassword"
            };

            var body = ObjectSerialize.Serialize(command);

            rabbit.Model.BasicPublish(
                                exchange: rabbit.ExchangeNme,
                                routingKey: "Send.QuestionFeedBack",
                                basicProperties: null,
                                body: body
            );
            Console.WriteLine(" [x] Sent {0}", command.QuestionFeedBackId);
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }


        //post resource status
        [HttpPost("isCheck")]
        public async Task<IActionResult> Status([FromBody]Status status)
        {

            // bool result = await _userRepository.PostNote(user);

            await _userRepository.UserResFind(status);
            return Ok();
        }

        //post user profile
        [HttpPost("UploadsProfilePic")]
        public async Task<IActionResult> UploadsProfilePic(IFormFileCollection files)
        {

            long size = files.Sum(f => f.Length);
            try
            {
                foreach (var formFile in files)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "./wwwroot/image", formFile.FileName);
                    var stream = new FileStream(filePath, FileMode.Create);
                    await formFile.CopyToAsync(stream);


                }
                return Ok(new { count = files.Count });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        // PUT: api/values/5
        //update user profile
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]User user)
        {
           // ObjectId id = new ObjectId(_id);
            if (ModelState.IsValid)
            {
                bool result = await _userRepository.FindNote(id);
                if (result)
                {
                    await _userRepository.Update(user);
                    return Ok(user);
                }
                else
                {
                    return BadRequest("Note already exists, please try again.");
                }
            }
            return BadRequest("Invalid Format");
        }
        // DELETE: api/values/5
        //delete user profile
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
           // ObjectId id = new ObjectId(_id);

            bool result = await _userRepository.FindNote(id);
            if (result)
            {
                await _userRepository.Delete(id);
                return Ok($"note with id : {id} deleted succesfully");
            }
            else
            {
                return NotFound($"Note with {id} not found.");
            }

        }
    }
}
